# AWS Cloud Deployment Guide for Films Application

## Overview

This guide provides instructions for deploying the Films ASP.NET Web Forms application to AWS. The application has been updated with cloud-ready configurations to support distributed deployment.

## ⚠️ Important Framework Limitation

**This application uses ASP.NET Web Forms on .NET Framework 4.6, which is Windows-only and cannot run on Linux.**

**Deployment Options:**
- ✅ **AWS EC2 Windows Instances** (Recommended for .NET Framework)
- ✅ **AWS Elastic Beanstalk with Windows Server** (Managed deployment)
- ❌ **AWS Lambda / ECS Linux Containers** (Not supported - requires .NET Core/5+)

**For true Linux cloud deployment, the application must be migrated to ASP.NET Core.**

---

## Cloud Readiness Fixes Applied

The following fixes have been implemented to make the application cloud-ready:

### 1. Database Configuration ✅
- **Fixed:** Hard-coded LocalDb connection strings with machine-specific names
- **Solution:** Replaced with environment variable placeholders (${DB_SERVER}, ${DB_NAME}, ${DB_USER}, ${DB_PASSWORD})
- **Benefit:** Database credentials are externalized and can be stored in AWS Secrets Manager or Parameter Store

### 2. Session State Management ✅
- **Fixed:** InProc session state (non-scalable)
- **Solution:** Changed to Custom mode using SQL Server-backed session storage
- **Benefit:** Supports horizontal scaling across multiple EC2 instances behind a load balancer

### 3. Authentication Configuration ✅
- **Fixed:** Forms Authentication without machine key (non-distributed)
- **Solution:** Added configurable machine key using environment variables
- **Benefit:** Authentication cookies work consistently across multiple server instances

### 4. Entity Framework Configuration ✅
- **Fixed:** LocalDb connection factory (development-only database)
- **Solution:** Changed to SqlConnectionFactory with cloud database connection
- **Benefit:** Works with AWS RDS SQL Server or SQL Server on EC2

### 5. Windows Integrated Security ✅
- **Fixed:** Connection strings using Integrated Security=SSPI (Windows authentication)
- **Solution:** Changed to SQL Server authentication with username/password
- **Benefit:** Compatible with AWS RDS SQL Server which doesn't support Windows authentication

### 6. Configuration Transformation ✅
- **Fixed:** Missing AWS-specific configuration transformation
- **Solution:** Created Web.AWS.config with production settings
- **Benefit:** Automatic configuration adjustment for AWS deployment

### 7. Environment Variable Integration ✅
- **Fixed:** No mechanism to inject environment-specific configuration
- **Solution:** Created CloudConfigHelper to replace placeholders at runtime
- **Benefit:** Follows 12-factor app principles for cloud-native configuration

---

## AWS Deployment Architecture

```
┌─────────────────────────────────────────────────────────────┐
│                     AWS Cloud Architecture                   │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  ┌──────────────────┐         ┌──────────────────┐          │
│  │  Application     │         │  Application     │          │
│  │  Load Balancer   │────────>│  Load Balancer   │          │
│  │  (ALB)           │         │  (ALB)           │          │
│  └──────────────────┘         └──────────────────┘          │
│         │                              │                     │
│         │                              │                     │
│  ┌──────▼──────┐              ┌───────▼──────┐              │
│  │  EC2 Win    │              │  EC2 Win     │              │
│  │  Instance 1 │              │  Instance 2  │              │
│  │  (IIS)      │              │  (IIS)       │              │
│  └──────┬──────┘              └───────┬──────┘              │
│         │                              │                     │
│         └──────────────┬───────────────┘                     │
│                        │                                     │
│                 ┌──────▼──────────┐                          │
│                 │   AWS RDS       │                          │
│                 │   SQL Server    │                          │
│                 │   (Multi-AZ)    │                          │
│                 └─────────────────┘                          │
│                                                               │
│  ┌─────────────────────────────────────────────────┐        │
│  │  AWS Secrets Manager / Parameter Store          │        │
│  │  - DB_SERVER, DB_USER, DB_PASSWORD              │        │
│  │  - MACHINE_VALIDATION_KEY, MACHINE_DECRYPTION_KEY│        │
│  └─────────────────────────────────────────────────┘        │
└─────────────────────────────────────────────────────────────┘
```

