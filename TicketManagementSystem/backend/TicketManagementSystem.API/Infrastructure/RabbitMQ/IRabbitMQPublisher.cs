using TicketManagementSystem.API.Events;

namespace TicketManagementSystem.API.Infrastructure.RabbitMQ;

/// <summary>
/// Interfaz para publicar eventos en RabbitMQ.
/// Proporciona métodos para publicar mensajes de forma asíncrona.
/// </summary>
public interface IRabbitMQPublisher
{
    /// <summary>
    /// Publica un evento en el exchange configurado.
    /// </summary>
    /// <typeparam name="T">Tipo del evento a publicar.</typeparam>
    /// <param name="event">El evento a publicar.</param>
    /// <param name="routingKey">Clave de enrutamiento opcional.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task PublishAsync<T>(T @event, string? routingKey = null, CancellationToken cancellationToken = default) where T : BaseEvent;
}