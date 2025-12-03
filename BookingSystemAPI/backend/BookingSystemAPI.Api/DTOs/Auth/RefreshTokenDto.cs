using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Api.DTOs.Auth;

/// <summary>
/// DTO para solicitar un nuevo token usando un refresh token.
/// </summary>
/// <example>
/// {
///     "refreshToken": "abc123xyz..."
/// }
/// </example>
public record RefreshTokenDto
{
    /// <summary>
    /// Token de refresco válido.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    [Required]
    public required string RefreshToken { get; init; }
}

/// <summary>
/// DTO para revocar un refresh token.
/// </summary>
public record RevokeTokenDto
{
    /// <summary>
    /// Token de refresco a revocar.
    /// </summary>
    [Required]
    public required string RefreshToken { get; init; }
}
