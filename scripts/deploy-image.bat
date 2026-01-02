@echo off
setlocal enabledelayedexpansion

echo =====================================
echo Films App - AWS ECS Fargate Deployment
echo =====================================
echo.

set PROJECT_NAME=films-app
set TASK_FAMILY=films-app-task
set SERVICE_NAME=films-app-service

set /p AWS_REGION="Enter AWS Region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS Cluster Name (e.g., films-cluster): "

echo.
echo --- Network Configuration ---
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
set /p IMAGE_URI="Enter Docker Image URI: "

echo.
echo --- Database Configuration ---
set /p DB_SERVER="Enter Database Server: "
set /p DB_NAME="Enter Database Name (default: filmsdb): "
if "!DB_NAME!"=="" set DB_NAME=filmsdb
set /p DB_USER="Enter Database User (default: filmsuser): "
if "!DB_USER!"=="" set DB_USER=filmsuser
set /p DB_PASSWORD="Enter Database Password: "

echo.
echo Getting AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo Account ID: !ACCOUNT_ID!

echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating ECS cluster...
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Failed to create ECS cluster
        exit /b 1
    )
)

echo.
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

set TARGET_GROUP_ARN=
if /i "!NEED_LB!"=="y" (
    echo.
    echo Creating Application Load Balancer and Target Group...
    
    set ALB_NAME=!PROJECT_NAME!-alb
    echo Creating ALB: !ALB_NAME!
    for /f "delims=" %%i in ('aws elbv2 create-load-balancer --name !ALB_NAME! --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --scheme internet-facing --type application --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%i
    
    if "!ALB_ARN!"=="" (
        echo ALB may already exist, retrieving ARN...
        for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names !ALB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text') do set ALB_ARN=%%i
    )
    
    set TG_NAME=!PROJECT_NAME!-tg
    echo Creating Target Group: !TG_NAME!
    for /f "delims=" %%i in ('aws elbv2 create-target-group --name !TG_NAME! --protocol HTTP --port 8080 --vpc-id !VPC_ID! --target-type ip --health-check-enabled --health-check-path "/health" --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    if "!TARGET_GROUP_ARN!"=="" (
        echo Target Group may already exist, retrieving ARN...
        for /f "delims=" %%i in ('aws elbv2 describe-target-groups --names !TG_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%i
    )
    
    echo Creating ALB Listener...
    aws elbv2 create-listener --load-balancer-arn !ALB_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul 2>&1
    
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns !ALB_ARN! --region !AWS_REGION! --query "LoadBalancers[0].DNSName" --output text') do set ALB_DNS=%%i
    echo Load Balancer DNS: !ALB_DNS!
)

echo.
echo Creating CloudWatch Log Group...
aws logs create-log-group --log-group-name "/ecs/films-app" --region !AWS_REGION! 2>nul

echo.
echo Preparing ECS task definition...

set TEMP_TASK_DEF=%TEMP%\task-definition.json
copy ecs\task-definition.json !TEMP_TASK_DEF! >nul

powershell -Command "(Get-Content '!TEMP_TASK_DEF!') -replace '{{IMAGE_URI}}', '!IMAGE_URI!' -replace '{{AWS_REGION}}', '!AWS_REGION!' -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' -replace '{{DB_SERVER}}', '!DB_SERVER!' -replace '{{DB_NAME}}', '!DB_NAME!' -replace '{{DB_USER}}', '!DB_USER!' -replace '{{DB_PASSWORD}}', '!DB_PASSWORD!' | Set-Content '!TEMP_TASK_DEF!'"

echo Registering ECS task definition...
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://!TEMP_TASK_DEF! --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%i

if "!TASK_DEF_ARN!"=="" (
    echo ERROR: Failed to register task definition
    del !TEMP_TASK_DEF!
    exit /b 1
)

echo Task Definition ARN: !TASK_DEF_ARN!
del !TEMP_TASK_DEF!

echo.
echo Preparing ECS service definition...

set TEMP_SERVICE_DEF=%TEMP%\service-definition.json
copy ecs\service-definition.json !TEMP_SERVICE_DEF! >nul

powershell -Command "(Get-Content '!TEMP_SERVICE_DEF!') -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' -replace '{{SUBNET_1}}', '!SUBNET_1!' -replace '{{SUBNET_2}}', '!SUBNET_2!' -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content '!TEMP_SERVICE_DEF!'"

if "!TARGET_GROUP_ARN!"=="" (
    powershell -Command "$json = Get-Content '!TEMP_SERVICE_DEF!' | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content '!TEMP_SERVICE_DEF!'"
)

echo Checking if service exists...
for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].serviceName" --output text 2^>nul') do set SERVICE_EXISTS=%%i

if "!SERVICE_EXISTS!"=="None" (
    echo Creating new ECS service...
    aws ecs create-service --cluster !CLUSTER_NAME! --service-name !SERVICE_NAME! --cli-input-json file://!TEMP_SERVICE_DEF! --region !AWS_REGION!
) else (
    echo Updating existing ECS service...
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --force-new-deployment --region !AWS_REGION!
)

del !TEMP_SERVICE_DEF!

echo.
echo Waiting for service to become stable...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo =====================================
echo Deployment Completed Successfully
echo =====================================
echo.
echo Service Details:
aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].[serviceName,status,runningCount,desiredCount]" --output table

if not "!ALB_DNS!"=="" (
    echo.
    echo Application URL: http://!ALB_DNS!
    echo Health Check: http://!ALB_DNS!/health
)

echo.
echo CloudWatch Logs: /ecs/films-app
echo.

endlocal