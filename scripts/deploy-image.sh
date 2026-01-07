#!/bin/bash
set -e
set -o pipefail

echo "=========================================="
echo "  AWS ECS Fargate Deployment Script"
echo "=========================================="
echo ""

# Configuration
PROJECT_NAME="aspwebformsprojectcont"
TASK_FAMILY="${PROJECT_NAME}-task"
SERVICE_NAME="${PROJECT_NAME}-service"
CONTAINER_NAME="$PROJECT_NAME"
LOG_GROUP="/ecs/$PROJECT_NAME"

# Prompt for AWS configuration
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
export AWS_DEFAULT_REGION="$AWS_REGION"

read -p "Enter ECS cluster name (e.g., my-ecs-cluster): " CLUSTER_NAME

echo ""
echo "=== Network Configuration ==="
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNET_INPUT
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Parse subnets
IFS=',' read -ra SUBNETS <<< "$SUBNET_INPUT"
SUBNET_1="${SUBNETS[0]}"
SUBNET_2="${SUBNETS[1]:-$SUBNET_1}"

echo ""
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): " IMAGE_URI

echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating ECS cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
}

echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "=== Creating Application Load Balancer ==="
    
    # Create ALB
    echo "Creating Application Load Balancer..."
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "${PROJECT_NAME}-alb" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text)
    
    echo "ALB created: $ALB_ARN"
    
    # Get ALB DNS name
    ALB_DNS=$(aws elbv2 describe-load-balancers \
        --load-balancer-arns "$ALB_ARN" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].DNSName' \
        --output text)
    
    # Create Target Group with ip target type (required for Fargate)
    echo "Creating Target Group..."
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "${PROJECT_NAME}-tg" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-enabled \
        --health-check-protocol HTTP \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text)
    
    echo "Target Group created: $TARGET_GROUP_ARN"
    
    # Create Listener
    echo "Creating ALB Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null
    
    echo "Listener created"
    
    USE_LB="true"
else
    echo "Skipping load balancer configuration"
    USE_LB="false"
fi

echo ""
echo "Creating CloudWatch log group..."
aws logs create-log-group --log-group-name "$LOG_GROUP" --region "$AWS_REGION" 2>/dev/null || echo "Log group already exists"

echo ""
echo "Preparing ECS task definition..."

# Create temporary task definition with replacements
TASK_DEF_FILE="ecs/task-definition.json"
TEMP_TASK_DEF="/tmp/task-definition-$$.json"

cat "$TASK_DEF_FILE" | \
    sed "s|{{IMAGE_URI}}|$IMAGE_URI|g" | \
    sed "s|{{AWS_REGION}}|$AWS_REGION|g" | \
    sed "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" > "$TEMP_TASK_DEF"

echo "Registering ECS task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://"$TEMP_TASK_DEF" \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task definition registered: $TASK_DEF_ARN"

rm -f "$TEMP_TASK_DEF"

echo ""
echo "Preparing ECS service definition..."

SERVICE_DEF_FILE="ecs/service-definition.json"
TEMP_SERVICE_DEF="/tmp/service-definition-$$.json"

if [ "$USE_LB" = "true" ]; then
    # Include load balancer configuration
    cat "$SERVICE_DEF_FILE" | \
        sed "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" | \
        sed "s|{{SUBNET_1}}|$SUBNET_1|g" | \
        sed "s|{{SUBNET_2}}|$SUBNET_2|g" | \
        sed "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" | \
        sed "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" > "$TEMP_SERVICE_DEF"
else
    # Remove load balancer configuration
    cat "$SERVICE_DEF_FILE" | \
        sed "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" | \
        sed "s|{{SUBNET_1}}|$SUBNET_1|g" | \
        sed "s|{{SUBNET_2}}|$SUBNET_2|g" | \
        sed "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" | \
        jq 'del(.loadBalancers) | del(.healthCheckGracePeriodSeconds)' > "$TEMP_SERVICE_DEF"
fi

echo ""
echo "Checking if service exists..."
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null || echo "None")

if [ "$SERVICE_EXISTS" = "None" ] || [ -z "$SERVICE_EXISTS" ]; then
    echo "Service does not exist. Creating new ECS service..."
    aws ecs create-service \
        --cli-input-json file://"$TEMP_SERVICE_DEF" \
        --region "$AWS_REGION" >/dev/null
    echo "Service created: $SERVICE_NAME"
else
    echo "Service exists. Updating ECS service..."
    
    NETWORK_CONFIG=$(jq -r '.networkConfiguration' "$TEMP_SERVICE_DEF")
    
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION" >/dev/null
    
    echo "Service updated: $SERVICE_NAME"
fi

rm -f "$TEMP_SERVICE_DEF"

echo ""
echo "Waiting for service to become stable..."
echo "This may take several minutes..."

aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "=========================================="
echo "  Deployment Completed Successfully"
echo "=========================================="
echo ""
echo "Service Details:"
aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].[serviceName,status,runningCount,desiredCount]' \
    --output table

if [ "$USE_LB" = "true" ]; then
    echo ""
    echo "Application URL: http://$ALB_DNS"
fi

echo ""
echo "CloudWatch Logs: $LOG_GROUP"
echo ""
echo "To view logs:"
echo "  aws logs tail $LOG_GROUP --follow --region $AWS_REGION"
echo ""
echo "To check service status:"
echo "  aws ecs describe-services --cluster $CLUSTER_NAME --services $SERVICE_NAME --region $AWS_REGION"
echo ""