using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using TicketManagementSystem.API.Settings;

namespace TicketManagementSystem.API.Infrastructure.RabbitMQ;

/// <summary>
/// Implementación de la conexión a RabbitMQ con manejo de reconexión automática.
/// Utiliza Polly para implementar retry exponencial.
/// </summary>
public class RabbitMQConnection : IRabbitMQConnection
{
    private readonly RabbitMQSettings _settings;
    private readonly ILogger<RabbitMQConnection> _logger;
    private readonly IConnectionFactory _connectionFactory;
    private IConnection? _connection;
    private bool _disposed;
    private readonly object _lock = new();

    private readonly AsyncRetryPolicy _retryPolicy;

    /// <summary>
    /// Inicializa una nueva instancia de RabbitMQConnection.
    /// </summary>
    /// <param name="settings">Configuración de RabbitMQ.</param>
    /// <param name="logger">Logger para registrar eventos.</param>
    public RabbitMQConnection(IOptions<RabbitMQSettings> settings, ILogger<RabbitMQConnection> logger)
    {
        _settings = settings.Value;
        _logger = logger;

        _connectionFactory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password,
            VirtualHost = _settings.VirtualHost,
            DispatchConsumersAsync = true
        };

        // Configurar política de reintento exponencial
        _retryPolicy = Policy
            .Handle<BrokerUnreachableException>()
            .Or<ConnectFailureException>()
            .WaitAndRetryAsync(
                _settings.RetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(_settings.RetryDelayMs * Math.Pow(2, retryAttempt - 1)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning(exception, "Error al conectar a RabbitMQ. Reintento {RetryCount} en {TimeSpan}.", retryCount, timeSpan);
                });
    }

    /// <summary>
    /// Indica si la conexión está activa.
    /// </summary>
    public bool IsConnected => _connection?.IsOpen == true && !_disposed;

    /// <summary>
    /// Intenta conectar a RabbitMQ si no está conectado.
    /// </summary>
    /// <returns>True si la conexión fue exitosa, false en caso contrario.</returns>
    public bool TryConnect()
    {
        _logger.LogInformation("Intentando conectar a RabbitMQ en {HostName}:{Port}", _settings.HostName, _settings.Port);

        return _retryPolicy.Execute(() =>
        {
            lock (_lock)
            {
                if (IsConnected)
                    return true;

                try
                {
                    _connection = _connectionFactory.CreateConnection();
                    _connection.ConnectionShutdown += OnConnectionShutdown;
                    _connection.CallbackException += OnCallbackException;
                    _connection.ConnectionBlocked += OnConnectionBlocked;

                    _logger.LogInformation("Conexión a RabbitMQ establecida exitosamente.");
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al conectar a RabbitMQ.");
                    return false;
                }
            }
        });
    }

    /// <summary>
    /// Crea un nuevo canal de RabbitMQ.
    /// </summary>
    /// <returns>Un objeto IModel que representa el canal.</returns>
    /// <exception cref="InvalidOperationException">Si no hay conexión activa.</exception>
    public IModel CreateModel()
    {
        if (!IsConnected)
        {
            _logger.LogWarning("Intentando crear un modelo sin conexión activa. Intentando reconectar...");
            if (!TryConnect())
                throw new InvalidOperationException("No se pudo establecer conexión con RabbitMQ.");
        }

        return _connection!.CreateModel();
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogWarning("Conexión a RabbitMQ cerrada. Código: {ReplyCode}, Texto: {ReplyText}", e.ReplyCode, e.ReplyText);
    }

    private void OnCallbackException(object? sender, CallbackExceptionEventArgs e)
    {
        _logger.LogError(e.Exception, "Error en callback de RabbitMQ.");
    }

    private void OnConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
    {
        _logger.LogWarning("Conexión a RabbitMQ bloqueada. Razón: {Reason}", e.Reason);
    }

    /// <summary>
    /// Libera los recursos utilizados por la conexión.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;

        try
        {
            if (_connection != null)
            {
                _connection.ConnectionShutdown -= OnConnectionShutdown;
                _connection.CallbackException -= OnCallbackException;
                _connection.ConnectionBlocked -= OnConnectionBlocked;
                _connection.Close();
                _connection.Dispose();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cerrar la conexión a RabbitMQ.");
        }
    }
}