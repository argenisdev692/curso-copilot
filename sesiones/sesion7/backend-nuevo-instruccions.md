# 🎯 Proyecto Final Backend: Sistema de Gestión de Reservas de Salas de Reuniones

## 📋 Descripción del Proyecto
Una API REST completa para gestión de reservas de salas de reuniones que incluye:

- Autenticación JWT con roles (Admin, Usuario, Gestor)
- CRUD de salas y reservas con validación de disponibilidad
- Sistema de notificaciones por email
- Auditoría de cambios y registro de accesos
- Integración con RabbitMQ para procesamiento de notificaciones
- Testing completo (unitario y de integración)
- CI/CD pipeline con GitHub Actions
- Documentación Swagger automática

## 🎯 Aplicación de Sub-temas por Sesión

> **Formatos de Prompt utilizados:**
> - **C.R.E.A.T.E**: Estructurado para tareas complejas
> - **C.O.R.E**: Natural/compacto para Copilot Chat

---

## Sesión 1: Introducción y Prompt Engineering

### Tema 1 - Scaffolding del Proyecto

**🔷 C.R.E.A.T.E (Estructurado):**
```
Crear carpeta raíz BookingSystemAPI/ y dentro carpeta backend/ con proyecto .NET 8 WebAPI.
Estructura N-Layer: Controllers/, Services/, Repositories/, Models/, DTOs/, Data/, Common/.
Instalar dependencias: EF Core 8, AutoMapper, FluentValidation, Swashbuckle, Serilog.
Configurar DI, ILogger<T>, appsettings por ambiente, User Secrets.
Incluir proyecto de tests separado con xUnit + FluentAssertions + NSubstitute.
Generar: Solution .sln + Program.cs con middleware + appsettings.json
NO incluir: Autenticación, datos de prueba.
```

**⚡ C.O.R.E (Natural):**
```
Crear BookingSystemAPI/ con subcarpeta backend/ proyecto .NET 8 WebAPI N-Layer. Instalar EF Core 8, AutoMapper, FluentValidation, Swashbuckle, Serilog. Estructura: Controllers, Services, Repositories, Models, DTOs, Common. Proyecto Tests con xUnit. DI + appsettings. Sin auth ni seed.
```

---

### Tema 2 - Modelo Room

**🔷 C.R.E.A.T.E:**
```
Entidad Room para EF Core 8 con FluentValidation.
Propiedades: Id, Name, Capacity, Equipment[] (JSON), Location, Status (enum: Available/Maintenance).
Implementar ISoftDelete (IsDeleted) e IAuditable (CreatedAt, UpdatedAt).
Crear configuración Fluent API separada con índice único en Name.
Generar: Room.cs, RoomStatus.cs, RoomConfiguration.cs, RoomValidator.cs
XML comments en propiedades públicas.
```

**⚡ C.O.R.E:**
```
Entity Room EF Core 8: Id, Name, Capacity, Equipment[], Location, Status(enum). Interfaces ISoftDelete + IAuditable. Fluent API config + FluentValidation. XML comments. Generar Room.cs + Configuration + Validator.
```

---

### Tema 3 - CRUD Reservas

**🔷 C.R.E.A.T.E:**
```
CRUD completo para Booking con EF Core 8 y Result Pattern.
Validaciones: sin solapamientos de horario, horario laboral 8:00-20:00, sala no en mantenimiento.
Patrón Repository + interfaces. AsNoTracking en lecturas.
Generar: IBookingService, BookingService, IBookingRepository, BookingRepository, BookingRequestDto, BookingResponseDto, Result<T>.
Edge cases: reserva en pasado, duración mín 15min, máx 8h.
```

**⚡ C.O.R.E:**
```
CRUD Booking con validación: no solapamientos, horario 8-20h, sala disponible. Result Pattern + Repository. AsNoTracking en reads. DTOs request/response. Edge: pasado, min 15min, max 8h.
```

---

## Sesión 2: Desarrollo e Integración

### Tema 5 - Autenticación JWT

**🔷 C.R.E.A.T.E:**
```
Autenticación JWT completa en .NET 8 con BCrypt.
Endpoints: Login (email/pwd → tokens), Register, RefreshToken.
Roles: Admin (todo), Manager (CRUD salas), User (reservas propias).
AccessToken 15min, RefreshToken 7d en BD.
Secretos en User Secrets, NO loggear tokens ni passwords.
Generar: AuthController, IAuthService, AuthService, ITokenService, TokenService, LoginRequest/Response, RegisterRequest, JwtSettings.
```

**⚡ C.O.R.E:**
```
JWT Auth .NET 8 + BCrypt: Login, Register, RefreshToken. Roles Admin|Manager|User. Access 15min, Refresh 7d. User Secrets. Generar AuthController + AuthService + TokenService + DTOs.
```

