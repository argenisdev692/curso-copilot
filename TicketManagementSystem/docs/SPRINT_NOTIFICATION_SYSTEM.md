# 🔔 Sprint Planning: Sistema de Notificaciones

## 📋 Información del Sprint

| Campo | Valor |
|-------|-------|
| **Sprint** | Sprint 7 - Sistema de Notificaciones |
| **Duración** | 2 semanas (10 días hábiles) |
| **Fecha Inicio** | 2025-11-25 |
| **Fecha Fin** | 2025-12-06 |
| **Capacidad del Equipo** | 80 Story Points |
| **Velocidad Promedio** | 75 SP/Sprint |

---

## 🎯 Objetivo del Sprint

> **Implementar un sistema de notificaciones robusto y escalable** que permita notificar a los usuarios sobre eventos relevantes del ciclo de vida de los tickets, incluyendo notificaciones por email, en tiempo real (SignalR), y preferencias de usuario configurables.

---

## 📚 Épicas

### EPIC-001: Infraestructura de Notificaciones
**Descripción:** Crear la base arquitectónica para el sistema de notificaciones multi-canal.

### EPIC-002: Notificaciones por Email
**Descripción:** Extender y mejorar el sistema de email existente con templates y nuevos eventos.

### EPIC-003: Notificaciones en Tiempo Real
**Descripción:** Implementar notificaciones push usando SignalR para comunicación instantánea.

### EPIC-004: Centro de Notificaciones (Frontend)
**Descripción:** Desarrollar la interfaz de usuario para visualizar y gestionar notificaciones.

### EPIC-005: Preferencias y Configuración
**Descripción:** Permitir a los usuarios configurar sus preferencias de notificación.

---

## 📝 Product Backlog - Issues/User Stories

### 🏗️ EPIC-001: Infraestructura de Notificaciones

#### US-001: Diseño del modelo de datos para notificaciones
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🔴 Critical |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** desarrollador backend  
**Quiero** definir las entidades de base de datos para notificaciones  
**Para** almacenar y gestionar todas las notificaciones del sistema

**Criterios de Aceptación:**
- [ ] Crear entidad `Notification` con campos: Id, UserId, Type, Title, Message, IsRead, CreatedAt, ReadAt, RelatedEntityType, RelatedEntityId
- [ ] Crear entidad `NotificationPreference` para preferencias de usuario
- [ ] Crear enum `NotificationType` (TicketCreated, TicketAssigned, TicketStatusChanged, CommentAdded, TicketResolved, TicketClosed, MentionedInComment)
- [ ] Crear enum `NotificationChannel` (Email, InApp, Push)
- [ ] Migraciones de EF Core aplicadas
- [ ] Índices optimizados para consultas frecuentes

**Tareas Técnicas:**
```
- [ ] Crear Models/Notification.cs
- [ ] Crear Models/NotificationPreference.cs
- [ ] Crear Models/NotificationType.cs (enum)
- [ ] Crear Models/NotificationChannel.cs (enum)
- [ ] Actualizar ApplicationDbContext
- [ ] Crear migración: AddNotificationSystem
- [ ] Crear INotificationRepository
- [ ] Crear NotificationRepository
```

**Dependencias:** Ninguna
**Asignado a:** Backend Developer

---

#### US-002: Servicio base de notificaciones (Patrón Strategy)
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🔴 Critical |
| **Story Points** | 8 |
| **Complejidad** | Alta |
| **Sprint** | Sprint 7 |

**Como** arquitecto de software  
**Quiero** implementar un servicio de notificaciones con patrón Strategy  
**Para** soportar múltiples canales de notificación de forma extensible

**Criterios de Aceptación:**
- [ ] Interfaz `INotificationChannel` definida
- [ ] Implementaciones para Email, InApp, Push (stub)
- [ ] Servicio orquestador `INotificationService` que coordina canales
- [ ] Patrón Observer para suscripción a eventos de dominio
- [ ] Logging estructurado con CorrelationId
- [ ] Tests unitarios con >80% cobertura

