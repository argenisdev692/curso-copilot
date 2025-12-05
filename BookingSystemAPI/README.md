# 📅 BookingSystemAPI

[![CI/CD Pipeline](https://github.com/argenisdev692/curso-copilot/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/argenisdev692/curso-copilot/actions/workflows/ci-cd.yml)
[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

Sistema de gestión de reservas de salas desarrollado con .NET 8 Web API.

## 🚀 Características

- ✅ **API RESTful** con OpenAPI/Swagger
- ✅ **Autenticación JWT** con refresh tokens
- ✅ **Entity Framework Core** con PostgreSQL
- ✅ **RabbitMQ** para mensajería con MassTransit
- ✅ **Health Checks** integrados
- ✅ **Docker** multi-stage optimizado
- ✅ **CI/CD** con GitHub Actions
- ✅ **Logging estructurado** con Serilog

## 📋 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started) (opcional)
- [PostgreSQL](https://www.postgresql.org/) o usar Docker

## 🛠️ Instalación

### Opción 1: Docker (Recomendado)

```bash
# Clonar repositorio
git clone https://github.com/argenisdev692/curso-copilot.git
cd curso-copilot/BookingSystemAPI/backend

# Copiar variables de entorno
cp .env.example .env

# Levantar servicios
docker-compose up -d --build

# Verificar estado
docker-compose ps
```

### Opción 2: Desarrollo Local

```bash
# Navegar al proyecto
cd BookingSystemAPI/backend/BookingSystemAPI.Api

# Restaurar dependencias
dotnet restore

# Ejecutar en modo desarrollo
dotnet run --environment Development
```

## 🔗 URLs de Servicios

| Servicio | URL |
|----------|-----|
| API | http://localhost:8080 |
| Swagger UI | http://localhost:8080/swagger |
| Health Check | http://localhost:8080/health |
| RabbitMQ Management | http://localhost:15672 |

## 📁 Estructura del Proyecto

```
BookingSystemAPI/
├── backend/
│   ├── BookingSystemAPI.Api/
│   │   ├── Controllers/       # Controladores API
│   │   ├── Services/          # Lógica de negocio
│   │   ├── Repositories/      # Acceso a datos
│   │   ├── Models/            # Entidades
│   │   ├── DTOs/              # Data Transfer Objects
│   │   ├── Validators/        # Validadores FluentValidation
│   │   ├── Common/            # Utilidades compartidas
│   │   └── Dockerfile         # Dockerfile multi-stage
│   ├── docker-compose.yml     # Orquestación Docker
│   └── .env.example           # Variables de entorno
└── tests/
    └── BookingSystemAPI.Tests/
```

## 🧪 Tests

```bash
# Ejecutar tests
cd BookingSystemAPI
dotnet test

# Con cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## 🔐 Configuración de Secrets (GitHub Actions)

Para el CI/CD, configura los siguientes secrets en tu repositorio:

| Secret | Descripción |
|--------|-------------|
| `AZURE_CREDENTIALS` | Credenciales de Azure Service Principal (JSON) |

### Crear Azure Service Principal

```bash
az ad sp create-for-rbac --name "github-actions-sp" \
  --role contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group} \
  --sdk-auth
```

## 📊 CI/CD Pipeline

El pipeline incluye los siguientes jobs:

1. **🔨 Build** - Compilación del proyecto
2. **🧪 Test** - Ejecución de tests con cobertura
3. **🔒 Security** - Análisis de vulnerabilidades
4. **🐳 Docker** - Build y push a GHCR
5. **🚀 Deploy Staging** - Despliegue a ambiente staging
6. **🚀 Deploy Production** - Despliegue a producción

## 📝 API Endpoints

### Autenticación
- `POST /api/auth/register` - Registro de usuario
- `POST /api/auth/login` - Inicio de sesión
- `POST /api/auth/refresh` - Refrescar token

### Reservas
- `GET /api/bookings` - Listar reservas
- `POST /api/bookings` - Crear reserva
- `GET /api/bookings/{id}` - Obtener reserva
- `PUT /api/bookings/{id}` - Actualizar reserva
- `DELETE /api/bookings/{id}` - Cancelar reserva

### Salas
- `GET /api/rooms` - Listar salas
- `POST /api/rooms` - Crear sala
- `GET /api/rooms/{id}` - Obtener sala
- `GET /api/rooms/{id}/availability` - Verificar disponibilidad

## 🤝 Contribución

1. Fork el proyecto
2. Crea tu feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push al branch (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver el archivo [LICENSE](LICENSE) para más detalles.

## 👥 Autores

- **BookingSystem Team** - *Desarrollo inicial*

---
⭐ Si este proyecto te fue útil, considera darle una estrella en GitHub!
