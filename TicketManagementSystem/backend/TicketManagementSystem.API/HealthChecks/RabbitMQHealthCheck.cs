using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using TicketManagementSystem.API.Infrastructure.RabbitMQ;
using TicketManagementSystem.API.Settings;

namespace TicketManagementSystem.API.HealthChecks;

/// <summary>
/// Health check personalizado para verificar la conectividad con RabbitMQ.
/// </summary>
public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly IRabbitMQConnection _connection;
    private readonly RabbitMQSettings _settings;

    /// <summary>
    /// Inicializa una nueva instancia de RabbitMQHealthCheck.
    /// </summary>
    /// <param name="connection">Conexión a RabbitMQ.</param>
    /// <param name="settings">Configuración de RabbitMQ.</param>
    public RabbitMQHealthCheck(IRabbitMQConnection connection, IOptions<RabbitMQSettings> settings)
    {
        _connection = connection;
        _settings = settings.Value;
    }

    /// <summary>
    /// Verifica el estado de salud de RabbitMQ.
    /// </summary>
    /// <param name="context">Contexto del health check.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Un resultado que indica el estado de salud.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            // Intentar conectar si no está conectado
            if (!_connection.IsConnected)
            {
                var connected = _connection.TryConnect();
                if (!connected)
                {
                    return HealthCheckResult.Unhealthy(
                        description: $"No se pudo conectar a RabbitMQ en {_settings.HostName}:{_settings.Port}",
                        exception: null,
                        data: new Dictionary<string, object>
                        {
                            ["HostName"] = _settings.HostName,
                            ["Port"] = _settings.Port,
                            ["VirtualHost"] = _settings.VirtualHost
                        });
                }
            }

            // Verificar que podemos crear un canal
            using var channel = _connection.CreateModel();

            // Intentar declarar el exchange para verificar permisos
            channel.ExchangeDeclare(
                exchange: _settings.ExchangeName,
                type: ExchangeType.Direct,
                passive: true); // Usar passive para no crear si no existe

            return HealthCheckResult.Healthy(
                description: $"RabbitMQ está saludable en {_settings.HostName}:{_settings.Port}",
                data: new Dictionary<string, object>
                {
                    ["HostName"] = _settings.HostName,
                    ["Port"] = _settings.Port,
                    ["VirtualHost"] = _settings.VirtualHost,
                    ["ExchangeName"] = _settings.ExchangeName
                });
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                description: $"Error verificando salud de RabbitMQ: {ex.Message}",
                exception: ex,
                data: new Dictionary<string, object>
                {
                    ["HostName"] = _settings.HostName,
                    ["Port"] = _settings.Port,
                    ["VirtualHost"] = _settings.VirtualHost,
                    ["ExceptionType"] = ex.GetType().Name
                });
        }
    }
}

/// <summary>
/// Extensiones para agregar el health check de RabbitMQ.
/// </summary>
public static class RabbitMQHealthCheckExtensions
{
    /// <summary>
    /// Agrega el health check de RabbitMQ a la colección de health checks.
    /// </summary>
    /// <param name="builder">Constructor de health checks.</param>
    /// <returns>El constructor modificado.</returns>
    public static IHealthChecksBuilder AddRabbitMQHealthCheck(this IHealthChecksBuilder builder)
    {
        builder.AddCheck<RabbitMQHealthCheck>("RabbitMQ", tags: new[] { "rabbitmq", "message-queue" });
        return builder;
    }
}