---

## Pre-Deployment Requirements

### 1. AWS Services Setup

#### A. RDS SQL Server Database
```bash
# Create RDS SQL Server instance
aws rds create-db-instance \
    --db-instance-identifier films-db \
    --db-instance-class db.t3.medium \
    --engine sqlserver-ex \
    --master-username admin \
    --master-user-password <YourSecurePassword> \
    --allocated-storage 20 \
    --vpc-security-group-ids sg-xxxxxxxxx \
    --db-subnet-group-name films-db-subnet \
    --publicly-accessible false \
    --multi-az
```

#### B. AWS Secrets Manager
```bash
# Store database credentials
aws secretsmanager create-secret \
    --name /films-app/database \
    --description "Database credentials for Films application" \
    --secret-string '{
      "DB_SERVER": "films-db.xxxxxxxxxx.us-east-1.rds.amazonaws.com",
      "DB_NAME": "films",
      "DB_AUTH_NAME": "aspnet-films-auth",
      "DB_USER": "admin",
      "DB_PASSWORD": "YourSecurePassword123!"
    }'

# Store machine keys for distributed authentication
aws secretsmanager create-secret \
    --name /films-app/machine-keys \
    --description "Machine keys for Forms Authentication" \
    --secret-string '{
      "MACHINE_VALIDATION_KEY": "<Generate 64-byte hex key>",
      "MACHINE_DECRYPTION_KEY": "<Generate 32-byte hex key>"
    }'
```

**Generate Machine Keys:**
```powershell
# Run in PowerShell to generate secure machine keys
Add-Type -AssemblyName System.Web
[System.Web.Security.MachineKey]::Encode(
    [byte[]](1..64 | ForEach-Object { Get-Random -Maximum 256 }),
    [System.Web.Security.MachineKeyProtection]::All
)
```

#### C. EC2 IAM Role
Create an IAM role for EC2 instances with permissions to access Secrets Manager:

```json
{
  "Version": "2012-10-17",
  "Statement": [
    {
      "Effect": "Allow",
      "Action": [
        "secretsmanager:GetSecretValue",
        "secretsmanager:DescribeSecret"
      ],
      "Resource": [
        "arn:aws:secretsmanager:us-east-1:123456789012:secret:/films-app/*"
      ]
    },
    {
      "Effect": "Allow",
      "Action": [
        "ssm:GetParameter",
        "ssm:GetParameters",
        "ssm:GetParametersByPath"
      ],
      "Resource": [
        "arn:aws:ssm:us-east-1:123456789012:parameter/films-app/*"
      ]
    }
  ]
}
```

---

## Deployment Steps

### Option 1: AWS Elastic Beanstalk (Recommended)

#### Step 1: Install EB CLI
```bash
pip install awsebcli
```

#### Step 2: Initialize Elastic Beanstalk
```bash
cd /path/to/NSPA
eb init -p "IIS 10.0 running on 64bit Windows Server 2019" films-app --region us-east-1
```

#### Step 3: Configure Environment Variables
Create `.ebextensions/environment.config`:

```yaml
option_settings:
  aws:elasticbeanstalk:application:environment:
    DB_SERVER: '{{resolve:secretsmanager:/films-app/database:SecretString:DB_SERVER}}'
    DB_NAME: '{{resolve:secretsmanager:/films-app/database:SecretString:DB_NAME}}'
    DB_AUTH_NAME: '{{resolve:secretsmanager:/films-app/database:SecretString:DB_AUTH_NAME}}'
    DB_USER: '{{resolve:secretsmanager:/films-app/database:SecretString:DB_USER}}'
    DB_PASSWORD: '{{resolve:secretsmanager:/films-app/database:SecretString:DB_PASSWORD}}'
    MACHINE_VALIDATION_KEY: '{{resolve:secretsmanager:/films-app/machine-keys:SecretString:MACHINE_VALIDATION_KEY}}'
    MACHINE_DECRYPTION_KEY: '{{resolve:secretsmanager:/films-app/machine-keys:SecretString:MACHINE_DECRYPTION_KEY}}'
    ASPNET_ENVIRONMENT: 'Production'
```

