using System.ComponentModel.DataAnnotations;

namespace BookingSystemAPI.Api.Models;

/// <summary>
/// Representa un token de actualización (refresh token) para renovar el JWT.
/// </summary>
public class RefreshToken : BaseEntity
{
    /// <summary>
    /// Token único de refresco.
    /// </summary>
    [Required]
    [MaxLength(512)]
    public required string Token { get; set; }

    /// <summary>
    /// Fecha de expiración del token.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Indica si el token ha sido revocado manualmente.
    /// </summary>
    public bool IsRevoked { get; set; } = false;

    /// <summary>
    /// Fecha en que fue revocado (si aplica).
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Dirección IP desde donde se creó el token.
    /// </summary>
    [MaxLength(45)]
    public string? CreatedByIp { get; set; }

    /// <summary>
    /// ID del usuario propietario del token.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Navegación al usuario.
    /// </summary>
    public virtual User User { get; set; } = null!;

    /// <summary>
    /// Indica si el token está activo (no expirado y no revocado).
    /// </summary>
    public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
}
