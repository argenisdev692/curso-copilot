# Changelog

Todos los cambios notables de este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/es-ES/1.0.0/),
y este proyecto adhiere a [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Planificado
- Notificaciones en tiempo real con SignalR
- Componente Timeline visual para historial
- Preferencias de notificación por usuario
- Integración con Microsoft Teams

---

## [2.1.0] - 2025-11-25

### 🎉 Highlights
- **Sistema de Notificaciones**: Implementación completa con cola asíncrona
- **Historial Mejorado**: Endpoint paginado con datos enriquecidos
- **Performance**: Mejora del 73% en consultas de historial

### Added
- ✨ `EmailNotificationService` - Servicio background para notificaciones
- ✨ `TicketHistoryDto` - DTO con información enriquecida de historial
- ✨ `TicketHistoryFilterDto` - Filtros avanzados (fecha, usuario, paginación)
- ✨ `TicketHistoryMappingProfile` - AutoMapper para TicketHistory
- ✨ Endpoint paginado `GET /api/tickets/{id}/history`
- 📚 `docs/SPRINT_NOTIFICATION_SYSTEM.md` - Sprint Planning
- 📚 `docs/GITHUB_ISSUES_NOTIFICATIONS.md` - Issues para GitHub
- 📚 `docs/REFINEMENT_TICKET_HISTORY.md` - Refinamiento Scrum
- 📚 `docs/PR_NOTIFICATION_SYSTEM.md` - Template de PR
- 📚 `RELEASE_NOTES.md` - Notas de release detalladas

### Changed
- 🔄 Response de historial ahora retorna `PagedResponse<TicketHistoryDto>`
- 🔄 `ITicketService.GetTicketHistoryAsync` acepta filtros de paginación
- 🔄 `TicketService` optimizado para evitar N+1 queries
- 🔄 `TicketsController.GetTicketHistory` soporta query parameters
- ⬆️ Angular actualizado a v19.2
- ⬆️ Tailwind CSS actualizado a v4.1
- ⬆️ MediatR actualizado a v13.1

### Deprecated
- ⚠️ `GetTicketHistoryAsync(int ticketId, CancellationToken ct)` - usar versión con `TicketHistoryFilterDto`

### Fixed
- 🐛 PagedResponse usaba `Data`/`TotalCount` incorrectos (corregido a `Items`/`TotalItems`)
- 🐛 `LogInfo` no existe en BaseService (corregido a `LogInformation`)
- 🐛 Historial no mostraba nombres de usuarios asignados
- 🐛 Conteo de paginación incorrecto en respuestas
- 🐛 Filtro de fecha no aplicaba timezone correctamente

### Security
- 🔒 Validación de autorización en endpoint de historial
- 🔒 Sanitización de inputs en filtros de búsqueda
- 🔒 Validación con FluentValidation para DTOs de filtro

---

## [2.0.0] - 2025-10-15

### Added
- Sistema de autenticación JWT completo
- CRUD completo de tickets con validaciones
- Sistema de comentarios en tickets
- Roles y permisos (Admin, Manager, User)
- Rate limiting con AspNetCoreRateLimit
- Health checks para monitoreo
- Logging estructurado con Serilog
- Swagger/OpenAPI documentación
- Frontend Angular con routing protegido

### Changed
- Migración de .NET 7 a .NET 8
- Migración de Angular 17 a Angular 18
- Arquitectura reorganizada con capas claras

---

## [1.5.0] - 2025-09-01

### Added
- Asignación de tickets a usuarios
- Filtros avanzados en lista de tickets
- Exportación a PDF con QuestPDF
- Dashboard con métricas básicas

### Changed
- Mejoras de UI/UX en formularios
- Optimización de queries EF Core

### Fixed
- Memory leak en subscripciones RxJS
- Validación de formularios reactivos

---

## [1.0.0] - 2025-07-01

### Added
- 🎉 Release inicial
- CRUD básico de tickets
- Autenticación básica
- Frontend Angular inicial
- Base de datos SQLite para desarrollo

---

## Leyenda

| Emoji | Significado |
|-------|-------------|
| ✨ | Nueva característica |
| 🔄 | Cambio en funcionalidad existente |
| ⬆️ | Actualización de dependencia |
| 🐛 | Corrección de bug |
| 🔒 | Mejora de seguridad |
| ⚠️ | Deprecación |
| 📚 | Documentación |
| 🎉 | Celebración/Hito importante |

---

## Links

- [Unreleased]: https://github.com/org/TicketManagementSystem/compare/v2.1.0...HEAD
- [2.1.0]: https://github.com/org/TicketManagementSystem/compare/v2.0.0...v2.1.0
- [2.0.0]: https://github.com/org/TicketManagementSystem/compare/v1.5.0...v2.0.0
- [1.5.0]: https://github.com/org/TicketManagementSystem/compare/v1.0.0...v1.5.0
- [1.0.0]: https://github.com/org/TicketManagementSystem/releases/tag/v1.0.0
