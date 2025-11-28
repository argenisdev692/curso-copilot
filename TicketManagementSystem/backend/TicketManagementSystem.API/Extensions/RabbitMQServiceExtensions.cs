using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketManagementSystem.API.Consumers;
using TicketManagementSystem.API.HealthChecks;
using TicketManagementSystem.API.Infrastructure.RabbitMQ;
using TicketManagementSystem.API.Settings;

namespace TicketManagementSystem.API.Extensions;

/// <summary>
/// Extensiones para configurar servicios relacionados con RabbitMQ.
/// </summary>
public static class RabbitMQServiceExtensions
{
    /// <summary>
    /// Agrega los servicios de RabbitMQ al contenedor de inyección de dependencias.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>La colección de servicios modificada.</returns>
    public static IServiceCollection AddRabbitMQServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar opciones de RabbitMQ
        services.Configure<RabbitMQSettings>(configuration.GetSection("RabbitMQ"));

        // Registrar conexión como singleton
        services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();

        // Registrar publicador
        services.AddTransient<IRabbitMQPublisher, RabbitMQPublisher>();

        // Registrar consumidores
        services.AddHostedService<NotificationConsumerHostedService>();

        // Registrar health check
        services.AddHealthChecks()
            .AddRabbitMQHealthCheck();

        return services;
    }
}

/// <summary>
/// Servicio hospedado para ejecutar el consumidor de notificaciones.
/// </summary>
public class NotificationConsumerHostedService : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private NotificationConsumer? _consumer;
    private bool _disposed;

    /// <summary>
    /// Inicializa una nueva instancia de NotificationConsumerHostedService.
    /// </summary>
    /// <param name="serviceProvider">Proveedor de servicios.</param>
    public NotificationConsumerHostedService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Inicia el servicio hospedado.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var connection = scope.ServiceProvider.GetRequiredService<IRabbitMQConnection>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<NotificationConsumer>>();

        _consumer = new NotificationConsumer(connection, logger);
        _consumer.StartConsuming();

        return Task.CompletedTask;
    }

    /// <summary>
    /// Detiene el servicio hospedado.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer?.Dispose();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Libera los recursos utilizados por el servicio.
    /// </summary>
    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        _consumer?.Dispose();
    }
}