---

### Tema 6 - Testing

**🔷 C.R.E.A.T.E:**
```
Tests unitarios para BookingService con xUnit + FluentAssertions + NSubstitute.
Tests requeridos:
- CreateBooking_WhenNoConflict_ReturnsSuccess
- CreateBooking_WhenOverlap_ReturnsConflictError
- CreateBooking_WhenOutsideWorkHours_ReturnsValidationError
- CancelBooking_WhenNotOwner_ReturnsForbidden
Patrón AAA, todos async, coverage >80%.
Generar: BookingServiceTests.cs, TestDataBuilder.cs
```

**⚡ C.O.R.E:**
```
Tests xUnit + FluentAssertions para BookingService: NoConflict_Success, Overlap_Error, OutsideHours_Error, NotOwner_Forbidden. AAA pattern, async, mock repos. Coverage >80%.
```

---

## Sesión 3: Testing y Refactorización

### Tema 7 - Refactorización SOLID

**🔷 C.R.E.A.T.E:**
```
Refactorizar BookingService.ValidateBooking() que viola SRP (100+ líneas).
Extraer: IAvailabilityValidator (no solapamientos), ISchedulePolicy (horarios), IRoomStatusChecker (estado sala).
Aplicar Strategy Pattern para validaciones.
AsNoTracking en queries, XML comments en interfaces.
Generar: IBookingValidator (base), AvailabilityValidator, SchedulePolicyValidator, RoomStatusValidator, BookingService refactorizado.
```

**⚡ C.O.R.E:**
```
Refactorizar #BookingService.cs viola SRP. Extraer IAvailabilityValidator, ISchedulePolicy, IRoomStatusChecker. Strategy Pattern. AsNoTracking, XML comments. Mantener compatibilidad tests.
```

---

### Tema 8 - BaseRepository

**🔷 C.R.E.A.T.E:**
```
BaseRepository<T> genérico para eliminar código CRUD duplicado.
Métodos: GetByIdAsync, GetAllAsync(filter?), AddAsync, UpdateAsync, DeleteAsync (soft delete si ISoftDelete).
Constraint: where T : class, IEntity. Virtual para override.
AsNoTracking en lecturas.
Generar: IRepository<T>, BaseRepository<T>, IEntity.cs, ISoftDelete.cs
```

**⚡ C.O.R.E:**
```
BaseRepository<T> genérico: GetById, GetAll, Add, Update, SoftDelete. Constraint IEntity, virtual methods, AsNoTracking. Generar IRepository<T> + BaseRepository + interfaces.
```

---

## Sesión 4: Documentación y Seguridad

### Tema 9 - Swagger

**🔷 C.R.E.A.T.E:**
```
Configurar Swagger/OpenAPI completo con autenticación JWT Bearer.
Documentar todos los endpoints con ProducesResponseType y SwaggerOperation.
Integrar XML comments del proyecto.
Agrupar por tags: Auth, Rooms, Bookings, Reports.
Generar: Program.cs (config Swagger), Controllers con decoradores, habilitar GenerateDocumentationFile en .csproj
```

**⚡ C.O.R.E:**
```
Swagger OpenAPI + JWT Bearer. ProducesResponseType + SwaggerOperation en controllers. XML comments integrados. Tags por grupo. Habilitar GenerateDocumentationFile.
```

---

### Tema 10 - Seguridad

**🔷 C.R.E.A.T.E:**
```
Implementar seguridad para producción.
Rate limiting: 100 req/min general, 5/min en auth endpoints.
Sanitización anti-XSS en strings de entrada.
Headers seguridad: X-Content-Type-Options, X-Frame-Options.
NO loggear PII. User Secrets para credenciales.
Generar: SecurityHeadersMiddleware, SanitizationMiddleware, SecurityExtensions, Program.cs actualizado.
```

**⚡ C.O.R.E:**
```
Seguridad producción: Rate limit 100/min (5/min auth), sanitización XSS, headers seguridad. No PII en logs, User Secrets. Generar middlewares + extensions.
```

---

## Sesión 5: CI/CD y Casos Avanzados

### Tema 11 - CI/CD

**🔷 C.R.E.A.T.E:**
```
Pipeline GitHub Actions para .NET 8 + Docker + Azure App Service.
Trigger: push main/develop, PR a main.
Jobs: restore → build → test → docker build → deploy.
Cache NuGet, secrets en GitHub Secrets, deploy solo desde main.
Dockerfile multi-stage optimizado.
Generar: .github/workflows/ci-cd.yml, Dockerfile, .dockerignore
```