**Diseño Técnico:**
```csharp
public interface INotificationChannel
{
    NotificationChannel Channel { get; }
    Task SendAsync(NotificationContext context, CancellationToken ct);
}

public interface INotificationService
{
    Task NotifyAsync(NotificationRequest request, CancellationToken ct);
    Task NotifyBatchAsync(IEnumerable<NotificationRequest> requests, CancellationToken ct);
}
```

**Tareas Técnicas:**
```
- [ ] Crear Services/Notifications/INotificationChannel.cs
- [ ] Crear Services/Notifications/INotificationService.cs
- [ ] Crear Services/Notifications/NotificationService.cs
- [ ] Crear Services/Notifications/Channels/EmailNotificationChannel.cs
- [ ] Crear Services/Notifications/Channels/InAppNotificationChannel.cs
- [ ] Crear Services/Notifications/Channels/PushNotificationChannel.cs (stub)
- [ ] Crear DTOs/NotificationRequest.cs
- [ ] Crear DTOs/NotificationContext.cs
- [ ] Registrar servicios en DI
- [ ] Tests unitarios
```

**Dependencias:** US-001
**Asignado a:** Senior Backend Developer

---

#### US-003: Sistema de eventos de dominio
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🔴 Critical |
| **Story Points** | 8 |
| **Complejidad** | Alta |
| **Sprint** | Sprint 7 |

**Como** desarrollador  
**Quiero** implementar eventos de dominio para desacoplar la lógica de notificaciones  
**Para** que el sistema sea mantenible y extensible

**Criterios de Aceptación:**
- [ ] Implementar patrón MediatR Notifications para eventos
- [ ] Eventos definidos: TicketCreatedEvent, TicketAssignedEvent, TicketStatusChangedEvent, CommentAddedEvent
- [ ] Handlers que disparan notificaciones apropiadas
- [ ] Ejecución asíncrona de handlers para no bloquear operaciones principales
- [ ] Manejo de errores con dead-letter queue

**Eventos de Dominio:**
```csharp
public record TicketCreatedEvent(int TicketId, int CreatedById) : INotification;
public record TicketAssignedEvent(int TicketId, int AssignedToId, int? PreviousAssigneeId) : INotification;
public record TicketStatusChangedEvent(int TicketId, Status OldStatus, Status NewStatus, int ChangedById) : INotification;
public record CommentAddedEvent(int CommentId, int TicketId, int AuthorId) : INotification;
```

**Tareas Técnicas:**
```
- [ ] Crear Features/Notifications/Events/TicketCreatedEvent.cs
- [ ] Crear Features/Notifications/Events/TicketAssignedEvent.cs
- [ ] Crear Features/Notifications/Events/TicketStatusChangedEvent.cs
- [ ] Crear Features/Notifications/Events/CommentAddedEvent.cs
- [ ] Crear Handlers para cada evento
- [ ] Integrar publicación de eventos en TicketService
- [ ] Integrar publicación de eventos en CommentService
- [ ] Tests de integración
```

**Dependencias:** US-002
**Asignado a:** Backend Developer

---

### 📧 EPIC-002: Notificaciones por Email

#### US-004: Templates de email HTML responsivos
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟡 High |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** usuario del sistema  
**Quiero** recibir emails con formato profesional y responsive  
**Para** tener una mejor experiencia al revisar notificaciones

**Criterios de Aceptación:**
- [ ] Templates HTML responsive (mobile-friendly)
- [ ] Soporte para variables dinámicas ({{TicketTitle}}, {{UserName}}, etc.)
- [ ] Templates para: Ticket Creado, Ticket Asignado, Cambio de Estado, Nuevo Comentario
- [ ] Footer con links para desuscribirse
- [ ] Preview en múltiples clientes de email (Outlook, Gmail)
- [ ] Motor de templates Razor o Scriban

