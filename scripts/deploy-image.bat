@echo off
setlocal enabledelayedexpansion

echo ============================================
echo   Films Web - AWS ECS Fargate Deployment
echo ============================================
echo.

set PROJECT_NAME=films-web
set TASK_FAMILY=films-web-task
set SERVICE_NAME=films-web-service

echo === AWS Configuration ===
set /p AWS_REGION="Enter AWS region (e.g. us-east-1): "
set AWS_DEFAULT_REGION=!AWS_REGION!

echo.
echo === ECS Cluster Configuration ===
set /p CLUSTER_NAME="Enter ECS cluster name (e.g. films-cluster): "

echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Cluster does not exist. Creating cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
    echo Cluster created successfully.
)

echo.
echo === Network Configuration ===
set /p VPC_ID="Enter VPC ID (e.g. vpc-0abc123def456): "
set /p SUBNET_IDS="Enter Subnet IDs comma-separated (e.g. subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g. sg-0abc123def): "

for /f "tokens=1,2 delims=," %%a in ("!SUBNET_IDS!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)
if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
echo === Docker Image Configuration ===
set /p IMAGE_URI="Enter Docker image URI (e.g. 123456789.dkr.ecr.us-east-1.amazonaws.com/films:latest): "

echo.
echo === Database Configuration ===
set /p DB_SERVER="Enter database server (e.g. films-db.abc123.us-east-1.rds.amazonaws.com): "
set /p DB_NAME="Enter database name (default FilmsDb): "
if "!DB_NAME!"=="" set DB_NAME=FilmsDb
set /p DB_USER="Enter database user (default admin): "
if "!DB_USER!"=="" set DB_USER=admin
set /p DB_PASSWORD="Enter database password: "

echo.
echo === Load Balancer Configuration ===
set /p NEEDS_LB="Do you need a load balancer for this service? (y/n): "

if /i "!NEEDS_LB!"=="y" (
    echo Creating Application Load Balancer...
    
    set ALB_NAME=films-web-alb
    for /f "delims=" %%a in ('aws elbv2 create-load-balancer --name !ALB_NAME! --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --scheme internet-facing --type application --ip-address-type ipv4 --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set ALB_ARN=%%a
    if "!ALB_ARN!"=="" (
        for /f "delims=" %%a in ('aws elbv2 describe-load-balancers --names !ALB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text') do set ALB_ARN=%%a
    )
    
    echo Load Balancer ARN: !ALB_ARN!
    
    set TG_NAME=films-web-tg
    for /f "delims=" %%a in ('aws elbv2 create-target-group --name !TG_NAME! --protocol HTTP --port 8080 --vpc-id !VPC_ID! --target-type ip --health-check-path "/health" --health-check-interval-seconds 30 --health-check-timeout-seconds 5 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%a
    if "!TARGET_GROUP_ARN!"=="" (
        for /f "delims=" %%a in ('aws elbv2 describe-target-groups --names !TG_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%a
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    aws elbv2 create-listener --load-balancer-arn !ALB_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul 2>&1
    
    for /f "delims=" %%a in ('aws elbv2 describe-load-balancers --load-balancer-arns !ALB_ARN! --region !AWS_REGION! --query "LoadBalancers[0].DNSName" --output text') do set ALB_DNS=%%a
    echo Load Balancer DNS: !ALB_DNS!
) else (
    set TARGET_GROUP_ARN=
    echo Skipping load balancer configuration.
)

echo.
echo Getting AWS Account ID...
for /f "delims=" %%a in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%a
echo Account ID: !ACCOUNT_ID!

echo.
echo Creating CloudWatch log group...
aws logs create-log-group --log-group-name "/ecs/!PROJECT_NAME!" --region !AWS_REGION! >nul 2>&1

echo.
echo Preparing task definition...
copy ecs\task-definition.json %TEMP%\task-definition.json >nul

powershell -Command "(Get-Content %TEMP%\task-definition.json) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' -replace '{{AWS_REGION}}', '!AWS_REGION!' -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' -replace '{{DB_SERVER}}', '!DB_SERVER!' -replace '{{DB_NAME}}', '!DB_NAME!' -replace '{{DB_USER}}', '!DB_USER!' -replace '{{DB_PASSWORD}}', '!DB_PASSWORD!' | Set-Content %TEMP%\task-definition.json"

echo Registering task definition...
for /f "delims=" %%a in ('aws ecs register-task-definition --cli-input-json file://%TEMP%\task-definition.json --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%a
echo Task Definition ARN: !TASK_DEF_ARN!

echo.
echo Preparing service definition...
copy ecs\service-definition.json %TEMP%\service-definition.json >nul

powershell -Command "(Get-Content %TEMP%\service-definition.json) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' -replace '{{SUBNET_1}}', '!SUBNET_1!' -replace '{{SUBNET_2}}', '!SUBNET_2!' -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content %TEMP%\service-definition.json"

if /i not "!NEEDS_LB!"=="y" (
    powershell -Command "$json = Get-Content %TEMP%\service-definition.json | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content %TEMP%\service-definition.json"
)

echo.
echo Checking if service exists...
for /f "delims=" %%a in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].serviceName" --output text 2^>nul') do set EXISTING_SERVICE=%%a

if "!EXISTING_SERVICE!"=="!SERVICE_NAME!" (
    echo Service exists. Updating service...
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --force-new-deployment --region !AWS_REGION!
) else (
    echo Service does not exist. Creating service...
    aws ecs create-service --cli-input-json file://%TEMP%\service-definition.json --region !AWS_REGION!
)

echo.
echo Waiting for service to become stable...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo ============================================
echo   DEPLOYMENT SUCCESSFUL!
echo ============================================
echo Cluster: !CLUSTER_NAME!
echo Service: !SERVICE_NAME!
echo Task Definition: !TASK_DEF_ARN!

if /i "!NEEDS_LB!"=="y" (
    echo Load Balancer URL: http://!ALB_DNS!
    echo.
    echo Access your application at: http://!ALB_DNS!
)

echo.
echo CloudWatch Logs: /ecs/!PROJECT_NAME!
echo.
echo To view logs:
echo   aws logs tail /ecs/!PROJECT_NAME! --follow --region !AWS_REGION!
echo.
echo To check service status:
echo   aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!
echo ============================================

endlocal