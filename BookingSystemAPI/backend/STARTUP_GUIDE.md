# 🚀 Guía de Inicio - BookingSystemAPI Backend

## Descripción

Este documento explica las dos formas de ejecutar el backend de BookingSystemAPI:
1. **Desarrollo Local** con `dotnet run`
2. **Docker** con `docker compose`

---

## 📋 Requisitos Previos

### Para Desarrollo Local (`dotnet run`)
- .NET 8 SDK
- SQL Server (LocalDB o instancia completa) o usar InMemory Database
- RabbitMQ (opcional para mensajería)

### Para Docker
- Docker Desktop
- Docker Compose v2+

---

## 🖥️ Opción 1: Desarrollo Local (`dotnet run`)

### Configuración

1. **Navegar al proyecto API:**
   ```powershell
   cd BookingSystemAPI.Api
   ```

2. **Configurar variables de entorno** (opcional):
   - Editar `appsettings.Development.json` o usar User Secrets
   - Por defecto usa **InMemory Database** en desarrollo

3. **Ejecutar la aplicación:**
   ```powershell
   dotnet run
   ```

### Puertos y URLs

| Servicio | URL |
|----------|-----|
| **API HTTP** | `http://localhost:5146` |
| **API HTTPS** | `https://localhost:7146` |
| **Swagger UI** | `http://localhost:5146/swagger` |
| **Health Check** | `http://localhost:5146/health` |

### Características en Desarrollo Local

- ✅ Hot Reload habilitado
- ✅ Base de datos InMemory (sin instalación de SQL Server)
- ✅ Logs detallados en consola
- ✅ Swagger UI disponible
- ⚠️ Datos se pierden al reiniciar (InMemory)

### Ejemplo de uso con el Frontend

```powershell
# Terminal 1 - Backend
cd BookingSystemAPI.Api
dotnet run

# Terminal 2 - Frontend (usa proxy.conf.json → puerto 5146)
cd ../frontend
npm start
```

---

## 🐳 Opción 2: Docker (`docker compose`)

### Servicios Incluidos

| Servicio | Imagen | Puerto |
|----------|--------|--------|
| **API** | bookingsystem-api | 8080 |
| **SQL Server** | mssql/server:2022-latest | 1433 |
| **RabbitMQ** | rabbitmq:3.13-management-alpine | 5672, 15672 |
| **Adminer** | adminer:latest | 8082 |

### Comandos Principales

```powershell
# Navegar al directorio backend
cd BookingSystemAPI/backend

# Iniciar todos los servicios
docker compose up -d

# Ver logs de la API
docker compose logs -f bookingsystem-api

# Ver estado de los servicios
docker compose ps

# Detener todos los servicios
docker compose down

# Detener y eliminar volúmenes (reset completo)
docker compose down -v
```

### Reconstruir después de cambios en el código

```powershell
# Reconstruir solo la API
docker compose build bookingsystem-api

# Reconstruir y reiniciar
docker compose up -d --build bookingsystem-api
```

### Puertos y URLs

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **API** | `http://localhost:8080` | API REST |
| **Swagger** | `http://localhost:8080/swagger` | Documentación |
| **Health** | `http://localhost:8080/health` | Estado de salud |
| **RabbitMQ UI** | `http://localhost:15672` | Panel RabbitMQ (guest/guest) |
| **Adminer** | `http://localhost:8082` | UI de base de datos |
| **SQL Server** | `localhost:1433` | Conexión directa |

### Ejemplo de uso con el Frontend

```powershell
# Terminal 1 - Backend (Docker)
cd BookingSystemAPI/backend
docker compose up -d

# Terminal 2 - Frontend (usa proxy.conf.docker.json → puerto 8080)
cd ../frontend
npm run start:docker
```

### Credenciales por Defecto

| Servicio | Usuario | Contraseña |
|----------|---------|------------|
| **SQL Server** | sa | BookingSystem123! |
| **RabbitMQ** | guest | guest |

---

## 📊 Comparación Rápida

| Característica | `dotnet run` | `docker compose` |
|----------------|--------------|------------------|
| **Puerto API** | 5146 | 8080 |
| **Base de datos** | InMemory | SQL Server |
| **Persistencia** | ❌ No | ✅ Sí |
| **RabbitMQ** | Opcional | ✅ Incluido |
| **Hot Reload** | ✅ Sí | ❌ Requiere rebuild |
| **Setup inicial** | Rápido | Descarga imágenes |
| **Recursos** | Bajo | ~2GB RAM (SQL Server) |

---

## 🔧 Solución de Problemas

### Error: Puerto en uso
```powershell
# Verificar qué proceso usa el puerto
netstat -ano | findstr :5146
netstat -ano | findstr :8080

# En Docker, detener contenedores
docker compose down
```

### Error: Base de datos no conecta (Docker)
```powershell
# Ver logs de SQL Server
docker compose logs sqlserver

# Reiniciar SQL Server
docker compose restart sqlserver

# Reset completo (elimina datos)
docker compose down -v
docker compose up -d
```

### Error: Tablas no existen
El sistema crea las tablas automáticamente al iniciar. Si hay problemas:
```powershell
# Docker: eliminar volumen y reiniciar
docker compose down -v
docker compose up -d
```

---

## 📝 Variables de Entorno

### Docker (docker-compose.yml)
```yaml
environment:
  - ASPNETCORE_ENVIRONMENT=Production
  - ConnectionStrings__DefaultConnection=Server=sqlserver;Database=BookingSystemDb;...
  - RabbitMQ__Host=rabbitmq
```

### Local (appsettings.Development.json o User Secrets)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BookingSystemDb;..."
  }
}
```

---

## ✅ Verificación de Funcionamiento

### Probar el Health Check
```powershell
# Local
curl http://localhost:5146/health

# Docker
curl http://localhost:8080/health
```

### Probar el Registro de Usuario
```powershell
# Docker
$body = @{
  email = "test@test.com"
  password = "Test123!@#"
  confirmPassword = "Test123!@#"
  firstName = "Test"
  lastName = "User"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:8080/api/auth/register" `
  -Method Post -ContentType "application/json" -Body $body
```
