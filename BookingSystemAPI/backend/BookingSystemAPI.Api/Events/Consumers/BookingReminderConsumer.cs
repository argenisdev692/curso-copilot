using MassTransit;

namespace BookingSystemAPI.Api.Events.Consumers;

/// <summary>
/// Consumer para eventos de recordatorio de reserva.
/// Procesa el evento y delega la notificación al handler correspondiente.
/// </summary>
public class BookingReminderConsumer : IConsumer<BookingReminderEvent>
{
    private readonly IBookingNotificationHandler _notificationHandler;
    private readonly ILogger<BookingReminderConsumer> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del consumer.
    /// </summary>
    /// <param name="notificationHandler">Handler de notificaciones.</param>
    /// <param name="logger">Logger para trazabilidad.</param>
    public BookingReminderConsumer(
        IBookingNotificationHandler notificationHandler,
        ILogger<BookingReminderConsumer> logger)
    {
        _notificationHandler = notificationHandler;
        _logger = logger;
    }

    /// <summary>
    /// Consume el evento de recordatorio de reserva.
    /// </summary>
    /// <param name="context">Contexto del mensaje.</param>
    public async Task Consume(ConsumeContext<BookingReminderEvent> context)
    {
        var @event = context.Message;
        var correlationId = context.CorrelationId?.ToString() ?? @event.CorrelationId;

        _logger.LogInformation(
            "[CONSUMER] Procesando BookingReminderEvent. BookingId: {BookingId}, MinutesBefore: {MinutesBefore}, CorrelationId: {CorrelationId}",
            @event.BookingId, @event.MinutesBefore, correlationId);

        try
        {
            var result = await _notificationHandler.HandleBookingReminderAsync(@event, context.CancellationToken);

            if (result)
            {
                _logger.LogInformation(
                    "[CONSUMER] Recordatorio de reserva enviado exitosamente. BookingId: {BookingId}",
                    @event.BookingId);
            }
            else
            {
                _logger.LogWarning(
                    "[CONSUMER] No se pudo enviar recordatorio de reserva. BookingId: {BookingId}",
                    @event.BookingId);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "[CONSUMER] Error al procesar BookingReminderEvent. BookingId: {BookingId}, CorrelationId: {CorrelationId}",
                @event.BookingId, correlationId);
            throw; // Re-throw para que MassTransit maneje el retry
        }
    }
}