**Templates Requeridos:**
1. `TicketCreated.html` - Notificación de ticket creado
2. `TicketAssigned.html` - Notificación de asignación
3. `TicketStatusChanged.html` - Cambio de estado
4. `NewComment.html` - Nuevo comentario en ticket
5. `TicketResolved.html` - Ticket resuelto
6. `DailyDigest.html` - Resumen diario (opcional)

**Tareas Técnicas:**
```
- [ ] Crear carpeta Templates/Email/
- [ ] Implementar IEmailTemplateService
- [ ] Crear EmailTemplateService con Razor Engine
- [ ] Diseñar template base (_Layout.html)
- [ ] Crear templates individuales
- [ ] Tests de renderizado
```

**Dependencias:** US-002
**Asignado a:** Full-Stack Developer

---

#### US-005: Cola de emails con retry y dead-letter
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟡 High |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** administrador del sistema  
**Quiero** que los emails fallidos se reintenten automáticamente  
**Para** garantizar la entrega de notificaciones importantes

**Criterios de Aceptación:**
- [ ] Política de reintentos con backoff exponencial (3 intentos)
- [ ] Dead-letter queue para emails que fallan después de reintentos
- [ ] Dashboard para ver estado de cola (métricas)
- [ ] Capacidad de reenvío manual desde dead-letter
- [ ] Logging detallado de cada intento

**Tareas Técnicas:**
```
- [ ] Refactorizar EmailNotificationService existente
- [ ] Implementar tabla EmailQueue en DB
- [ ] Crear EmailQueueProcessor (Background Service)
- [ ] Implementar dead-letter logic
- [ ] Crear endpoint admin para gestión de cola
- [ ] Métricas con Application Insights
```

**Dependencias:** US-004
**Asignado a:** Backend Developer

---

### 🔴 EPIC-003: Notificaciones en Tiempo Real

#### US-006: Hub de SignalR para notificaciones
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟡 High |
| **Story Points** | 8 |
| **Complejidad** | Alta |
| **Sprint** | Sprint 7 |

**Como** usuario activo en la aplicación  
**Quiero** recibir notificaciones instantáneas sin refrescar la página  
**Para** estar informado en tiempo real sobre cambios en mis tickets

**Criterios de Aceptación:**
- [ ] SignalR Hub configurado con autenticación JWT
- [ ] Grupos por usuario para notificaciones personalizadas
- [ ] Reconexión automática con backoff
- [ ] Fallback a long-polling si WebSockets no disponible
- [ ] Rate limiting para prevenir spam
- [ ] Métricas de conexiones activas

**Métodos del Hub:**
```csharp
public interface INotificationHub
{
    Task ReceiveNotification(NotificationDto notification);
    Task ReceiveNotificationCount(int unreadCount);
    Task MarkAsRead(int notificationId);
}
```

**Tareas Técnicas:**
```
- [ ] Instalar Microsoft.AspNetCore.SignalR
- [ ] Crear Hubs/NotificationHub.cs
- [ ] Configurar autenticación JWT en SignalR
- [ ] Crear IHubNotificationService
- [ ] Integrar con NotificationService
- [ ] Configurar CORS para SignalR
- [ ] Tests de conexión
```

**Dependencias:** US-002
**Asignado a:** Senior Backend Developer

---

#### US-007: Cliente SignalR en Angular
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟡 High |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** usuario frontend  
**Quiero** que la aplicación Angular se conecte al hub de notificaciones  
**Para** recibir actualizaciones en tiempo real

**Criterios de Aceptación:**
- [ ] Servicio Angular SignalR con reconexión automática
- [ ] Observable de notificaciones para componentes
- [ ] Manejo de estado de conexión (connected, connecting, disconnected)
- [ ] Toast notifications para nuevas notificaciones
- [ ] Badge counter en el icono de notificaciones
- [ ] Sonido opcional para nuevas notificaciones

**Tareas Técnicas:**
```
- [ ] npm install @microsoft/signalr
- [ ] Crear services/notification-hub.service.ts
- [ ] Crear models/notification.model.ts
- [ ] Crear store/notification.state.ts (NgRx o señales)
- [ ] Integrar con AuthService para token JWT
- [ ] Crear componente toast-notification
- [ ] Tests de servicio
```

