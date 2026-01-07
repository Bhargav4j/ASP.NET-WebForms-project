@echo off
setlocal enabledelayedexpansion

echo =====================================
echo AWS ECS Fargate Deployment Script
echo =====================================
echo.

set PROJECT_NAME=aspnetwebfprojcontain06
set TASK_FAMILY=!PROJECT_NAME!-task
set SERVICE_NAME=!PROJECT_NAME!-service
set CONTAINER_NAME=!PROJECT_NAME!
set CONTAINER_PORT=8080

set /p AWS_REGION="Enter AWS region (e.g., us-east-1): "
set /p CLUSTER_NAME="Enter ECS cluster name (e.g., my-ecs-cluster): "

echo.
echo === Network Configuration ===
set /p VPC_ID="Enter VPC ID (e.g., vpc-0abc123def456): "
set /p SUBNET_IDS="Enter Subnet IDs comma-separated (e.g., subnet-0abc123,subnet-0def456): "
set /p SECURITY_GROUP="Enter Security Group ID (e.g., sg-0abc123def): "

echo.
set /p IMAGE_URI="Enter Docker image URI (e.g., 123456789.dkr.ecr.us-east-1.amazonaws.com/app:latest): "

for /f "tokens=1,2 delims=," %%a in ("!SUBNET_IDS!") do (
    set SUBNET_1=%%a
    set SUBNET_2=%%b
)

if "!SUBNET_2!"=="" set SUBNET_2=!SUBNET_1!

echo.
echo Getting AWS Account ID...
for /f "delims=" %%i in ('aws sts get-caller-identity --query Account --output text') do set ACCOUNT_ID=%%i
echo Account ID: !ACCOUNT_ID!

echo.
echo Checking if ECS cluster exists...
aws ecs describe-clusters --clusters !CLUSTER_NAME! --region !AWS_REGION! >nul 2>&1
if !ERRORLEVEL! neq 0 (
    echo Creating ECS cluster: !CLUSTER_NAME!
    aws ecs create-cluster --cluster-name !CLUSTER_NAME! --region !AWS_REGION!
)

echo.
set /p NEED_LB="Do you need a load balancer for this service? (y/n): "

if /i "!NEED_LB!"=="y" (
    echo.
    echo === Creating Application Load Balancer ===
    
    set LB_NAME=!PROJECT_NAME!-alb
    set TG_NAME=!PROJECT_NAME!-tg
    
    echo Creating Application Load Balancer: !LB_NAME!
    for /f "delims=" %%i in ('aws elbv2 create-load-balancer --name !LB_NAME! --subnets !SUBNET_1! !SUBNET_2! --security-groups !SECURITY_GROUP! --scheme internet-facing --type application --ip-address-type ipv4 --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text 2^>nul') do set LB_ARN=%%i
    
    if "!LB_ARN!"=="" (
        for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --names !LB_NAME! --region !AWS_REGION! --query "LoadBalancers[0].LoadBalancerArn" --output text') do set LB_ARN=%%i
    )
    
    echo Load Balancer ARN: !LB_ARN!
    
    echo Creating Target Group: !TG_NAME!
    for /f "delims=" %%i in ('aws elbv2 create-target-group --name !TG_NAME! --protocol HTTP --port !CONTAINER_PORT! --vpc-id !VPC_ID! --target-type ip --health-check-enabled --health-check-protocol HTTP --health-check-path "/health" --health-check-interval-seconds 30 --health-check-timeout-seconds 5 --healthy-threshold-count 2 --unhealthy-threshold-count 3 --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text 2^>nul') do set TARGET_GROUP_ARN=%%i
    
    if "!TARGET_GROUP_ARN!"=="" (
        for /f "delims=" %%i in ('aws elbv2 describe-target-groups --names !TG_NAME! --region !AWS_REGION! --query "TargetGroups[0].TargetGroupArn" --output text') do set TARGET_GROUP_ARN=%%i
    )
    
    echo Target Group ARN: !TARGET_GROUP_ARN!
    
    echo Creating listener on port 80...
    aws elbv2 create-listener --load-balancer-arn !LB_ARN! --protocol HTTP --port 80 --default-actions Type=forward,TargetGroupArn=!TARGET_GROUP_ARN! --region !AWS_REGION! >nul 2>&1
    
    for /f "delims=" %%i in ('aws elbv2 describe-load-balancers --load-balancer-arns !LB_ARN! --region !AWS_REGION! --query "LoadBalancers[0].DNSName" --output text') do set LB_DNS=%%i
    echo Load Balancer DNS: !LB_DNS!
) else (
    set TARGET_GROUP_ARN=
)

