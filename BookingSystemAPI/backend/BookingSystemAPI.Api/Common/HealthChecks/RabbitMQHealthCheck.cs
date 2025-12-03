using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace BookingSystemAPI.Api.Common.HealthChecks;

/// <summary>
/// Health check para verificar la conectividad con RabbitMQ.
/// </summary>
public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMQHealthCheck> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del health check de RabbitMQ.
    /// </summary>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <param name="logger">Logger para trazabilidad.</param>
    public RabbitMQHealthCheck(
        IConfiguration configuration,
        ILogger<RabbitMQHealthCheck> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Ejecuta el health check de RabbitMQ.
    /// </summary>
    /// <param name="context">Contexto del health check.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado del health check.</returns>
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var host = _configuration["RabbitMQ:Host"] ?? "localhost";
            var port = _configuration.GetValue<int>("RabbitMQ:Port", 5672);
            var username = _configuration["RabbitMQ:Username"] ?? "guest";
            var password = _configuration["RabbitMQ:Password"] ?? "guest";
            var virtualHost = _configuration["RabbitMQ:VirtualHost"] ?? "/";

            var factory = new ConnectionFactory
            {
                HostName = host,
                Port = port,
                UserName = username,
                Password = password,
                VirtualHost = virtualHost,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(5)
            };

            using var connection = await factory.CreateConnectionAsync(cancellationToken);
            using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            var data = new Dictionary<string, object>
            {
                { "host", host },
                { "port", port },
                { "virtualHost", virtualHost },
                { "isOpen", connection.IsOpen }
            };

            _logger.LogDebug("[HEALTHCHECK] RabbitMQ health check passed. Host: {Host}", host);

            return HealthCheckResult.Healthy("RabbitMQ connection is healthy", data);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "[HEALTHCHECK] RabbitMQ health check failed");

            return HealthCheckResult.Unhealthy(
                "RabbitMQ connection failed",
                ex,
                new Dictionary<string, object>
                {
                    { "error", ex.Message }
                });
        }
    }
}
