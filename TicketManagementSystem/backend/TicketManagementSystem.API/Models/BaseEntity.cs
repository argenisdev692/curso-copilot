/// <summary>
/// Clase base para todas las entidades del sistema.
/// </summary>
public abstract class BaseEntity : IAuditable, ISoftDelete
{
    /// <summary>
    /// Identificador único de la entidad.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Fecha de creación de la entidad.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha de última actualización de la entidad.
    /// </summary>
    public DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Indica si la entidad ha sido eliminada suavemente.
    /// </summary>
    public bool IsDeleted { get; set; }
}