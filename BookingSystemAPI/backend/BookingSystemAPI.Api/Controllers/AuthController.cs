using System.Threading.RateLimiting;
using BookingSystemAPI.Api.Common.Responses;
using BookingSystemAPI.Api.Controllers;
using BookingSystemAPI.Api.DTOs.Auth;
using BookingSystemAPI.Api.Services;
using BookingSystemAPI.Api.Validators.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BookingSystemAPI.Api.Controllers;

/// <summary>
/// Controlador para operaciones de autenticación.
/// Incluye registro, login, refresh tokens y revocación.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("auth")]
[Tags("🔐 Autenticación")]
public class AuthController : BaseApiController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
        : base(logger)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="registerDto">Datos del registro.</param>
    /// <returns>Respuesta con el token JWT y refresh token si es exitoso.</returns>
    /// <remarks>
    /// Ejemplo de solicitud:
    /// 
    ///     POST /api/auth/register
    ///     {
    ///         "email": "usuario@ejemplo.com",
    ///         "password": "MiContraseña123!",
    ///         "confirmPassword": "MiContraseña123!",
    ///         "firstName": "Juan",
    ///         "lastName": "Pérez"
    ///     }
    /// 
    /// La contraseña debe cumplir:
    /// - Mínimo 8 caracteres
    /// - Al menos una mayúscula
    /// - Al menos una minúscula
    /// - Al menos un número
    /// - Al menos un carácter especial
    /// </remarks>
    /// <response code="200">Usuario registrado exitosamente</response>
    /// <response code="400">Datos de entrada inválidos o email ya registrado</response>
    /// <response code="429">Demasiadas solicitudes - espere antes de intentar nuevamente</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var validator = new RegisterValidator();
        var validationResult = await validator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), CorrelationId));
        }

        var ipAddress = GetClientIpAddress();
        var result = await _authService.RegisterAsync(registerDto, ipAddress);

        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Error?.Message ?? "Error desconocido", CorrelationId));
        }

        return Ok(ApiResponse<AuthResponseDto>.Ok(result.Value!, "Usuario registrado exitosamente", CorrelationId));
    }

    /// <summary>
    /// Autentica a un usuario y devuelve tokens JWT.
    /// </summary>
    /// <param name="loginDto">Datos del login.</param>
    /// <returns>Respuesta con el token JWT y refresh token si es exitoso.</returns>
    /// <remarks>
    /// Ejemplo de solicitud:
    /// 
    ///     POST /api/auth/login
    ///     {
    ///         "email": "usuario@ejemplo.com",
    ///         "password": "MiContraseña123!"
    ///     }
    /// 
    /// Respuesta exitosa:
    /// 
    ///     {
    ///         "success": true,
    ///         "data": {
    ///             "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    ///             "refreshToken": "abc123...",
    ///             "expiresAt": "2025-12-02T15:30:00Z",
    ///             "user": {
    ///                 "id": 1,
    ///                 "email": "usuario@ejemplo.com",
    ///                 "firstName": "Juan",
    ///                 "lastName": "Pérez"
    ///             }
    ///         }
    ///     }
    /// </remarks>
    /// <response code="200">Login exitoso</response>
    /// <response code="400">Credenciales inválidas</response>
    /// <response code="429">Demasiadas solicitudes - protección contra fuerza bruta</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var validator = new LoginValidator();
        var validationResult = await validator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse<object>.Fail(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)), CorrelationId));
        }

        var ipAddress = GetClientIpAddress();
        var result = await _authService.LoginAsync(loginDto, ipAddress);

        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Error?.Message ?? "Error desconocido", CorrelationId));
        }

        return Ok(ApiResponse<AuthResponseDto>.Ok(result.Value!, "Login exitoso", CorrelationId));
    }

    /// <summary>
    /// Renueva el token de acceso usando un refresh token válido.
    /// </summary>
    /// <param name="refreshTokenDto">Refresh token actual.</param>
    /// <returns>Nuevos tokens de acceso y refresh.</returns>
    /// <remarks>
    /// Ejemplo de solicitud:
    /// 
    ///     POST /api/auth/refresh
    ///     {
    ///         "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
    ///     }
    /// 
    /// El refresh token anterior será revocado automáticamente (rotación de tokens).
    /// </remarks>
    /// <response code="200">Token renovado exitosamente</response>
    /// <response code="400">Refresh token inválido o expirado</response>
    /// <response code="429">Demasiadas solicitudes</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenDto.RefreshToken))
        {
            return BadRequest(ApiResponse<object>.Fail("El refresh token es requerido.", CorrelationId));
        }

        var ipAddress = GetClientIpAddress();
        var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken, ipAddress);

        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Error?.Message ?? "Error desconocido", CorrelationId));
        }

        return Ok(ApiResponse<AuthResponseDto>.Ok(result.Value!, "Token renovado exitosamente", CorrelationId));
    }

    /// <summary>
    /// Revoca un refresh token para cerrar sesión o invalidar tokens comprometidos.
    /// </summary>
    /// <param name="revokeTokenDto">Token a revocar.</param>
    /// <returns>Confirmación de revocación.</returns>
    /// <remarks>
    /// Ejemplo de solicitud:
    /// 
    ///     POST /api/auth/revoke
    ///     {
    ///         "refreshToken": "dGhpcyBpcyBhIHJlZnJlc2ggdG9rZW4..."
    ///     }
    /// 
    /// Use este endpoint para:
    /// - Cerrar sesión de forma segura
    /// - Invalidar tokens si sospecha que fueron comprometidos
    /// </remarks>
    /// <response code="200">Token revocado exitosamente</response>
    /// <response code="400">Token no encontrado o ya revocado</response>
    [HttpPost("revoke")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenDto revokeTokenDto)
    {
        if (string.IsNullOrWhiteSpace(revokeTokenDto.RefreshToken))
        {
            return BadRequest(ApiResponse<object>.Fail("El refresh token es requerido.", CorrelationId));
        }

        var ipAddress = GetClientIpAddress();
        var result = await _authService.RevokeTokenAsync(revokeTokenDto.RefreshToken, ipAddress);

        if (!result.IsSuccess)
        {
            return BadRequest(ApiResponse<object>.Fail(result.Error?.Message ?? "Error desconocido", CorrelationId));
        }

        return Ok(ApiResponse<bool>.Ok(true, "Token revocado exitosamente", CorrelationId));
    }

    /// <summary>
    /// Obtiene la dirección IP del cliente.
    /// </summary>
    private string? GetClientIpAddress()
    {
        // Primero intentar obtener la IP del header X-Forwarded-For (para proxies/load balancers)
        var forwardedFor = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
        {
            return forwardedFor.Split(',').FirstOrDefault()?.Trim();
        }

        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}