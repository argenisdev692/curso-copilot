🎯 Proyecto Final Backend: Sistema de Gestión de Reservas de Salas de Reuniones

📋 Descripción del Proyecto
Una API REST completa para gestión de reservas de salas de reuniones que incluye:

Autenticación JWT con roles (Admin, Usuario, Gestor)
CRUD de salas y reservas con validación de disponibilidad
Sistema de notificaciones por email
Auditoría de cambios y registro de accesos
Integración con RabbitMQ para procesamiento de notificaciones
Testing completo (unitario y de integración)
CI/CD pipeline con GitHub Actions
Documentación Swagger automática

🎯 Aplicación de Sub-temas por Sesión

> **Formatos de Prompt utilizados:**
> - **C.R.E.A.T.E**: Estructurado para tareas complejas
> - **C.O.R.E**: Natural/compacto para Copilot Chat

---

## Sesión 1: Introducción y Prompt Engineering

### Tema 1 - Scaffolding del Proyecto
- Tarea: Crear el proyecto base .NET 8 WebAPI con estructura N-Layer, instalar dependencias (EF Core, AutoMapper, FluentValidation, Swashbuckle, Serilog), configurar DI y appsettings.

### Tema 2 - Modelo Room
- Tarea: Crear entidad Room con propiedades, interfaces ISoftDelete e IAuditable, configuración Fluent API y validaciones.

### Tema 3 - CRUD Reservas
- Tarea: Implementar CRUD completo para Booking con validaciones de disponibilidad, patrón Repository y Result Pattern.

## Sesión 2: Desarrollo e Integración

### Tema 5 - Autenticación JWT
- Tarea: Implementar autenticación JWT completa con roles, endpoints de login/register/refresh y manejo de tokens.

### Tema 6 - Testing
- Tarea: Crear tests unitarios para BookingService con xUnit, FluentAssertions y NSubstitute, cubriendo casos de éxito y error.

## Sesión 3: Testing y Refactorización

### Tema 7 - Refactorización SOLID
- Tarea: Refactorizar BookingService aplicando SRP, extrayendo validadores con Strategy Pattern.

### Tema 8 - BaseRepository
- Tarea: Crear BaseRepository genérico para eliminar duplicación de código CRUD, con soporte para soft delete.

## Sesión 4: Documentación y Seguridad

### Tema 9 - Swagger
- Tarea: Configurar Swagger/OpenAPI completo con autenticación JWT, decoradores en controladores y integración de XML comments.

### Tema 10 - Seguridad
- Tarea: Implementar medidas de seguridad para producción: rate limiting, sanitización XSS, headers de seguridad y manejo de secretos.

## Sesión 5: CI/CD y Casos Avanzados

### Tema 11 - CI/CD
- Tarea: Crear pipeline GitHub Actions para .NET 8 con Docker y despliegue a Azure App Service.

### Tema 12 - RabbitMQ
- Tarea: Integrar mensajería asíncrona con MassTransit y RabbitMQ para eventos de notificaciones.

### Tema 12b - Redis Cache
- Tarea: Implementar caché distribuido con Redis para optimizar consultas de salas y disponibilidad.

## Sesión 6: VBA y Proyecto Final

### Tema 13 - Exportación Excel
- Tarea: Crear endpoint para reporte Excel con estadísticas de uso de salas usando ClosedXML.

### Tema 14 - Proyecto Final
- Tarea: Integrar todos los componentes del proyecto en una solución completa y funcional.