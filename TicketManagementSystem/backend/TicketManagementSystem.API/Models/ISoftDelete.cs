/// <summary>
/// Interfaz para entidades con eliminación suave.
/// </summary>
public interface ISoftDelete
{
    /// <summary>
    /// Indica si la entidad ha sido eliminada suavemente.
    /// </summary>
    bool IsDeleted { get; set; }
}