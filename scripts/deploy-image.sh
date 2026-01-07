#!/bin/bash
set -e
set -o pipefail

echo "====================================="
echo "AWS ECS Fargate Deployment Script"
echo "====================================="
echo ""

# Project configuration
PROJECT_NAME="aspnetwebfprojcontain06"
TASK_FAMILY="${PROJECT_NAME}-task"
SERVICE_NAME="${PROJECT_NAME}-service"
CONTAINER_NAME="${PROJECT_NAME}"
CONTAINER_PORT=8080

# Prompt for AWS configuration
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
read -p "Enter ECS cluster name (e.g., my-ecs-cluster): " CLUSTER_NAME

echo ""
echo "=== Network Configuration ==="
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNET_IDS
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

echo ""
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): " IMAGE_URI

# Convert subnet IDs to array
IFS=',' read -ra SUBNETS <<< "$SUBNET_IDS"
SUBNET_1="${SUBNETS[0]}"
SUBNET_2="${SUBNETS[1]:-$SUBNET_1}"

echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Creating ECS cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
}

echo ""
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "=== Creating Application Load Balancer ==="
    
    LB_NAME="${PROJECT_NAME}-alb"
    TG_NAME="${PROJECT_NAME}-tg"
    
    echo "Creating Application Load Balancer: $LB_NAME"
    LB_ARN=$(aws elbv2 create-load-balancer \
        --name "$LB_NAME" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null || aws elbv2 describe-load-balancers --names "$LB_NAME" --region "$AWS_REGION" --query 'LoadBalancers[0].LoadBalancerArn' --output text)
    
    echo "Load Balancer ARN: $LB_ARN"
    
    echo "Creating Target Group: $TG_NAME"
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "$TG_NAME" \
        --protocol HTTP \
        --port "$CONTAINER_PORT" \
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
        --output text 2>/dev/null || aws elbv2 describe-target-groups --names "$TG_NAME" --region "$AWS_REGION" --query 'TargetGroups[0].TargetGroupArn' --output text)
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    echo "Creating listener on port 80..."
    aws elbv2 create-listener \
        --load-balancer-arn "$LB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null 2>&1 || echo "Listener already exists"
    
    LB_DNS=$(aws elbv2 describe-load-balancers --load-balancer-arns "$LB_ARN" --region "$AWS_REGION" --query 'LoadBalancers[0].DNSName' --output text)
    echo "Load Balancer DNS: $LB_DNS"
else
    TARGET_GROUP_ARN=""
fi

echo ""
echo "=== Preparing Task Definition ==="
cp ecs/task-definition.json ecs/task-definition-resolved.json

# Replace placeholders in task definition
sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" ecs/task-definition-resolved.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" ecs/task-definition-resolved.json
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" ecs/task-definition-resolved.json

echo "Registering task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://ecs/task-definition-resolved.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task Definition ARN: $TASK_DEF_ARN"

echo ""
echo "=== Creating CloudWatch Log Group ==="
LOG_GROUP="/ecs/${PROJECT_NAME}"
aws logs create-log-group --log-group-name "$LOG_GROUP" --region "$AWS_REGION" 2>/dev/null || echo "Log group already exists"

echo ""
echo "=== Preparing Service Definition ==="
cp ecs/service-definition.json ecs/service-definition-resolved.json

# Replace placeholders in service definition
sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" ecs/service-definition-resolved.json
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" ecs/service-definition-resolved.json
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" ecs/service-definition-resolved.json
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" ecs/service-definition-resolved.json

if [ -z "$TARGET_GROUP_ARN" ]; then
    # Remove loadBalancers section if no load balancer
    jq 'del(.loadBalancers) | del(.healthCheckGracePeriodSeconds)' ecs/service-definition-resolved.json > ecs/service-definition-temp.json
    mv ecs/service-definition-temp.json ecs/service-definition-resolved.json
else
    sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" ecs/service-definition-resolved.json
fi

echo ""
echo "=== Deploying Service ==="

# Check if service exists
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null)

if [ "$SERVICE_EXISTS" = "$SERVICE_NAME" ]; then
    echo "Updating existing service: $SERVICE_NAME"
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --region "$AWS_REGION" \
        --force-new-deployment
else
    echo "Creating new service: $SERVICE_NAME"
    aws ecs create-service \
        --cli-input-json file://ecs/service-definition-resolved.json \
        --region "$AWS_REGION"
fi

echo ""
echo "Waiting for service to become stable..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "====================================="
echo "Deployment completed successfully!"
echo "====================================="
echo ""
echo "Service Details:"
aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].{Name:serviceName,Status:status,Running:runningCount,Desired:desiredCount}'

if [ ! -z "$LB_DNS" ]; then
    echo ""
    echo "Application URL: http://$LB_DNS"
fi

echo ""
echo "CloudWatch Logs: $LOG_GROUP"
echo ""
echo "To view logs, run:"
echo "aws logs tail $LOG_GROUP --follow --region $AWS_REGION"

# Cleanup temporary files
rm -f ecs/task-definition-resolved.json ecs/service-definition-resolved.json