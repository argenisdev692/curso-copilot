# 🐰 Integración de RabbitMQ con GitHub Copilot

> **Framework utilizado**: C.R.E.A.T.E.  
> **Proyecto**: TicketManagementSystem  
> **Objetivo**: Implementar mensajería asíncrona para notificaciones de tickets

---

## 📋 Prompt Estructurado (C.R.E.A.T.E.)

### C - Context (Contexto)

```markdown
**Proyecto:** TicketManagementSystem - API REST en .NET 8 con EF Core 9
**Arquitectura actual:** 
- Patrón Repository + Unit of Work
- CQRS con MediatR
- Result Pattern para manejo de errores
- Inyección de dependencias

**Archivos relevantes:**
- Services/TicketService.cs - Lógica de negocio de tickets
- Controllers/TicketsController.cs - Endpoints REST
- Models/Ticket.cs - Entidad principal
- Program.cs - Configuración de servicios

**Stack tecnológico:**
- .NET 8, C# 12
- Entity Framework Core 9
- AutoMapper, FluentValidation
- Docker para infraestructura
```

---

### R - Request (Solicitud)

```markdown
**Objetivo principal:**
Implementar RabbitMQ como sistema de mensajería para:

1. **Notificaciones asíncronas** cuando:
   - Se crea un nuevo ticket
   - Se asigna un ticket a un usuario
   - Cambia el estado de un ticket
   - Se agrega un comentario

2. **Desacoplamiento** entre:
   - API (producer) y servicios de notificación (consumers)
   - Posibles integraciones futuras (email, Slack, webhooks)

3. **Componentes a crear:**
   - Servicio de conexión a RabbitMQ (singleton)
   - Publisher genérico para eventos
   - Consumer base con retry logic
   - DTOs para mensajes (eventos)
   - Configuración en appsettings.json
   - Health check para RabbitMQ
```

---

### E - Examples (Ejemplos)

```markdown
**Flujo esperado - Creación de Ticket:**

1. Usuario crea ticket vía POST /api/tickets
2. TicketService.CreateAsync() guarda en BD
3. Publica evento TicketCreatedEvent en exchange "ticket.events"
4. Consumer recibe mensaje y envía notificación

**Ejemplo de mensaje:**
```json
{
  "eventType": "TicketCreated",
  "timestamp": "2025-11-26T10:30:00Z",
  "correlationId": "abc-123-def",
  "payload": {
    "ticketId": 42,
    "title": "Error en login",
    "priority": "High",
    "createdById": 5,
    "assignedToId": null
  }
}
```

**Ejemplo de configuración esperada:**
```json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "Port": 5672,
    "UserName": "guest",
    "Password": "guest",
    "VirtualHost": "/",
    "ExchangeName": "ticket.events",
    "RetryCount": 3,
    "RetryDelayMs": 1000
  }
}
```
```

---

### A - Adjustments (Ajustes/Personalizaciones)

```markdown
**Requisitos específicos:**

1. **Resiliencia:**
   - Retry exponencial en caso de fallo de conexión
   - Dead Letter Queue (DLQ) para mensajes fallidos
   - Circuit breaker pattern (opcional, mencionar cómo)

2. **Observabilidad:**
   - Logging estructurado con ILogger<T>
   - CorrelationId en todos los mensajes
   - Métricas de mensajes enviados/recibidos

3. **Configuración:**
   - Usar IOptions<RabbitMQSettings> pattern
   - Conexión como Singleton (una por aplicación)
   - Channel pooling para mejor performance

4. **Seguridad:**
   - Credenciales en User Secrets o Azure Key Vault
   - NO hardcodear passwords
   - Validar certificados en producción (TLS)

5. **Testing:**
   - Interface IRabbitMQPublisher para mockear
   - Testcontainers para integration tests
```

---

### T - Type of Output (Tipo de Salida)

