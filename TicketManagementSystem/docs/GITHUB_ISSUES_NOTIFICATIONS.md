# 📋 GitHub Issues - Sistema de Notificaciones

> Este archivo contiene los issues listos para crear en GitHub Issues/Projects.
> Cada issue incluye labels, milestones y templates para copiar directamente.

---

## Labels Sugeridos

```
priority:critical    #FF0000
priority:high        #FF6600
priority:medium      #FFCC00
type:feature         #0052CC
type:infrastructure  #6554C0
type:frontend        #36B37E
type:backend         #00875A
epic:notifications   #8777D9
size:S (1-3 SP)      #E8E8E8
size:M (5 SP)        #C0C0C0
size:L (8 SP)        #808080
size:XL (13+ SP)     #404040
```

---

## Issue #1: [INFRA] Diseño del modelo de datos para notificaciones

**Labels:** `epic:notifications`, `type:infrastructure`, `priority:critical`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points

### Descripción
Crear las entidades de base de datos necesarias para almacenar y gestionar todas las notificaciones del sistema.

### Tareas
- [ ] Crear `Models/Notification.cs`
- [ ] Crear `Models/NotificationPreference.cs`
- [ ] Crear `Models/NotificationType.cs` (enum)
- [ ] Crear `Models/NotificationChannel.cs` (enum)
- [ ] Actualizar `ApplicationDbContext`
- [ ] Crear migración `AddNotificationSystem`
- [ ] Crear `INotificationRepository`
- [ ] Crear `NotificationRepository`

### Modelo de Datos
```csharp
public class Notification
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
    public string? RelatedEntityType { get; set; }
    public int? RelatedEntityId { get; set; }
    
    public virtual User User { get; set; }
}

public enum NotificationType
{
    TicketCreated,
    TicketAssigned,
    TicketStatusChanged,
    CommentAdded,
    TicketResolved,
    TicketClosed,
    MentionedInComment
}
```

### Criterios de Aceptación
- [ ] Entidades creadas con relaciones correctas
- [ ] Migraciones de EF Core aplicadas sin errores
- [ ] Índices optimizados para `UserId`, `IsRead`, `CreatedAt`
- [ ] Repository con métodos CRUD básicos

### Definition of Done
- [ ] Code review aprobado
- [ ] Tests unitarios
- [ ] Migración aplicada en ambiente de desarrollo

---

## Issue #2: [BACKEND] Servicio base de notificaciones (Patrón Strategy)

**Labels:** `epic:notifications`, `type:backend`, `priority:critical`, `size:L`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 8 Story Points  
**Blocked by:** #1

### Descripción
Implementar un servicio de notificaciones extensible usando el patrón Strategy para soportar múltiples canales de notificación (Email, In-App, Push).

### Tareas
- [ ] Crear `Services/Notifications/INotificationChannel.cs`
- [ ] Crear `Services/Notifications/INotificationService.cs`
- [ ] Crear `Services/Notifications/NotificationService.cs`
- [ ] Crear `Services/Notifications/Channels/EmailNotificationChannel.cs`
- [ ] Crear `Services/Notifications/Channels/InAppNotificationChannel.cs`
- [ ] Crear `Services/Notifications/Channels/PushNotificationChannel.cs` (stub)
- [ ] Crear `DTOs/NotificationRequest.cs`
- [ ] Crear `DTOs/NotificationContext.cs`
- [ ] Registrar servicios en contenedor DI
- [ ] Tests unitarios con mocks

### Diseño Técnico
```csharp
public interface INotificationChannel
{
    NotificationChannelType Channel { get; }
    Task<bool> SendAsync(NotificationContext context, CancellationToken ct = default);
    bool IsEnabled(NotificationPreference preference);
}

public interface INotificationService
{
    Task NotifyAsync(NotificationRequest request, CancellationToken ct = default);
    Task NotifyBatchAsync(IEnumerable<NotificationRequest> requests, CancellationToken ct = default);
}

public class NotificationService : INotificationService
{
    private readonly IEnumerable<INotificationChannel> _channels;
    private readonly INotificationRepository _repository;
    private readonly ILogger<NotificationService> _logger;
    
    // Orquesta los canales según preferencias del usuario
}
```

### Criterios de Aceptación
- [ ] Patrón Strategy implementado correctamente
- [ ] Cada canal es independiente y testeable
- [ ] Logging estructurado con CorrelationId
- [ ] Manejo de errores por canal sin afectar otros canales
- [ ] Cobertura de tests >80%

---

## Issue #3: [BACKEND] Sistema de eventos de dominio con MediatR

