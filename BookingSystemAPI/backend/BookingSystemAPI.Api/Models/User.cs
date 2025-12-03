using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Api.Models;

/// <summary>
/// Representa un usuario en el sistema.
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Correo electrónico del usuario. Debe ser único.
    /// </summary>
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    public required string Email { get; set; }

    /// <summary>
    /// Hash de la contraseña del usuario.
    /// </summary>
    [Required]
    [MaxLength(256)]
    public required string PasswordHash { get; set; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string FirstName { get; set; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public required string LastName { get; set; }

    /// <summary>
    /// Indica si el usuario está activo.
    /// </summary>
    public bool IsActive { get; set; } = true;
}