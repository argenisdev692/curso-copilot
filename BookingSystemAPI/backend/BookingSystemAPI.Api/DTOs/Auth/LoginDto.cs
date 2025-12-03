using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Api.DTOs.Auth;

/// <summary>
/// DTO para el login de un usuario.
/// </summary>
/// <example>
/// {
///     "email": "usuario@ejemplo.com",
///     "password": "MiContraseña123!"
/// }
/// </example>
public record LoginDto
{
    /// <summary>
    /// Correo electrónico del usuario.
    /// </summary>
    /// <example>usuario@ejemplo.com</example>
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public required string Email { get; init; }

    /// <summary>
    /// Contraseña del usuario.
    /// </summary>
    /// <example>MiContraseña123!</example>
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public required string Password { get; init; }
}