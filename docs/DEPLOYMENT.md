# AWS ECS Fargate Deployment Guide for Films Application

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [AWS Infrastructure Setup](#aws-infrastructure-setup)
3. [Local Development Setup](#local-development-setup)
4. [Docker Image Build and Push](#docker-image-build-and-push)
5. [ECS Fargate Deployment](#ecs-fargate-deployment)
6. [Configuration Management](#configuration-management)
7. [Monitoring and Logging](#monitoring-and-logging)
8. [Troubleshooting](#troubleshooting)
9. [Scaling and Performance](#scaling-and-performance)
10. [Security Best Practices](#security-best-practices)

---

## Prerequisites

### Required Tools

- **Docker Desktop** (v20.10+)
  - Download: https://www.docker.com/products/docker-desktop
  - Verify: `docker --version`

- **AWS CLI** (v2.x)
  - Installation: https://aws.amazon.com/cli/
  - Verify: `aws --version`
  - Configure: `aws configure`

- **.NET SDK** (v8.0)
  - Download: https://dotnet.microsoft.com/download
  - Verify: `dotnet --version`

- **Git**
  - Download: https://git-scm.com/downloads
  - Verify: `git --version`

### AWS Account Requirements

- Active AWS account with appropriate permissions
- IAM user with the following permissions:
  - ECS Full Access
  - ECR Full Access
  - VPC Read Access
  - IAM Role Creation (for ECS Task Execution Role)
  - CloudWatch Logs Full Access
  - Application Load Balancer Management

### AWS Infrastructure Components

- **VPC** with at least 2 subnets in different availability zones
- **Security Group** configured for HTTP/HTTPS traffic
- **IAM Roles**:
  - `ecsTaskExecutionRole` - Allows ECS to pull images and write logs
  - `ecsTaskRole` - Grants permissions to the application (optional)

---

## AWS Infrastructure Setup

### 1. Create VPC and Subnets

If you don't have a VPC configured:

```bash
# Create VPC
VPC_ID=$(aws ec2 create-vpc \
  --cidr-block 10.0.0.0/16 \
  --query 'Vpc.VpcId' \
  --output text)

echo "VPC ID: $VPC_ID"

# Enable DNS hostnames
aws ec2 modify-vpc-attribute \
  --vpc-id $VPC_ID \
  --enable-dns-hostnames

# Create Internet Gateway
IGW_ID=$(aws ec2 create-internet-gateway \
  --query 'InternetGateway.InternetGatewayId' \
  --output text)

aws ec2 attach-internet-gateway \
  --vpc-id $VPC_ID \
  --internet-gateway-id $IGW_ID

# Create Subnets (2 public subnets in different AZs)
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

echo "Subnet 1: $SUBNET_1"
echo "Subnet 2: $SUBNET_2"

# Create Route Table
ROUTE_TABLE_ID=$(aws ec2 create-route-table \
  --vpc-id $VPC_ID \
  --query 'RouteTable.RouteTableId' \
  --output text)

aws ec2 create-route \
  --route-table-id $ROUTE_TABLE_ID \
  --destination-cidr-block 0.0.0.0/0 \
  --gateway-id $IGW_ID

# Associate subnets with route table
aws ec2 associate-route-table \
  --subnet-id $SUBNET_1 \
  --route-table-id $ROUTE_TABLE_ID

aws ec2 associate-route-table \
  --subnet-id $SUBNET_2 \
  --route-table-id $ROUTE_TABLE_ID
```

### 2. Create Security Group

```bash
# Create Security Group
SG_ID=$(aws ec2 create-security-group \
  --group-name films-sg \
  --description "Security group for Films application" \
  --vpc-id $VPC_ID \
  --query 'GroupId' \
  --output text)

echo "Security Group ID: $SG_ID"

# Allow HTTP traffic
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 80 \
  --cidr 0.0.0.0/0

# Allow HTTPS traffic
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 443 \
  --cidr 0.0.0.0/0

# Allow application port (8080)
aws ec2 authorize-security-group-ingress \
  --group-id $SG_ID \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0
```

### 3. Create IAM Roles

#### ECS Task Execution Role

```bash
# Create trust policy
cat > /tmp/ecs-trust-policy.json <<EOF
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
  --assume-role-policy-document file:///tmp/ecs-trust-policy.json

# Attach AWS managed policy
aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (Optional)

```bash
# Create task role for application permissions
aws iam create-role \
  --role-name ecsTaskRole \
  --assume-role-policy-document file:///tmp/ecs-trust-policy.json

# Attach policies based on your application needs
# Example: S3 access
aws iam attach-role-policy \
  --role-name ecsTaskRole \
  --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess
```

### 4. Create CloudWatch Log Group

```bash
# Create log group for application logs
aws logs create-log-group \
  --log-group-name /ecs/films-app \
  --region us-east-1

# Set retention period (optional - 7 days)
aws logs put-retention-policy \
  --log-group-name /ecs/films-app \
  --retention-in-days 7
```

---

## Local Development Setup

### 1. Clone Repository

```bash
git clone <repository-url>
cd films-application
```

### 2. Build and Test Locally

```bash
# Restore dependencies
dotnet restore

# Build application
dotnet build -c Release

# Run tests
dotnet test

# Run application locally
dotnet run
```

### 3. Test with Docker Locally

```bash
# Build Docker image
docker build -t films-app:local .

# Run container locally
docker run -d -p 8080:8080 \
  --name films-app-local \
  -e ASPNETCORE_ENVIRONMENT=Development \
  films-app:local

# Test application
curl http://localhost:8080/health

# View logs
docker logs films-app-local

# Stop and remove container
docker stop films-app-local
docker rm films-app-local
```

### 4. Test with Docker Compose

```bash
# Start application with docker-compose
docker-compose up -d

# View logs
docker-compose logs -f

# Stop application
docker-compose down
```

---

## Docker Image Build and Push

### Option 1: Using AWS ECR

#### Step 1: Run Build and Push Script

**Linux/macOS:**

```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

**Windows:**

```cmd
scripts\build-push.bat
```

#### Step 2: Follow Interactive Prompts

```
1. Select registry type: 1 (AWS ECR)
2. Enter AWS region: us-east-1
3. Enter AWS Account ID: 123456789012
4. Enter image tag: v1.0.0
```

The script will:
- Authenticate with AWS ECR
- Create ECR repository if it doesn't exist
- Build Docker image
- Tag and push to ECR

### Option 2: Using Docker Hub

#### Step 1: Run Build and Push Script

```bash
./scripts/build-push.sh  # Linux/macOS
scripts\build-push.bat   # Windows
```

#### Step 2: Follow Interactive Prompts

```
1. Select registry type: 2 (Docker Hub)
2. Enter Docker Hub username: yourusername
3. Enter Docker Hub password: ********
4. Enter image tag: v1.0.0
```

### Manual Build and Push

If you prefer manual control:

```bash
# Set variables
REGION=us-east-1
ACCOUNT_ID=123456789012
REPO_NAME=films
IMAGE_TAG=v1.0.0

# Authenticate with ECR
aws ecr get-login-password --region $REGION | \
  docker login --username AWS --password-stdin \
  $ACCOUNT_ID.dkr.ecr.$REGION.amazonaws.com

# Build image
docker build -t $REPO_NAME:$IMAGE_TAG .

# Tag image
docker tag $REPO_NAME:$IMAGE_TAG \
  $ACCOUNT_ID.dkr.ecr.$REGION.amazonaws.com/$REPO_NAME:$IMAGE_TAG

# Push image
docker push $ACCOUNT_ID.dkr.ecr.$REGION.amazonaws.com/$REPO_NAME:$IMAGE_TAG
```

---

## ECS Fargate Deployment

### Understanding ECS Fargate

AWS Fargate is a serverless compute engine for containers that removes the need to provision and manage servers. Key benefits:

- **Serverless**: No EC2 instances to manage
- **Automatic Scaling**: Scale containers based on demand
- **Pay-per-use**: Only pay for resources consumed
- **Integrated**: Works with AWS services (ALB, CloudWatch, IAM)

### ECS Task Definition Explained

The task definition (`ecs/task-definition.json`) defines:

- **Family**: Task definition name (`films-task`)
- **Launch Type**: Fargate compatibility
- **Network Mode**: `awsvpc` (required for Fargate)
- **CPU and Memory**: Valid Fargate combinations
  - CPU: 512 (.5 vCPU)
  - Memory: 1024 MB (1 GB)
- **Execution Role**: Allows ECS to pull images and write logs
- **Container Definitions**:
  - Image URI
  - Port mappings (8080)
  - Environment variables
  - Health checks
  - Logging configuration

### Valid Fargate CPU/Memory Combinations

| CPU (units) | vCPU | Memory (MB) |
|-------------|------|-------------|
| 256 | 0.25 | 512, 1024, 2048 |
| 512 | 0.5 | 1024, 2048, 3072, 4096 |
| 1024 | 1 | 2048-8192 (increments of 1024) |
| 2048 | 2 | 4096-16384 (increments of 1024) |
| 4096 | 4 | 8192-30720 (increments of 1024) |

### ECS Service Configuration

The service definition (`ecs/service-definition.json`) defines:

- **Service Name**: `films-service`
- **Desired Count**: 2 (number of tasks)
- **Launch Type**: FARGATE
- **Network Configuration**:
  - Subnets (2 in different AZs)
  - Security groups
  - Public IP assignment
- **Load Balancer Integration** (optional)
- **Deployment Configuration**:
  - Maximum percent: 200%
  - Minimum healthy percent: 50%
  - Circuit breaker with rollback
- **Tags**: Environment, application metadata

### Deployment Walkthrough

#### Step 1: Prepare Environment

Ensure you have:
- Docker image pushed to ECR
- VPC with subnets configured
- Security group created
- IAM roles created
- CloudWatch log group created

#### Step 2: Run Deployment Script

**Linux/macOS:**

```bash
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh
```

**Windows:**

```cmd
scripts\deploy-image.bat
```

#### Step 3: Follow Interactive Prompts

The script will prompt for:

1. **AWS Region**: `us-east-1`
2. **ECS Cluster Name**: `films-cluster`
3. **VPC ID**: `vpc-0abc123def456`
4. **Subnet IDs**: `subnet-0abc123,subnet-0def456`
5. **Security Group ID**: `sg-0abc123def`
6. **Docker Image URI**: `123456789012.dkr.ecr.us-east-1.amazonaws.com/films:v1.0.0`
7. **Load Balancer**: `y` or `n`

#### Step 4: Monitor Deployment

The script will:
1. Retrieve AWS Account ID
2. Check/create ECS cluster
3. Create Application Load Balancer (if requested)
4. Register task definition
5. Create/update ECS service
6. Wait for service stability
7. Display deployment status and URLs

#### Step 5: Verify Deployment

```bash
# Check service status
aws ecs describe-services \
  --cluster films-cluster \
  --services films-service \
  --region us-east-1

# List running tasks
aws ecs list-tasks \
  --cluster films-cluster \
  --service-name films-service \
  --region us-east-1

# Get task details
TASK_ARN=$(aws ecs list-tasks \
  --cluster films-cluster \
  --service-name films-service \
  --region us-east-1 \
  --query 'taskArns[0]' \
  --output text)

aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks $TASK_ARN \
  --region us-east-1
```

---

## Configuration Management

### Environment Variables

The application uses the following environment variables:

- `ASPNETCORE_ENVIRONMENT`: Production, Staging, Development
- `ASPNETCORE_URLS`: HTTP binding URL (http://+:8080)
- `DOTNET_RUNNING_IN_CONTAINER`: true
- `ConnectionStrings__DefaultConnection`: Database connection string

### Updating Environment Variables

#### Option 1: Update Task Definition

1. Edit `ecs/task-definition.json`
2. Add/modify environment variables in `containerDefinitions[].environment`
3. Re-register task definition
4. Update service with new task definition

#### Option 2: Use AWS Systems Manager Parameter Store

```bash
# Store secret
aws ssm put-parameter \
  --name "/films/production/db-connection" \
  --value "Server=myserver;Database=mydb;User=myuser;Password=mypass" \
  --type "SecureString" \
  --region us-east-1

# Reference in task definition
"secrets": [
  {
    "name": "ConnectionStrings__DefaultConnection",
    "valueFrom": "/films/production/db-connection"
  }
]
```

#### Option 3: Use AWS Secrets Manager

```bash
# Create secret
aws secretsmanager create-secret \
  --name films/db-credentials \
  --secret-string '{"username":"myuser","password":"mypass"}' \
  --region us-east-1

# Reference in task definition
"secrets": [
  {
    "name": "DB_USERNAME",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789012:secret:films/db-credentials:username::"
  },
  {
    "name": "DB_PASSWORD",
    "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789012:secret:films/db-credentials:password::"
  }
]
```

### Configuration Files

For advanced configuration, mount configuration files using EFS:

1. Create EFS file system
2. Mount EFS to task
3. Store appsettings.Production.json in EFS
4. Reference in application

---

## Monitoring and Logging

### CloudWatch Logs

All application logs are sent to CloudWatch Logs:

**Log Group**: `/ecs/films-app`

#### View Logs

```bash
# Tail logs in real-time
aws logs tail /ecs/films-app --follow --region us-east-1

# Filter logs by pattern
aws logs tail /ecs/films-app \
  --filter-pattern "ERROR" \
  --follow \
  --region us-east-1

# View logs for specific time range
aws logs tail /ecs/films-app \
  --since 1h \
  --region us-east-1
```

#### CloudWatch Logs Insights

Run queries on logs:

```
fields @timestamp, @message
| filter @message like /ERROR/
| sort @timestamp desc
| limit 20
```

### Container Insights

Enable Container Insights for advanced metrics:

```bash
# Enable for cluster
aws ecs put-account-setting \
  --name containerInsights \
  --value enabled \
  --region us-east-1
```

View metrics:
- CPU utilization
- Memory utilization
- Network I/O
- Task count

### Custom Metrics

Publish custom metrics to CloudWatch:

```bash
# Example: Publish request count
aws cloudwatch put-metric-data \
  --namespace "Films/Application" \
  --metric-name "RequestCount" \
  --value 100 \
  --unit Count \
  --region us-east-1
```

### Health Checks

The application exposes a health endpoint:

**Endpoint**: `GET /health`

Health check configuration:
- **Interval**: 30 seconds
- **Timeout**: 5 seconds
- **Healthy Threshold**: 2
- **Unhealthy Threshold**: 3
- **Start Period**: 60 seconds

---

## Troubleshooting

### Common Issues

#### 1. Task Fails to Start

**Symptoms**: Task enters STOPPED state immediately

**Causes**:
- Invalid CPU/memory combination
- Image pull errors (ECR permissions)
- Application crash on startup

**Solutions**:

```bash
# Check task stopped reason
aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks <task-arn> \
  --region us-east-1 \
  --query 'tasks[0].stoppedReason'

# Check CloudWatch logs
aws logs tail /ecs/films-app --region us-east-1

# Verify image exists in ECR
aws ecr describe-images \
  --repository-name films \
  --region us-east-1
```

#### 2. Service Not Stable

**Symptoms**: Service cannot reach desired task count

**Causes**:
- Health check failures
- Insufficient resources in subnets
- Security group blocking traffic

**Solutions**:

```bash
# Check service events
aws ecs describe-services \
  --cluster films-cluster \
  --services films-service \
  --region us-east-1 \
  --query 'services[0].events[0:10]'

# Test health endpoint
curl http://<task-ip>:8080/health

# Verify security group rules
aws ec2 describe-security-groups \
  --group-ids <sg-id> \
  --region us-east-1
```

#### 3. Cannot Access Application

**Symptoms**: Load balancer returns 503 or timeout

**Causes**:
- Target group health checks failing
- Security group not allowing traffic
- Tasks not registered with target group

**Solutions**:

```bash
# Check target health
aws elbv2 describe-target-health \
  --target-group-arn <tg-arn> \
  --region us-east-1

# Check load balancer listeners
aws elbv2 describe-listeners \
  --load-balancer-arn <lb-arn> \
  --region us-east-1

# Verify DNS resolution
nslookup <alb-dns-name>
```

#### 4. High Memory Usage

**Symptoms**: Tasks killed due to OOM (Out of Memory)

**Solutions**:

```bash
# Increase task memory in task definition
# Edit ecs/task-definition.json
"memory": "2048"  # Increase from 1024 to 2048

# Re-register and update service
aws ecs register-task-definition --cli-input-json file://ecs/task-definition.json
aws ecs update-service \
  --cluster films-cluster \
  --service films-service \
  --task-definition films-task \
  --force-new-deployment
```

#### 5. Database Connection Issues

**Symptoms**: Application logs show database connection errors

**Solutions**:

1. Verify connection string format
2. Check security group allows database port
3. Ensure database is accessible from ECS subnets
4. Validate credentials using Parameter Store/Secrets Manager

```bash
# Test database connectivity from task
aws ecs execute-command \
  --cluster films-cluster \
  --task <task-id> \
  --container films-app \
  --interactive \
  --command "/bin/bash"

# Inside container, test connection
curl -v telnet://<db-host>:<db-port>
```

---

## Scaling and Performance

### Auto Scaling

Configure Service Auto Scaling based on metrics:

#### Target Tracking Scaling

```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --scalable-dimension ecs:service:DesiredCount \
  --resource-id service/films-cluster/films-service \
  --min-capacity 2 \
  --max-capacity 10 \
  --region us-east-1

# Create scaling policy (CPU-based)
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --scalable-dimension ecs:service:DesiredCount \
  --resource-id service/films-cluster/films-service \
  --policy-name films-cpu-scaling \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration '{
    "TargetValue": 70.0,
    "PredefinedMetricSpecification": {
      "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
    },
    "ScaleInCooldown": 300,
    "ScaleOutCooldown": 60
  }' \
  --region us-east-1
```

#### Step Scaling

```bash
# Create CloudWatch alarm
aws cloudwatch put-metric-alarm \
  --alarm-name films-high-cpu \
  --alarm-description "Alarm when CPU exceeds 80%" \
  --metric-name CPUUtilization \
  --namespace AWS/ECS \
  --statistic Average \
  --period 300 \
  --threshold 80 \
  --comparison-operator GreaterThanThreshold \
  --evaluation-periods 2 \
  --dimensions Name=ServiceName,Value=films-service Name=ClusterName,Value=films-cluster \
  --region us-east-1

# Create step scaling policy
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --scalable-dimension ecs:service:DesiredCount \
  --resource-id service/films-cluster/films-service \
  --policy-name films-step-scaling \
  --policy-type StepScaling \
  --step-scaling-policy-configuration '{
    "AdjustmentType": "PercentChangeInCapacity",
    "StepAdjustments": [
      {
        "MetricIntervalLowerBound": 0,
        "ScalingAdjustment": 50
      }
    ],
    "Cooldown": 60
  }' \
  --region us-east-1
```

### Blue/Green Deployments

Implement zero-downtime deployments:

1. Create new task definition revision
2. Update service with new task definition
3. ECS gradually replaces old tasks with new tasks
4. Circuit breaker automatically rolls back on failures

```bash
# Deploy new version
aws ecs update-service \
  --cluster films-cluster \
  --service films-service \
  --task-definition films-task:2 \
  --force-new-deployment \
  --region us-east-1
```

### Performance Optimization

#### .NET-Specific Optimizations

1. **Use ReadyToRun (R2R) Images**:
   ```dockerfile
   RUN dotnet publish -c Release -o /app/publish -r linux-x64 --self-contained false /p:PublishReadyToRun=true
   ```

2. **Enable Tiered Compilation**:
   ```dockerfile
   ENV DOTNET_TieredCompilation=1
   ```

3. **Configure Garbage Collection**:
   ```dockerfile
   ENV DOTNET_gcServer=1
   ENV DOTNET_gcConcurrent=1
   ```

4. **Set Culture to Invariant** (if applicable):
   ```dockerfile
   ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
   ```

---

## Security Best Practices

### 1. Use Non-Root User

The Dockerfile creates and uses a non-root user:

```dockerfile
RUN groupadd -r appuser && useradd -r -g appuser appuser
USER appuser
```

### 2. Secure Secrets Management

- **Never** hardcode secrets in task definitions
- Use AWS Secrets Manager or Parameter Store
- Grant task role minimal permissions

### 3. Network Security

```bash
# Restrict security group to specific IP ranges
aws ec2 authorize-security-group-ingress \
  --group-id <sg-id> \
  --protocol tcp \
  --port 8080 \
  --cidr 10.0.0.0/16  # VPC CIDR only
```

### 4. Enable VPC Flow Logs

```bash
aws ec2 create-flow-logs \
  --resource-type VPC \
  --resource-ids <vpc-id> \
  --traffic-type ALL \
  --log-destination-type cloud-watch-logs \
  --log-group-name /aws/vpc/flow-logs
```

### 5. Image Scanning

```bash
# Enable ECR image scanning
aws ecr put-image-scanning-configuration \
  --repository-name films \
  --image-scanning-configuration scanOnPush=true \
  --region us-east-1

# View scan results
aws ecr describe-image-scan-findings \
  --repository-name films \
  --image-id imageTag=v1.0.0 \
  --region us-east-1
```

### 6. Least Privilege IAM

Grant only required permissions:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "s3:GetObject",
        "s3:ListBucket"
      ],
      "Resource": [
        "arn:aws:s3:::films-bucket/*",
        "arn:aws:s3:::films-bucket"
      ]
    }
  ]
}
```

---

## Additional Resources

### AWS Documentation

- [AWS Fargate User Guide](https://docs.aws.amazon.com/AmazonECS/latest/userguide/what-is-fargate.html)
- [ECS Task Definitions](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/task_definitions.html)
- [ECS Service Auto Scaling](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/service-auto-scaling.html)

### .NET Resources

- [.NET 8 Release Notes](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [ASP.NET Core Health Checks](https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/health-checks)
- [Containerized .NET Applications](https://learn.microsoft.com/en-us/dotnet/core/docker/introduction)

### Support

For issues or questions:
- AWS Support: https://console.aws.amazon.com/support/
- Community Forums: https://forums.aws.amazon.com/forum.jspa?forumID=187

---

**Last Updated**: 2025-12-31
**Version**: 1.0.0
**Application**: Films ASP.NET Core Application
**Platform**: AWS ECS Fargate