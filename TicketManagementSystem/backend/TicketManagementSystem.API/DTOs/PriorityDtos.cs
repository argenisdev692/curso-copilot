using System.ComponentModel.DataAnnotations;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// DTO para representar una prioridad en respuestas de la API.
    /// </summary>
    public class PriorityDto
    {
        /// <summary>
        /// Identificador único de la prioridad.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la prioridad.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Nivel numérico de la prioridad.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Descripción de la prioridad.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Color hexadecimal para representar la prioridad.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Indica si la prioridad está activa.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Fecha de creación.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Fecha de última actualización.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }

    /// <summary>
    /// DTO para crear una nueva prioridad.
    /// </summary>
    public class CreatePriorityDto
    {
        /// <summary>
        /// Nombre de la prioridad (requerido, máximo 50 caracteres).
        /// </summary>
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Nivel numérico de la prioridad (requerido, entre 1 y 100).
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int Level { get; set; }

        /// <summary>
        /// Descripción opcional de la prioridad (máximo 200 caracteres).
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Color hexadecimal opcional (máximo 7 caracteres, formato #RRGGBB).
        /// </summary>
        [MaxLength(7)]
        public string? Color { get; set; }
    }

    /// <summary>
    /// DTO para actualizar una prioridad existente.
    /// </summary>
    public class UpdatePriorityDto
    {
        /// <summary>
        /// Nombre de la prioridad (requerido, máximo 50 caracteres).
        /// </summary>
        [Required]
        [MaxLength(50)]
        public required string Name { get; set; }

        /// <summary>
        /// Nivel numérico de la prioridad (requerido, entre 1 y 100).
        /// </summary>
        [Required]
        [Range(1, 100)]
        public int Level { get; set; }

        /// <summary>
        /// Descripción opcional de la prioridad (máximo 200 caracteres).
        /// </summary>
        [MaxLength(200)]
        public string? Description { get; set; }

        /// <summary>
        /// Color hexadecimal opcional (máximo 7 caracteres, formato #RRGGBB).
        /// </summary>
        [MaxLength(7)]
        public string? Color { get; set; }

        /// <summary>
        /// Indica si la prioridad está activa.
        /// </summary>
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO para consultas paginadas de prioridades.
    /// </summary>
    public class GetPrioritiesQueryParameters
    {
        /// <summary>
        /// Número de página (mínimo 1, por defecto 1).
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Elementos por página (mínimo 1, máximo 100, por defecto 10).
        /// </summary>
        [Range(1, 100)]
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Filtrar por nombre (búsqueda parcial).
        /// </summary>
        [MaxLength(50)]
        public string? Name { get; set; }

        /// <summary>
        /// Filtrar por estado activo/inactivo.
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Ordenar por: name, level, createdAt (por defecto level).
        /// </summary>
        public string? SortBy { get; set; } = "level";

        /// <summary>
        /// Orden: asc o desc (por defecto asc).
        /// </summary>
        public string? SortOrder { get; set; } = "asc";
    }
}