#!/bin/bash
set -e
set -o pipefail

echo "====================================="
echo "Films App - AWS ECS Fargate Deployment"
echo "====================================="
echo ""

# Project configuration
PROJECT_NAME="films-app"
TASK_FAMILY="films-app-task"
SERVICE_NAME="films-app-service"

# Prompt for AWS configuration
read -p "Enter AWS Region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS Cluster Name (e.g., films-cluster): " CLUSTER_NAME

echo ""
echo "--- Network Configuration ---"
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNETS_INPUT
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Parse subnets
IFS=',' read -ra SUBNETS <<< "$SUBNETS_INPUT"
SUBNET_1=${SUBNETS[0]}
SUBNET_2=${SUBNETS[1]:-$SUBNET_1}

echo ""
read -p "Enter Docker Image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films-app:latest): " IMAGE_URI

echo ""
echo "--- Database Configuration ---"
read -p "Enter Database Server (e.g., films-db.abc123.us-east-1.rds.amazonaws.com): " DB_SERVER
read -p "Enter Database Name (default: filmsdb): " DB_NAME
DB_NAME=${DB_NAME:-filmsdb}
read -p "Enter Database User (default: filmsuser): " DB_USER
DB_USER=${DB_USER:-filmsuser}
read -sp "Enter Database Password: " DB_PASSWORD
echo ""

echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating ECS cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to create ECS cluster"
        exit 1
    fi
}

echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "Creating Application Load Balancer and Target Group..."
    
    # Create ALB
    ALB_NAME="$PROJECT_NAME-alb"
    echo "Creating ALB: $ALB_NAME"
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "$ALB_NAME" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null || echo "")
    
    if [ -z "$ALB_ARN" ]; then
        echo "ALB may already exist, retrieving ARN..."
        ALB_ARN=$(aws elbv2 describe-load-balancers \
            --names "$ALB_NAME" \
            --region "$AWS_REGION" \
            --query 'LoadBalancers[0].LoadBalancerArn' \
            --output text)
    fi
    
    # Create Target Group
    TG_NAME="$PROJECT_NAME-tg"
    echo "Creating Target Group: $TG_NAME"
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "$TG_NAME" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text 2>/dev/null || echo "")
    
    if [ -z "$TARGET_GROUP_ARN" ]; then
        echo "Target Group may already exist, retrieving ARN..."
        TARGET_GROUP_ARN=$(aws elbv2 describe-target-groups \
            --names "$TG_NAME" \
            --region "$AWS_REGION" \
            --query 'TargetGroups[0].TargetGroupArn' \
            --output text)
    fi
    
    # Create Listener
    echo "Creating ALB Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null 2>&1 || echo "Listener may already exist"
    
    # Get ALB DNS name
    ALB_DNS=$(aws elbv2 describe-load-balancers \
        --load-balancer-arns "$ALB_ARN" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].DNSName' \
        --output text)
    
    echo "Load Balancer DNS: $ALB_DNS"
    echo "Target Group ARN: $TARGET_GROUP_ARN"
else
    echo "Skipping load balancer creation"
    TARGET_GROUP_ARN=""
fi

echo ""
echo "Creating CloudWatch Log Group..."
aws logs create-log-group --log-group-name "/ecs/films-app" --region "$AWS_REGION" 2>/dev/null || echo "Log group already exists"

echo ""
echo "Preparing ECS task definition..."

# Create temporary task definition with replacements
TEMP_TASK_DEF="/tmp/task-definition-$$.json"
cp ecs/task-definition.json "$TEMP_TASK_DEF"

sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" "$TEMP_TASK_DEF"
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" "$TEMP_TASK_DEF"
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" "$TEMP_TASK_DEF"
sed -i "s|{{DB_SERVER}}|$DB_SERVER|g" "$TEMP_TASK_DEF"
sed -i "s|{{DB_NAME}}|$DB_NAME|g" "$TEMP_TASK_DEF"
sed -i "s|{{DB_USER}}|$DB_USER|g" "$TEMP_TASK_DEF"
sed -i "s|{{DB_PASSWORD}}|$DB_PASSWORD|g" "$TEMP_TASK_DEF"

echo "Registering ECS task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://"$TEMP_TASK_DEF" \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

if [ -z "$TASK_DEF_ARN" ]; then
    echo "ERROR: Failed to register task definition"
    rm -f "$TEMP_TASK_DEF"
    exit 1
fi

echo "Task Definition ARN: $TASK_DEF_ARN"
rm -f "$TEMP_TASK_DEF"

echo ""
echo "Preparing ECS service definition..."

# Create temporary service definition
TEMP_SERVICE_DEF="/tmp/service-definition-$$.json"
cp ecs/service-definition.json "$TEMP_SERVICE_DEF"

sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" "$TEMP_SERVICE_DEF"
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" "$TEMP_SERVICE_DEF"
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" "$TEMP_SERVICE_DEF"
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" "$TEMP_SERVICE_DEF"

if [ -n "$TARGET_GROUP_ARN" ]; then
    sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" "$TEMP_SERVICE_DEF"
else
    # Remove loadBalancers section if no load balancer
    sed -i '/"loadBalancers"/,/],/d' "$TEMP_SERVICE_DEF"
    sed -i '/"healthCheckGracePeriodSeconds"/d' "$TEMP_SERVICE_DEF"
fi

echo "Checking if service exists..."
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null || echo "None")

if [ "$SERVICE_EXISTS" == "None" ] || [ -z "$SERVICE_EXISTS" ]; then
    echo "Creating new ECS service..."
    aws ecs create-service \
        --cluster "$CLUSTER_NAME" \
        --service-name "$SERVICE_NAME" \
        --cli-input-json file://"$TEMP_SERVICE_DEF" \
        --region "$AWS_REGION"
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to create ECS service"
        rm -f "$TEMP_SERVICE_DEF"
        exit 1
    fi
else
    echo "Updating existing ECS service..."
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION"
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to update ECS service"
        rm -f "$TEMP_SERVICE_DEF"
        exit 1
    fi
fi

rm -f "$TEMP_SERVICE_DEF"

echo ""
echo "Waiting for service to become stable..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "====================================="
echo "Deployment Completed Successfully"
echo "====================================="
echo ""
echo "Service Details:"
aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].[serviceName,status,runningCount,desiredCount]' \
    --output table

if [ -n "$ALB_DNS" ]; then
    echo ""
    echo "Application URL: http://$ALB_DNS"
    echo "Health Check: http://$ALB_DNS/health"
fi

echo ""
echo "CloudWatch Logs: /ecs/films-app"
echo ""
echo "To view logs:"
echo "aws logs tail /ecs/films-app --follow --region $AWS_REGION"
echo ""
echo "To view running tasks:"
echo "aws ecs list-tasks --cluster $CLUSTER_NAME --service-name $SERVICE_NAME --region $AWS_REGION"
echo ""