**⚡ C.O.R.E:**
```
GitHub Actions .NET 8: restore → build → test → docker → deploy Azure. Cache NuGet, GitHub Secrets. Multi-stage Dockerfile. Solo deploy desde main. Generar ci-cd.yml + Dockerfile.
```

---

### Tema 12 - RabbitMQ

**🔷 C.R.E.A.T.E:**
```
Mensajería asíncrona con MassTransit + RabbitMQ.
Eventos: BookingCreatedEvent, BookingCancelledEvent, BookingReminderEvent.
Publisher genérico desde BookingService, Consumer para notificaciones (interfaz).
Retry exponencial, Dead Letter Queue, CorrelationId en eventos.
Health check de RabbitMQ, reconexión automática.
Generar: Events/, IEventPublisher, EventPublisher, BookingNotificationConsumer, RabbitMQExtensions, RabbitMQHealthCheck.
```

**⚡ C.O.R.E:**
```
MassTransit + RabbitMQ: eventos BookingCreated/Cancelled/Reminder. Publisher + Consumer notificaciones. Retry exponencial, DLQ, CorrelationId, health check. Reconexión automática.
```

---

### Tema 12b - Redis Cache

**🔷 C.R.E.A.T.E:**
```
Caché distribuido con StackExchange.Redis.
Cachear: lista salas (TTL 1h), disponibilidad por sala (TTL 5min).
Invalidar caché al crear/cancelar reserva.
Servicio genérico ICacheService con Get<T>, Set<T>, Remove.
Fallback a BD si Redis no disponible.
Generar: ICacheService, RedisCacheService, RedisExtensions.
```

**⚡ C.O.R.E:**
```
Redis cache: salas TTL 1h, disponibilidad TTL 5min. Invalidar en writes. ICacheService genérico. Fallback a BD. Generar CacheService + Extensions.
```

---

## Sesión 6: VBA y Proyecto Final

### Tema 13 - Exportación Excel

**🔷 C.R.E.A.T.E:**
```
Endpoint reporte Excel con ClosedXML.
GET /api/reports/room-usage?from={date}&to={date}
Estadísticas: reservas por sala, horas totales, ocupación %.
Streaming para archivos grandes, headers descarga correctos.
Formato profesional con colores y bordes.
Generar: ReportsController, IReportService, ReportService, RoomUsageReportDto.
```

**⚡ C.O.R.E:**
```
ClosedXML reporte Excel: GET /reports/room-usage stats por sala. Streaming, headers descarga. Formato profesional. Generar ReportsController + ReportService + DTO.
```

---

### Tema 14 - Proyecto Final

> Integración de todos los componentes usando los prompts anteriores en secuencia.

---

## 🏗️ Arquitectura Sugerida
```
BookingSystemAPI/
├── backend/
│   ├── BookingSystemAPI.API/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── RoomsController.cs
│   │   │   ├── BookingsController.cs
│   │   │   └── ReportsController.cs
│   │   ├── Middleware/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   ├── BookingSystemAPI.Core/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   ├── Models/
│   │   ├── DTOs/
│   │   └── Common/
│   ├── BookingSystemAPI.Infrastructure/
│   │   ├── Data/
│   │   ├── RabbitMQ/
│   │   └── Redis/
│   ├── BookingSystemAPI.Tests/
│   │   ├── UnitTests/
│   │   └── IntegrationTests/
│   ├── Dockerfile
│   └── BookingSystemAPI.sln
├── .github/
│   └── workflows/
│       └── ci-cd.yml
└── README.md
```

## 📦 Prompts Reutilizables (carpeta .github/prompts/)

De los prompts creados, se utilizan directamente:
- `ci-cd-github-actions.prompt.md` → Tema 11
- `docker-dotnet.prompt.md` → Tema 11

Los demás prompts ya están integrados en este documento por tema.
⏱️ Distribución de Tiempo (4 horas)

Configuración inicial (30 min): Proyecto, EF Core, JWT
Modelos y Repositorios (45 min): Entidades + DB Context
Controladores y Servicios (60 min): CRUD + Validaciones
Seguridad y Testing (45 min): JWT + Tests básicos
Notificaciones RabbitMQ (30 min): Cola de mensajes
CI/CD y Documentación (30 min): Pipeline + Swagger

🎓 Entregables
✅ Código fuente completo en repositorio Git
✅ README con instrucciones de instalación
✅ Documentación Swagger accesible
✅ Pipeline CI/CD funcional
✅ Al menos 20 tests unitarios pasando
✅ Dockerfile para deployment
✅ Demostración en vivo de endpoints principales
💡 Ventajas de este Proyecto

Realista: Problema empresarial común
Completo: Cubre todos los temas del curso
Escalable: Se puede extender con más features
Demostrable: Fácil de presentar con Swagger UI
Portfolio: Proyecto profesional para mostrar