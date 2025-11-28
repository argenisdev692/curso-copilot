namespace TicketManagementSystem.API.Events;

/// <summary>
/// Clase base para todos los eventos del dominio.
/// Proporciona propiedades comunes como MessageId, Timestamp y CorrelationId.
/// </summary>
public abstract class BaseEvent
{
    /// <summary>
    /// Identificador único del mensaje.
    /// </summary>
    public Guid MessageId { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Tipo del evento (ej: "TicketCreated", "TicketAssigned").
    /// </summary>
    public string EventType { get; set; } = string.Empty;

    /// <summary>
    /// Timestamp del evento en UTC.
    /// </summary>
    public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Identificador de correlación para rastrear la cadena de eventos.
    /// </summary>
    public string? CorrelationId { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de BaseEvent.
    /// </summary>
    /// <param name="eventType">Tipo del evento.</param>
    /// <param name="correlationId">Identificador de correlación opcional.</param>
    protected BaseEvent(string eventType, string? correlationId = null)
    {
        EventType = eventType;
        CorrelationId = correlationId ?? Guid.NewGuid().ToString();
    }
}