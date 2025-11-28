using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using TicketManagementSystem.API.Events;
using TicketManagementSystem.API.Settings;

namespace TicketManagementSystem.API.Infrastructure.RabbitMQ;

/// <summary>
/// Implementación del publicador de eventos en RabbitMQ.
/// Incluye circuit breaker para manejar fallos de conexión.
/// </summary>
public class RabbitMQPublisher : IRabbitMQPublisher
{
    private readonly IRabbitMQConnection _connection;
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQPublisher> _logger;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

    /// <summary>
    /// Inicializa una nueva instancia de RabbitMQPublisher.
    /// </summary>
    /// <param name="connection">Conexión a RabbitMQ.</param>
    /// <param name="settings">Configuración de RabbitMQ.</param>
    /// <param name="logger">Logger para registrar eventos.</param>
    public RabbitMQPublisher(
        IRabbitMQConnection connection,
        IOptions<RabbitMQSettings> settings,
        ILogger<RabbitMQPublisher> logger)
    {
        _connection = connection;
        _settings = settings.Value;
        _logger = logger;

        // Configurar circuit breaker
        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (exception, breakDelay) =>
                {
                    _logger.LogWarning(exception, "Circuit breaker activado. Duración: {BreakDelay}", breakDelay);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker reseteado.");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker en estado half-open.");
                });
    }

    /// <summary>
    /// Publica un evento en el exchange configurado.
    /// </summary>
    /// <typeparam name="T">Tipo del evento a publicar.</typeparam>
    /// <param name="event">El evento a publicar.</param>
    /// <param name="routingKey">Clave de enrutamiento opcional.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public async Task PublishAsync<T>(T @event, string? routingKey = null, CancellationToken cancellationToken = default) where T : BaseEvent
    {
        await _circuitBreakerPolicy.ExecuteAsync(async () =>
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            using var channel = _connection.CreateModel();

            // Declarar el exchange si no existe
            channel.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false);

            var message = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true; // Mensaje persistente
            properties.MessageId = @event.MessageId.ToString();
            properties.Timestamp = new AmqpTimestamp(@event.Timestamp.ToUnixTimeSeconds());
            properties.Type = @event.EventType;

            if (!string.IsNullOrEmpty(@event.CorrelationId))
            {
                properties.CorrelationId = @event.CorrelationId;
            }

            channel.BasicPublish(
                exchange: _settings.ExchangeName,
                routingKey: routingKey ?? @event.EventType,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Evento {EventType} publicado en exchange {Exchange} con routing key {RoutingKey}",
                @event.EventType, _settings.ExchangeName, routingKey ?? @event.EventType);
        });
    }
}