# Deployment Guide

## Entornos

### Desarrollo (Development)
- **URL**: http://localhost:4200 (frontend), https://localhost:5001 (backend)
- **Base de datos**: Local SQL Server / LocalDB
- **Configuración**: `appsettings.Development.json`

### Staging
- **URL**: https://staging-tickets.example.com
- **Base de datos**: Azure SQL Database (Tier S1)
- **Configuración**: Variables de entorno

### Producción
- **URL**: https://tickets.example.com
- **Base de datos**: Azure SQL Database (Tier S2)
- **Configuración**: Azure Key Vault

## Backend Deployment

### Azure App Service

1. **Crear App Service**
```bash
az appservice plan create \
  --name ticket-api-plan \
  --resource-group ticket-rg \
  --sku B1

az webapp create \
  --name ticket-api \
  --resource-group ticket-rg \
  --plan ticket-api-plan \
  --runtime "DOTNET|9.0"
```

2. **Configurar Variables de Entorno**
```bash
az webapp config appsettings set \
  --name ticket-api \
  --resource-group ticket-rg \
  --settings \
    ConnectionStrings__DefaultConnection="Server=tcp:ticket-db.database.windows.net;Database=TicketDB;..." \
    Jwt__SecretKey="your-secret-key" \
    ASPNETCORE_ENVIRONMENT="Production"
```

3. **Deploy con Git**
```bash
az webapp deployment source config \
  --name ticket-api \
  --resource-group ticket-rg \
  --repo-url https://github.com/your-org/ticket-system \
  --branch main \
  --manual-integration
```

### Docker Deployment

```dockerfile
# Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["TicketManagementSystem.API.csproj", "."]
RUN dotnet restore "./TicketManagementSystem.API.csproj"
COPY . .
RUN dotnet build "TicketManagementSystem.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketManagementSystem.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketManagementSystem.API.dll"]
```

```bash
# Build and push
docker build -t ticket-api:latest .
docker tag ticket-api:latest your-registry.azurecr.io/ticket-api:latest
docker push your-registry.azurecr.io/ticket-api:latest
```

## Frontend Deployment

### Azure Static Web Apps

1. **Crear Static Web App**
```bash
az staticwebapp create \
  --name ticket-frontend \
  --resource-group ticket-rg \
  --source https://github.com/your-org/ticket-system \
  --branch main \
  --app-location "frontend/ticket-system-app" \
  --output-location "dist/ticket-system-app" \
  --login-with-github
```

2. **Configurar Variables de Entorno**
```bash
az staticwebapp environment set \
  --name ticket-frontend \
  --resource-group ticket-rg \
  --environment-name production \
  --vars \
    API_URL=https://ticket-api.azurewebsites.net/api
```

### Build Commands
```json
{
  "buildCommand": "npm run build --prod",
  "outputLocation": "dist/ticket-system-app"
}
```

## Base de Datos

### Migraciones EF Core

```bash
# Generar migración
dotnet ef migrations add InitialCreate

# Aplicar migración
dotnet ef database update

# Para producción
dotnet ef database update --connection "ProductionConnectionString"
```

### Backup Strategy

```sql
-- Backup semanal
BACKUP DATABASE TicketDB
TO DISK = 'C:\Backups\TicketDB_Weekly.bak'
WITH COMPRESSION;

-- Backup diario de logs
BACKUP LOG TicketDB
TO DISK = 'C:\Backups\TicketDB_Log.bak';
```

## CI/CD Pipeline

### GitHub Actions

```yaml
# .github/workflows/deploy.yml
name: Deploy to Azure

on:
  push:
    branches: [ main ]

jobs:
  build-and-deploy-backend:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 9.0.x

    - name: Restore dependencies
      run: dotnet restore ./backend/TicketManagementSystem.API

    - name: Build
      run: dotnet build ./backend/TicketManagementSystem.API --no-restore -c Release

    - name: Test
      run: dotnet test ./backend/TicketManagementSystem.API --no-build

    - name: Publish
      run: dotnet publish ./backend/TicketManagementSystem.API --no-build -c Release -o ./publish

    - name: Deploy to Azure
      uses: azure/webapps-deploy@v3
      with:
        app-name: ticket-api
        slot-name: production
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ./publish

  build-and-deploy-frontend:
    runs-on: ubuntu-latest
    steps:
    - uses: actions/checkout@v4

    - name: Setup Node.js
      uses: actions/setup-node@v4
      with:
        node-version: '20'

    - name: Install dependencies
      run: npm ci
      working-directory: frontend/ticket-system-app

    - name: Build
      run: npm run build --prod
      working-directory: frontend/ticket-system-app

    - name: Deploy to Azure Static Web Apps
      uses: Azure/static-web-apps-deploy@v1
      with:
        azure_static_web_apps_api_token: ${{ secrets.AZURE_STATIC_WEB_APPS_API_TOKEN }}
        repo_token: ${{ secrets.GITHUB_TOKEN }}
        action: "upload"
        app_location: "frontend/ticket-system-app"
        output_location: "dist/ticket-system-app"
```

## Monitoreo

### Application Insights

```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();
```

### Health Checks

```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!)
    .AddCheck<ExternalServiceHealthCheck>("External Services");

app.MapHealthChecks("/health");
```

### Logging

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

## Rollback Strategy

### Backend Rollback
```bash
# Revertir a versión anterior
az webapp deployment slot swap \
  --name ticket-api \
  --resource-group ticket-rg \
  --slot staging \
  --target-slot production
```

### Database Rollback
```sql
-- Revertir migración
ALTER DATABASE TicketDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE TicketDB FROM DISK = 'C:\Backups\TicketDB_Previous.bak' WITH REPLACE;
ALTER DATABASE TicketDB SET MULTI_USER;
```

### Frontend Rollback
```bash
# Deploy versión anterior desde Git
git checkout v1.2.0
npm run build --prod
az staticwebapp environment set \
  --name ticket-frontend \
  --resource-group ticket-rg \
  --environment-name production \
  --source ./dist/ticket-system-app
```