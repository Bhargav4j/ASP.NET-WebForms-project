@echo off
setlocal enabledelayedexpansion

REM Script to build and push Docker image for Films application
echo ================================================
echo Docker Image Build and Push Script
echo ================================================
echo.

REM Set project name
set PROJECT_NAME=films

REM Sanitize image name using PowerShell
for /f "delims=" %%i in ('powershell -Command "'!PROJECT_NAME!'.ToLower() -replace '[^a-z0-9]+', '-' -replace '^-+^|-+$', ''"') do set IMAGE_NAME=%%i

echo Project: !PROJECT_NAME!
echo Sanitized Image Name: !IMAGE_NAME!
echo.

REM Prompt for image tag
set /p IMAGE_TAG="Enter image tag (default: latest): "
if "!IMAGE_TAG!"=="" set IMAGE_TAG=latest

REM Sanitize tag using PowerShell
for /f "delims=" %%i in ('powershell -Command "'!IMAGE_TAG!'.ToLower() -replace '[^a-z0-9.-]+', '-' -replace '^-+^|-+$', ''"') do set IMAGE_TAG=%%i

if "!IMAGE_TAG!"=="" set IMAGE_TAG=latest

echo Image Tag: !IMAGE_TAG!
echo.

REM Select registry type
echo Select container registry:
echo 1. AWS ECR (Elastic Container Registry)
echo 2. Docker Hub
set /p REGISTRY_CHOICE="Enter choice (1 or 2): "

if "!REGISTRY_CHOICE!"=="1" (
    echo.
    echo === AWS ECR Configuration ===
    
    REM Prompt for AWS region
    set /p AWS_REGION="Enter AWS region (e.g., us-east-1): "
    
    REM Prompt for AWS account ID
    set /p AWS_ACCOUNT_ID="Enter AWS Account ID: "
    
    REM Set ECR repository name
    set ECR_REPO=!IMAGE_NAME!
    
    REM Build registry URL
    set REGISTRY_URL=!AWS_ACCOUNT_ID!.dkr.ecr.!AWS_REGION!.amazonaws.com
    set FULL_IMAGE_NAME=!REGISTRY_URL!/!ECR_REPO!:!IMAGE_TAG!
    
    echo.
    echo Authenticating with AWS ECR...
    
    REM Get ECR login password and authenticate
    for /f "delims=" %%p in ('aws ecr get-login-password --region !AWS_REGION!') do set ECR_PASSWORD=%%p
    echo !ECR_PASSWORD! | docker login --username AWS --password-stdin !REGISTRY_URL!
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: ECR authentication failed
        exit /b 1
    )
    
    echo Authentication successful
    echo.
    
    REM Check if ECR repository exists
    echo Checking if ECR repository exists...
    aws ecr describe-repositories --repository-names !ECR_REPO! --region !AWS_REGION! >nul 2>&1
    
    if !ERRORLEVEL! neq 0 (
        echo Repository does not exist. Creating ECR repository: !ECR_REPO!
        aws ecr create-repository --repository-name !ECR_REPO! --region !AWS_REGION!
        
        if !ERRORLEVEL! neq 0 (
            echo ERROR: Failed to create ECR repository
            exit /b 1
        )
        
        echo ECR repository created successfully
    ) else (
        echo ECR repository already exists
    )
    
) else if "!REGISTRY_CHOICE!"=="2" (
    echo.
    echo === Docker Hub Configuration ===
    
    REM Prompt for Docker Hub credentials
    set /p DOCKER_USERNAME="Enter Docker Hub username: "
    set /p DOCKER_PASSWORD="Enter Docker Hub password: "
    
    REM Build full image name
    set FULL_IMAGE_NAME=!DOCKER_USERNAME!/!IMAGE_NAME!:!IMAGE_TAG!
    
    echo.
    echo Authenticating with Docker Hub...
    echo !DOCKER_PASSWORD! | docker login --username !DOCKER_USERNAME! --password-stdin
    
    if !ERRORLEVEL! neq 0 (
        echo ERROR: Docker Hub authentication failed
        exit /b 1
    )
    
    echo Authentication successful
    
) else (
    echo Invalid choice. Exiting.
    exit /b 1
)

echo.
echo ================================================
echo Building Docker image: !FULL_IMAGE_NAME!
echo ================================================
echo.

REM Build Docker image
docker build -t !FULL_IMAGE_NAME! -f Dockerfile .

if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker build failed
    exit /b 1
)

echo.
echo Docker build completed successfully
echo.

echo ================================================
echo Pushing Docker image: !FULL_IMAGE_NAME!
echo ================================================
echo.

REM Push Docker image
docker push !FULL_IMAGE_NAME!

if !ERRORLEVEL! neq 0 (
    echo ERROR: Docker push failed
    exit /b 1
)

echo.
echo ================================================
echo Build and push completed successfully!
echo ================================================
echo.
echo Image: !FULL_IMAGE_NAME!
echo.
echo You can now use this image for deployment.
echo.

endlocal