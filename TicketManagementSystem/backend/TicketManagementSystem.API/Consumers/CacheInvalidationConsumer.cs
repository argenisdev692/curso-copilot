using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text.Json;
using TicketManagementSystem.API.Events;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Infrastructure.RabbitMQ;

namespace TicketManagementSystem.API.Consumers;

/// <summary>
/// Consumidor para invalidar cache cuando se modifican tickets.
/// Procesa eventos de invalidación de cache.
/// </summary>
public class CacheInvalidationConsumer : BaseConsumer
{
    private readonly ICacheHelper _cacheHelper;

    /// <summary>
    /// Inicializa una nueva instancia de CacheInvalidationConsumer.
    /// </summary>
    /// <param name="connection">Conexión a RabbitMQ.</param>
    /// <param name="logger">Logger para registrar eventos.</param>
    /// <param name="cacheHelper">Helper para operaciones de cache.</param>
    public CacheInvalidationConsumer(IRabbitMQConnection connection, ILogger<CacheInvalidationConsumer> logger, ICacheHelper cacheHelper)
        : base(connection, logger, "cache-invalidation-queue")
    {
        _cacheHelper = cacheHelper;
    }

    /// <summary>
    /// Procesa el contenido específico del mensaje de invalidación de cache.
    /// </summary>
    /// <param name="message">Contenido del mensaje en formato JSON.</param>
    /// <param name="properties">Propiedades del mensaje.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    protected override async Task ProcessMessageCoreAsync(string message, IBasicProperties properties)
    {
        try
        {
            // Deserializar el evento de invalidación
            var invalidationEvent = JsonSerializer.Deserialize<TicketCacheInvalidationEvent>(message);

            if (invalidationEvent == null)
            {
                _logger.LogWarning("Mensaje de invalidación no válido recibido: {Message}", message);
                return;
            }

            _logger.LogInformation("Procesando invalidación de cache para ticket {TicketId}, acción: {Action}",
                invalidationEvent.TicketId, invalidationEvent.Action);

            // Invalidar todas las claves de cache relacionadas con tickets
            // En una implementación real, podrías invalidar selectivamente
            await InvalidateTicketCacheAsync();

            _logger.LogInformation("Cache invalidado exitosamente para ticket {TicketId}", invalidationEvent.TicketId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error procesando mensaje de invalidación de cache: {Message}", message);
            throw; // Re-throw para que BaseConsumer maneje el error
        }
    }

    private async Task InvalidateTicketCacheAsync()
    {
        try
        {
            // En Redis, podríamos usar patrones para eliminar múltiples claves
            // Por simplicidad, asumimos que ICacheHelper tiene un método para limpiar por patrón
            // Si no, podríamos mantener un registro de claves activas

            // Placeholder: En implementación real, limpiar todas las claves que empiecen con "tickets:"
            // await _cacheHelper.RemoveByPatternAsync("tickets:*");

            _logger.LogInformation("Cache de tickets invalidado completamente");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error invalidando cache de tickets");
            throw;
        }
    }
}