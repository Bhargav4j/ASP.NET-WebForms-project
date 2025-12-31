@echo off
setlocal enabledelayedexpansion

REM AWS ECS Fargate Deployment Script for Films Application
echo ================================================
echo AWS ECS Fargate Deployment Script
echo ================================================
echo.

REM Configuration
set PROJECT_NAME=films
set TASK_FAMILY=films-task
set SERVICE_NAME=films-service
set CONTAINER_NAME=films-app
set CONTAINER_PORT=8080

REM Prompt for AWS configuration
echo === AWS Configuration ===
set /p AWS_REGION="Enter AWS region (e.g., us-east-1): "
set AWS_DEFAULT_REGION=!AWS_REGION!

REM Get AWS Account ID
echo.
echo Retrieving AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i

if "!ACCOUNT_ID!"=="" (
    echo ERROR: Failed to retrieve AWS Account ID. Please check your AWS credentials.
    exit /b 1
)

echo AWS Account ID: !ACCOUNT_ID!
echo.

REM Prompt for ECS cluster
set /p CLUSTER_NAME="Enter ECS cluster name (e.g., films-cluster): "

REM Check if cluster exists
echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1

if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating ECS cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Failed to create ECS cluster
        exit /b 1
    )
    
    echo ECS cluster created successfully
) else (
    echo ECS cluster already exists
)

echo.

REM Prompt for network configuration
echo === Network Configuration ===
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNETS_INPUT="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "

