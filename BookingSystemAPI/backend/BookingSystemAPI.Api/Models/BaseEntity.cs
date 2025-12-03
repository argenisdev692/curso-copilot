namespace BookingSystemAPI.Api.Models;

/// <summary>
/// Interfaz para entidades con información de auditoría.
/// </summary>
public interface IAuditable
{
    /// <summary>
    /// Fecha de creación de la entidad.
    /// </summary>
    DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de última actualización de la entidad.
    /// </summary>
    DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Interfaz para entidades con eliminación suave (soft delete).
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Indica si la entidad está marcada como eliminada.
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// Fecha de eliminación de la entidad.
    /// </summary>
    DateTime? DeletedAt { get; set; }
}

/// <summary>
/// Clase base para todas las entidades del dominio.
/// </summary>
public abstract class BaseEntity : IAuditable, ISoftDelete
{
    /// <summary>
    /// Identificador único de la entidad.
    /// </summary>
    public int Id { get; set; }

    /// <inheritdoc/>
    public DateTime CreatedAt { get; set; }

    /// <inheritdoc/>
    public DateTime? UpdatedAt { get; set; }

    /// <inheritdoc/>
    public bool IsDeleted { get; set; }

    /// <inheritdoc/>
    public DateTime? DeletedAt { get; set; }
}