#### Step 4: Create Environment and Deploy
```bash
# Create environment
eb create films-app-prod \
    --instance-type t3.medium \
    --database.engine sqlserver-ex \
    --database.size 20 \
    --elb-type application \
    --envvars DB_SERVER=<rds-endpoint>,DB_NAME=films,...

# Deploy application
eb deploy
```

---

### Option 2: AWS EC2 Windows Manual Deployment

#### Step 1: Launch EC2 Windows Instance
```bash
aws ec2 run-instances \
    --image-id ami-xxxxxxxxx \
    --instance-type t3.medium \
    --key-name films-app-key \
    --security-group-ids sg-xxxxxxxxx \
    --iam-instance-profile Name=FilmsAppEC2Role \
    --user-data file://user-data.txt
```

#### Step 2: Install Prerequisites on EC2
Connect via RDP and run:

```powershell
# Install IIS and ASP.NET
Install-WindowsFeature Web-Server
Install-WindowsFeature Web-Asp-Net45
Install-WindowsFeature Web-WebSockets

# Install .NET Framework 4.6
# (Usually pre-installed on Windows Server 2019)

# Install Web Deploy
Invoke-WebRequest -Uri https://download.microsoft.com/download/...web-deploy.msi -OutFile web-deploy.msi
Start-Process msiexec.exe -ArgumentList '/i', 'web-deploy.msi', '/quiet' -Wait
```

#### Step 3: Set Environment Variables
```powershell
# Retrieve secrets from AWS Secrets Manager
$secrets = aws secretsmanager get-secret-value --secret-id /films-app/database --query SecretString --output text | ConvertFrom-Json

# Set system environment variables
[Environment]::SetEnvironmentVariable("DB_SERVER", $secrets.DB_SERVER, "Machine")
[Environment]::SetEnvironmentVariable("DB_NAME", $secrets.DB_NAME, "Machine")
[Environment]::SetEnvironmentVariable("DB_AUTH_NAME", $secrets.DB_AUTH_NAME, "Machine")
[Environment]::SetEnvironmentVariable("DB_USER", $secrets.DB_USER, "Machine")
[Environment]::SetEnvironmentVariable("DB_PASSWORD", $secrets.DB_PASSWORD, "Machine")

# Retrieve and set machine keys
$keys = aws secretsmanager get-secret-value --secret-id /films-app/machine-keys --query SecretString --output text | ConvertFrom-Json
[Environment]::SetEnvironmentVariable("MACHINE_VALIDATION_KEY", $keys.MACHINE_VALIDATION_KEY, "Machine")
[Environment]::SetEnvironmentVariable("MACHINE_DECRYPTION_KEY", $keys.MACHINE_DECRYPTION_KEY, "Machine")

# Restart IIS to apply environment variables
iisreset
```

#### Step 4: Deploy Application
```powershell
# Build and publish application
msbuild FIlms.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=AWS

# Or use Web Deploy
msdeploy -verb:sync -source:package="FIlms.zip" -dest:auto,computerName="localhost"
```

---

## Database Migration

### Step 1: Backup Existing Database
```sql
-- On development machine with LocalDb
BACKUP DATABASE [films]
TO DISK = 'C:\Temp\films-backup.bak'
WITH FORMAT, INIT, COMPRESSION;
```

### Step 2: Restore to AWS RDS
```powershell
# Upload backup to S3
aws s3 cp films-backup.bak s3://films-app-backups/films-backup.bak

# Restore to RDS (requires SQL Server native backup/restore)
# Note: This requires RDS SQL Server Enterprise or Standard edition
aws rds restore-db-instance-from-s3 \
    --db-instance-identifier films-db \
    --s3-bucket-name films-app-backups \
    --s3-prefix films-backup.bak \
    --engine sqlserver-se \
    --master-username admin \
    --master-user-password <password>
```

