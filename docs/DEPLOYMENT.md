# Films Web Application - AWS ECS Fargate Deployment Guide

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Local Development Setup](#local-development-setup)
3. [Docker Deployment](#docker-deployment)
4. [AWS ECS Fargate Prerequisites](#aws-ecs-fargate-prerequisites)
5. [ECS Fargate Setup](#ecs-fargate-setup)
6. [ECS Task Definition Explained](#ecs-task-definition-explained)
7. [ECS Service Configuration](#ecs-service-configuration)
8. [Deployment Walkthrough](#deployment-walkthrough)
9. [Troubleshooting](#troubleshooting)
10. [Scaling and Management](#scaling-and-management)
11. [Security Considerations](#security-considerations)

---

## Prerequisites

### Required Software
- **.NET 8.0 SDK** or later
- **Docker Desktop** (for local development and building images)
- **AWS CLI v2** (for ECS deployment)
- **Git** (for version control)

### AWS Account Requirements
- Active AWS account with appropriate permissions
- IAM user with permissions for:
  - ECS (tasks, services, clusters)
  - ECR (repository management, image push/pull)
  - CloudWatch Logs (log group creation)
  - IAM (role creation and management)
  - EC2 (VPC, subnets, security groups)
  - Elastic Load Balancing (optional, for ALB)

### Verify Installations

```bash
# Check .NET version
dotnet --version
# Expected: 8.0.x or later

# Check Docker
docker --version
# Expected: Docker version 20.x or later

# Check AWS CLI
aws --version
# Expected: aws-cli/2.x or later

# Configure AWS credentials
aws configure
# Enter AWS Access Key ID, Secret Access Key, Region, and Output format
```

---

## Local Development Setup

### 1. Clone the Repository

```bash
git clone <repository-url>
cd ASP5
```

### 2. Restore Dependencies

```bash
dotnet restore Films.sln
```

### 3. Configure Database Connection

Update `src/Films.Web/appsettings.Development.json` with your local database connection:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=FilmsDb;User Id=sa;Password=YourPassword;MultipleActiveResultSets=True;TrustServerCertificate=True"
  }
}
```

### 4. Run Database Migrations (if applicable)

```bash
cd src/Films.Web
dotnet ef database update
```

### 5. Run the Application Locally

```bash
dotnet run --project src/Films.Web/Films.Web.csproj
```

Access the application at: `http://localhost:5000` or `https://localhost:5001`

---

## Docker Deployment

### Build Docker Image Locally

```bash
# From the repository root
docker build -f Dockerfile -t films-web:local .
```

### Run Container Locally

```bash
docker run -d \
  --name films-web \
  -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e DB_SERVER=host.docker.internal \
  -e DB_NAME=FilmsDb \
  -e DB_USER=sa \
  -e DB_PASSWORD=YourPassword \
  films-web:local
```

Access the application at: `http://localhost:8080`

### Using Docker Compose

```bash
# Start the application
docker-compose up -d

# View logs
docker-compose logs -f

# Stop the application
docker-compose down
```

---

## AWS ECS Fargate Prerequisites

### 1. VPC and Networking Setup

You need:
- **VPC** with at least 2 subnets in different Availability Zones
- **Internet Gateway** attached to VPC (for public access)
- **Route tables** configured for internet access
- **Security Group** allowing inbound traffic on port 8080

#### Create Security Group

```bash
# Create security group
aws ec2 create-security-group \
  --group-name films-web-sg \
  --description "Security group for Films web application" \
  --vpc-id vpc-xxxxxxxxx \
  --region us-east-1

# Allow inbound traffic on port 8080
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxxxxxx \
  --protocol tcp \
  --port 8080 \
  --cidr 0.0.0.0/0 \
  --region us-east-1

# Allow inbound traffic on port 80 (for ALB)
aws ec2 authorize-security-group-ingress \
  --group-id sg-xxxxxxxxx \
  --protocol tcp \
  --port 80 \
  --cidr 0.0.0.0/0 \
  --region us-east-1
```

### 2. IAM Roles

#### ECS Task Execution Role

Create `ecsTaskExecutionRole` with the following trust policy:

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

Attach managed policy: `AmazonECSTaskExecutionRolePolicy`

```bash
aws iam create-role \
  --role-name ecsTaskExecutionRole \
  --assume-role-policy-document file://trust-policy.json

aws iam attach-role-policy \
  --role-name ecsTaskExecutionRole \
  --policy-arn arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy
```

#### ECS Task Role (Optional)

Create `ecsTaskRole` for application-specific permissions (e.g., S3, DynamoDB access).

### 3. CloudWatch Log Group

Create log group for application logs:

```bash
aws logs create-log-group \
  --log-group-name /ecs/films-web \
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

### 2. Create ECR Repository

```bash
aws ecr create-repository \
  --repository-name films-web \
  --region us-east-1
```

### 3. Build and Push Docker Image

Use the provided scripts:

**Linux/macOS:**
```bash
chmod +x scripts/build-push.sh
./scripts/build-push.sh
```

**Windows:**
```cmd
scripts\build-push.bat
```

Follow the prompts to:
1. Select registry (AWS ECR or Docker Hub)
2. Enter registry credentials
3. Build and push the image

---

## ECS Task Definition Explained

### Key Components

#### 1. Launch Type and Network Mode
- **requiresCompatibilities**: `["FARGATE"]` - Specifies AWS Fargate launch type
- **networkMode**: `"awsvpc"` - Required for Fargate, provides ENI with private IP

#### 2. CPU and Memory

**Valid Fargate CPU/Memory Combinations:**

| CPU (vCPU) | Memory (MB) |
|------------|-------------|
| 256 (.25)  | 512, 1024, 2048 |
| 512 (.5)   | 1024, 2048, 3072, 4096 |
| 1024 (1)   | 2048-8192 (increments of 1024) |
| 2048 (2)   | 4096-16384 (increments of 1024) |
| 4096 (4)   | 8192-30720 (increments of 1024) |

**Default Configuration:**
- **cpu**: "512" (0.5 vCPU)
- **memory**: "1024" (1 GB)

#### 3. IAM Roles
- **executionRoleArn**: Allows ECS to pull images from ECR and send logs to CloudWatch
- **taskRoleArn**: Grants application-specific AWS permissions (optional)

#### 4. Container Definition
- **name**: Container name (used in service definition)
- **image**: Docker image URI from ECR
- **essential**: `true` - Task stops if this container fails
- **portMappings**: Port 8080 for application traffic
- **environment**: Application environment variables
- **logConfiguration**: CloudWatch Logs integration

---

## ECS Service Configuration

### Key Components

#### 1. Launch Type
- **launchType**: `"FARGATE"` - Uses serverless compute

#### 2. Network Configuration
- **awsvpcConfiguration**: Defines subnets, security groups, and public IP assignment
- **assignPublicIp**: `"ENABLED"` - Allows direct internet access (for development)

#### 3. Deployment Configuration
- **maximumPercent**: 200 - Allows up to 2x desired count during deployment
- **minimumHealthyPercent**: 50 - Maintains at least 50% of desired count during deployment

#### 4. Load Balancer Integration (Optional)
- **targetGroupArn**: ARN of Application Load Balancer target group
- **containerName**: Must match container name in task definition
- **containerPort**: Application port (8080)
- **healthCheckGracePeriodSeconds**: 300 - Time before health checks start

#### 5. Tags
- **enableECSManagedTags**: Automatically tags tasks
- **propagateTags**: "SERVICE" - Propagates service tags to tasks
- **tags**: Custom resource tags for organization and billing

---

## Deployment Walkthrough

### Step 1: Build and Push Image

```bash
# Linux/macOS
./scripts/build-push.sh

# Windows
scripts\build-push.bat
```

**Prompts:**
1. Image tag (default: `latest`)
2. Registry selection (1. AWS ECR, 2. Docker Hub)
3. Registry credentials

**Output:** Docker image URI (e.g., `123456789.dkr.ecr.us-east-1.amazonaws.com/films-web:latest`)

### Step 2: Deploy to ECS Fargate

```bash
# Linux/macOS
./scripts/deploy-image.sh

# Windows
scripts\deploy-image.bat
```

**Prompts:**
1. AWS region (e.g., `us-east-1`)
2. ECS cluster name (e.g., `films-cluster`)
3. VPC ID (e.g., `vpc-0abc123def456`)
4. Subnet IDs (comma-separated, e.g., `subnet-0abc123,subnet-0def456`)
5. Security group ID (e.g., `sg-0abc123def`)
6. Docker image URI (from Step 1)
7. Database configuration:
   - Server hostname
   - Database name
   - Username
   - Password
8. Load balancer requirement (y/n)

**Deployment Process:**
1. Creates/validates ECS cluster
2. Creates CloudWatch log group
3. Registers task definition
4. Creates/updates ALB and target group (if requested)
5. Creates/updates ECS service
6. Waits for service stability
7. Displays deployment status and URLs

### Step 3: Verify Deployment

```bash
# Check service status
aws ecs describe-services \
  --cluster films-cluster \
  --services films-web-service \
  --region us-east-1

# View running tasks
aws ecs list-tasks \
  --cluster films-cluster \
  --service-name films-web-service \
  --region us-east-1

# View logs
aws logs tail /ecs/films-web --follow --region us-east-1
```

### Step 4: Access Application

- **With Load Balancer:** `http://<alb-dns-name>`
- **Without Load Balancer:** Use task's public IP (found in ECS console or via CLI)

---

## Troubleshooting

### Task Failures

#### Symptom: Tasks fail to start

**Check CloudWatch Logs:**
```bash
aws logs tail /ecs/films-web --follow --region us-east-1
```

**Common Causes:**
1. **Image pull errors**: Verify ECR permissions and image URI
2. **Database connection errors**: Check security group rules and DB credentials
3. **Resource limits**: Ensure CPU/memory combination is valid

#### Symptom: Task starts but health checks fail

**Verify Health Endpoint:**
```bash
# SSH into task or use AWS Session Manager
curl http://localhost:8080/health
```

**Common Causes:**
1. Health endpoint not configured
2. Application startup time exceeds health check grace period
3. Database migration failures

### Network Issues

#### Symptom: Cannot access application

**Check Security Group Rules:**
```bash
aws ec2 describe-security-groups \
  --group-ids sg-xxxxxxxxx \
  --region us-east-1
```

**Verify:**
1. Inbound rule allows traffic on port 8080
2. Subnets have route to Internet Gateway
3. Public IP is assigned to tasks (if no ALB)

### CPU/Memory Errors

#### Symptom: Task stopped with error "OutOfMemory"

**Solution:** Increase memory in task definition:

```json
{
  "cpu": "1024",
  "memory": "2048"
}
```

**Valid combinations:** See [ECS Task Definition Explained](#ecs-task-definition-explained)

### Deployment Failures

#### Symptom: Service update fails

**Check Service Events:**
```bash
aws ecs describe-services \
  --cluster films-cluster \
  --services films-web-service \
  --region us-east-1 \
  --query 'services[0].events'
```

**Common Causes:**
1. Task definition registration failed
2. Service cannot reach desired count (capacity issues)
3. Target group health checks failing

---

## Scaling and Management

### Manual Scaling

```bash
# Scale to 4 tasks
aws ecs update-service \
  --cluster films-cluster \
  --service films-web-service \
  --desired-count 4 \
  --region us-east-1
```

### Service Auto Scaling

#### 1. Register Scalable Target

```bash
aws application-autoscaling register-scalable-target \
  --service-namespace ecs \
  --resource-id service/films-cluster/films-web-service \
  --scalable-dimension ecs:service:DesiredCount \
  --min-capacity 2 \
  --max-capacity 10 \
  --region us-east-1
```

#### 2. Create Scaling Policy (Target Tracking)

```bash
aws application-autoscaling put-scaling-policy \
  --service-namespace ecs \
  --resource-id service/films-cluster/films-web-service \
  --scalable-dimension ecs:service:DesiredCount \
  --policy-name cpu-scaling-policy \
  --policy-type TargetTrackingScaling \
  --target-tracking-scaling-policy-configuration file://scaling-policy.json \
  --region us-east-1
```

**scaling-policy.json:**
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

For zero-downtime deployments, use AWS CodeDeploy with ECS:

1. Create CodeDeploy application and deployment group
2. Configure task definition with multiple task sets
3. Deploy new version with traffic shifting

### Rolling Updates

```bash
# Force new deployment with updated task definition
aws ecs update-service \
  --cluster films-cluster \
  --service films-web-service \
  --task-definition films-web-task:2 \
  --force-new-deployment \
  --region us-east-1
```

---

## Security Considerations

### 1. Use Secrets Manager for Sensitive Data

Instead of environment variables, use AWS Secrets Manager:

```json
{
  "secrets": [
    {
      "name": "DB_PASSWORD",
      "valueFrom": "arn:aws:secretsmanager:us-east-1:123456789:secret:films-db-password"
    }
  ]
}
```

### 2. Enable Container Insights

```bash
aws ecs update-cluster-settings \
  --cluster films-cluster \
  --settings name=containerInsights,value=enabled \
  --region us-east-1
```

### 3. Use Private Subnets with NAT Gateway

For production:
1. Place tasks in private subnets
2. Use NAT Gateway for outbound internet access
3. Use ALB in public subnets for inbound traffic

### 4. Restrict Security Group Rules

```bash
# Allow traffic only from ALB security group
aws ec2 authorize-security-group-ingress \
  --group-id sg-task-sg \
  --protocol tcp \
  --port 8080 \
  --source-group sg-alb-sg \
  --region us-east-1
```

### 5. Enable VPC Flow Logs

```bash
aws ec2 create-flow-logs \
  --resource-type VPC \
  --resource-ids vpc-xxxxxxxxx \
  --traffic-type ALL \
  --log-destination-type cloud-watch-logs \
  --log-group-name /aws/vpc/flowlogs \
  --region us-east-1
```

### 6. Implement IAM Task Roles

Grant least-privilege permissions for application AWS API calls.

---

## .NET-Specific Recommendations

### 1. Configure ASP.NET Core for Production

- Enable HTTPS redirection
- Configure HSTS headers
- Use structured logging with Serilog
- Implement comprehensive health checks

### 2. Optimize Startup Performance

- Use ReadyToRun (R2R) compilation
- Enable assembly trimming for smaller images
- Configure Kestrel server limits

### 3. Memory Management

- Set appropriate GC settings for containerized environments
- Monitor memory usage with CloudWatch Container Insights
- Configure ASP.NET Core request limits

### 4. Monitoring and Observability

- Integrate Application Insights for telemetry
- Configure custom metrics for business KPIs
- Use distributed tracing for microservices

### 5. Database Migrations

- Run migrations as separate ECS tasks (not in application startup)
- Use AWS Systems Manager Run Command for migration tasks
- Implement idempotent migration scripts

---

## Additional Resources

- [AWS ECS Documentation](https://docs.aws.amazon.com/ecs/)
- [AWS Fargate Documentation](https://docs.aws.amazon.com/AmazonECS/latest/developerguide/AWS_Fargate.html)
- [.NET on AWS](https://aws.amazon.com/developer/language/net/)
- [ASP.NET Core Best Practices](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/best-practices)

---

## Support

For issues or questions:
1. Check CloudWatch Logs: `/ecs/films-web`
2. Review ECS service events
3. Verify security group and network configuration
4. Consult AWS documentation and support

---

**Document Version:** 1.0  
**Last Updated:** 2025-12-31  
**Target Platform:** AWS ECS Fargate  
**Application:** Films Web (.NET 8.0)