echo.
echo === Preparing Task Definition ===
copy ecs\task-definition.json ecs\task-definition-resolved.json >nul

powershell -Command "(Get-Content ecs\task-definition-resolved.json) -replace '{{IMAGE_URI}}', '!IMAGE_URI!' | Set-Content ecs\task-definition-resolved.json"
powershell -Command "(Get-Content ecs\task-definition-resolved.json) -replace '{{AWS_REGION}}', '!AWS_REGION!' | Set-Content ecs\task-definition-resolved.json"
powershell -Command "(Get-Content ecs\task-definition-resolved.json) -replace '{{ACCOUNT_ID}}', '!ACCOUNT_ID!' | Set-Content ecs\task-definition-resolved.json"

echo Registering task definition...
for /f "delims=" %%i in ('aws ecs register-task-definition --cli-input-json file://ecs/task-definition-resolved.json --region !AWS_REGION! --query "taskDefinition.taskDefinitionArn" --output text') do set TASK_DEF_ARN=%%i

echo Task Definition ARN: !TASK_DEF_ARN!

echo.
echo === Creating CloudWatch Log Group ===
set LOG_GROUP=/ecs/!PROJECT_NAME!
aws logs create-log-group --log-group-name !LOG_GROUP! --region !AWS_REGION! 2>nul

echo.
echo === Preparing Service Definition ===
copy ecs\service-definition.json ecs\service-definition-resolved.json >nul

powershell -Command "(Get-Content ecs\service-definition-resolved.json) -replace '{{CLUSTER_NAME}}', '!CLUSTER_NAME!' | Set-Content ecs\service-definition-resolved.json"
powershell -Command "(Get-Content ecs\service-definition-resolved.json) -replace '{{SUBNET_1}}', '!SUBNET_1!' | Set-Content ecs\service-definition-resolved.json"
powershell -Command "(Get-Content ecs\service-definition-resolved.json) -replace '{{SUBNET_2}}', '!SUBNET_2!' | Set-Content ecs\service-definition-resolved.json"
powershell -Command "(Get-Content ecs\service-definition-resolved.json) -replace '{{SECURITY_GROUP}}', '!SECURITY_GROUP!' | Set-Content ecs\service-definition-resolved.json"

if "!TARGET_GROUP_ARN!"=="" (
    powershell -Command "$json = Get-Content ecs\service-definition-resolved.json | ConvertFrom-Json; $json.PSObject.Properties.Remove('loadBalancers'); $json.PSObject.Properties.Remove('healthCheckGracePeriodSeconds'); $json | ConvertTo-Json -Depth 10 | Set-Content ecs\service-definition-resolved.json"
) else (
    powershell -Command "(Get-Content ecs\service-definition-resolved.json) -replace '{{TARGET_GROUP_ARN}}', '!TARGET_GROUP_ARN!' | Set-Content ecs\service-definition-resolved.json"
)

echo.
echo === Deploying Service ===

for /f "delims=" %%i in ('aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].serviceName" --output text 2^>nul') do set SERVICE_EXISTS=%%i

if "!SERVICE_EXISTS!"=="!SERVICE_NAME!" (
    echo Updating existing service: !SERVICE_NAME!
    aws ecs update-service --cluster !CLUSTER_NAME! --service !SERVICE_NAME! --task-definition !TASK_DEF_ARN! --region !AWS_REGION! --force-new-deployment
) else (
    echo Creating new service: !SERVICE_NAME!
    aws ecs create-service --cli-input-json file://ecs/service-definition-resolved.json --region !AWS_REGION!
)

echo.
echo Waiting for service to become stable...
aws ecs wait services-stable --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION!

echo.
echo =====================================
echo Deployment completed successfully!
echo =====================================
echo.
echo Service Details:
aws ecs describe-services --cluster !CLUSTER_NAME! --services !SERVICE_NAME! --region !AWS_REGION! --query "services[0].{Name:serviceName,Status:status,Running:runningCount,Desired:desiredCount}"

if not "!LB_DNS!"=="" (
    echo.
    echo Application URL: http://!LB_DNS!
)

echo.
echo CloudWatch Logs: !LOG_GROUP!
echo.
echo To view logs, run:
echo aws logs tail !LOG_GROUP! --follow --region !AWS_REGION!

del ecs\task-definition-resolved.json ecs\service-definition-resolved.json 2>nul

endlocal