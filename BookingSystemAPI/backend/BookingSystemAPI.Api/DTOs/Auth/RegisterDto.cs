using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Api.DTOs.Auth;

/// <summary>
/// DTO para el registro de un nuevo usuario.
/// </summary>
/// <example>
/// {
///     "email": "nuevo.usuario@ejemplo.com",
///     "password": "MiContraseña123!",
///     "confirmPassword": "MiContraseña123!",
///     "firstName": "Juan",
///     "lastName": "Pérez"
/// }
/// </example>
public record RegisterDto
{
    /// <summary>
    /// Correo electrónico del usuario. Debe ser único en el sistema.
    /// </summary>
    /// <example>nuevo.usuario@ejemplo.com</example>
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    [MaxLength(256, ErrorMessage = "El correo electrónico no puede exceder los 256 caracteres.")]
    public required string Email { get; init; }

    /// <summary>
    /// Contraseña del usuario. Debe tener mínimo 8 caracteres, 
    /// incluir mayúsculas, minúsculas, números y caracteres especiales.
    /// </summary>
    /// <example>MiContraseña123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    public required string Password { get; init; }

    /// <summary>
    /// Confirmación de la contraseña. Debe coincidir con la contraseña.
    /// </summary>
    /// <example>MiContraseña123!</example>
    [Required(ErrorMessage = "La confirmación de contraseña es obligatoria.")]
    [Compare(nameof(Password), ErrorMessage = "La confirmación de contraseña no coincide.")]
    public required string ConfirmPassword { get; init; }

    /// <summary>
    /// Nombre del usuario.
    /// </summary>
    /// <example>Juan</example>
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public required string FirstName { get; init; }

    /// <summary>
    /// Apellido del usuario.
    /// </summary>
    /// <example>Pérez</example>
    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
    public required string LastName { get; init; }
}