**Labels:** `epic:notifications`, `type:backend`, `priority:critical`, `size:L`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 8 Story Points  
**Blocked by:** #2

### Descripción
Implementar eventos de dominio usando MediatR Notifications para desacoplar la lógica de negocio de las notificaciones.

### Tareas
- [ ] Crear `Features/Notifications/Events/TicketCreatedEvent.cs`
- [ ] Crear `Features/Notifications/Events/TicketAssignedEvent.cs`
- [ ] Crear `Features/Notifications/Events/TicketStatusChangedEvent.cs`
- [ ] Crear `Features/Notifications/Events/CommentAddedEvent.cs`
- [ ] Crear handlers para cada evento
- [ ] Integrar publicación de eventos en `TicketService`
- [ ] Integrar publicación de eventos en `CommentService`
- [ ] Tests de integración

### Eventos de Dominio
```csharp
public record TicketCreatedEvent(int TicketId, int CreatedById, string Title) : INotification;

public record TicketAssignedEvent(
    int TicketId, 
    int AssignedToId, 
    int? PreviousAssigneeId,
    string TicketTitle
) : INotification;

public record TicketStatusChangedEvent(
    int TicketId, 
    Status OldStatus, 
    Status NewStatus, 
    int ChangedById
) : INotification;

public record CommentAddedEvent(
    int CommentId, 
    int TicketId, 
    int AuthorId,
    IEnumerable<int>? MentionedUserIds
) : INotification;
```

### Ejemplo de Handler
```csharp
public class TicketAssignedHandler : INotificationHandler<TicketAssignedEvent>
{
    private readonly INotificationService _notificationService;
    
    public async Task Handle(TicketAssignedEvent notification, CancellationToken ct)
    {
        await _notificationService.NotifyAsync(new NotificationRequest
        {
            UserId = notification.AssignedToId,
            Type = NotificationType.TicketAssigned,
            Title = "Nuevo ticket asignado",
            Message = $"Se te ha asignado el ticket: {notification.TicketTitle}",
            RelatedEntityType = "Ticket",
            RelatedEntityId = notification.TicketId
        }, ct);
    }
}
```

### Criterios de Aceptación
- [ ] Eventos publicados desde servicios de negocio
- [ ] Handlers ejecutan notificaciones correctamente
- [ ] Ejecución asíncrona (no bloquea operación principal)
- [ ] Errores en handlers no afectan la operación principal

---

## Issue #4: [BACKEND] Templates de email HTML responsivos

**Labels:** `epic:notifications`, `type:backend`, `priority:high`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #2

### Descripción
Crear templates de email HTML profesionales y responsivos usando Razor Engine para las diferentes notificaciones del sistema.

### Tareas
- [ ] Crear carpeta `Templates/Email/`
- [ ] Implementar `IEmailTemplateService`
- [ ] Crear `EmailTemplateService` con RazorLight
- [ ] Diseñar template base `_Layout.cshtml`
- [ ] Crear template `TicketCreated.cshtml`
- [ ] Crear template `TicketAssigned.cshtml`
- [ ] Crear template `TicketStatusChanged.cshtml`
- [ ] Crear template `NewComment.cshtml`
- [ ] Crear template `TicketResolved.cshtml`
- [ ] Tests de renderizado

### Estructura de Templates
```
Templates/
├── Email/
│   ├── _Layout.cshtml
│   ├── TicketCreated.cshtml
│   ├── TicketAssigned.cshtml
│   ├── TicketStatusChanged.cshtml
│   ├── NewComment.cshtml
│   └── TicketResolved.cshtml
```

### Template Base (_Layout.cshtml)
```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title</title>
    <style>
        /* Inline CSS para compatibilidad */
    </style>
</head>
<body>
    <table class="container">
        <tr><td class="header">🎫 Ticket Management System</td></tr>
        <tr><td class="content">@RenderBody()</td></tr>
        <tr><td class="footer">
            <a href="{{UnsubscribeUrl}}">Configurar preferencias</a>
        </td></tr>
    </table>
</body>
</html>
```

### Criterios de Aceptación
- [ ] Templates responsive (mobile-friendly)
- [ ] Variables dinámicas funcionando
- [ ] Renderizado correcto en Outlook, Gmail, Apple Mail
- [ ] Footer con link de preferencias

---

## Issue #5: [BACKEND] Cola de emails con retry y dead-letter

**Labels:** `epic:notifications`, `type:backend`, `priority:high`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #4

### Descripción
Implementar una cola de emails robusta con reintentos automáticos y dead-letter queue para garantizar la entrega de notificaciones.

