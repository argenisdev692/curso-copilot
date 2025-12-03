using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Auth;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Interfaz para el servicio de autenticación.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registra un nuevo usuario.
    /// </summary>
    /// <param name="registerDto">Datos del registro.</param>
    /// <param name="ipAddress">Dirección IP del cliente.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto, string? ipAddress = null);

    /// <summary>
    /// Autentica a un usuario y genera un token JWT.
    /// </summary>
    /// <param name="loginDto">Datos del login.</param>
    /// <param name="ipAddress">Dirección IP del cliente.</param>
    /// <returns>Resultado con el token si es exitoso.</returns>
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto, string? ipAddress = null);

    /// <summary>
    /// Renueva el token de acceso usando un refresh token válido.
    /// </summary>
    /// <param name="refreshToken">Token de refresco.</param>
    /// <param name="ipAddress">Dirección IP del cliente.</param>
    /// <returns>Nuevo par de tokens si el refresh token es válido.</returns>
    Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken, string? ipAddress = null);

    /// <summary>
    /// Revoca un refresh token para invalidar sesiones.
    /// </summary>
    /// <param name="refreshToken">Token a revocar.</param>
    /// <param name="ipAddress">Dirección IP del cliente.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result<bool>> RevokeTokenAsync(string refreshToken, string? ipAddress = null);
}