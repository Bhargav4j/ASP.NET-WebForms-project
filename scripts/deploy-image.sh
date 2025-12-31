#!/bin/bash
set -e
set -o pipefail

echo "============================================"
echo "  Films Web - AWS ECS Fargate Deployment"
echo "============================================"
echo ""

# Configuration
PROJECT_NAME="films-web"
TASK_FAMILY="films-web-task"
SERVICE_NAME="films-web-service"

# Prompt for AWS configuration
echo "=== AWS Configuration ==="
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
export AWS_DEFAULT_REGION="$AWS_REGION"

echo ""
echo "=== ECS Cluster Configuration ==="
read -p "Enter ECS cluster name (e.g., films-cluster): " CLUSTER_NAME

echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1 || {
    echo "Cluster does not exist. Creating cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
    echo "Cluster created successfully."
}

echo ""
echo "=== Network Configuration ==="
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNET_IDS
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Split subnets
IFS=',' read -ra SUBNETS <<< "$SUBNET_IDS"
SUBNET_1="${SUBNETS[0]}"
SUBNET_2="${SUBNETS[1]:-$SUBNET_1}"

echo ""
echo "=== Docker Image Configuration ==="
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films:latest): " IMAGE_URI

echo ""
echo "=== Database Configuration ==="
read -p "Enter database server (e.g., films-db.abc123.us-east-1.rds.amazonaws.com): " DB_SERVER
read -p "Enter database name (default: FilmsDb): " DB_NAME
DB_NAME=${DB_NAME:-FilmsDb}
read -p "Enter database user (default: admin): " DB_USER
DB_USER=${DB_USER:-admin}
read -sp "Enter database password: " DB_PASSWORD
echo ""

echo ""
echo "=== Load Balancer Configuration ==="
read -p "Do you need a load balancer for this service? (y/n): " NEEDS_LB

if [[ "$NEEDS_LB" =~ ^[Yy]$ ]]; then
    echo "Creating Application Load Balancer..."
    
    # Create ALB
    ALB_NAME="films-web-alb"
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "$ALB_NAME" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null || aws elbv2 describe-load-balancers \
        --names "$ALB_NAME" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text)
    
    echo "Load Balancer ARN: $ALB_ARN"
    
    # Create Target Group with target-type ip (required for Fargate)
    TG_NAME="films-web-tg"
    TARGET_GROUP_ARN=$(aws elbv2 create-target-group \
        --name "$TG_NAME" \
        --protocol HTTP \
        --port 8080 \
        --vpc-id "$VPC_ID" \
        --target-type ip \
        --health-check-path "/health" \
        --health-check-interval-seconds 30 \
        --health-check-timeout-seconds 5 \
        --healthy-threshold-count 2 \
        --unhealthy-threshold-count 3 \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text 2>/dev/null || aws elbv2 describe-target-groups \
        --names "$TG_NAME" \
        --region "$AWS_REGION" \
        --query 'TargetGroups[0].TargetGroupArn' \
        --output text)
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    # Create Listener
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null 2>&1 || echo "Listener already exists"
    
    # Get ALB DNS name
    ALB_DNS=$(aws elbv2 describe-load-balancers \
        --load-balancer-arns "$ALB_ARN" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].DNSName' \
        --output text)
    
    echo "Load Balancer DNS: $ALB_DNS"
else
    TARGET_GROUP_ARN=""
    echo "Skipping load balancer configuration."
fi

echo ""
echo "Getting AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
echo "Account ID: $ACCOUNT_ID"

echo ""
echo "Creating CloudWatch log group..."
aws logs create-log-group --log-group-name "/ecs/$PROJECT_NAME" --region "$AWS_REGION" 2>/dev/null || echo "Log group already exists"

echo ""
echo "Preparing task definition..."
cp ecs/task-definition.json /tmp/task-definition.json

# Replace placeholders
sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" /tmp/task-definition.json
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" /tmp/task-definition.json
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" /tmp/task-definition.json
sed -i "s|{{DB_SERVER}}|$DB_SERVER|g" /tmp/task-definition.json
sed -i "s|{{DB_NAME}}|$DB_NAME|g" /tmp/task-definition.json
sed -i "s|{{DB_USER}}|$DB_USER|g" /tmp/task-definition.json
sed -i "s|{{DB_PASSWORD}}|$DB_PASSWORD|g" /tmp/task-definition.json

echo "Registering task definition..."
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file:///tmp/task-definition.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

echo "Task Definition ARN: $TASK_DEF_ARN"

echo ""
echo "Preparing service definition..."
cp ecs/service-definition.json /tmp/service-definition.json

# Replace placeholders
sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" /tmp/service-definition.json
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" /tmp/service-definition.json
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" /tmp/service-definition.json
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" /tmp/service-definition.json

if [[ "$NEEDS_LB" =~ ^[Yy]$ ]]; then
    sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" /tmp/service-definition.json
else
    # Remove loadBalancers section if no LB
    sed -i '/"loadBalancers"/,/],/d' /tmp/service-definition.json
    sed -i '/"healthCheckGracePeriodSeconds"/d' /tmp/service-definition.json
fi

echo ""
echo "Checking if service exists..."
EXISTING_SERVICE=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null)

if [ "$EXISTING_SERVICE" = "$SERVICE_NAME" ]; then
    echo "Service exists. Updating service..."
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION"
else
    echo "Service does not exist. Creating service..."
    aws ecs create-service \
        --cli-input-json file:///tmp/service-definition.json \
        --region "$AWS_REGION"
fi

echo ""
echo "Waiting for service to become stable..."
aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

echo ""
echo "============================================"
echo "  DEPLOYMENT SUCCESSFUL!"
echo "============================================"
echo "Cluster: $CLUSTER_NAME"
echo "Service: $SERVICE_NAME"
echo "Task Definition: $TASK_DEF_ARN"

if [[ "$NEEDS_LB" =~ ^[Yy]$ ]]; then
    echo "Load Balancer URL: http://$ALB_DNS"
    echo ""
    echo "Access your application at: http://$ALB_DNS"
fi

echo ""
echo "CloudWatch Logs: /ecs/$PROJECT_NAME"
echo ""
echo "To view logs:"
echo "  aws logs tail /ecs/$PROJECT_NAME --follow --region $AWS_REGION"
echo ""
echo "To check service status:"
echo "  aws ecs describe-services --cluster $CLUSTER_NAME --services $SERVICE_NAME --region $AWS_REGION"
echo "============================================"