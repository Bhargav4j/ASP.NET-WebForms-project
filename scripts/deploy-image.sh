#!/bin/bash
set -e
set -o pipefail

# AWS ECS Fargate Deployment Script for Films Application
echo "================================================"
echo "AWS ECS Fargate Deployment Script"
echo "================================================"
echo ""

# Configuration
PROJECT_NAME="films"
TASK_FAMILY="films-task"
SERVICE_NAME="films-service"
CONTAINER_NAME="films-app"
CONTAINER_PORT=8080

# Prompt for AWS configuration
echo "=== AWS Configuration ==="
read -p "Enter AWS region (e.g., us-east-1): " AWS_REGION
export AWS_DEFAULT_REGION="$AWS_REGION"

# Get AWS Account ID
echo ""
echo "Retrieving AWS Account ID..."
ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)

if [ -z "$ACCOUNT_ID" ]; then
    echo "ERROR: Failed to retrieve AWS Account ID. Please check your AWS credentials."
    exit 1
fi

echo "AWS Account ID: $ACCOUNT_ID"
echo ""

# Prompt for ECS cluster
read -p "Enter ECS cluster name (e.g., films-cluster): " CLUSTER_NAME

# Check if cluster exists, create if not
echo ""
echo "Checking if ECS cluster exists..."
aws ecs describe-clusters --clusters "$CLUSTER_NAME" --region "$AWS_REGION" >/dev/null 2>&1

if [ $? -ne 0 ]; then
    echo "Cluster does not exist. Creating ECS cluster: $CLUSTER_NAME"
    aws ecs create-cluster --cluster-name "$CLUSTER_NAME" --region "$AWS_REGION"
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to create ECS cluster"
        exit 1
    fi
    
    echo "ECS cluster created successfully"
else
    echo "ECS cluster already exists"
fi

echo ""

# Prompt for network configuration
echo "=== Network Configuration ==="
read -p "Enter VPC ID (e.g., vpc-0abc123def456): " VPC_ID
read -p "Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): " SUBNETS_INPUT
read -p "Enter Security Group ID (e.g., sg-0abc123def): " SECURITY_GROUP

# Convert comma-separated subnets to array
IFS=',' read -ra SUBNETS <<< "$SUBNETS_INPUT"
SUBNET_1="${SUBNETS[0]}"
SUBNET_2="${SUBNETS[1]:-$SUBNET_1}"

echo ""
echo "VPC ID: $VPC_ID"
echo "Subnet 1: $SUBNET_1"
echo "Subnet 2: $SUBNET_2"
echo "Security Group: $SECURITY_GROUP"
echo ""

# Prompt for Docker image URI
read -p "Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films:latest): " IMAGE_URI

echo ""
echo "Docker Image: $IMAGE_URI"
echo ""

# Ask about load balancer
read -p "Do you need a load balancer for this service? (y/n): " NEED_LB

LB_CONFIG=""
if [[ "$NEED_LB" =~ ^[Yy]$ ]]; then
    echo ""
    echo "=== Load Balancer Configuration ==="
    echo "Creating Application Load Balancer..."
    
    # Create ALB
    ALB_NAME="${PROJECT_NAME}-alb"
    echo "Creating ALB: $ALB_NAME"
    
    ALB_ARN=$(aws elbv2 create-load-balancer \
        --name "$ALB_NAME" \
        --subnets "$SUBNET_1" "$SUBNET_2" \
        --security-groups "$SECURITY_GROUP" \
        --scheme internet-facing \
        --type application \
        --ip-address-type ipv4 \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].LoadBalancerArn' \
        --output text 2>/dev/null)
    
    if [ -z "$ALB_ARN" ]; then
        echo "WARNING: Failed to create new ALB. It may already exist."
        ALB_ARN=$(aws elbv2 describe-load-balancers \
            --names "$ALB_NAME" \
            --region "$AWS_REGION" \
            --query 'LoadBalancers[0].LoadBalancerArn' \
            --output text 2>/dev/null)
    fi
    
    echo "ALB ARN: $ALB_ARN"
    
    # Create Target Group with target-type ip (required for Fargate)
    TG_NAME="${PROJECT_NAME}-tg"
    echo "Creating Target Group: $TG_NAME (target-type: ip)"
    
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
        --output text 2>/dev/null)
    
    if [ -z "$TARGET_GROUP_ARN" ]; then
        echo "WARNING: Failed to create new Target Group. It may already exist."
        TARGET_GROUP_ARN=$(aws elbv2 describe-target-groups \
            --names "$TG_NAME" \
            --region "$AWS_REGION" \
            --query 'TargetGroups[0].TargetGroupArn' \
            --output text 2>/dev/null)
    fi
    
    echo "Target Group ARN: $TARGET_GROUP_ARN"
    
    # Create Listener
    echo "Creating ALB Listener..."
    aws elbv2 create-listener \
        --load-balancer-arn "$ALB_ARN" \
        --protocol HTTP \
        --port 80 \
        --default-actions Type=forward,TargetGroupArn="$TARGET_GROUP_ARN" \
        --region "$AWS_REGION" >/dev/null 2>&1 || echo "Listener may already exist"
    
    echo "Load Balancer configuration completed"
    echo ""
else
    echo "Skipping load balancer configuration"
    TARGET_GROUP_ARN=""
fi

# Create temporary directory for modified JSON files
TEMP_DIR=$(mktemp -d)
trap "rm -rf $TEMP_DIR" EXIT

echo "=== Preparing Task Definition ==="