REM Parse subnets
for /f "tokens=1,2 delims=," %%a in ("!SUBNETS_INPUT!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)

if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
echo VPC ID: !VPC_ID!
echo Subnet 1: !SUBNET_1!
echo Subnet 2: !SUBNET_2!
echo Security Group: !SECURITY_GROUP!
echo.

REM Prompt for Docker image URI
set /p IMAGE_URI="Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/films:latest): "

echo.
echo Docker Image: !IMAGE_URI!
echo.

REM Ask about load balancer
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

set TARGET_GROUP_ARN=
if /i "!NEED_LB!"=="y" (
    echo.
    echo === Load Balancer Configuration ===
    echo Creating Application Load Balancer...
    
    set ALB_NAME=!PROJECT_NAME!-alb
    echo Creating ALB: !ALB_NAME!
    
    for /f "delims=" %%i in ('aws elbv2 create-load-balancer --name !ALB_NAME! --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --scheme internet-facing --type application --ip-address-type ipv4 --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%i
    
    if "!ALB_ARN!"=="" (
        echo WARNING: Failed to create new ALB. It may already exist.
        for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names !ALB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%i
    )
    
    echo ALB ARN: !ALB_ARN!
    
    set TG_NAME=!PROJECT_NAME!-tg
    echo Creating Target Group: !TG_NAME! (target-type: ip)
    
    for /f "delims=" %%i in ('aws elbv2 create-target-group --name !TG_NAME! --protocol HTTP --port !CONTAINER_PORT! --vpc-id !VPC_ID! --target-type ip --health-check-enabled --health-check-protocol HTTP --health-check-path "/health" --health-check-interval-seconds 30 --health-check-timeout-seconds 5 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    if "!TARGET_GROUP_ARN!"=="" (
        echo WARNING: Failed to create new Target Group. It may already exist.
        for /f "delims=" %%i in ('aws elbv2 describe-target-groups --names !TG_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    echo Creating ALB Listener...
    aws elbv2 create-listener --load-balancer-arn !ALB_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul 2>&1
    
    echo Load Balancer configuration completed
    echo.
) else (
    echo Skipping load balancer configuration
)

REM Create temporary directory
set TEMP_DIR=%TEMP%\ecs-deploy-%RANDOM%
mkdir !TEMP_DIR!

echo === Preparing Task Definition ===

REM Copy and modify task definition
copy ecs\task-definition.json !TEMP_DIR!\task-definition.json >nul

REM Replace placeholders using PowerShell
powershell -Command "(Get-Content '!TEMP_DIR!\task-definition.json') -replace '{{IMAGE_URI}}', '!IMAGE_URI!' -replace '{{AWS_REGION}}', '!AWS_REGION!' -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content '!TEMP_DIR!\task-definition.json'"

echo Task definition prepared
echo.

REM Register task definition
echo === Registering Task Definition ===
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://!TEMP_DIR!\task-definition.json --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%i

if "!TASK_DEF_ARN!"=="" (
    echo ERROR: Failed to register task definition
    rmdir /s /q !TEMP_DIR!
    exit /b 1
)

echo Task definition registered: !TASK_DEF_ARN!
echo.

REM Prepare service definition
echo === Preparing Service Definition ===

copy ecs\service-definition.json !TEMP_DIR!\service-definition.json >nul

REM Replace placeholders
powershell -Command "(Get-Content '!TEMP_DIR!\service-definition.json') -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' -replace '{{SUBNET_1}}', '!SUBNET_1!' -replace '{{SUBNET_2}}', '!SUBNET_2!' -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content '!TEMP_DIR!\service-definition.json'"

if "!TARGET_GROUP_ARN!"=="" (
    powershell -Command "$json = Get-Content '!TEMP_DIR!\service-definition.json' | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content '!TEMP_DIR!\service-definition.json'"
)

echo Service definition prepared
echo.

REM Check if service exists
echo === Checking Service Status ===
for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].serviceName" --output text 2^>nul') do set SERVICE_EXISTS=%%i

if "!SERVICE_EXISTS!"=="!SERVICE_NAME!" (
    echo Service exists. Updating service...
    
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --force-new-deployment --region !AWS_REGION! >nul
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Failed to update service
        rmdir /s /q !TEMP_DIR!
        exit /b 1
    )
    
    echo Service updated successfully
) else (
    echo Service does not exist. Creating new service...
    
    aws ecs create-service --cli-input-json file://!TEMP_DIR!\service-definition.json --region !AWS_REGION! >nul
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Failed to create service
        rmdir /s /q !TEMP_DIR!
        exit /b 1
    )
    
    echo Service created successfully
)

echo.

REM Wait for service stability
echo === Waiting for Service Stability ===
echo This may take a few minutes...
echo.

aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

if !ERRORLEVEL! neq 0 (
    echo WARNING: Service did not stabilize within the expected time
    echo Check the ECS console for more details
) else (
    echo Service is stable
)

echo.

REM Verify deployment
echo === Deployment Verification ===

for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].runningCount" --output text') do set RUNNING_COUNT=%%i

for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].desiredCount" --output text') do set DESIRED_COUNT=%%i

echo Running Tasks: !RUNNING_COUNT! / !DESIRED_COUNT!
echo.

if not "!ALB_ARN!"=="" (
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns !ALB_ARN! --region !AWS_REGION! --query "LoadBalancers[0].DNSName" --output text') do set ALB_DNS=%%i
    
    echo Application URL: http://!ALB_DNS!
    echo Health Check: http://!ALB_DNS!/health
    echo.
)

echo CloudWatch Logs: /ecs/!PROJECT_NAME!-app
echo View logs: aws logs tail /ecs/!PROJECT_NAME!-app --follow --region !AWS_REGION!
echo.

echo ================================================
echo Deployment completed successfully!
echo ================================================
echo.
echo Next steps:
echo 1. Verify the service is running: aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!
echo 2. Check task status: aws ecs list-tasks --cluster !CLUSTER_NAME! --service-name !SERVICE_NAME! --region !AWS_REGION!
echo 3. View logs: aws logs tail /ecs/!PROJECT_NAME!-app --follow --region !AWS_REGION!

if not "!ALB_DNS!"=="" (
    echo 4. Access application: http://!ALB_DNS!
)

echo.

REM Cleanup
rmdir /s /q !TEMP_DIR!

endlocal