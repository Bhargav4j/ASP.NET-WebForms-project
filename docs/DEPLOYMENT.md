# Films Application - AWS ECS Fargate Deployment Guide

## Table of Contents

1. [Prerequisites](#prerequisites)
2. [Project Overview](#project-overview)
3. [Local Development Setup](#local-development-setup)
4. [Docker Containerization](#docker-containerization)
5. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
6. [ECS Fargate Setup](#ecs-fargate-setup)
7. [Building and Pushing Docker Images](#building-and-pushing-docker-images)
8. [ECS Task Definition](#ecs-task-definition)
9. [ECS Service Configuration](#ecs-service-configuration)
10. [Deployment Walkthrough](#deployment-walkthrough)
11. [Monitoring and Logging](#monitoring-and-logging)
12. [Troubleshooting](#troubleshooting)
13. [Scaling and Management](#scaling-and-management)
14. [Security Best Practices](#security-best-practices)

---

## Prerequisites

### Required Tools

- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **AWS CLI v2** - [Installation Guide](https://docs.aws.amazon.com/cli/latest/userguide/install-cliv2.html)
- **Git** - Version control

### AWS Requirements

- **AWS Account** with appropriate permissions
- **IAM User** with the following permissions:
  - ECS Full Access
  - ECR Full Access
  - CloudWatch Logs Full Access
  - IAM Role Creation (for task execution role)
  - VPC and Security Group Access
  - Application Load Balancer Access (if using ALB)

### Configure AWS CLI

```bash
aws configure
# Provide:
# - AWS Access Key ID
# - AWS Secret Access Key
# - Default region (e.g., us-east-1)
# - Output format (json recommended)
```

---

## Project Overview

### Technology Stack

- **Framework**: .NET 8.0 ASP.NET Core
- **Application Type**: Web Application (Razor Pages)
- **Database**: SQL Server (Entity Framework Core)
- **Logging**: Serilog
- **Authentication**: ASP.NET Core Identity
- **Health Checks**: Built-in ASP.NET Core Health Checks

### Application Architecture

```
Films.Web/                    # ASP.NET Core Web Application
├── Films.Domain/             # Domain entities and interfaces
├── Films.Application/        # Application services and logic
├── Films.Infrastructure/     # Data access and external services
└── Films.Web/                # Web UI and API endpoints
```

### Key Configuration Files

- **appsettings.json** - Application configuration
- **appsettings.Development.json** - Development settings
- **Program.cs** - Application entry point
- **Films.Web.csproj** - Project file

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ASP12
```

### 2. Restore Dependencies

```bash
dotnet restore Films.sln
```

### 3. Configure Database Connection

Update `src/Films.Web/appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FilmsDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```

### 4. Run Database Migrations

```bash
cd src/Films.Web
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run --project src/Films.Web/Films.Web.csproj
```

Access the application at: `http://localhost:5000` or `https://localhost:5001`

---

## Docker Containerization

### Dockerfile Overview

The multi-stage Dockerfile optimizes the build process:

**Build Stage**:
- Uses `mcr.microsoft.com/dotnet/sdk:8.0` for building
- Restores NuGet packages (cached layer)
- Compiles the application
- Publishes to `/app/publish`

**Runtime Stage**:
- Uses `mcr.microsoft.com/dotnet/aspnet:8.0` for runtime
- Copies published artifacts
- Runs as non-root user for security
- Exposes port 8080

### Build Docker Image Locally

```bash
# From project root
docker build -f Dockerfile -t films-app:latest .
```

### Run Container Locally

```bash
docker run -d \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e DB_SERVER=your-db-server \
  -e DB_NAME=filmsdb \
  -e DB_USER=dbuser \
  -e DB_PASSWORD=dbpassword \
  --name films-app \
  films-app:latest
```

### Test the Container

```bash
# Health check
curl http://localhost:8080/health

# View logs
docker logs films-app

# Stop container
docker stop films-app
```

---

## AWS ECS Fargate Prerequisites

### 1. Create VPC and Networking

You need:
- **VPC** with CIDR block (e.g., 10.0.0.0/16)
- **At least 2 public subnets** in different availability zones
- **Internet Gateway** attached to VPC
- **Route table** with route to Internet Gateway

```bash
# Create VPC (if needed)
aws ec2 create-vpc --cidr-block 10.0.0.0/16 --region us-east-1

# Create subnets
aws ec2 create-subnet --vpc-id vpc-xxxxx --cidr-block 10.0.1.0/24 --availability-zone us-east-1a
aws ec2 create-subnet --vpc-id vpc-xxxxx --cidr-block 10.0.2.0/24 --availability-zone us-east-1b
```

### 2. Create Security Group

```bash
# Create security group
aws ec2 create-security-group \
  --group-name films-app-sg \
  --description "Security group for Films application" \
  --vpc-id vpc-xxxxx \
  --region us-east-1

# Allow inbound HTTP traffic
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0 \
  --region us-east-1

# Allow inbound traffic from ALB (if using load balancer)
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 8080 \
  --source-group sg-alb-xxxxx \
  --region us-east-1
```

### 3. Create IAM Roles

#### ECS Task Execution Role

Create `ecs-task-execution-role-trust-policy.json`:

```json
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
```

Create the role:

```bash
aws iam create-role \
  --role-name ecsTaskExecutionRole \
  --assume-role-policy-document file://ecs-task-execution-role-trust-policy.json

aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (Optional)

For application-specific AWS service access:

```bash
aws iam create-role \
  --role-name ecsTaskRole \
  --assume-role-policy-document file://ecs-task-execution-role-trust-policy.json

# Attach policies as needed (e.g., S3 access, Secrets Manager)
aws iam attach-role-policy \
  --role-name ecsTaskRole \
  --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess
```

### 4. Create RDS Database (if needed)

```bash
aws rds create-db-instance \
  --db-instance-identifier films-db \
  --db-instance-class db.t3.micro \
  --engine sqlserver-ex \
  --master-username admin \
  --master-user-password YourPassword123! \
  --allocated-storage 20 \
  --vpc-security-group-ids sg-xxxxx \
  --db-subnet-group-name your-db-subnet-group \
  --region us-east-1
```

---

## ECS Fargate Setup

### 1. Create ECS Cluster

```bash
aws ecs create-cluster \
  --cluster-name films-cluster \
  --region us-east-1
```

### 2. Create CloudWatch Log Group

```bash
aws logs create-log-group \
  --log-group-name /ecs/films-app \
  --region us-east-1
```

### 3. Create ECR Repository

```bash
aws ecr create-repository \
  --repository-name films-app \
  --region us-east-1
```

---

## Building and Pushing Docker Images

### Using build-push.sh (Linux/macOS)

```bash
cd scripts
chmod +x build-push.sh
./build-push.sh
```

### Using build-push.bat (Windows)

```cmd
cd scripts
build-push.bat
```

### Script Features

1. **Interactive Registry Selection**: Choose between AWS ECR or Docker Hub
2. **Tag Sanitization**: Automatically formats image names and tags
3. **ECR Repository Auto-Creation**: Creates repository if it doesn't exist
4. **Authentication Handling**: Manages registry login automatically
5. **Build and Push**: Builds Docker image and pushes to registry

### Manual Build and Push (ECR)

```bash
# Authenticate with ECR
aws ecr get-login-password --region us-east-1 | \
  docker login --username AWS --password-stdin \
  123456789012.dkr.ecr.us-east-1.amazonaws.com

# Build image
docker build -f Dockerfile -t films-app:latest .

# Tag image
docker tag films-app:latest \
  123456789012.dkr.ecr.us-east-1.amazonaws.com/films-app:latest

# Push image
docker push 123456789012.dkr.ecr.us-east-1.amazonaws.com/films-app:latest
```

---

## ECS Task Definition

### Key Components

#### CPU and Memory Configuration

**Valid Fargate CPU/Memory Combinations**:

| CPU (vCPU) | Memory (MB) |
|------------|-------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024, 2048, 3072, 4096 |
| 1024 (1)   | 2048-8192 (increments of 1024) |
| 2048 (2)   | 4096-16384 (increments of 1024) |
| 4096 (4)   | 8192-30720 (increments of 1024) |

**Default Configuration**: CPU: 512, Memory: 1024

#### Container Definition

```json
{
  "name": "films-app",
  "image": "123456789012.dkr.ecr.us-east-1.amazonaws.com/films-app:latest",
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
      "awslogs-group": "/ecs/films-app",
      "awslogs-region": "us-east-1",
      "awslogs-stream-prefix": "ecs"
    }
  }
}
```

### Register Task Definition

```bash
aws ecs register-task-definition \
  --cli-input-json file://ecs/task-definition.json \
  --region us-east-1
```

---

## ECS Service Configuration

### Service Components

#### Network Configuration

```json
{
  "networkConfiguration": {
    "awsvpcConfiguration": {
      "subnets": ["subnet-xxxxx", "subnet-yyyyy"],
      "securityGroups": ["sg-xxxxx"],
      "assignPublicIp": "ENABLED"
    }
  }
}
```

#### Deployment Configuration

```json
{
  "deploymentConfiguration": {
    "maximumPercent": 200,
    "minimumHealthyPercent": 50,
    "deploymentCircuitBreaker": {
      "enable": true,
      "rollback": true
    }
  }
}
```

#### Load Balancer Integration (Optional)

```json
{
  "loadBalancers": [
    {
      "targetGroupArn": "arn:aws:elasticloadbalancing:...",
      "containerName": "films-app",
      "containerPort": 8080
    }
  ],
  "healthCheckGracePeriodSeconds": 300
}
```

### Create Service

```bash
aws ecs create-service \
  --cluster films-cluster \
  --service-name films-app-service \
  --cli-input-json file://ecs/service-definition.json \
  --region us-east-1
```

---

## Deployment Walkthrough

### Step 1: Build and Push Image

```bash
cd /path/to/ASP12
./scripts/build-push.sh
```

Follow prompts:
1. Select AWS ECR (option 1)
2. Enter AWS Region: `us-east-1`
3. Enter AWS Account ID: `123456789012`
4. Enter ECR Repository Name: `films-app`
5. Enter image tag: `v1.0.0` (or `latest`)

### Step 2: Deploy to ECS

```bash
./scripts/deploy-image.sh
```

Follow prompts:
1. AWS Region: `us-east-1`
2. ECS Cluster Name: `films-cluster`
3. VPC ID: `vpc-xxxxx`
4. Subnet IDs: `subnet-xxxxx,subnet-yyyyy`
5. Security Group ID: `sg-xxxxx`
6. Docker Image URI: `123456789012.dkr.ecr.us-east-1.amazonaws.com/films-app:v1.0.0`
7. Database Configuration:
   - Server: `films-db.abc123.us-east-1.rds.amazonaws.com`
   - Database: `filmsdb`
   - User: `admin`
   - Password: `YourPassword123!`
8. Load Balancer: `y` (if needed)

### Step 3: Wait for Deployment

The script automatically waits for service stability. Monitor progress:

```bash
aws ecs describe-services \
  --cluster films-cluster \
  --services films-app-service \
  --region us-east-1
```

### Step 4: Verify Deployment

```bash
# Check running tasks
aws ecs list-tasks \
  --cluster films-cluster \
  --service-name films-app-service \
  --region us-east-1

# Get task details
aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks <task-arn> \
  --region us-east-1
```

### Step 5: Test Application

If using Application Load Balancer:

```bash
# Get ALB DNS name
aws elbv2 describe-load-balancers \
  --names films-app-alb \
  --query 'LoadBalancers[0].DNSName' \
  --output text

# Test health endpoint
curl http://<alb-dns-name>/health

# Access application
curl http://<alb-dns-name>/
```

---

## Monitoring and Logging

### CloudWatch Logs

#### View Logs

```bash
# Tail logs in real-time
aws logs tail /ecs/films-app --follow --region us-east-1

# View specific log stream
aws logs get-log-events \
  --log-group-name /ecs/films-app \
  --log-stream-name ecs/films-app/<task-id> \
  --region us-east-1
```

#### Filter Logs

```bash
# Filter by pattern
aws logs filter-log-events \
  --log-group-name /ecs/films-app \
  --filter-pattern "ERROR" \
  --region us-east-1
```

### CloudWatch Metrics

ECS automatically publishes metrics:

- **CPUUtilization**
- **MemoryUtilization**
- **TaskCount**
- **RunningTaskCount**

#### View Metrics

```bash
aws cloudwatch get-metric-statistics \
  --namespace AWS/ECS \
  --metric-name CPUUtilization \
  --dimensions Name=ServiceName,Value=films-app-service Name=ClusterName,Value=films-cluster \
  --start-time 2024-01-01T00:00:00Z \
  --end-time 2024-01-02T00:00:00Z \
  --period 3600 \
  --statistics Average \
  --region us-east-1
```

### Application Insights (Optional)

For .NET applications, integrate Application Insights:

1. Install NuGet package:
   ```bash
   dotnet add package Microsoft.ApplicationInsights.AspNetCore
   ```

2. Configure in `Program.cs`:
   ```csharp
   builder.Services.AddApplicationInsightsTelemetry();
   ```

3. Add environment variable in task definition:
   ```json
   {"name": "APPLICATIONINSIGHTS_CONNECTION_STRING", "value": "your-connection-string"}
   ```

---

## Troubleshooting

### Common Issues

#### 1. Task Fails to Start

**Symptom**: Task transitions to STOPPED state immediately

**Causes**:
- Invalid CPU/memory combination
- Container image not accessible
- Missing IAM permissions
- Invalid environment variables

**Solution**:

```bash
# Check stopped task reason
aws ecs describe-tasks \
  --cluster films-cluster \
  --tasks <task-arn> \
  --query 'tasks[0].stoppedReason' \
  --output text

# View container logs
aws logs tail /ecs/films-app --since 1h
```

#### 2. Container Fails Health Checks

**Symptom**: Tasks continuously restart

**Causes**:
- Health endpoint not responding
- Application startup too slow
- Database connection issues

**Solution**:

```bash
# Increase health check grace period in service definition
"healthCheckGracePeriodSeconds": 300

# Check application logs
aws logs tail /ecs/films-app --follow

# Test health endpoint from within task
aws ecs execute-command \
  --cluster films-cluster \
  --task <task-id> \
  --container films-app \
  --interactive \
  --command "/bin/bash"
```

#### 3. Network Connectivity Issues

**Symptom**: Cannot reach external services or database

**Causes**:
- Security group rules blocking traffic
- Incorrect subnet configuration
- No route to internet gateway

**Solution**:

```bash
# Verify security group rules
aws ec2 describe-security-groups \
  --group-ids sg-xxxxx \
  --region us-east-1

# Check route table
aws ec2 describe-route-tables \
  --filters "Name=vpc-id,Values=vpc-xxxxx" \
  --region us-east-1

# Test database connectivity from task
aws ecs execute-command \
  --cluster films-cluster \
  --task <task-id> \
  --container films-app \
  --interactive \
  --command "/bin/bash"
# Then: telnet <db-host> 1433
```

#### 4. CPU/Memory Errors

**Symptom**: "Cannot allocate memory" or "CPU limit exceeded"

**Causes**:
- Invalid Fargate CPU/memory combination
- Application requires more resources

**Solution**:

```bash
# Update task definition with valid combination
# Valid: cpu=512, memory=1024
# Valid: cpu=1024, memory=2048

aws ecs update-service \
  --cluster films-cluster \
  --service films-app-service \
  --task-definition films-app-task:2 \
  --force-new-deployment
```

#### 5. Image Pull Errors

**Symptom**: "CannotPullContainerError"

**Causes**:
- Incorrect image URI
- ECR authentication failure
- Missing executionRoleArn permissions

**Solution**:

```bash
# Verify image exists
aws ecr describe-images \
  --repository-name films-app \
  --region us-east-1

# Check execution role permissions
aws iam get-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-name ECRAccess

# Ensure execution role has ECR permissions
aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly
```

### Debugging Commands

```bash
# Enable ECS Exec for interactive debugging
aws ecs update-service \
  --cluster films-cluster \
  --service films-app-service \
  --enable-execute-command

# Connect to running container
aws ecs execute-command \
  --cluster films-cluster \
  --task <task-arn> \
  --container films-app \
  --interactive \
  --command "/bin/bash"

# View task events
aws ecs describe-services \
  --cluster films-cluster \
  --services films-app-service \
  --query 'services[0].events[0:10]'
```

---

## Scaling and Management

### Manual Scaling

```bash
# Update desired count
aws ecs update-service \
  --cluster films-cluster \
  --service films-app-service \
  --desired-count 4 \
  --region us-east-1
```

### Auto Scaling

#### Create Scalable Target

```bash
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/films-cluster/films-app-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10 \
  --region us-east-1
```

#### Create Scaling Policy (Target Tracking)

```bash
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/films-cluster/films-app-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name films-app-cpu-scaling \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json \
  --region us-east-1
```

**scaling-policy.json**:

```json
{
  "TargetValue": 70.0,
  "PredefinedMetricSpecification": {
    "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
  },
  "ScaleOutCooldown": 60,
  "ScaleInCooldown": 300
}
```

### Blue/Green Deployments

For zero-downtime deployments with AWS CodeDeploy:

1. Create CodeDeploy application:
   ```bash
   aws deploy create-application \
     --application-name films-app \
     --compute-platform ECS
   ```

2. Create deployment group:
   ```bash
   aws deploy create-deployment-group \
     --application-name films-app \
     --deployment-group-name films-app-dg \
     --deployment-config-name CodeDeployDefault.ECSAllAtOnce \
     --service-role-arn arn:aws:iam::123456789012:role/CodeDeployServiceRole \
     --ecs-services clusterName=films-cluster,serviceName=films-app-service \
     --load-balancer-info targetGroupInfoList=[{name=films-app-tg}]
   ```

### Rolling Updates

```bash
# Force new deployment with latest image
aws ecs update-service \
  --cluster films-cluster \
  --service films-app-service \
  --force-new-deployment \
  --region us-east-1
```

---

## Security Best Practices

### 1. Use Secrets Manager for Sensitive Data

Instead of environment variables, use AWS Secrets Manager:

```bash
# Create secret
aws secretsmanager create-secret \
  --name films-app/db-password \
  --secret-string "YourPassword123!" \
  --region us-east-1
```

Update task definition:

```json
{
  "secrets": [
    {
      "name": "DB_PASSWORD",
      "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789012:secret:films-app/db-password"
    }
  ]
}
```

### 2. Enable VPC Flow Logs

```bash
aws ec2 create-flow-logs \
  --resource-type VPC \
  --resource-ids vpc-xxxxx \
  --traffic-type ALL \
  --log-destination-type cloud-watch-logs \
  --log-group-name /aws/vpc/films-app \
  --deliver-logs-permission-arn arn:aws:iam::123456789012:role/flowlogsRole
```

### 3. Use Private Subnets with NAT Gateway

For production:
- Place ECS tasks in private subnets
- Use NAT Gateway for outbound internet access
- Place ALB in public subnets

### 4. Implement IAM Least Privilege

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "ecr:GetAuthorizationToken",
        "ecr:BatchCheckLayerAvailability",
        "ecr:GetDownloadUrlForLayer",
        "ecr:BatchGetImage"
      ],
      "Resource": "*"
    },
    {
      "Effect": "Allow",
      "Action": [
        "logs:CreateLogStream",
        "logs:PutLogEvents"
      ],
      "Resource": "arn:aws:logs:*:*:log-group:/ecs/films-app:*"
    }
  ]
}
```

### 5. Enable Container Insights

```bash
aws ecs update-cluster-settings \
  --cluster films-cluster \
  --settings name=containerInsights,value=enabled \
  --region us-east-1
```

### 6. Scan Images for Vulnerabilities

```bash
# Enable ECR scanning
aws ecr put-image-scanning-configuration \
  --repository-name films-app \
  --image-scanning-configuration scanOnPush=true \
  --region us-east-1

# View scan results
aws ecr describe-image-scan-findings \
  --repository-name films-app \
  --image-id imageTag=latest \
  --region us-east-1
```

---

## Additional Resources

- [AWS ECS Documentation](https://docs.aws.amazon.com/ecs/)
- [AWS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/AWS_Fargate.html)
- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [ASP.NET Core Best Practices](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)
- [Docker Best Practices](https://docs.docker.com/develop/dev-best-practices/)

---

## Support and Troubleshooting

For issues or questions:

1. Check CloudWatch logs: `/ecs/films-app`
2. Review ECS service events
3. Verify security group rules
4. Ensure database connectivity
5. Check IAM role permissions

---

**Last Updated**: January 2, 2026  
**Version**: 1.0.0  
**Platform**: AWS ECS Fargate  
**Application**: Films App (.NET 8.0)