# Copy and modify task definition
cp ecs/task-definition.json "$TEMP_DIR/task-definition.json"

# Replace placeholders in task definition
sed -i "s|{{IMAGE_URI}}|$IMAGE_URI|g" "$TEMP_DIR/task-definition.json"
sed -i "s|{{AWS_REGION}}|$AWS_REGION|g" "$TEMP_DIR/task-definition.json"
sed -i "s|{{ACCOUNT_ID}}|$ACCOUNT_ID|g" "$TEMP_DIR/task-definition.json"

echo "Task definition prepared"
echo ""

# Register task definition
echo "=== Registering Task Definition ==="
TASK_DEF_ARN=$(aws ecs register-task-definition \
    --cli-input-json file://$TEMP_DIR/task-definition.json \
    --region "$AWS_REGION" \
    --query 'taskDefinition.taskDefinitionArn' \
    --output text)

if [ -z "$TASK_DEF_ARN" ]; then
    echo "ERROR: Failed to register task definition"
    exit 1
fi

echo "Task definition registered: $TASK_DEF_ARN"
echo ""

# Prepare service definition
echo "=== Preparing Service Definition ==="

cp ecs/service-definition.json "$TEMP_DIR/service-definition.json"

# Replace placeholders
sed -i "s|{{CLUSTER_NAME}}|$CLUSTER_NAME|g" "$TEMP_DIR/service-definition.json"
sed -i "s|{{SUBNET_1}}|$SUBNET_1|g" "$TEMP_DIR/service-definition.json"
sed -i "s|{{SUBNET_2}}|$SUBNET_2|g" "$TEMP_DIR/service-definition.json"
sed -i "s|{{SECURITY_GROUP}}|$SECURITY_GROUP|g" "$TEMP_DIR/service-definition.json"

if [ -n "$TARGET_GROUP_ARN" ]; then
    sed -i "s|{{TARGET_GROUP_ARN}}|$TARGET_GROUP_ARN|g" "$TEMP_DIR/service-definition.json"
else
    # Remove loadBalancers section if no load balancer
    sed -i '/{TARGET_GROUP_ARN}/d' "$TEMP_DIR/service-definition.json"
    sed -i '/"loadBalancers":/,/],/d' "$TEMP_DIR/service-definition.json"
    sed -i '/"healthCheckGracePeriodSeconds":/d' "$TEMP_DIR/service-definition.json"
fi

echo "Service definition prepared"
echo ""

# Check if service exists
echo "=== Checking Service Status ==="
SERVICE_EXISTS=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].serviceName' \
    --output text 2>/dev/null)

if [ "$SERVICE_EXISTS" == "$SERVICE_NAME" ]; then
    echo "Service exists. Updating service..."
    
    aws ecs update-service \
        --cluster "$CLUSTER_NAME" \
        --service "$SERVICE_NAME" \
        --task-definition "$TASK_DEF_ARN" \
        --force-new-deployment \
        --region "$AWS_REGION" >/dev/null
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to update service"
        exit 1
    fi
    
    echo "Service updated successfully"
else
    echo "Service does not exist. Creating new service..."
    
    aws ecs create-service \
        --cli-input-json file://$TEMP_DIR/service-definition.json \
        --region "$AWS_REGION" >/dev/null
    
    if [ $? -ne 0 ]; then
        echo "ERROR: Failed to create service"
        exit 1
    fi
    
    echo "Service created successfully"
fi

echo ""

# Wait for service to stabilize
echo "=== Waiting for Service Stability ==="
echo "This may take a few minutes..."
echo ""

aws ecs wait services-stable \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION"

if [ $? -ne 0 ]; then
    echo "WARNING: Service did not stabilize within the expected time"
    echo "Check the ECS console for more details"
else
    echo "Service is stable"
fi

echo ""

# Verify deployment
echo "=== Deployment Verification ==="

RUNNING_COUNT=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].runningCount' \
    --output text)

DESIRED_COUNT=$(aws ecs describe-services \
    --cluster "$CLUSTER_NAME" \
    --services "$SERVICE_NAME" \
    --region "$AWS_REGION" \
    --query 'services[0].desiredCount' \
    --output text)

echo "Running Tasks: $RUNNING_COUNT / $DESIRED_COUNT"
echo ""

if [ -n "$ALB_ARN" ]; then
    ALB_DNS=$(aws elbv2 describe-load-balancers \
        --load-balancer-arns "$ALB_ARN" \
        --region "$AWS_REGION" \
        --query 'LoadBalancers[0].DNSName' \
        --output text)
    
    echo "Application URL: http://$ALB_DNS"
    echo "Health Check: http://$ALB_DNS/health"
    echo ""
fi

echo "CloudWatch Logs: /ecs/$PROJECT_NAME-app"
echo "View logs: aws logs tail /ecs/$PROJECT_NAME-app --follow --region $AWS_REGION"
echo ""

echo "================================================"
echo "Deployment completed successfully!"
echo "================================================"
echo ""
echo "Next steps:"
echo "1. Verify the service is running: aws ecs describe-services --cluster $CLUSTER_NAME --services $SERVICE_NAME --region $AWS_REGION"
echo "2. Check task status: aws ecs list-tasks --cluster $CLUSTER_NAME --service-name $SERVICE_NAME --region $AWS_REGION"
echo "3. View logs: aws logs tail /ecs/$PROJECT_NAME-app --follow --region $AWS_REGION"

if [ -n "$ALB_DNS" ]; then
    echo "4. Access application: http://$ALB_DNS"
fi

echo ""