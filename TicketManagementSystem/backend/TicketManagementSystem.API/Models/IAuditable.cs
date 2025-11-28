/// <summary>
/// Interfaz para entidades auditables.
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
    DateTime UpdatedAt { get; set; }
}