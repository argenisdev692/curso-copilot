using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TicketManagementSystem.API.Events;
using TicketManagementSystem.API.Infrastructure.RabbitMQ;

namespace TicketManagementSystem.API.Consumers;

/// <summary>
/// Clase base para consumidores de RabbitMQ.
/// Proporciona lógica común de consumo con retry y manejo de errores.
/// </summary>
public abstract class BaseConsumer : IDisposable
{
    protected readonly IRabbitMQConnection _connection;
    protected readonly ILogger _logger;
    protected IModel? _channel;
    protected string _queueName;
    protected bool _disposed;

    private readonly AsyncRetryPolicy _retryPolicy;

    /// <summary>
    /// Inicializa una nueva instancia de BaseConsumer.
    /// </summary>
    /// <param name="connection">Conexión a RabbitMQ.</param>
    /// <param name="logger">Logger para registrar eventos.</param>
    /// <param name="queueName">Nombre de la cola a consumir.</param>
    protected BaseConsumer(IRabbitMQConnection connection, ILogger logger, string queueName)
    {
        _connection = connection;
        _logger = logger;
        _queueName = queueName;

        // Configurar política de reintento para procesamiento de mensajes
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Error procesando mensaje. Reintento {RetryCount} en {TimeSpan}.", retryCount, timeSpan);
                });
    }

    /// <summary>
    /// Inicia el consumo de mensajes.
    /// </summary>
    public virtual void StartConsuming()
    {
        if (!_connection.IsConnected)
        {
            _connection.TryConnect();
        }

        _channel = _connection.CreateModel();

        // Declarar la cola
        _channel.QueueDeclare(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        // Configurar prefetch para controlar backpressure
        _channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            await ProcessMessageAsync(ea);
        };

        _channel.BasicConsume(queue: _queueName, autoAck: false, consumer: consumer);

        _logger.LogInformation("Consumidor iniciado para cola {QueueName}", _queueName);
    }

    /// <summary>
    /// Procesa un mensaje recibido.
    /// </summary>
    /// <param name="ea">Argumentos del evento de mensaje recibido.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    private async Task ProcessMessageAsync(BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation("Mensaje recibido en cola {QueueName}: {Message}", _queueName, message);

            await _retryPolicy.ExecuteAsync(async () =>
            {
                await ProcessMessageCoreAsync(message, ea.BasicProperties);
            });

            // Confirmar procesamiento exitoso
            _channel?.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            _logger.LogInformation("Mensaje procesado exitosamente en cola {QueueName}", _queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando mensaje en cola {QueueName}", _queueName);

            // Rechazar mensaje y enviarlo a DLQ si está configurado
            _channel?.BasicNack(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
        }
    }

    /// <summary>
    /// Método abstracto para procesar el contenido específico del mensaje.
    /// Debe ser implementado por las clases derivadas.
    /// </summary>
    /// <param name="message">Contenido del mensaje en formato JSON.</param>
    /// <param name="properties">Propiedades del mensaje.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    protected abstract Task ProcessMessageCoreAsync(string message, IBasicProperties properties);

    /// <summary>
    /// Libera los recursos utilizados por el consumidor.
    /// </summary>
    public virtual void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            _channel?.Close();
            _channel?.Dispose();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cerrar el canal del consumidor.");
        }
    }
}