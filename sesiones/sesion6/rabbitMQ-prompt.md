# 🐰 Prompt: Integración RabbitMQ con Copilot

> **Framework**: C.R.E.A.T.E.  

---

## C - Context (Contexto)

```markdown
Proyecto: TicketManagementSystem - API REST en .NET 8 con EF Core 9

Arquitectura actual: 
- Patrón Repository + Unit of Work
- CQRS con MediatR
- Result Pattern para manejo de errores
- Inyección de dependencias

Archivos relevantes:
- Services/TicketService.cs - Lógica de negocio de tickets
- Controllers/TicketsController.cs - Endpoints REST
- Models/Ticket.cs - Entidad principal
- Program.cs - Configuración de servicios

Stack: .NET 8, C# 12, EF Core 9, AutoMapper, FluentValidation, Docker
```

---

## R - Request (Solicitud)

```markdown
Implementar RabbitMQ como sistema de mensajería para:

1. Notificaciones asíncronas cuando:
   - Se crea un nuevo ticket
   - Se asigna un ticket a un usuario
   - Cambia el estado de un ticket
   - Se agrega un comentario

2. Desacoplamiento entre:
   - API (producer) y servicios de notificación (consumers)
   - Posibles integraciones futuras (email, Slack, webhooks)

3. Componentes a crear:
   - Servicio de conexión a RabbitMQ (singleton)
   - Publisher genérico para eventos
   - Consumer base con retry logic
   - DTOs para mensajes (eventos)
   - Configuración en appsettings.json
   - Health check para RabbitMQ
```

---

## E - Examples (Ejemplos)

```markdown
Flujo esperado - Creación de Ticket:
1. Usuario crea ticket vía POST /api/tickets
2. TicketService.CreateAsync() guarda en BD
3. Publica evento TicketCreatedEvent en exchange "ticket.events"
4. Consumer recibe mensaje y envía notificación

Ejemplo de mensaje JSON:
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

Ejemplo de configuración appsettings.json:
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

---

## A - Adjustments (Ajustes)

```markdown
Requisitos específicos:

1. Resiliencia:
   - Retry exponencial en caso de fallo de conexión
   - Dead Letter Queue (DLQ) para mensajes fallidos
   - Circuit breaker pattern

2. Observabilidad:
   - Logging estructurado con ILogger<T>
   - CorrelationId en todos los mensajes
   - Métricas de mensajes enviados/recibidos

3. Configuración:
   - Usar IOptions<RabbitMQSettings> pattern
   - Conexión como Singleton
   - Channel pooling para mejor performance

4. Seguridad:
   - Credenciales en User Secrets o Azure Key Vault
   - NO hardcodear passwords
   - Validar certificados en producción (TLS)

5. Testing:
   - Interface IRabbitMQPublisher para mockear
   - Testcontainers para integration tests
```

---

## T - Type of Output (Tipo de Salida)

```markdown
Archivos a generar:

1. Configuración:
   - Settings/RabbitMQSettings.cs

2. Infraestructura:
   - Infrastructure/RabbitMQ/IRabbitMQConnection.cs
   - Infrastructure/RabbitMQ/RabbitMQConnection.cs
   - Infrastructure/RabbitMQ/IRabbitMQPublisher.cs
   - Infrastructure/RabbitMQ/RabbitMQPublisher.cs

3. Eventos:
   - Events/BaseEvent.cs
   - Events/TicketCreatedEvent.cs
   - Events/TicketAssignedEvent.cs
   - Events/TicketStatusChangedEvent.cs

4. Consumer:
   - Consumers/BaseConsumer.cs
   - Consumers/NotificationConsumer.cs

5. Extensiones:
   - Extensions/RabbitMQServiceExtensions.cs

6. Health Check:
   - HealthChecks/RabbitMQHealthCheck.cs

Formato:
- Comentarios XML en métodos públicos
- Async/await en todas las operaciones I/O
- Usar CancellationToken donde aplique
```

---

## E - Extras (Información Adicional)

```markdown
Edge cases a considerar:

1. RabbitMQ no disponible al inicio:
   - La API debe iniciar aunque RabbitMQ no esté
   - Implementar reconexión automática
   - Loggear warnings, no lanzar excepciones

2. Mensajes duplicados:
   - Diseñar consumers idempotentes
   - Incluir messageId único en cada evento

3. Orden de mensajes:
   - Para un mismo ticket, mantener orden FIFO
   - Usar routing key basado en ticketId

4. Backpressure:
   - Limitar mensajes en memoria (prefetch count)
   - Acknowledge manual después de procesar

NO incluir:
- Implementación completa de email/Slack (solo interfaces)
- UI de administración
- Migración de datos existentes
```

---

Comienza creando en este orden:
1. RabbitMQSettings.cs
2. IRabbitMQConnection.cs y RabbitMQConnection.cs
3. IRabbitMQPublisher.cs y RabbitMQPublisher.cs
4. BaseEvent.cs y TicketCreatedEvent.cs
```

---

## 📁 Estructura Resultante

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

## 📦 NuGet Requeridos

```bash
dotnet add package RabbitMQ.Client
dotnet add package Polly
dotnet add package AspNetCore.HealthChecks.Rabbitmq
```

---

## 🐳 Docker Compose

```yaml
services:
  rabbitmq:
    image: rabbitmq:3-management-alpine
    container_name: ticketsystem-rabbitmq
    ports:
      - "5672:5672"
      - "15672:15672"
    environment:
      RABBITMQ_DEFAULT_USER: guest
      RABBITMQ_DEFAULT_PASS: guest
    volumes:
      - rabbitmq_data:/var/lib/rabbitmq

volumes:
  rabbitmq_data:
```