```markdown
**Archivos a generar:**

1. **Configuración:**
   - `Settings/RabbitMQSettings.cs` - Record con configuración
   - Actualizar `appsettings.json` con sección RabbitMQ

2. **Infraestructura:**
   - `Infrastructure/RabbitMQ/IRabbitMQConnection.cs` - Interface
   - `Infrastructure/RabbitMQ/RabbitMQConnection.cs` - Singleton
   - `Infrastructure/RabbitMQ/IRabbitMQPublisher.cs` - Interface
   - `Infrastructure/RabbitMQ/RabbitMQPublisher.cs` - Implementación

3. **Eventos:**
   - `Events/BaseEvent.cs` - Clase base con metadata
   - `Events/TicketCreatedEvent.cs`
   - `Events/TicketAssignedEvent.cs`
   - `Events/TicketStatusChangedEvent.cs`

4. **Consumer (ejemplo):**
   - `Consumers/BaseConsumer.cs` - Con retry logic
   - `Consumers/NotificationConsumer.cs` - Ejemplo

5. **Extensiones:**
   - `Extensions/RabbitMQServiceExtensions.cs` - Para Program.cs

6. **Health Check:**
   - `HealthChecks/RabbitMQHealthCheck.cs`

7. **Docker:**
   - Actualizar `docker-compose.yml` con servicio RabbitMQ

**Formato del código:**
- Comentarios XML en métodos públicos
- Async/await en todas las operaciones I/O
- Usar CancellationToken donde aplique
- Seguir convenciones del proyecto existente
```

---

### E - Extras (Información Adicional)

```markdown
**Edge cases a considerar:**

1. **RabbitMQ no disponible al inicio:**
   - La API debe iniciar aunque RabbitMQ no esté
   - Implementar reconexión automática
   - Loggear warnings, no lanzar excepciones

2. **Mensajes duplicados:**
   - Diseñar consumers idempotentes
   - Incluir messageId único en cada evento

3. **Orden de mensajes:**
   - Para un mismo ticket, mantener orden FIFO
   - Usar routing key basado en ticketId

4. **Backpressure:**
   - Limitar mensajes en memoria (prefetch count)
   - Acknowledge manual después de procesar

**Consideraciones de producción:**

- Cluster de RabbitMQ para HA
- Monitoreo con Management Plugin
- Alertas por cola saturada
- Backup de mensajes persistentes

**NO incluir:**
- Implementación completa de email/Slack (solo interfaces)
- UI de administración
- Migración de datos existentes
```

---

## 🚀 Prompt Listo para Copilot

Copia este prompt completo en el chat de Copilot o usa `#file:rabbitMQ-con-copilot.md` para referenciarlo:

```markdown
@workspace Usando el framework C.R.E.A.T.E. definido en este archivo, 
implementa la integración de RabbitMQ para TicketManagementSystem.

Comienza por:
1. Crear RabbitMQSettings.cs con la configuración
2. Crear la interfaz y clase de conexión (IRabbitMQConnection)
3. Crear el publisher genérico (IRabbitMQPublisher)

Sigue las especificaciones del archivo para patrones, 
manejo de errores y estructura de carpetas.
```

---

## 📁 Estructura de Carpetas Resultante

```
TicketManagementSystem.API/
├── Settings/
│   └── RabbitMQSettings.cs
├── Infrastructure/
│   └── RabbitMQ/
│       ├── IRabbitMQConnection.cs
│       ├── RabbitMQConnection.cs
│       ├── IRabbitMQPublisher.cs
│       └── RabbitMQPublisher.cs
├── Events/
│   ├── BaseEvent.cs
│   ├── TicketCreatedEvent.cs
│   ├── TicketAssignedEvent.cs
│   └── TicketStatusChangedEvent.cs
├── Consumers/
│   ├── BaseConsumer.cs
│   └── NotificationConsumer.cs
├── Extensions/
│   └── RabbitMQServiceExtensions.cs
└── HealthChecks/
    └── RabbitMQHealthCheck.cs
```

---

## ✅ Ejemplo de Código Esperado

### RabbitMQSettings.cs

```csharp
namespace TicketManagementSystem.API.Settings;

/// <summary>
/// Configuración para conexión a RabbitMQ
/// </summary>
public record RabbitMQSettings
{
    public const string SectionName = "RabbitMQ";
    
    public string HostName { get; init; } = "localhost";
    public int Port { get; init; } = 5672;
    public string UserName { get; init; } = "guest";
    public string Password { get; init; } = "guest";
    public string VirtualHost { get; init; } = "/";
    public string ExchangeName { get; init; } = "ticket.events";
    public int RetryCount { get; init; } = 3;
    public int RetryDelayMs { get; init; } = 1000;
    public bool UseSsl { get; init; } = false;
}
```

### IRabbitMQPublisher.cs

