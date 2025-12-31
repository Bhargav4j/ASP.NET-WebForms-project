@echo off
setlocal enabledelayedexpansion

echo ============================================
echo   Films Web Application - Docker Build
echo ============================================
echo.

set PROJECT_NAME=Films

REM Sanitize project name
set IMAGE_NAME=!PROJECT_NAME!
for %%i in (A B C D E F G H I J K L M N O P Q R S T U V W X Y Z) do set IMAGE_NAME=!IMAGE_NAME:%%i=%%i!
for /f "delims=" %%a in ('powershell -Command "'!IMAGE_NAME!'.ToLower() -replace '[^a-z0-9]+', '-' -replace '^-+', '' -replace '-+$', ''"') do set IMAGE_NAME=%%a

set /p IMAGE_TAG="Enter image tag (default latest): "
if "!IMAGE_TAG!"==" " set IMAGE_TAG=latest
for /f "delims=" %%a in ('powershell -Command "'!IMAGE_TAG!'.ToLower() -replace '[^a-z0-9.-]+', '-' -replace '^-+', '' -replace '-+$', ''"') do set IMAGE_TAG=%%a

echo.
echo Select container registry:
echo 1. AWS ECR
echo 2. Docker Hub
set /p REGISTRY_CHOICE="Enter choice (1 or 2): "

if "!REGISTRY_CHOICE!"=="1" (
    echo.
    echo === AWS ECR Configuration ===
    set /p AWS_REGION="Enter AWS Region (e.g. us-east-1): "
    set /p AWS_ACCOUNT_ID="Enter AWS Account ID: "
    set /p ECR_REPO="Enter ECR Repository Name (default !IMAGE_NAME!): "
    if "!ECR_REPO!"=="" set ECR_REPO=!IMAGE_NAME!
    
    set REGISTRY_URL=!AWS_ACCOUNT_ID!.dkr.ecr.!AWS_REGION!.amazonaws.com
    set FULL_IMAGE_NAME=!REGISTRY_URL!/!ECR_REPO!:!IMAGE_TAG!
    
    echo.
    echo Authenticating with AWS ECR...
    aws ecr get-login-password --region !AWS_REGION! | docker login --username AWS --password-stdin !REGISTRY_URL!
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: ECR login failed
        exit /b 1
    )
    
    echo Checking if ECR repository exists...
    aws ecr describe-repositories --repository-names !ECR_REPO! --region !AWS_REGION! >nul 2>&1
    if !ERRORLEVEL! neq 0 (
        echo Creating ECR repository: !ECR_REPO!
        aws ecr create-repository --repository-name !ECR_REPO! --region !AWS_REGION!
    )
    
) else if "!REGISTRY_CHOICE!"=="2" (
    echo.
    echo === Docker Hub Configuration ===
    set /p DOCKER_USERNAME="Enter Docker Hub username: "
    set /p DOCKER_PASSWORD="Enter Docker Hub password/token: "
    
    set FULL_IMAGE_NAME=!DOCKER_USERNAME!/!IMAGE_NAME!:!IMAGE_TAG!
    
    echo Authenticating with Docker Hub...
    echo !DOCKER_PASSWORD! | docker login --username !DOCKER_USERNAME! --password-stdin
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Docker Hub login failed
        exit /b 1
    )
) else (
    echo Invalid choice. Exiting.
    exit /b 1
)

echo.
echo ============================================
echo Building Docker image: !FULL_IMAGE_NAME!
echo ============================================

docker build -f Dockerfile -t !FULL_IMAGE_NAME! .

if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker build failed
    exit /b 1
)

echo.
echo ============================================
echo Pushing image to registry...
echo ============================================

docker push !FULL_IMAGE_NAME!

if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker push failed
    exit /b 1
)

echo.
echo ============================================
echo SUCCESS!
echo ============================================
echo Image: !FULL_IMAGE_NAME!
for /f "tokens=1 delims=/" %%a in ("!FULL_IMAGE_NAME!") do echo Registry: %%a
echo.
echo Next steps:
echo 1. Update ECS task definition with image URI
echo 2. Run deploy-image.bat to deploy to ECS
echo ============================================

endlocal