**Dependencias:** US-006
**Asignado a:** Frontend Developer

---

### 🖥️ EPIC-004: Centro de Notificaciones (Frontend)

#### US-008: Componente de lista de notificaciones
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟢 Medium |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** usuario  
**Quiero** ver todas mis notificaciones en un panel dedicado  
**Para** revisar el historial de actividad relevante

**Criterios de Aceptación:**
- [ ] Dropdown desde el header con últimas 10 notificaciones
- [ ] Página completa /notifications con paginación
- [ ] Filtros: Todas, No leídas, Por tipo
- [ ] Acción rápida: Marcar como leída/no leída
- [ ] Click en notificación navega al recurso relacionado
- [ ] Diseño responsive
- [ ] Skeleton loading

**Mockup de UI:**
```
┌─────────────────────────────────────┐
│ 🔔 Notificaciones (5)          [✓] │
├─────────────────────────────────────┤
│ ● Ticket #123 asignado a ti        │
│   hace 5 minutos                    │
├─────────────────────────────────────┤
│ ○ Nuevo comentario en Ticket #98   │
│   hace 1 hora                       │
├─────────────────────────────────────┤
│ ○ Ticket #76 cambió a Resuelto     │
│   hace 3 horas                      │
├─────────────────────────────────────┤
│         [Ver todas →]               │
└─────────────────────────────────────┘
```

**Tareas Técnicas:**
```
- [ ] Crear components/notifications/notification-dropdown/
- [ ] Crear components/notifications/notification-list/
- [ ] Crear components/notifications/notification-item/
- [ ] Crear pages/notifications/
- [ ] Crear services/notification.service.ts (HTTP)
- [ ] Integrar con notification-hub.service.ts
- [ ] Estilos CSS/SCSS responsive
- [ ] Tests de componentes
```

**Dependencias:** US-007
**Asignado a:** Frontend Developer

---

#### US-009: API de notificaciones (CRUD)
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟢 Medium |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** frontend  
**Quiero** endpoints REST para gestionar notificaciones  
**Para** listar, marcar como leídas y eliminar notificaciones

**Endpoints Requeridos:**
```
GET    /api/notifications?page=1&pageSize=20&isRead=false&type=TicketAssigned
GET    /api/notifications/unread-count
PUT    /api/notifications/{id}/read
PUT    /api/notifications/mark-all-read
DELETE /api/notifications/{id}
```

**Criterios de Aceptación:**
- [ ] Endpoints protegidos con [Authorize]
- [ ] Solo notificaciones del usuario autenticado
- [ ] Paginación con PagedResult<T>
- [ ] Filtros por tipo y estado de lectura
- [ ] Soft delete
- [ ] Documentación Swagger

**Tareas Técnicas:**
```
- [ ] Crear Controllers/NotificationsController.cs
- [ ] Crear DTOs/NotificationDto.cs
- [ ] Crear DTOs/NotificationFilterDto.cs
- [ ] Implementar INotificationService.GetUserNotificationsAsync()
- [ ] Configurar AutoMapper profiles
- [ ] Tests de controller
```

**Dependencias:** US-001
**Asignado a:** Backend Developer

---

### ⚙️ EPIC-005: Preferencias y Configuración

#### US-010: Preferencias de notificación por usuario
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟢 Medium |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** usuario  
**Quiero** configurar qué notificaciones recibir y por qué canal  
**Para** personalizar mi experiencia y evitar spam

**Criterios de Aceptación:**
- [ ] Página de preferencias en perfil de usuario
- [ ] Matriz de configuración: Tipo de Evento × Canal
- [ ] Opción de desactivar todas las notificaciones
- [ ] Horario de "No molestar" (opcional)
- [ ] Valores por defecto sensatos para nuevos usuarios
- [ ] Persistencia inmediata de cambios