Alternatively, use SQL Server Management Studio to restore:
1. Connect to RDS endpoint
2. Right-click Databases → Restore Database
3. Select backup file location (S3 or local with VPN)

---

## Post-Deployment Configuration

### 1. Configure Application Load Balancer

```bash
# Create target group
aws elbv2 create-target-group \
    --name films-app-tg \
    --protocol HTTP \
    --port 80 \
    --vpc-id vpc-xxxxxxxxx \
    --health-check-path /Default.aspx \
    --health-check-interval-seconds 30

# Register instances
aws elbv2 register-targets \
    --target-group-arn arn:aws:elasticloadbalancing:... \
    --targets Id=i-xxxxxxxxx Id=i-yyyyyyyyy

# Create load balancer
aws elbv2 create-load-balancer \
    --name films-app-alb \
    --subnets subnet-xxxxxxxx subnet-yyyyyyyy \
    --security-groups sg-xxxxxxxxx \
    --scheme internet-facing
```

### 2. Configure HTTPS/SSL

```bash
# Request ACM certificate
aws acm request-certificate \
    --domain-name films.example.com \
    --validation-method DNS

# Add HTTPS listener to ALB
aws elbv2 create-listener \
    --load-balancer-arn arn:aws:elasticloadbalancing:... \
    --protocol HTTPS \
    --port 443 \
    --certificates CertificateArn=arn:aws:acm:... \
    --default-actions Type=forward,TargetGroupArn=arn:aws:elasticloadbalancing:...
```

### 3. Configure Auto Scaling

```bash
# Create launch template
aws ec2 create-launch-template \
    --launch-template-name films-app-template \
    --version-description "Films app v1" \
    --launch-template-data file://launch-template.json

# Create Auto Scaling group
aws autoscaling create-auto-scaling-group \
    --auto-scaling-group-name films-app-asg \
    --launch-template LaunchTemplateName=films-app-template \
    --min-size 2 \
    --max-size 6 \
    --desired-capacity 2 \
    --target-group-arns arn:aws:elasticloadbalancing:... \
    --vpc-zone-identifier "subnet-xxxxxxxx,subnet-yyyyyyyy"

# Configure scaling policies
aws autoscaling put-scaling-policy \
    --auto-scaling-group-name films-app-asg \
    --policy-name scale-up \
    --scaling-adjustment 1 \
    --adjustment-type ChangeInCapacity
```

---

## Environment Variable Reference

| Variable | Description | Example | Required |
|----------|-------------|---------|----------|
| `DB_SERVER` | RDS endpoint or EC2 SQL Server hostname | `films-db.xxxxx.us-east-1.rds.amazonaws.com` | Yes |
| `DB_NAME` | Main database name | `films` | Yes |
| `DB_AUTH_NAME` | Authentication database name | `aspnet-films-auth` | Yes |
| `DB_USER` | Database username | `admin` | Yes |
| `DB_PASSWORD` | Database password | `SecurePassword123!` | Yes |
| `MACHINE_VALIDATION_KEY` | 64-byte hex key for Forms Auth | `ABC123...` | Yes (Production) |
| `MACHINE_DECRYPTION_KEY` | 32-byte hex key for Forms Auth | `DEF456...` | Yes (Production) |
| `ASPNET_ENVIRONMENT` | Environment name | `Production`, `Staging` | No |

---

## Monitoring and Logging

### CloudWatch Logs Integration

Create `web.config` logging configuration:

```xml
<system.diagnostics>
  <trace autoflush="true">
    <listeners>
      <add name="CloudWatchListener"
           type="Amazon.CloudWatch.Logs.TraceListener, AWS.Logger.CloudWatch"
           logGroup="/aws/elasticbeanstalk/films-app"
           logStreamNamePrefix="app-" />
    </listeners>
  </trace>
</system.diagnostics>
```

### Application Performance Monitoring