### Tareas
- [ ] Refactorizar `EmailNotificationService` existente
- [ ] Crear tabla `EmailQueue` en base de datos
- [ ] Crear `EmailQueueProcessor` (Background Service)
- [ ] Implementar lógica de dead-letter
- [ ] Crear endpoint admin para gestión de cola
- [ ] Métricas con logging estructurado

### Modelo de Cola
```csharp
public class EmailQueueItem
{
    public int Id { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
    public int Attempts { get; set; }
    public DateTime? LastAttemptAt { get; set; }
    public string? LastError { get; set; }
    public EmailQueueStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
    public int? RelatedNotificationId { get; set; }
}

public enum EmailQueueStatus
{
    Pending,
    Processing,
    Sent,
    Failed,
    DeadLetter
}
```

### Política de Reintentos
- Máximo 3 intentos
- Backoff exponencial: 1min, 5min, 15min
- Después de 3 fallos → Dead Letter Queue

### Criterios de Aceptación
- [ ] Reintentos con backoff exponencial
- [ ] Dead-letter queue funcional
- [ ] Endpoint para ver estado de cola
- [ ] Logs detallados de cada intento

---

## Issue #6: [BACKEND] Hub de SignalR para notificaciones

**Labels:** `epic:notifications`, `type:backend`, `priority:high`, `size:L`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 8 Story Points  
**Blocked by:** #2

### Descripción
Implementar un Hub de SignalR para enviar notificaciones en tiempo real a los usuarios conectados.

### Tareas
- [ ] Instalar `Microsoft.AspNetCore.SignalR`
- [ ] Crear `Hubs/NotificationHub.cs`
- [ ] Configurar autenticación JWT en SignalR
- [ ] Crear `IHubNotificationService`
- [ ] Crear `HubNotificationService`
- [ ] Integrar con `InAppNotificationChannel`
- [ ] Configurar CORS para SignalR
- [ ] Tests de conexión

### NotificationHub
```csharp
[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public async Task MarkAsRead(int notificationId)
    {
        // Lógica para marcar como leída
    }
}

public interface INotificationHubClient
{
    Task ReceiveNotification(NotificationDto notification);
    Task UpdateUnreadCount(int count);
}
```

### Configuración en Program.cs
```csharp
builder.Services.AddSignalR();

// ...

app.MapHub<NotificationHub>("/hubs/notifications");
```

### Criterios de Aceptación
- [ ] Autenticación JWT funcionando
- [ ] Grupos por usuario
- [ ] Reconexión automática
- [ ] Métricas de conexiones
- [ ] Tests de integración

---

## Issue #7: [FRONTEND] Cliente SignalR en Angular

**Labels:** `epic:notifications`, `type:frontend`, `priority:high`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #6

### Descripción
Implementar el cliente SignalR en Angular para recibir notificaciones en tiempo real y mantener la conexión con el servidor.

### Tareas
- [ ] `npm install @microsoft/signalr`
- [ ] Crear `services/notification-hub.service.ts`
- [ ] Crear `models/notification.model.ts`
- [ ] Crear estado de notificaciones (Signals o NgRx)
- [ ] Integrar con `AuthService` para token JWT
- [ ] Manejar reconexión automática
- [ ] Tests de servicio

### NotificationHubService
```typescript
@Injectable({ providedIn: 'root' })
export class NotificationHubService {
  private hubConnection: HubConnection | null = null;
  private notifications$ = new BehaviorSubject<Notification[]>([]);
  private connectionState$ = new BehaviorSubject<HubConnectionState>('Disconnected');

  constructor(private authService: AuthService) {}

  async connect(): Promise<void> {
    const token = this.authService.getToken();
    
    this.hubConnection = new HubConnectionBuilder()
      .withUrl('/hubs/notifications', {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      this.notifications$.next([notification, ...this.notifications$.value]);
      // Mostrar toast
    });

    await this.hubConnection.start();
  }

  disconnect(): Promise<void> {
    return this.hubConnection?.stop() ?? Promise.resolve();
  }
}
```

### Criterios de Aceptación
- [ ] Conexión automática al login
- [ ] Desconexión al logout
- [ ] Reconexión automática
- [ ] Estado de conexión visible
- [ ] Notificaciones en tiempo real funcionando

---

## Issue #8: [FRONTEND] Componente de lista de notificaciones

**Labels:** `epic:notifications`, `type:frontend`, `priority:medium`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #7

### Descripción
Crear los componentes de interfaz de usuario para mostrar y gestionar las notificaciones del usuario.

