# ASP.NET Core Application Deployment Guide - AWS ECS Fargate

## Table of Contents

1. [Overview](#overview)
2. [Prerequisites](#prerequisites)
3. [Project Structure](#project-structure)
4. [Local Development](#local-development)
5. [Docker Deployment](#docker-deployment)
6. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
7. [AWS ECS Fargate Setup](#aws-ecs-fargate-setup)
8. [ECS Task Definition Explained](#ecs-task-definition-explained)
9. [ECS Service Configuration](#ecs-service-configuration)
10. [Deployment Walkthrough](#deployment-walkthrough)
11. [Configuration Management](#configuration-management)
12. [Monitoring and Logging](#monitoring-and-logging)
13. [Troubleshooting](#troubleshooting)
14. [Security Considerations](#security-considerations)
15. [Scaling and Performance](#scaling-and-performance)

## Overview

This guide provides comprehensive instructions for deploying the ASP.NET Core 8.0 application (`ASPNETWebFprojContain06`) to AWS ECS Fargate. The application is containerized using Docker and deployed to a serverless container orchestration platform.

### Technology Stack

- **Framework**: ASP.NET Core 8.0
- **Runtime**: .NET 8.0
- **Build Tool**: dotnet CLI
- **Container Platform**: Docker
- **Orchestration**: AWS ECS Fargate
- **Load Balancer**: Application Load Balancer (ALB)
- **Logging**: AWS CloudWatch Logs

## Prerequisites

### Required Tools

1. **Docker Desktop** (v20.10+)
   - [Download for Windows](https://www.docker.com/products/docker-desktop)
   - [Download for macOS](https://www.docker.com/products/docker-desktop)
   - [Install on Linux](https://docs.docker.com/engine/install/)

2. **AWS CLI** (v2.x)
   ```bash
   # Verify installation
   aws --version
   
   # Configure AWS credentials
   aws configure
   ```

3. **.NET SDK 8.0** (for local development)
   ```bash
   # Verify installation
   dotnet --version
   ```

4. **Git** (for version control)
   ```bash
   git --version
   ```

### AWS Account Requirements

- Active AWS account with appropriate permissions
- IAM user with the following permissions:
  - ECS full access
  - ECR full access
  - EC2 (for VPC, subnets, security groups)
  - CloudWatch Logs
  - Elastic Load Balancing
  - IAM role creation

## Project Structure

```
ASPNETWebFprojContain06/
├── Controllers/              # MVC/API controllers
├── Models/                   # Data models
├── Views/                    # Razor views (if MVC)
├── appsettings.json         # Application configuration
├── appsettings.Development.json
├── appsettings.Production.json
├── Program.cs               # Application entry point
├── Startup.cs               # Application startup configuration
├── Dockerfile               # Docker build instructions
├── docker-compose.yml       # Docker Compose configuration
├── .dockerignore           # Docker ignore patterns
├── ecs/
│   ├── task-definition.json    # ECS task definition
│   └── service-definition.json # ECS service definition
├── scripts/
│   ├── build-push.sh          # Linux/Mac build script
│   ├── build-push.bat         # Windows build script
│   ├── deploy-image.sh        # Linux/Mac deployment script
│   └── deploy-image.bat       # Windows deployment script
└── docs/
    └── DEPLOYMENT.md          # This file
```

## Local Development

### Running Locally (Without Docker)

```bash
# Restore dependencies
dotnet restore

# Build the project
dotnet build

# Run the application
dotnet run

# Access at http://localhost:5000 or https://localhost:5001
```

### Running with Docker Compose

```bash
# Build and start the container
docker-compose up --build

# Access at http://localhost:8080

# Stop the container
docker-compose down
```

## Docker Deployment

### Building the Docker Image

The Dockerfile uses a multi-stage build for optimization:

1. **Build Stage**: Uses `mcr.microsoft.com/dotnet/sdk:8.0` to compile the application
2. **Runtime Stage**: Uses `mcr.microsoft.com/dotnet/aspnet:8.0` for a minimal runtime image

```bash
# Build manually
docker build -t aspnetwebfprojcontain06:latest .

# Run locally
docker run -p 8080:8080 aspnetwebfprojcontain06:latest
```

### Pushing to Container Registry

#### Option 1: AWS ECR (Recommended for ECS)

```bash
# Run the build and push script
# Linux/macOS
chmod +x scripts/build-push.sh
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

#### Option 2: Docker Hub

```bash
# The script will prompt for registry selection
# Choose option 2 for Docker Hub
./scripts/build-push.sh
```

## AWS ECS Fargate Prerequisites

### 1. VPC and Networking

Your ECS tasks require a VPC with:

- **VPC**: Virtual Private Cloud
- **Subnets**: At least 2 subnets in different availability zones (public or private)
- **Internet Gateway**: For public subnet access
- **NAT Gateway**: If using private subnets

```bash
# List existing VPCs
aws ec2 describe-vpcs --query 'Vpcs[*].[VpcId,CidrBlock,Tags[?Key==`Name`].Value|[0]]' --output table

# List subnets
aws ec2 describe-subnets --query 'Subnets[*].[SubnetId,VpcId,CidrBlock,AvailabilityZone]' --output table
```

### 2. Security Groups

Create a security group that allows:

- **Inbound**: Port 8080 (application port) from ALB security group
- **Outbound**: All traffic (for pulling images, external APIs)

```bash
# Create security group
aws ec2 create-security-group \
  --group-name aspnetwebfprojcontain06-sg \
  --description "Security group for ASP.NET Core application" \
  --vpc-id vpc-xxxxx

# Add inbound rule (replace sg-xxxxx with ALB security group)
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxx \
  --protocol tcp \
  --port 8080 \
  --source-group sg-alb-xxxxx
```

### 3. IAM Roles

#### Task Execution Role (Required)

Allows ECS to pull images from ECR and send logs to CloudWatch.

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

Attach managed policies:
- `arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy`

```bash
# Create execution role
aws iam create-role \
  --role-name ecsTaskExecutionRole \
  --assume-role-policy-document file://trust-policy.json

# Attach policy
aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### Task Role (Optional)

Provides permissions for the application to access AWS services.

```bash
# Create task role
aws iam create-role \
  --role-name ecsTaskRole \
  --assume-role-policy-document file://trust-policy.json

# Attach custom policies as needed
aws iam attach-role-policy \
  --role-name ecsTaskRole \
  --policy-arn arn:aws:iam::aws:policy/AmazonS3ReadOnlyAccess
```

## AWS ECS Fargate Setup

### 1. Create ECS Cluster

```bash
aws ecs create-cluster --cluster-name aspnetwebfprojcontain06-cluster
```

### 2. Create CloudWatch Log Group

```bash
aws logs create-log-group --log-group-name /ecs/aspnetwebfprojcontain06
```

### 3. Push Docker Image to ECR

Use the provided build-push scripts:

```bash
# Linux/macOS
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

Select option 1 (AWS ECR) and provide:
- AWS Region
- AWS Account ID
- Repository name

## ECS Task Definition Explained

### Key Components

#### Fargate Configuration

```json
{
  "requiresCompatibilities": ["FARGATE"],
  "networkMode": "awsvpc",
  "cpu": "512",
  "memory": "1024"
}
```

**Valid CPU/Memory Combinations:**

| CPU (vCPU) | Memory (MB) |
|------------|-------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024-4096 (increments of 1024) |
| 1024 (1)   | 2048-8192 (increments of 1024) |
| 2048 (2)   | 4096-16384 (increments of 1024) |
| 4096 (4)   | 8192-30720 (increments of 1024) |

#### Container Definition

```json
{
  "name": "aspnetwebfprojcontain06",
  "image": "{{IMAGE_URI}}",
  "essential": true,
  "portMappings": [
    {
      "containerPort": 8080,
      "protocol": "tcp"
    }
  ]
}
```

#### Environment Variables

```json
"environment": [
  {"name": "ASPNETCORE_ENVIRONMENT", "value": "Production"},
  {"name": "ASPNETCORE_URLS", "value": "http://+:8080"},
  {"name": "DOTNET_RUNNING_IN_CONTAINER", "value": "true"}
]
```

#### Logging Configuration

```json
"logConfiguration": {
  "logDriver": "awslogs",
  "options": {
    "awslogs-group": "/ecs/aspnetwebfprojcontain06",
    "awslogs-region": "us-east-1",
    "awslogs-stream-prefix": "ecs"
  }
}
```

## ECS Service Configuration

### Service Parameters

```json
{
  "serviceName": "aspnetwebfprojcontain06-service",
  "launchType": "FARGATE",
  "desiredCount": 2,
  "networkConfiguration": {
    "awsvpcConfiguration": {
      "subnets": ["subnet-xxx", "subnet-yyy"],
      "securityGroups": ["sg-xxx"],
      "assignPublicIp": "ENABLED"
    }
  }
}
```

### Deployment Configuration

```json
"deploymentConfiguration": {
  "maximumPercent": 200,
  "minimumHealthyPercent": 50
}
```

- **maximumPercent**: Maximum percentage of tasks that can run during deployment (200% = double the desired count)
- **minimumHealthyPercent**: Minimum percentage of tasks that must remain healthy (50% = half the desired count)

### Load Balancer Configuration

```json
"loadBalancers": [
  {
    "targetGroupArn": "arn:aws:elasticloadbalancing:...",
    "containerName": "aspnetwebfprojcontain06",
    "containerPort": 8080
  }
],
"healthCheckGracePeriodSeconds": 300
```

## Deployment Walkthrough

### Step 1: Build and Push Docker Image

```bash
# Linux/macOS
chmod +x scripts/build-push.sh
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

**Prompts:**
1. Select registry type (1. AWS ECR or 2. Docker Hub)
2. Enter AWS region (e.g., us-east-1)
3. Enter AWS Account ID
4. Enter repository name (default: aspnetwebfprojcontain06)
5. Enter image tag (default: latest)

### Step 2: Deploy to ECS Fargate

```bash
# Linux/macOS
chmod +x scripts/deploy-image.sh
./scripts/deploy-image.sh

# Windows
scripts\deploy-image.bat
```

**Prompts:**
1. AWS region
2. ECS cluster name
3. VPC ID
4. Subnet IDs (comma-separated)
5. Security Group ID
6. Docker image URI (from Step 1)
7. Load balancer requirement (y/n)

### Step 3: Verify Deployment

```bash
# Check service status
aws ecs describe-services \
  --cluster aspnetwebfprojcontain06-cluster \
  --services aspnetwebfprojcontain06-service

# Check running tasks
aws ecs list-tasks \
  --cluster aspnetwebfprojcontain06-cluster \
  --service-name aspnetwebfprojcontain06-service

# View logs
aws logs tail /ecs/aspnetwebfprojcontain06 --follow
```

## Configuration Management

### Environment-Specific Settings

The application uses `appsettings.{Environment}.json` files:

- `appsettings.json`: Base configuration
- `appsettings.Development.json`: Development overrides
- `appsettings.Production.json`: Production overrides

The `ASPNETCORE_ENVIRONMENT` variable determines which file is loaded.

### Secrets Management

**Option 1: AWS Secrets Manager**

```json
"secrets": [
  {
    "name": "ConnectionString",
    "valueFrom": "arn:aws:secretsmanager:region:account:secret:name"
  }
]
```

**Option 2: Environment Variables**

```json
"environment": [
  {
    "name": "ConnectionStrings__DefaultConnection",
    "value": "Server=...;Database=...;"
  }
]
```

### Database Connection Strings

For external databases (RDS, Azure SQL, etc.), use environment variables:

```bash
ConnectionStrings__DefaultConnection="Server=mydb.us-east-1.rds.amazonaws.com;Database=myapp;..."
```

## Monitoring and Logging

### CloudWatch Logs

```bash
# Tail logs in real-time
aws logs tail /ecs/aspnetwebfprojcontain06 --follow

# Filter logs
aws logs filter-log-events \
  --log-group-name /ecs/aspnetwebfprojcontain06 \
  --filter-pattern "ERROR"
```

### CloudWatch Metrics

ECS automatically publishes metrics:

- CPUUtilization
- MemoryUtilization
- NetworkRxBytes
- NetworkTxBytes

### Application Insights (Optional)

Add Application Insights for enhanced monitoring:

```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### Health Checks

The application exposes `/health` endpoint:

```bash
curl http://your-alb-dns/health
```

## Troubleshooting

### Common Issues

#### 1. Task Fails to Start

**Symptom**: Tasks transition from PENDING to STOPPED

**Solutions**:
- Check CloudWatch logs for errors
- Verify IAM execution role permissions
- Ensure image URI is correct
- Check CPU/memory limits

```bash
# Describe stopped task
aws ecs describe-tasks \
  --cluster aspnetwebfprojcontain06-cluster \
  --tasks task-id
```

#### 2. Network Connectivity Issues

**Symptom**: Cannot pull image from ECR or access external APIs

**Solutions**:
- Verify security group allows outbound traffic
- Check NAT Gateway (for private subnets)
- Ensure execution role has ECR permissions

#### 3. Health Check Failures

**Symptom**: Tasks fail ALB health checks

**Solutions**:
- Verify `/health` endpoint is accessible
- Check security group allows traffic from ALB
- Increase `healthCheckGracePeriodSeconds`
- Review application logs for startup errors

#### 4. Out of Memory Errors

**Symptom**: Tasks stop with memory errors

**Solutions**:
- Increase task memory allocation
- Optimize .NET memory settings
- Check for memory leaks

```bash
# Update service with more memory
aws ecs update-service \
  --cluster aspnetwebfprojcontain06-cluster \
  --service aspnetwebfprojcontain06-service \
  --task-definition aspnetwebfprojcontain06-task:2  # New revision with more memory
```

#### 5. Slow Startup Times

**Solutions**:
- Use ReadyToRun images for faster startup
- Implement proper health check grace period
- Pre-warm the application

### Debugging Commands

```bash
# View service events
aws ecs describe-services \
  --cluster aspnetwebfprojcontain06-cluster \
  --services aspnetwebfprojcontain06-service \
  --query 'services[0].events[:10]'

# Get task definition
aws ecs describe-task-definition \
  --task-definition aspnetwebfprojcontain06-task

# Check task status
aws ecs describe-tasks \
  --cluster aspnetwebfprojcontain06-cluster \
  --tasks task-id
```

## Security Considerations

### 1. Container Security

- Use official Microsoft base images
- Run as non-root user (implemented in Dockerfile)
- Scan images for vulnerabilities
- Keep base images updated

```bash
# Scan image with AWS ECR
aws ecr start-image-scan \
  --repository-name aspnetwebfprojcontain06 \
  --image-id imageTag=latest
```

### 2. Network Security

- Use private subnets for tasks
- Restrict security group rules
- Enable VPC Flow Logs
- Use AWS WAF with ALB

### 3. Secrets Management

- Never hardcode secrets in images
- Use AWS Secrets Manager or Parameter Store
- Rotate credentials regularly
- Use IAM roles for AWS service access

### 4. HTTPS Configuration

- Terminate SSL at ALB (recommended)
- Use ACM for certificate management
- Configure HSTS headers

```csharp
// Program.cs
app.UseHsts();
app.UseHttpsRedirection();
```

### 5. Authentication & Authorization

- Implement proper authentication (JWT, OAuth, etc.)
- Use authorization policies
- Enable CORS only for trusted origins

## Scaling and Performance

### Auto Scaling Configuration

#### Target Tracking Scaling

```bash
# Register scalable target
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/aspnetwebfprojcontain06-cluster/aspnetwebfprojcontain06-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10

# Create scaling policy
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/aspnetwebfprojcontain06-cluster/aspnetwebfprojcontain06-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-scaling-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json
```

**scaling-policy.json**:
```json
{
  "TargetValue": 70.0,
  "PredefinedMetricSpecification": {
    "PredefinedMetricType": "ECSServiceAverageCPUUtilization"
  },
  "ScaleOutCooldown": 60,
  "ScaleInCooldown": 60
}
```

### Performance Optimization

#### 1. .NET Runtime Optimizations

```dockerfile
# Use ReadyToRun for faster startup
RUN dotnet publish -c Release -o /app/publish -r linux-x64 --self-contained false /p:PublishReadyToRun=true
```

#### 2. Kestrel Configuration

```json
// appsettings.Production.json
{
  "Kestrel": {
    "Limits": {
      "MaxConcurrentConnections": 100,
      "MaxRequestBodySize": 10485760,
      "KeepAliveTimeout": "00:02:00"
    }
  }
}
```

#### 3. Response Caching

```csharp
// Program.cs
builder.Services.AddResponseCaching();
app.UseResponseCaching();
```

#### 4. Application Insights

Monitor performance with Application Insights:

```bash
dotnet add package Microsoft.ApplicationInsights.AspNetCore
```

### Blue/Green Deployments

```bash
# Deploy new version
aws ecs create-service \
  --cluster aspnetwebfprojcontain06-cluster \
  --service-name aspnetwebfprojcontain06-service-green \
  --task-definition aspnetwebfprojcontain06-task:2 \
  --desired-count 2 \
  --launch-type FARGATE

# Test green environment
# Switch traffic at ALB level
# Decommission blue environment
```

## Additional Resources

- [AWS ECS Documentation](https://docs.aws.amazon.com/ecs/)
- [AWS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/userguide/AWS_Fargate.html)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Docker Documentation](https://docs.docker.com/)
- [.NET Container Images](https://hub.docker.com/_/microsoft-dotnet)

## Support

For issues or questions:

1. Check CloudWatch logs for application errors
2. Review ECS service events
3. Consult AWS documentation
4. Contact your DevOps team

---

**Last Updated**: 2026-01-07
**Version**: 1.0.0