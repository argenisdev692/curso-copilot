using BookingSystemAPI.Api.Common.HealthChecks;
using BookingSystemAPI.Api.Events;
using BookingSystemAPI.Api.Events.Consumers;
using MassTransit;

namespace BookingSystemAPI.Api.Extensions;

/// <summary>
/// Extensiones para configurar RabbitMQ y MassTransit.
/// </summary>
public static class RabbitMQExtensions
{
    /// <summary>
    /// Configura MassTransit con RabbitMQ incluyendo retry exponencial, DLQ y reconexión automática.
    /// </summary>
    /// <param name="services">Colección de servicios.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>Colección de servicios modificada.</returns>
    public static IServiceCollection AddRabbitMQMessaging(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var rabbitMQSection = configuration.GetSection("RabbitMQ");
        var isEnabled = rabbitMQSection.GetValue<bool>("Enabled", false);

        if (!isEnabled)
        {
            // Registrar implementación en memoria para desarrollo sin RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddConsumer<BookingCreatedConsumer>();
                x.AddConsumer<BookingCancelledConsumer>();
                x.AddConsumer<BookingReminderConsumer>();

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IEventPublisher, EventPublisher>();
            services.AddScoped<IBookingNotificationHandler, LoggingNotificationHandler>();

            return services;
        }

        var host = rabbitMQSection["Host"] ?? "localhost";
        var port = rabbitMQSection.GetValue<int>("Port", 5672);
        var username = rabbitMQSection["Username"] ?? "guest";
        var password = rabbitMQSection["Password"] ?? "guest";
        var virtualHost = rabbitMQSection["VirtualHost"] ?? "/";

        // Configuración de retry
        var retrySection = rabbitMQSection.GetSection("Retry");
        var retryCount = retrySection.GetValue<int>("Count", 5);
        var retryIntervalSeconds = retrySection.GetValue<int>("IntervalSeconds", 10);
        var retryIntervalIncrementSeconds = retrySection.GetValue<int>("IntervalIncrementSeconds", 30);

        services.AddMassTransit(x =>
        {
            // Registrar consumers
            x.AddConsumer<BookingCreatedConsumer>(cfg =>
            {
                cfg.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));
            });

            x.AddConsumer<BookingCancelledConsumer>(cfg =>
            {
                cfg.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));
            });

            x.AddConsumer<BookingReminderConsumer>(cfg =>
            {
                cfg.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));
            });

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host, (ushort)port, virtualHost, h =>
                {
                    h.Username(username);
                    h.Password(password);

                    // Configurar reconexión automática
                    h.RequestedConnectionTimeout(TimeSpan.FromSeconds(30));
                });

                // Configuración de endpoints con error queue (Dead Letter Queue implícita en MassTransit)
                cfg.ReceiveEndpoint("booking-created", e =>
                {
                    e.UseDelayedRedelivery(r => r.Intervals(
                        TimeSpan.FromMinutes(5),
                        TimeSpan.FromMinutes(15),
                        TimeSpan.FromMinutes(30)));
                    e.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));
                    e.ConfigureConsumer<BookingCreatedConsumer>(context);
                });

                cfg.ReceiveEndpoint("booking-cancelled", e =>
                {
                    e.UseDelayedRedelivery(r => r.Intervals(
                        TimeSpan.FromMinutes(5),
                        TimeSpan.FromMinutes(15),
                        TimeSpan.FromMinutes(30)));
                    e.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));
                    e.ConfigureConsumer<BookingCancelledConsumer>(context);
                });

                cfg.ReceiveEndpoint("booking-reminder", e =>
                {
                    e.UseDelayedRedelivery(r => r.Intervals(
                        TimeSpan.FromMinutes(5),
                        TimeSpan.FromMinutes(15),
                        TimeSpan.FromMinutes(30)));
                    e.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));
                    e.ConfigureConsumer<BookingReminderConsumer>(context);
                });

                // Configuración global
                cfg.UseMessageRetry(r => ConfigureRetry(r, retryCount, retryIntervalSeconds, retryIntervalIncrementSeconds));

                // Configurar serialización JSON
                cfg.UseNewtonsoftJsonSerializer();
                cfg.UseNewtonsoftJsonDeserializer();

                // Configurar endpoints automáticamente
                cfg.ConfigureEndpoints(context);
            });
        });

        // Registrar servicios
        services.AddScoped<IEventPublisher, EventPublisher>();
        services.AddScoped<IBookingNotificationHandler, LoggingNotificationHandler>();

        return services;
    }

    /// <summary>
    /// Agrega el health check de RabbitMQ.
    /// </summary>
    /// <param name="builder">Builder de health checks.</param>
    /// <param name="configuration">Configuración de la aplicación.</param>
    /// <returns>Builder de health checks modificado.</returns>
    public static IHealthChecksBuilder AddRabbitMQHealthCheck(
        this IHealthChecksBuilder builder,
        IConfiguration configuration)
    {
        var isEnabled = configuration.GetValue<bool>("RabbitMQ:Enabled", false);

        if (isEnabled)
        {
            builder.AddCheck<RabbitMQHealthCheck>(
                "rabbitmq",
                failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded,
                tags: new[] { "messaging", "rabbitmq" });
        }

        return builder;
    }

    /// <summary>
    /// Configura la política de retry exponencial.
    /// </summary>
    private static void ConfigureRetry(
        IRetryConfigurator retryConfigurator,
        int retryCount,
        int intervalSeconds,
        int intervalIncrementSeconds)
    {
        retryConfigurator.Exponential(
            retryCount,
            TimeSpan.FromSeconds(intervalSeconds),
            TimeSpan.FromMinutes(5), // Máximo intervalo
            TimeSpan.FromSeconds(intervalIncrementSeconds));

        // Ignorar ciertas excepciones que no tienen sentido reintentar
        retryConfigurator.Ignore<ArgumentNullException>();
        retryConfigurator.Ignore<ArgumentException>();
        retryConfigurator.Ignore<InvalidOperationException>();
    }
}