```csharp
using TicketManagementSystem.API.Events;

namespace TicketManagementSystem.API.Infrastructure.RabbitMQ;

/// <summary>
/// Interface para publicar eventos a RabbitMQ
/// </summary>
public interface IRabbitMQPublisher
{
    /// <summary>
    /// Publica un evento de forma asíncrona
    /// </summary>
    /// <typeparam name="T">Tipo del evento (debe heredar de BaseEvent)</typeparam>
    /// <param name="event">Evento a publicar</param>
    /// <param name="routingKey">Routing key para el mensaje</param>
    /// <param name="ct">Token de cancelación</param>
    /// <returns>True si se publicó correctamente</returns>
    Task<bool> PublishAsync<T>(T @event, string routingKey, CancellationToken ct = default) 
        where T : BaseEvent;
}
```

### BaseEvent.cs

```csharp
namespace TicketManagementSystem.API.Events;

/// <summary>
/// Clase base para todos los eventos de dominio
/// </summary>
public abstract record BaseEvent
{
    /// <summary>
    /// Identificador único del mensaje
    /// </summary>
    public Guid MessageId { get; init; } = Guid.NewGuid();
    
    /// <summary>
    /// Tipo de evento para deserialización
    /// </summary>
    public string EventType => GetType().Name;
    
    /// <summary>
    /// Timestamp UTC de creación
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    
    /// <summary>
    /// ID de correlación para trazabilidad
    /// </summary>
    public string? CorrelationId { get; init; }
}
```

### TicketCreatedEvent.cs

```csharp
namespace TicketManagementSystem.API.Events;

/// <summary>
/// Evento emitido cuando se crea un nuevo ticket
/// </summary>
public record TicketCreatedEvent : BaseEvent
{
    public int TicketId { get; init; }
    public required string Title { get; init; }
    public required string Priority { get; init; }
    public int CreatedById { get; init; }
    public int? AssignedToId { get; init; }
}
```

---

## 🔄 Uso en TicketService

```csharp
// En TicketService.CreateAsync()
public async Task<Result<Ticket>> CreateAsync(CreateTicketDto dto, int userId, CancellationToken ct)
{
    // ... lógica existente de creación ...
    
    var ticket = await _ticketRepository.AddAsync(newTicket, ct);
    await _unitOfWork.SaveChangesAsync(ct);
    
    // Publicar evento de forma asíncrona (fire-and-forget con logging)
    _ = _rabbitMQPublisher.PublishAsync(
        new TicketCreatedEvent
        {
            TicketId = ticket.Id,
            Title = ticket.Title,
            Priority = ticket.Priority.ToString(),
            CreatedById = userId,
            AssignedToId = ticket.AssignedToId,
            CorrelationId = Activity.Current?.Id
        },
        routingKey: $"ticket.{ticket.Id}.created",
        ct
    );
    
    return Result<Ticket>.Success(ticket);
}
```

---

## 🐳 Docker Compose

```yaml
# Agregar a docker-compose.yml
services:
  rabbitmq:
    image: rabbitmq:3-management-alpine
    container_name: ticketsystem-rabbitmq
    ports:
      - "5672:5672"   # AMQP
      - "15672:15672" # Management UI
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq
    healthcheck:
      test: rabbitmq-diagnostics -q ping
      interval: 30s
      timeout: 10s
      retries: 3

volumes:
  rabbitmq_data:
```

---

## 📚 Recursos Adicionales

| Recurso | URL |
|---------|-----|
| RabbitMQ .NET Client | https://www.rabbitmq.com/dotnet.html |
| MassTransit (alternativa) | https://masstransit.io/ |
| EasyNetQ (wrapper) | https://github.com/EasyNetQ/EasyNetQ |
| Polly (resiliencia) | https://github.com/App-vNext/Polly |

---

## ⚠️ Notas Importantes

1. **Paquetes NuGet requeridos:**
   ```bash
   dotnet add package RabbitMQ.Client
   dotnet add package Polly
   dotnet add package AspNetCore.HealthChecks.Rabbitmq
   ```

2. **Variables de entorno para producción:**
   ```bash
   RABBITMQ__HOSTNAME=rabbitmq.production.local
   RABBITMQ__USERNAME=app_user
   RABBITMQ__PASSWORD=<from-keyvault>
   RABBITMQ__USESSL=true
   ```

3. **Orden de implementación sugerido:**
   1. Settings y configuración
   2. Conexión singleton
   3. Publisher básico
   4. Integrar en un Service (ej: TicketService)
   5. Consumer de ejemplo
   6. Health checks
   7. Tests de integración
