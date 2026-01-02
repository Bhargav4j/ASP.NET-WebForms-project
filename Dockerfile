# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder

WORKDIR /src

# Copy solution file
COPY Films.sln ./

# Copy project files for dependency caching
COPY src/Films.Domain/Films.Domain.csproj src/Films.Domain/
COPY src/Films.Application/Films.Application.csproj src/Films.Application/
COPY src/Films.Infrastructure/Films.Infrastructure.csproj src/Films.Infrastructure/
COPY src/Films.Web/Films.Web.csproj src/Films.Web/

# Restore dependencies
RUN dotnet restore src/Films.Web/Films.Web.csproj

# Copy all source code
COPY src/ src/

# Build the application
RUN dotnet build src/Films.Web/Films.Web.csproj -c Release --no-restore

# Publish the application
RUN dotnet publish src/Films.Web/Films.Web.csproj -c Release -o /app/publish --no-build

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

# Create non-root user for security
RUN groupadd -r filmsapp && useradd -r -g filmsapp filmsapp

# Copy published application
COPY --from=builder /app/publish .

# Set ownership
RUN chown -R filmsapp:filmsapp /app

# Switch to non-root user
USER filmsapp

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_URLS=http://+:8080 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Expose application port
EXPOSE 8080

# Entry point
ENTRYPOINT ["dotnet", "Films.Web.dll"]