### Tareas
- [ ] Crear `components/notifications/notification-dropdown/`
- [ ] Crear `components/notifications/notification-list/`
- [ ] Crear `components/notifications/notification-item/`
- [ ] Crear `pages/notifications/`
- [ ] Crear `services/notification.service.ts` (HTTP)
- [ ] Integrar con `notification-hub.service.ts`
- [ ] Estilos responsive
- [ ] Tests de componentes

### Componentes
```
src/app/
├── components/
│   └── notifications/
│       ├── notification-dropdown/
│       │   ├── notification-dropdown.component.ts
│       │   ├── notification-dropdown.component.html
│       │   └── notification-dropdown.component.scss
│       ├── notification-list/
│       └── notification-item/
├── pages/
│   └── notifications/
│       ├── notifications.component.ts
│       └── notifications.routes.ts
└── services/
    └── notification.service.ts
```

### UI Specs
- Dropdown desde icono de campana en header
- Badge con contador de no leídas
- Lista con scroll virtual si hay muchas
- Filtros: Todas | No leídas | Por tipo
- Acciones: Marcar leída, eliminar
- Click navega al recurso relacionado

### Criterios de Aceptación
- [ ] Dropdown funcional desde header
- [ ] Badge con contador actualizado en tiempo real
- [ ] Paginación en página completa
- [ ] Filtros funcionando
- [ ] Diseño responsive
- [ ] Skeleton loading

---

## Issue #9: [BACKEND] API REST de notificaciones

**Labels:** `epic:notifications`, `type:backend`, `priority:medium`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #1

### Descripción
Crear los endpoints REST para gestionar las notificaciones del usuario autenticado.

### Tareas
- [ ] Crear `Controllers/NotificationsController.cs`
- [ ] Crear `DTOs/NotificationDto.cs`
- [ ] Crear `DTOs/NotificationFilterDto.cs`
- [ ] Implementar métodos en `INotificationService`
- [ ] Configurar AutoMapper profiles
- [ ] Documentar en Swagger
- [ ] Tests de controller

### Endpoints
```
GET    /api/notifications
       Query: page, pageSize, isRead, type
       Response: PagedResult<NotificationDto>

GET    /api/notifications/unread-count
       Response: { count: number }

GET    /api/notifications/{id}
       Response: NotificationDto

PUT    /api/notifications/{id}/read
       Response: 204 No Content

PUT    /api/notifications/mark-all-read
       Response: { markedCount: number }

DELETE /api/notifications/{id}
       Response: 204 No Content
```

### NotificationDto
```csharp
public record NotificationDto
{
    public int Id { get; init; }
    public NotificationType Type { get; init; }
    public string Title { get; init; }
    public string Message { get; init; }
    public bool IsRead { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? ReadAt { get; init; }
    public string? RelatedEntityType { get; init; }
    public int? RelatedEntityId { get; init; }
    public string? RelatedEntityUrl { get; init; }
}
```

### Criterios de Aceptación
- [ ] Endpoints protegidos con `[Authorize]`
- [ ] Solo retorna notificaciones del usuario autenticado
- [ ] Paginación correcta
- [ ] Filtros funcionando
- [ ] Swagger documentado con ejemplos

---

## Issue #10: [FULLSTACK] Preferencias de notificación por usuario

**Labels:** `epic:notifications`, `type:frontend`, `type:backend`, `priority:medium`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #1, #8

### Descripción
Permitir a los usuarios configurar sus preferencias de notificación, eligiendo qué tipos de notificaciones recibir y por qué canal.

### Tareas Backend
- [ ] Crear endpoint `GET /api/users/me/notification-preferences`
- [ ] Crear endpoint `PUT /api/users/me/notification-preferences`
- [ ] Crear `NotificationPreferenceDto`
- [ ] Lógica para valores por defecto

### Tareas Frontend
- [ ] Crear página de preferencias en perfil
- [ ] Matriz de configuración (Tipo × Canal)
- [ ] Guardar cambios automáticamente
- [ ] Feedback visual de guardado

### Modelo de Preferencias
```csharp
public class NotificationPreference
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public NotificationType Type { get; set; }
    public bool EmailEnabled { get; set; } = true;
    public bool InAppEnabled { get; set; } = true;
    public bool PushEnabled { get; set; } = false;
    
    public virtual User User { get; set; }
}
```

### UI de Preferencias
| Evento | Email | In-App | Push |
|--------|-------|--------|------|
| Ticket asignado | ✓ | ✓ | ✓ |
| Cambio de estado | ○ | ✓ | ○ |
| Nuevo comentario | ✓ | ✓ | ○ |
| Mencionado | ✓ | ✓ | ✓ |

### Criterios de Aceptación
- [ ] UI intuitiva de configuración
- [ ] Cambios se guardan correctamente
- [ ] NotificationService respeta preferencias
- [ ] Valores por defecto sensatos

