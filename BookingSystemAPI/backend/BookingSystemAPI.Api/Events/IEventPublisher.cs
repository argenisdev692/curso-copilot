namespace BookingSystemAPI.Api.Events;

/// <summary>
/// Interfaz genérica para publicación de eventos.
/// Abstrae la implementación de mensajería (MassTransit, RabbitMQ, etc.).
/// </summary>
public interface IEventPublisher
{
    /// <summary>
    /// Publica un evento de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento a publicar.</typeparam>
    /// <param name="event">Instancia del evento.</param>
    /// <param name="correlationId">CorrelationId opcional para trazabilidad. Si no se proporciona, se usa el del evento.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task PublishAsync<TEvent>(TEvent @event, string? correlationId = null, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;

    /// <summary>
    /// Publica múltiples eventos de forma asíncrona.
    /// </summary>
    /// <typeparam name="TEvent">Tipo de los eventos a publicar.</typeparam>
    /// <param name="events">Colección de eventos.</param>
    /// <param name="correlationId">CorrelationId común para todos los eventos.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, string? correlationId = null, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;

    /// <summary>
    /// Envía un evento a una cola específica (punto a punto).
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento a enviar.</typeparam>
    /// <param name="event">Instancia del evento.</param>
    /// <param name="queueName">Nombre de la cola destino.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Task que representa la operación asíncrona.</returns>
    Task SendAsync<TEvent>(TEvent @event, string queueName, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;

    /// <summary>
    /// Programa un evento para ser publicado en un momento futuro.
    /// </summary>
    /// <typeparam name="TEvent">Tipo del evento a programar.</typeparam>
    /// <param name="event">Instancia del evento.</param>
    /// <param name="scheduledTime">Fecha y hora de publicación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Identificador del mensaje programado.</returns>
    Task<Guid> SchedulePublishAsync<TEvent>(TEvent @event, DateTime scheduledTime, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent;
}
