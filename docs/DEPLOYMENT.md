# ASP.NET Core Application - AWS ECS Fargate Deployment Guide

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Overview](#project-overview)
3. [Local Development Setup](#local-development-setup)
4. [Docker Deployment](#docker-deployment)
5. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
6. [ECS Fargate Setup](#ecs-fargate-setup)
7. [ECS Task Definition Explained](#ecs-task-definition-explained)
8. [ECS Service Configuration](#ecs-service-configuration)
9. [ECS Fargate Deployment Walkthrough](#ecs-fargate-deployment-walkthrough)
10. [ECS-Specific Troubleshooting](#ecs-specific-troubleshooting)
11. [ECS Fargate Scaling and Management](#ecs-fargate-scaling-and-management)
12. [Configuration Management](#configuration-management)
13. [Security Considerations](#security-considerations)
14. [Monitoring and Observability](#monitoring-and-observability)

---

## Prerequisites

### Required Tools

- **.NET SDK 8.0 or later**: [Download](https://dotnet.microsoft.com/download)
- **Docker Desktop**: [Download](https://www.docker.com/products/docker-desktop)
- **AWS CLI v2**: [Download](https://aws.amazon.com/cli/)
- **Git**: [Download](https://git-scm.com/downloads)
- **jq** (for JSON parsing in scripts): [Download](https://stedolan.github.io/jq/)

### AWS Account Requirements

- Active AWS account with appropriate permissions
- AWS IAM user with programmatic access
- IAM permissions for ECS, ECR, CloudWatch, VPC, and IAM role management

### Knowledge Prerequisites

- Basic understanding of .NET and ASP.NET Core
- Familiarity with Docker containerization
- Basic AWS services knowledge (ECS, ECR, VPC)
- Command-line interface proficiency

---

## Project Overview

### Application Details

- **Project Name**: aspwebformsprojectcont
- **Technology Stack**: ASP.NET Core 8.0
- **Application Type**: Web Application
- **Framework**: .NET 8.0
- **Build Tool**: dotnet CLI
- **Application Port**: 8080
- **Health Endpoint**: /health

### Architecture

This application uses a multi-stage Docker build process:

1. **Build Stage**: Uses `mcr.microsoft.com/dotnet/sdk:8.0` for compilation
2. **Runtime Stage**: Uses `mcr.microsoft.com/dotnet/runtime:8.0` for deployment
3. **Deployment**: AWS ECS Fargate with awsvpc networking

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd aspwebformsprojectcont
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Application

```bash
dotnet build -c Release
```

### 4. Run Locally

```bash
dotnet run --project <project-file>.csproj
```

The application will be available at `http://localhost:8080`.

### 5. Verify Health Endpoint

```bash
curl http://localhost:8080/health
```

---

## Docker Deployment

### Build Docker Image Locally

```bash
docker build -t aspwebformsprojectcont:latest .
```

### Run Container Locally

```bash
docker run -d -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:8080 \
  --name aspwebformsprojectcont \
  aspwebformsprojectcont:latest
```

### Verify Container Health

```bash
docker ps
curl http://localhost:8080/health
```

### Using Docker Compose

```bash
docker-compose up -d
```

### View Logs

```bash
docker logs -f aspwebformsprojectcont
```

### Stop Container

```bash
docker stop aspwebformsprojectcont
docker rm aspwebformsprojectcont
```

---

## AWS ECS Fargate Prerequisites

### 1. Configure AWS CLI

```bash
aws configure
```

Provide:
- AWS Access Key ID
- AWS Secret Access Key
- Default region (e.g., us-east-1)
- Default output format (json)

### 2. Create VPC and Networking Components

If you don't have an existing VPC:

```bash
# Create VPC
VPC_ID=$(aws ec2 create-vpc \
  --cidr-block 10.0.0.0/16 \
  --query 'Vpc.VpcId' \
  --output text)

# Create Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
  --query 'InternetGateway.InternetGatewayId' \
  --output text)

# Attach Internet Gateway to VPC
aws ec2 attach-internet-gateway \
  --vpc-id $VPC_ID \
  --internet-gateway-id $IGW_ID

# Create Subnets (at least 2 in different AZs)
SUBNET_1=$(aws ec2 create-subnet \
  --vpc-id $VPC_ID \
  --cidr-block 10.0.1.0/24 \
  --availability-zone us-east-1a \
  --query 'Subnet.SubnetId' \
  --output text)

SUBNET_2=$(aws ec2 create-subnet \
  --vpc-id $VPC_ID \
  --cidr-block 10.0.2.0/24 \
  --availability-zone us-east-1b \
  --query 'Subnet.SubnetId' \
  --output text)

# Create Route Table
ROUTE_TABLE_ID=$(aws ec2 create-route-table \
  --vpc-id $VPC_ID \
  --query 'RouteTable.RouteTableId' \
  --output text)

# Create Route to Internet Gateway
aws ec2 create-route \
  --route-table-id $ROUTE_TABLE_ID \
  --destination-cidr-block 0.0.0.0/0 \
  --gateway-id $IGW_ID

# Associate Subnets with Route Table
aws ec2 associate-route-table \
  --subnet-id $SUBNET_1 \
  --route-table-id $ROUTE_TABLE_ID

aws ec2 associate-route-table \
  --subnet-id $SUBNET_2 \
  --route-table-id $ROUTE_TABLE_ID

# Create Security Group
SECURITY_GROUP_ID=$(aws ec2 create-security-group \
  --group-name aspwebformsprojectcont-sg \
  --description "Security group for aspwebformsprojectcont ECS tasks" \
  --vpc-id $VPC_ID \
  --query 'GroupId' \
  --output text)

# Allow inbound traffic on port 8080
aws ec2 authorize-security-group-ingress \
  --group-id $SECURITY_GROUP_ID \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0

# Allow outbound traffic
aws ec2 authorize-security-group-egress \
  --group-id $SECURITY_GROUP_ID \
  --protocol -1 \
  --cidr 0.0.0.0/0
```

### 3. Create IAM Roles

#### ECS Task Execution Role

```bash
# Create trust policy document
cat > ecs-task-execution-trust-policy.json <<EOF
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Principal": {
        "Service": "ecs-tasks.amazonaws.com"
      },
      "Action": "sts:AssumeRole"
    }
  ]
}
EOF

# Create role
aws iam create-role \
  --role-name ecsTaskExecutionRole \
  --assume-role-policy-document file://ecs-task-execution-trust-policy.json

# Attach managed policy
aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (Optional - for application permissions)

```bash
# Create task role
aws iam create-role \
  --role-name ecsTaskRole \
  --assume-role-policy-document file://ecs-task-execution-trust-policy.json

# Attach policies as needed (e.g., S3, DynamoDB access)
```

### 4. Create CloudWatch Log Group

```bash
aws logs create-log-group \
  --log-group-name /ecs/aspwebformsprojectcont
```

---

## ECS Fargate Setup

### 1. Create ECS Cluster

```bash
aws ecs create-cluster --cluster-name aspwebformsprojectcont-cluster
```

### 2. Create ECR Repository

```bash
aws ecr create-repository \
  --repository-name aspwebformsprojectcont \
  --region us-east-1
```

### 3. Authenticate Docker to ECR

```bash
aws ecr get-login-password --region us-east-1 | \
  docker login --username AWS --password-stdin \
  <account-id>.dkr.ecr.us-east-1.amazonaws.com
```

---

## ECS Task Definition Explained

### Key Components

#### 1. Launch Type Configuration

```json
"requiresCompatibilities": ["FARGATE"],
"networkMode": "awsvpc"
```

- **requiresCompatibilities**: Specifies Fargate launch type
- **networkMode**: awsvpc is required for Fargate (provides each task with its own ENI)

#### 2. CPU and Memory

```json
"cpu": "512",
"memory": "1024"
```

**Valid Fargate CPU/Memory Combinations:**

| CPU (vCPU) | Memory (MB) |
|------------|-------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024-4096 (increments of 1024) |
| 1024 (1)   | 2048-8192 (increments of 1024) |
| 2048 (2)   | 4096-16384 (increments of 1024) |
| 4096 (4)   | 8192-30720 (increments of 1024) |

#### 3. Execution Role

```json
"executionRoleArn": "arn:aws:iam::<account-id>:role/ecsTaskExecutionRole"
```

Allows ECS to:
- Pull container images from ECR
- Write logs to CloudWatch
- Retrieve secrets from Secrets Manager/Parameter Store

#### 4. Container Definitions

```json
"containerDefinitions": [
  {
    "name": "aspwebformsprojectcont",
    "image": "<account-id>.dkr.ecr.us-east-1.amazonaws.com/aspwebformsprojectcont:latest",
    "essential": true,
    "portMappings": [
      {
        "containerPort": 8080,
        "protocol": "tcp"
      }
    ],
    "environment": [
      {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"},
      {"name": "ASPNETCORE_URLS", "value": "http://+:8080"}
    ],
    "logConfiguration": {
      "logDriver": "awslogs",
      "options": {
        "awslogs-group": "/ecs/aspwebformsprojectcont",
        "awslogs-region": "us-east-1",
        "awslogs-stream-prefix": "ecs"
      }
    }
  }
]
```

---

## ECS Service Configuration

### Key Components

#### 1. Launch Type and Networking

```json
"launchType": "FARGATE",
"networkConfiguration": {
  "awsvpcConfiguration": {
    "subnets": ["subnet-xxx", "subnet-yyy"],
    "securityGroups": ["sg-xxx"],
    "assignPublicIp": "ENABLED"
  }
}
```

- **subnets**: At least 2 subnets in different AZs for high availability
- **securityGroups**: Controls inbound/outbound traffic
- **assignPublicIp**: ENABLED for tasks to reach internet (for pulling images, etc.)

#### 2. Deployment Configuration

```json
"deploymentConfiguration": {
  "maximumPercent": 200,
  "minimumHealthyPercent": 50,
  "deploymentCircuitBreaker": {
    "enable": true,
    "rollback": true
  }
}
```

- **maximumPercent**: Maximum percentage of desired tasks during deployment
- **minimumHealthyPercent**: Minimum percentage of healthy tasks during deployment
- **deploymentCircuitBreaker**: Automatically rolls back failed deployments

#### 3. Load Balancer Integration

```json
"loadBalancers": [
  {
    "targetGroupArn": "arn:aws:elasticloadbalancing:...",
    "containerName": "aspwebformsprojectcont",
    "containerPort": 8080
  }
],
"healthCheckGracePeriodSeconds": 300
```

- **targetGroupArn**: ARN of ALB/NLB target group
- **healthCheckGracePeriodSeconds**: Time to wait before health checks start

---

## ECS Fargate Deployment Walkthrough

### Step 1: Build and Push Docker Image

#### Linux/macOS

```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

#### Windows

```cmd
scripts\build-push.bat
```

Follow the prompts to:
1. Select registry type (AWS ECR recommended)
2. Enter AWS region and account details
3. Script will build and push the image

### Step 2: Deploy to ECS Fargate

#### Linux/macOS

```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

#### Windows

```cmd
scripts\deploy-image.bat
```

Provide the following information when prompted:
- AWS region
- ECS cluster name
- VPC ID
- Subnet IDs (comma-separated)
- Security group ID
- Docker image URI from Step 1
- Load balancer requirement (y/n)

The script will:
1. Create/verify ECS cluster
2. Create Application Load Balancer (if requested)
3. Create Target Group with health checks
4. Register task definition
5. Create or update ECS service
6. Wait for service stability
7. Display deployment status and URLs

### Step 3: Verify Deployment

```bash
# Check service status
aws ecs describe-services \
  --cluster aspwebformsprojectcont-cluster \
  --services aspwebformsprojectcont-service

# List running tasks
aws ecs list-tasks \
  --cluster aspwebformsprojectcont-cluster \
  --service-name aspwebformsprojectcont-service

# View task details
aws ecs describe-tasks \
  --cluster aspwebformsprojectcont-cluster \
  --tasks <task-arn>
```

### Step 4: Access Application

If using load balancer:
```bash
# Get ALB DNS name
ALB_DNS=$(aws elbv2 describe-load-balancers \
  --names aspwebformsprojectcont-alb \
  --query 'LoadBalancers[0].DNSName' \
  --output text)

echo "Application URL: http://$ALB_DNS"

# Test health endpoint
curl http://$ALB_DNS/health
```

---

## ECS-Specific Troubleshooting

### Task Fails to Start

#### Check Task Logs

```bash
aws logs tail /ecs/aspwebformsprojectcont --follow
```

#### Common Issues

1. **Invalid CPU/Memory Combination**
   - Error: "Invalid CPU or memory value specified"
   - Solution: Use valid Fargate CPU/memory combinations (see table above)

2. **Image Pull Errors**
   - Error: "CannotPullContainerError"
   - Solution:
     - Verify ECR repository exists
     - Check execution role has ECR permissions
     - Verify image URI is correct

3. **Task Execution Role Issues**
   - Error: "Unable to assume execution role"
   - Solution:
     - Verify ecsTaskExecutionRole exists
     - Check role has correct trust policy
     - Ensure role has AmazonECSTaskExecutionRolePolicy attached

### Network Issues

#### Task Cannot Reach Internet

```bash
# Verify subnet has internet gateway route
aws ec2 describe-route-tables \
  --filters "Name=association.subnet-id,Values=<subnet-id>"

# Check security group rules
aws ec2 describe-security-groups \
  --group-ids <security-group-id>
```

#### Task Not Accessible from Load Balancer

```bash
# Verify target group health
aws elbv2 describe-target-health \
  --target-group-arn <target-group-arn>

# Check security group allows traffic on port 8080
# Verify health check path is correct (/health)
```

### Application Errors

#### View Application Logs

```bash
# Stream logs in real-time
aws logs tail /ecs/aspwebformsprojectcont --follow

# View specific time range
aws logs tail /ecs/aspwebformsprojectcont \
  --since 1h \
  --format short
```

#### Connect to Running Task

```bash
# Enable ECS Exec (one-time setup)
aws ecs update-service \
  --cluster aspwebformsprojectcont-cluster \
  --service aspwebformsprojectcont-service \
  --enable-execute-command

# Connect to task
TASK_ARN=$(aws ecs list-tasks \
  --cluster aspwebformsprojectcont-cluster \
  --service-name aspwebformsprojectcont-service \
  --query 'taskArns[0]' \
  --output text)

aws ecs execute-command \
  --cluster aspwebformsprojectcont-cluster \
  --task $TASK_ARN \
  --container aspwebformsprojectcont \
  --interactive \
  --command "/bin/bash"
```

---

## ECS Fargate Scaling and Management

### Manual Scaling

```bash
# Scale service to 4 tasks
aws ecs update-service \
  --cluster aspwebformsprojectcont-cluster \
  --service aspwebformsprojectcont-service \
  --desired-count 4
```

### Auto Scaling

#### Register Scalable Target

```bash
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --scalable-dimension ecs:service:DesiredCount \
  --resource-id service/aspwebformsprojectcont-cluster/aspwebformsprojectcont-service \
  --min-capacity 2 \
  --max-capacity 10
```

#### Create Scaling Policy (CPU-based)

```bash
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --scalable-dimension ecs:service:DesiredCount \
  --resource-id service/aspwebformsprojectcont-cluster/aspwebformsprojectcont-service \
  --policy-name cpu-scaling-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json
```

scaling-policy.json:
```json
{
  "TargetValue": 70.0,
  "PredefinedMetricSpecification": {
    "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
  },
  "ScaleInCooldown": 300,
  "ScaleOutCooldown": 60
}
```

### Blue/Green Deployments

```bash
# Update task definition to new version
NEW_TASK_DEF=$(aws ecs register-task-definition \
  --cli-input-json file://ecs/task-definition.json \
  --query 'taskDefinition.taskDefinitionArn' \
  --output text)

# Update service with new task definition
aws ecs update-service \
  --cluster aspwebformsprojectcont-cluster \
  --service aspwebformsprojectcont-service \
  --task-definition $NEW_TASK_DEF

# Monitor deployment
aws ecs wait services-stable \
  --cluster aspwebformsprojectcont-cluster \
  --services aspwebformsprojectcont-service
```

---

## Configuration Management

### Environment Variables

Update task definition with environment variables:

```json
"environment": [
  {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"},
  {"name": "ConnectionStrings__DefaultConnection", "value": "Server=..."}
]
```

### Using AWS Secrets Manager

```bash
# Create secret
aws secretsmanager create-secret \
  --name aspwebformsprojectcont/db-password \
  --secret-string "mysecretpassword"

# Reference in task definition
"secrets": [
  {
    "name": "DB_PASSWORD",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:aspwebformsprojectcont/db-password"
  }
]
```

### Using AWS Systems Manager Parameter Store

```bash
# Create parameter
aws ssm put-parameter \
  --name /aspwebformsprojectcont/config/api-key \
  --value "myapikey" \
  --type SecureString

# Reference in task definition
"secrets": [
  {
    "name": "API_KEY",
    "valueFrom": "arn:aws:ssm:us-east-1:123456789:parameter/aspwebformsprojectcont/config/api-key"
  }
]
```

---

## Security Considerations

### 1. IAM Roles and Permissions

- Use separate execution and task roles
- Follow principle of least privilege
- Regularly audit IAM policies

### 2. Network Security

- Use private subnets for tasks when possible
- Restrict security group rules to minimum required ports
- Use AWS PrivateLink for ECR and CloudWatch access

### 3. Secrets Management

- Never hardcode secrets in task definitions
- Use AWS Secrets Manager or Parameter Store
- Rotate secrets regularly

### 4. Container Security

- Use official Microsoft base images
- Run containers as non-root user (already configured)
- Regularly update base images for security patches
- Scan images for vulnerabilities

```bash
# Scan image with ECR
aws ecr start-image-scan \
  --repository-name aspwebformsprojectcont \
  --image-id imageTag=latest

# Get scan results
aws ecr describe-image-scan-findings \
  --repository-name aspwebformsprojectcont \
  --image-id imageTag=latest
```

### 5. HTTPS/TLS

- Use Application Load Balancer with SSL/TLS certificate
- Terminate TLS at load balancer
- Use AWS Certificate Manager for certificates

```bash
# Request certificate
aws acm request-certificate \
  --domain-name app.example.com \
  --validation-method DNS

# Add HTTPS listener to ALB
aws elbv2 create-listener \
  --load-balancer-arn <alb-arn> \
  --protocol HTTPS \
  --port 443 \
  --certificates CertificateArn=<cert-arn> \
  --default-actions Type=forward,TargetGroupArn=<tg-arn>
```

---

## Monitoring and Observability

### CloudWatch Metrics

```bash
# View CPU utilization
aws cloudwatch get-metric-statistics \
  --namespace AWS/ECS \
  --metric-name CPUUtilization \
  --dimensions Name=ServiceName,Value=aspwebformsprojectcont-service Name=ClusterName,Value=aspwebformsprojectcont-cluster \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-01T23:59:59Z \
  --period 3600 \
  --statistics Average

# View memory utilization
aws cloudwatch get-metric-statistics \
  --namespace AWS/ECS \
  --metric-name MemoryUtilization \
  --dimensions Name=ServiceName,Value=aspwebformsprojectcont-service Name=ClusterName,Value=aspwebformsprojectcont-cluster \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-01T23:59:59Z \
  --period 3600 \
  --statistics Average
```

### CloudWatch Logs Insights

```bash
# Query logs for errors
aws logs start-query \
  --log-group-name /ecs/aspwebformsprojectcont \
  --start-time $(date -u -d '1 hour ago' +%s) \
  --end-time $(date -u +%s) \
  --query-string 'fields @timestamp, @message | filter @message like /ERROR/ | sort @timestamp desc | limit 20'
```

### Application Insights (Optional)

For advanced monitoring, integrate Application Insights:

1. Add Application Insights NuGet package
2. Configure in appsettings.json
3. Set instrumentation key as environment variable in task definition

### CloudWatch Alarms

```bash
# Create alarm for high CPU
aws cloudwatch put-metric-alarm \
  --alarm-name aspwebformsprojectcont-high-cpu \
  --alarm-description "Alert when CPU exceeds 80%" \
  --metric-name CPUUtilization \
  --namespace AWS/ECS \
  --statistic Average \
  --period 300 \
  --evaluation-periods 2 \
  --threshold 80 \
  --comparison-operator GreaterThanThreshold \
  --dimensions Name=ServiceName,Value=aspwebformsprojectcont-service Name=ClusterName,Value=aspwebformsprojectcont-cluster
```

---

## Additional Resources

- [AWS ECS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/AWS_Fargate.html)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)
- [AWS Well-Architected Framework](https://aws.amazon.com/architecture/well-architected/)

---

## Support and Troubleshooting

For issues or questions:

1. Check CloudWatch logs: `/ecs/aspwebformsprojectcont`
2. Review ECS service events
3. Verify IAM permissions
4. Check AWS service health dashboard
5. Consult AWS support or community forums

---

**Last Updated**: 2026-01-07  
**Version**: 1.0.0  
**Deployment Platform**: AWS ECS Fargate
