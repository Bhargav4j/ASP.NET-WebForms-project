# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy solution and project files for dependency caching
COPY Films.sln ./
COPY src/Films.Domain/Films.Domain.csproj ./src/Films.Domain/
COPY src/Films.Application/Films.Application.csproj ./src/Films.Application/
COPY src/Films.Infrastructure/Films.Infrastructure.csproj ./src/Films.Infrastructure/
COPY src/Films.Web/Films.Web.csproj ./src/Films.Web/

# Restore dependencies
RUN dotnet restore Films.sln

# Copy source code
COPY src/ ./src/

# Build the application
RUN dotnet build Films.sln -c Release --no-restore

# Publish the web project
RUN dotnet publish src/Films.Web/Films.Web.csproj -c Release -o /app/publish --no-build

# Runtime stage - using explicit base image
FROM docker.io/library/nginx:latest

WORKDIR /app

# Copy published application from builder
COPY --from=builder /app/publish .

# Create non-root user for security
RUN groupadd -r filmsapp && useradd -r -g filmsapp filmsapp || true

# Set ownership
RUN chown -R filmsapp:filmsapp /app || true

# Switch to non-root user
USER filmsapp

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose application port
EXPOSE 8080

# Application entry point
ENTRYPOINT ["dotnet", "Films.Web.dll"]