Consider integrating:
- **AWS X-Ray** for distributed tracing
- **CloudWatch Application Insights** for .NET monitoring
- **New Relic** or **Datadog** for APM

---

## Troubleshooting

### Issue: Environment variables not loading
**Solution:** Ensure CloudConfigHelper.InitializeCloudConfiguration() is called in Global.asax Application_Start

### Issue: Database connection fails
**Solution:**
1. Verify RDS security group allows inbound traffic from EC2 instances
2. Check environment variables are set correctly: `Get-ChildItem Env:DB_*`
3. Verify RDS endpoint is correct

### Issue: Forms Authentication not working across instances
**Solution:** Ensure MACHINE_VALIDATION_KEY and MACHINE_DECRYPTION_KEY are identical across all instances

### Issue: Session state lost
**Solution:** Verify session database is created:
```sql
-- Run this on RDS SQL Server
USE [aspnet-films-auth]
GO
EXEC dbo.aspnet_regsql
    @sessiontype 'c',
    @sstype 'c'
```

---

## Migration to ASP.NET Core (Future)

For full Linux cloud compatibility, consider migrating to ASP.NET Core:

**Benefits:**
- ✅ Runs on Linux (lower AWS costs)
- ✅ Containerization support (ECS, EKS, Lambda)
- ✅ Better performance and lower memory footprint
- ✅ Modern dependency injection and middleware
- ✅ Native cloud patterns support

**Migration Path:**
1. Upgrade Entity Framework 5 → Entity Framework Core
2. Convert Web Forms pages → Razor Pages or MVC
3. Update authentication → ASP.NET Core Identity
4. Retarget to .NET 8 or later
5. Update packages and dependencies
6. Containerize with Docker
7. Deploy to ECS/EKS on Linux

---

## Cost Optimization

**Estimated Monthly Costs (us-east-1):**
- 2x EC2 t3.medium Windows: ~$120/month
- RDS SQL Server Express (t3.medium): ~$80/month
- Application Load Balancer: ~$25/month
- Data transfer: ~$20/month
- **Total: ~$245/month**

**Optimization Tips:**
1. Use Reserved Instances for production (save 30-50%)
2. Enable Auto Scaling to scale down during low traffic
3. Use RDS Multi-AZ only for production
4. Consider migration to Linux with ASP.NET Core (save ~40% on compute)

---

## Security Checklist

- [ ] Database credentials stored in AWS Secrets Manager
- [ ] Machine keys stored in AWS Secrets Manager (never in source control)
- [ ] SSL/TLS enabled on Application Load Balancer
- [ ] Security groups configured with least privilege
- [ ] RDS database not publicly accessible
- [ ] Regular security patches applied to Windows EC2 instances
- [ ] CloudWatch logging enabled for audit trails
- [ ] IAM roles used instead of access keys
- [ ] SQL Server encrypted connections enabled
- [ ] Regular RDS snapshots configured

---

## Support and Maintenance

**Regular Maintenance Tasks:**
1. Apply Windows Updates monthly
2. Patch SQL Server quarterly
3. Review CloudWatch logs weekly
4. Monitor RDS performance metrics
5. Rotate database credentials every 90 days
6. Review Auto Scaling policies quarterly
7. Test disaster recovery procedures quarterly

**Disaster Recovery:**
- RDS automated backups: 7-day retention
- RDS snapshots: Manual snapshots before major changes
- Application backups: Store deployment packages in S3

---

## Additional Resources

- [AWS Elastic Beanstalk .NET Documentation](https://docs.aws.amazon.com/elasticbeanstalk/latest/dg/create_deploy_NET.html)
- [AWS RDS SQL Server Documentation](https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/CHAP_SQLServer.html)
- [ASP.NET Web Forms Deployment](https://docs.microsoft.com/en-us/aspnet/web-forms/overview/deployment/)
- [AWS Secrets Manager Best Practices](https://docs.aws.amazon.com/secretsmanager/latest/userguide/best-practices.html)

---

**Last Updated:** 2026-01-22
**Maintainer:** Cloud Readiness Team
