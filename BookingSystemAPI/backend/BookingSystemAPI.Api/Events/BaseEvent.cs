namespace BookingSystemAPI.Api.Events;

/// <summary>
/// Clase base para todos los eventos del sistema.
/// Incluye CorrelationId para trazabilidad distribuida.
/// </summary>
public abstract class BaseEvent
{
    /// <summary>
    /// Identificador único del evento.
    /// </summary>
    public Guid EventId { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Identificador de correlación para trazabilidad entre servicios.
    /// </summary>
    public string CorrelationId { get; init; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Fecha y hora de creación del evento (UTC).
    /// </summary>
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Nombre del servicio que originó el evento.
    /// </summary>
    public string SourceService { get; init; } = "BookingSystemAPI";

    /// <summary>
    /// Versión del esquema del evento para compatibilidad.
    /// </summary>
    public string SchemaVersion { get; init; } = "1.0";
}