**UI de Preferencias:**
```
┌──────────────────────────────────────────────────────┐
│         Preferencias de Notificación                 │
├──────────────────────────────────────────────────────┤
│                          │ Email │ In-App │ Push    │
├──────────────────────────┼───────┼────────┼─────────┤
│ Ticket asignado          │  ✓    │   ✓    │   ✓     │
│ Cambio de estado         │  ○    │   ✓    │   ○     │
│ Nuevo comentario         │  ✓    │   ✓    │   ○     │
│ Ticket resuelto          │  ✓    │   ✓    │   ✓     │
│ Mencionado en comentario │  ✓    │   ✓    │   ✓     │
└──────────────────────────────────────────────────────┘
```

**Tareas Técnicas:**
```
- [ ] Crear endpoints /api/users/me/notification-preferences
- [ ] Crear DTOs/NotificationPreferenceDto.cs
- [ ] Crear componente Angular notification-preferences
- [ ] Integrar con NotificationService para respetar preferencias
- [ ] Seed data con preferencias por defecto
- [ ] Tests E2E
```

**Dependencias:** US-001, US-008
**Asignado a:** Full-Stack Developer

---

#### US-011: Notificaciones de menciones (@usuario)
| Campo | Valor |
|-------|-------|
| **Tipo** | User Story |
| **Prioridad** | 🟢 Medium |
| **Story Points** | 5 |
| **Complejidad** | Media |
| **Sprint** | Sprint 7 |

**Como** usuario  
**Quiero** ser notificado cuando alguien me mencione en un comentario  
**Para** responder rápidamente a solicitudes directas

**Criterios de Aceptación:**
- [ ] Parser de menciones en comentarios (@username)
- [ ] Autocompletado de usuarios al escribir @
- [ ] Notificación específica para menciones
- [ ] Link directo al comentario desde la notificación
- [ ] Highlight del nombre mencionado en el comentario

**Tareas Técnicas:**
```
- [ ] Crear MentionParser service
- [ ] Modificar CommentService para detectar menciones
- [ ] Crear MentionedInCommentEvent
- [ ] Componente Angular de autocompletado @mention
- [ ] Tests de parsing de menciones
```

**Dependencias:** US-003, US-008
**Asignado a:** Full-Stack Developer

---

## 📊 Resumen de Estimación

### Por Épica

| Épica | Story Points | % del Total |
|-------|--------------|-------------|
| EPIC-001: Infraestructura | 21 | 31% |
| EPIC-002: Email | 10 | 15% |
| EPIC-003: Tiempo Real | 13 | 19% |
| EPIC-004: Frontend | 10 | 15% |
| EPIC-005: Preferencias | 10 | 15% |
| **Buffer/Contingencia** | 5 | 7% |
| **TOTAL** | **69** | **100%** |

### Por Prioridad

| Prioridad | Stories | Story Points |
|-----------|---------|--------------|
| 🔴 Critical | 3 | 21 |
| 🟡 High | 4 | 23 |
| 🟢 Medium | 4 | 20 |
| **TOTAL** | **11** | **64** |

### Por Complejidad

| Complejidad | Stories | Promedio SP |
|-------------|---------|-------------|
| Alta | 3 | 8 |
| Media | 8 | 5 |
| Baja | 0 | - |

---

## 📅 Cronograma del Sprint

### Semana 1 (Días 1-5)

| Día | User Stories | Actividades |
|-----|--------------|-------------|
| 1 | US-001 | Diseño de modelo de datos, crear entidades |
| 2 | US-001, US-002 | Completar migraciones, iniciar servicio base |
| 3 | US-002 | Implementar patrón Strategy, canales de notificación |
| 4 | US-003 | Eventos de dominio con MediatR |
| 5 | US-003, US-004 | Finalizar eventos, iniciar templates email |

### Semana 2 (Días 6-10)

| Día | User Stories | Actividades |
|-----|--------------|-------------|
| 6 | US-004, US-005 | Templates email, cola con retry |
| 7 | US-006 | SignalR Hub backend |
| 8 | US-007, US-008 | Cliente SignalR Angular, lista notificaciones |
| 9 | US-009, US-010 | API CRUD, preferencias usuario |
| 10 | US-011, QA | Menciones, testing final, bug fixes |