---

## Issue #11: [FULLSTACK] Notificaciones de menciones (@usuario)

**Labels:** `epic:notifications`, `type:frontend`, `type:backend`, `priority:medium`, `size:M`  
**Milestone:** Sprint 7 - Sistema de Notificaciones  
**Estimate:** 5 Story Points  
**Blocked by:** #3, #8

### Descripción
Implementar la funcionalidad de mencionar usuarios en comentarios usando @username y notificarlos cuando son mencionados.

### Tareas Backend
- [ ] Crear `MentionParserService`
- [ ] Modificar `CommentService` para detectar menciones
- [ ] Crear `MentionedInCommentEvent`
- [ ] Handler para notificar usuarios mencionados

### Tareas Frontend
- [ ] Componente de autocompletado @mention
- [ ] Integrar en textarea de comentarios
- [ ] Highlight de menciones en comentarios renderizados

### MentionParser
```csharp
public interface IMentionParserService
{
    IEnumerable<string> ExtractMentions(string text);
    Task<IEnumerable<int>> ResolveUserIdsAsync(IEnumerable<string> usernames);
}

public class MentionParserService : IMentionParserService
{
    private static readonly Regex MentionRegex = new(@"@(\w+)", RegexOptions.Compiled);
    
    public IEnumerable<string> ExtractMentions(string text)
    {
        return MentionRegex.Matches(text)
            .Select(m => m.Groups[1].Value)
            .Distinct();
    }
}
```

### Autocompletado Frontend
```typescript
@Component({
  selector: 'app-mention-input',
  template: `
    <textarea 
      [(ngModel)]="content"
      (input)="onInput($event)">
    </textarea>
    <div class="mention-dropdown" *ngIf="showSuggestions">
      <div *ngFor="let user of filteredUsers" (click)="selectUser(user)">
        @{{user.username}} - {{user.fullName}}
      </div>
    </div>
  `
})
```

### Criterios de Aceptación
- [ ] Parser detecta @username correctamente
- [ ] Autocompletado muestra usuarios
- [ ] Usuarios mencionados reciben notificación
- [ ] Menciones se muestran destacadas en comentarios

---

## 📊 Resumen de Issues

| # | Título | SP | Prioridad | Dependencias |
|---|--------|----|-----------|--------------| 
| 1 | Modelo de datos | 5 | Critical | - |
| 2 | Servicio Strategy | 8 | Critical | #1 |
| 3 | Eventos MediatR | 8 | Critical | #2 |
| 4 | Templates email | 5 | High | #2 |
| 5 | Cola emails | 5 | High | #4 |
| 6 | Hub SignalR | 8 | High | #2 |
| 7 | Cliente Angular | 5 | High | #6 |
| 8 | UI Notificaciones | 5 | Medium | #7 |
| 9 | API REST | 5 | Medium | #1 |
| 10 | Preferencias | 5 | Medium | #1, #8 |
| 11 | Menciones | 5 | Medium | #3, #8 |
| **Total** | | **64** | | |

---

## 🔗 Diagrama de Dependencias

```
┌─────────┐
│ Issue 1 │ (Modelo de Datos)
└────┬────┘
     │
     ├─────────────────────────────┐
     │                             │
     ▼                             ▼
┌─────────┐                   ┌─────────┐
│ Issue 2 │                   │ Issue 9 │
│(Strategy)│                  │(API REST)│
└────┬────┘                   └─────────┘
     │
     ├───────────────┬───────────────┐
     │               │               │
     ▼               ▼               ▼
┌─────────┐    ┌─────────┐    ┌─────────┐
│ Issue 3 │    │ Issue 4 │    │ Issue 6 │
│(MediatR)│    │(Templates)│   │(SignalR)│
└────┬────┘    └────┬────┘    └────┬────┘
     │               │               │
     │               ▼               ▼
     │         ┌─────────┐    ┌─────────┐
     │         │ Issue 5 │    │ Issue 7 │
     │         │(Cola)   │    │(Angular)│
     │         └─────────┘    └────┬────┘
     │                              │
     │                              ▼
     │                        ┌─────────┐
     │                        │ Issue 8 │
     │                        │(UI)     │
     │                        └────┬────┘
     │                              │
     ├──────────────────────────────┤
     │                              │
     ▼                              ▼
┌─────────┐                   ┌─────────┐
│ Issue 11│                   │ Issue 10│
│(Mentions)│                  │(Prefs)  │
└─────────┘                   └─────────┘
```

---

*Generado: 2025-11-25 | Sprint 7 - Sistema de Notificaciones*
