using MassTransit;

namespace BookingSystemAPI.Api.Events;

/// <summary>
/// Implementación del publicador de eventos usando MassTransit.
/// Proporciona publicación con CorrelationId, logging y manejo de errores.
/// </summary>
public class EventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;
    private readonly ILogger<EventPublisher> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del publicador de eventos.
    /// </summary>
    /// <param name="publishEndpoint">Endpoint de publicación de MassTransit.</param>
    /// <param name="bus">Bus de mensajería.</param>
    /// <param name="logger">Logger para trazabilidad.</param>
    public EventPublisher(
        IPublishEndpoint publishEndpoint,
        IBus bus,
        ILogger<EventPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _bus = bus;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task PublishAsync<TEvent>(TEvent @event, string? correlationId = null, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent
    {
        var eventType = typeof(TEvent).Name;
        var effectiveCorrelationId = correlationId ?? @event.CorrelationId;

        try
        {
            _logger.LogInformation(
                "[MESSAGING] Publicando evento {EventType}. EventId: {EventId}, CorrelationId: {CorrelationId}",
                eventType, @event.EventId, effectiveCorrelationId);

            await _publishEndpoint.Publish(@event, context =>
            {
                context.CorrelationId = Guid.TryParse(effectiveCorrelationId, out var guid) ? guid : Guid.NewGuid();
                context.MessageId = @event.EventId;
                context.Headers.Set("X-Source-Service", @event.SourceService);
                context.Headers.Set("X-Schema-Version", @event.SchemaVersion);
                context.Headers.Set("X-Event-Type", eventType);
            }, cancellationToken);

            _logger.LogInformation(
                "[MESSAGING] Evento publicado exitosamente. EventType: {EventType}, EventId: {EventId}",
                eventType, @event.EventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[MESSAGING] Error al publicar evento {EventType}. EventId: {EventId}, CorrelationId: {CorrelationId}",
                eventType, @event.EventId, effectiveCorrelationId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task PublishBatchAsync<TEvent>(IEnumerable<TEvent> events, string? correlationId = null, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent
    {
        var eventList = events.ToList();
        var eventType = typeof(TEvent).Name;

        _logger.LogInformation(
            "[MESSAGING] Publicando lote de {Count} eventos {EventType}. CorrelationId: {CorrelationId}",
            eventList.Count, eventType, correlationId);

        foreach (var @event in eventList)
        {
            await PublishAsync(@event, correlationId, cancellationToken);
        }

        _logger.LogInformation(
            "[MESSAGING] Lote de {Count} eventos {EventType} publicado exitosamente",
            eventList.Count, eventType);
    }

    /// <inheritdoc />
    public async Task SendAsync<TEvent>(TEvent @event, string queueName, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent
    {
        var eventType = typeof(TEvent).Name;

        try
        {
            _logger.LogInformation(
                "[MESSAGING] Enviando evento {EventType} a cola {QueueName}. EventId: {EventId}",
                eventType, queueName, @event.EventId);

            var sendEndpoint = await _bus.GetSendEndpoint(new Uri($"queue:{queueName}"));

            await sendEndpoint.Send(@event, context =>
            {
                context.CorrelationId = Guid.TryParse(@event.CorrelationId, out var guid) ? guid : Guid.NewGuid();
                context.MessageId = @event.EventId;
                context.Headers.Set("X-Source-Service", @event.SourceService);
                context.Headers.Set("X-Event-Type", eventType);
            }, cancellationToken);

            _logger.LogInformation(
                "[MESSAGING] Evento enviado exitosamente a cola {QueueName}. EventId: {EventId}",
                queueName, @event.EventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[MESSAGING] Error al enviar evento {EventType} a cola {QueueName}. EventId: {EventId}",
                eventType, queueName, @event.EventId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<Guid> SchedulePublishAsync<TEvent>(TEvent @event, DateTime scheduledTime, CancellationToken cancellationToken = default)
        where TEvent : BaseEvent
    {
        var eventType = typeof(TEvent).Name;

        try
        {
            _logger.LogInformation(
                "[MESSAGING] Programando evento {EventType} para {ScheduledTime}. EventId: {EventId}",
                eventType, scheduledTime, @event.EventId);

            var scheduler = _bus.CreateMessageScheduler();

            var scheduledMessage = await scheduler.SchedulePublish(
                scheduledTime.ToUniversalTime(),
                @event,
                cancellationToken);

            _logger.LogInformation(
                "[MESSAGING] Evento programado exitosamente. EventType: {EventType}, ScheduledTime: {ScheduledTime}, TokenId: {TokenId}",
                eventType, scheduledTime, scheduledMessage.TokenId);

            return scheduledMessage.TokenId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[MESSAGING] Error al programar evento {EventType}. EventId: {EventId}",
                eventType, @event.EventId);
            throw;
        }
    }
}
