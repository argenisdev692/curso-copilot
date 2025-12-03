using MassTransit;

namespace BookingSystemAPI.Api.Events.Consumers;

/// <summary>
/// Consumer para eventos de reserva creada.
/// Procesa el evento y delega la notificación al handler correspondiente.
/// </summary>
public class BookingCreatedConsumer : IConsumer<BookingCreatedEvent>
{
    private readonly IBookingNotificationHandler _notificationHandler;
    private readonly ILogger<BookingCreatedConsumer> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del consumer.
    /// </summary>
    /// <param name="notificationHandler">Handler de notificaciones.</param>
    /// <param name="logger">Logger para trazabilidad.</param>
    public BookingCreatedConsumer(
        IBookingNotificationHandler notificationHandler,
        ILogger<BookingCreatedConsumer> logger)
    {
        _notificationHandler = notificationHandler;
        _logger = logger;
    }

    /// <summary>
    /// Consume el evento de reserva creada.
    /// </summary>
    /// <param name="context">Contexto del mensaje.</param>
    public async Task Consume(ConsumeContext<BookingCreatedEvent> context)
    {
        var @event = context.Message;
        var correlationId = context.CorrelationId?.ToString() ?? @event.CorrelationId;

        _logger.LogInformation(
            "[CONSUMER] Procesando BookingCreatedEvent. BookingId: {BookingId}, CorrelationId: {CorrelationId}",
            @event.BookingId, correlationId);

        try
        {
            var result = await _notificationHandler.HandleBookingCreatedAsync(@event, context.CancellationToken);

            if (result)
            {
                _logger.LogInformation(
                    "[CONSUMER] Notificación de reserva creada enviada exitosamente. BookingId: {BookingId}",
                    @event.BookingId);
            }
            else
            {
                _logger.LogWarning(
                    "[CONSUMER] No se pudo enviar notificación de reserva creada. BookingId: {BookingId}",
                    @event.BookingId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[CONSUMER] Error al procesar BookingCreatedEvent. BookingId: {BookingId}, CorrelationId: {CorrelationId}",
                @event.BookingId, correlationId);
            throw; // Re-throw para que MassTransit maneje el retry
        }
    }
}