---

## 🔄 Definition of Done (DoD)

- [ ] Código completo y funcionando en rama feature
- [ ] Code review aprobado por al menos 1 peer
- [ ] Tests unitarios con >80% cobertura
- [ ] Tests de integración pasando
- [ ] Documentación XML en métodos públicos
- [ ] Sin errores de SonarQube críticos o bloqueantes
- [ ] Swagger actualizado para nuevos endpoints
- [ ] PR aprobado y mergeado a develop
- [ ] Deployado en ambiente de QA
- [ ] QA sign-off

---

## ⚠️ Riesgos y Mitigaciones

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|------------|
| SignalR en producción con balanceador | Media | Alto | Usar Azure SignalR Service o Redis backplane |
| Volumen alto de notificaciones | Media | Medio | Implementar batch processing y throttling |
| Compatibilidad de templates email | Alta | Bajo | Usar MJML o tablas HTML básicas |
| Performance de queries | Baja | Alto | Índices optimizados, paginación, caché |
| SMTP bloqueado por proveedor | Media | Alto | Usar servicio transaccional (SendGrid, SES) |

---

## 🔧 Dependencias Técnicas

### NuGet Packages (Backend)
```xml
<PackageReference Include="Microsoft.AspNetCore.SignalR" Version="1.1.0" />
<PackageReference Include="RazorLight" Version="2.3.1" />
<PackageReference Include="Polly" Version="8.2.0" />
<PackageReference Include="MediatR" Version="12.2.0" />
```

### NPM Packages (Frontend)
```json
{
  "@microsoft/signalr": "^8.0.0",
  "ngx-toastr": "^18.0.0"
}
```

---

## 📈 Métricas de Éxito

| Métrica | Target | Medición |
|---------|--------|----------|
| Tiempo de entrega de email | < 30 segundos | Application Insights |
| Latencia SignalR | < 100ms | Métricas de hub |
| Tasa de entrega email | > 98% | Logs SMTP |
| Usuarios con preferencias configuradas | > 50% | Query DB |
| Reducción en emails perdidos | -90% | Comparación pre/post |

---

## 📝 Notas del Sprint Planning

### Decisiones Tomadas
1. **SignalR vs WebSockets puro**: Se eligió SignalR por integración nativa con ASP.NET Core
2. **Template Engine**: RazorLight para templates de email por familiaridad del equipo
3. **Cola de mensajes**: Channel<T> interno vs RabbitMQ - usar interno por simplicidad, migrar si escala
4. **Push Notifications**: Stub inicial, implementación completa en Sprint 8

### Deuda Técnica Identificada
- [ ] Refactorizar EmailNotificationService existente para usar nuevo sistema
- [ ] Migrar notificaciones hardcodeadas en TicketAssignmentService

### Preguntas Abiertas
- ¿Integración con Slack/Teams en futuro?
- ¿Notificaciones SMS para críticos?

---

## 🔖 Changelog del Sprint

### v1.0.0 - 2025-11-25
- ✅ Sprint Planning inicial creado
- ✅ 11 User Stories definidas
- ✅ Estimaciones completadas
- ✅ Cronograma establecido

### Próxima Actualización
- Daily Standup: 2025-11-26
- Sprint Review: 2025-12-06

---

## 👥 Equipo Asignado

| Rol | Nombre | Responsabilidades |
|-----|--------|-------------------|
| Product Owner | [PO Name] | Priorización, aceptación |
| Scrum Master | [SM Name] | Facilitación, impedimentos |
| Tech Lead | [TL Name] | Arquitectura, code review |
| Backend Dev Sr | [Dev1] | US-002, US-003, US-006 |
| Backend Dev | [Dev2] | US-001, US-005, US-009 |
| Frontend Dev | [Dev3] | US-007, US-008 |
| Full-Stack Dev | [Dev4] | US-004, US-010, US-011 |

---

*Documento generado automáticamente - Última actualización: 2025-11-25*
