using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.Common.Security;
using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.DTOs.Auth;
using BookingSystemAPI.Api.Models;
using BookingSystemAPI.Api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Implementación del servicio de autenticación con soporte para Refresh Tokens y Account Lockout.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AuthService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IAccountLockoutService _lockoutService;

    public AuthService(
        IUserRepository userRepository,
        ApplicationDbContext dbContext,
        ILogger<AuthService> logger,
        IConfiguration configuration,
        IAccountLockoutService lockoutService)
    {
        _userRepository = userRepository;
        _dbContext = dbContext;
        _logger = logger;
        _configuration = configuration;
        _lockoutService = lockoutService;
    }

    /// <inheritdoc />
    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto registerDto, string? ipAddress = null)
    {
        try
        {
            // Verificar si el email ya existe (sin revelar información específica)
            if (await _userRepository.EmailExistsAsync(registerDto.Email))
            {
                // Log interno pero mensaje genérico al usuario (OWASP - no revelar existencia de cuenta)
                _logger.LogWarning("[SECURITY] Intento de registro con email ya existente: {Email}", 
                    HashEmail(registerDto.Email));
                
                // Agregar delay aleatorio para prevenir timing attacks
                await AddRandomDelayAsync();
                
                return Result<AuthResponseDto>.Failure(new Error("RegistrationFailed", 
                    "No se pudo completar el registro. Por favor, verifique los datos e intente nuevamente."));
            }

            // Crear el usuario
            var user = new User
            {
                Email = registerDto.Email.ToLowerInvariant().Trim(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password, workFactor: 12),
                FirstName = SanitizeInput(registerDto.FirstName),
                LastName = SanitizeInput(registerDto.LastName),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            _logger.LogInformation("[AUTH] Usuario registrado exitosamente. UserId: {UserId}", user.Id);

            // Generar tokens
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id, ipAddress);
            var response = CreateAuthResponse(user, accessToken, refreshToken.Token);

            return Result<AuthResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AUTH] Error durante el registro del usuario");
            return Result<AuthResponseDto>.Failure(new Error("RegistrationError", 
                "Error interno del servidor durante el registro."));
        }
    }

    /// <inheritdoc />
    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto, string? ipAddress = null)
    {
        try
        {
            var normalizedEmail = loginDto.Email.ToLowerInvariant().Trim();

            // Verificar bloqueo de IP primero
            var (ipLocked, ipRemainingTime) = _lockoutService.IsIpLocked(ipAddress);
            if (ipLocked)
            {
                _logger.LogWarning("[SECURITY] IP bloqueada intentando login. IP: {IP}", ipAddress);
                return Result<AuthResponseDto>.Failure(new Error("TooManyAttempts", 
                    $"Demasiados intentos fallidos. Intente nuevamente en {ipRemainingTime?.Minutes ?? 30} minutos."));
            }

            // Verificar bloqueo de cuenta
            var (accountLocked, accountRemainingTime) = _lockoutService.IsAccountLocked(normalizedEmail);
            if (accountLocked)
            {
                _logger.LogWarning("[SECURITY] Cuenta bloqueada intentando login. Email: {Email}", 
                    HashEmail(normalizedEmail));
                
                // Agregar delay para prevenir timing attacks
                await AddRandomDelayAsync();
                
                return Result<AuthResponseDto>.Failure(new Error("AccountLocked", 
                    $"Cuenta temporalmente bloqueada. Intente nuevamente en {accountRemainingTime?.Minutes ?? 15} minutos."));
            }

            var user = await _userRepository.GetByEmailAsync(normalizedEmail);

            // Verificación de credenciales con timing constante
            bool isValidPassword = false;
            if (user != null)
            {
                isValidPassword = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            }
            else
            {
                // Ejecutar hash de todas formas para prevenir timing attacks
                BCrypt.Net.BCrypt.HashPassword("dummy-password-check", workFactor: 12);
            }

            if (user == null || !isValidPassword)
            {
                // Registrar intento fallido
                var shouldLock = _lockoutService.RegisterFailedAttempt(normalizedEmail, ipAddress);
                
                _logger.LogWarning("[SECURITY] Intento de login fallido. Email: {Email}, IP: {IP}, Bloqueado: {Locked}", 
                    HashEmail(normalizedEmail), ipAddress, shouldLock);
                
                // Agregar delay aleatorio
                await AddRandomDelayAsync();
                
                return Result<AuthResponseDto>.Failure(new Error("InvalidCredentials", "Credenciales inválidas."));
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("[SECURITY] Intento de login con cuenta desactivada. UserId: {UserId}", user.Id);
                return Result<AuthResponseDto>.Failure(new Error("AccountDisabled", "La cuenta está desactivada."));
            }

            // Login exitoso - resetear intentos fallidos
            _lockoutService.ResetFailedAttempts(normalizedEmail);
            
            _logger.LogInformation("[AUTH] Login exitoso. UserId: {UserId}, IP: {IP}", user.Id, ipAddress);

            // Generar tokens
            var accessToken = GenerateJwtToken(user);
            var refreshToken = await GenerateRefreshTokenAsync(user.Id, ipAddress);
            var response = CreateAuthResponse(user, accessToken, refreshToken.Token);

            return Result<AuthResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[AUTH] Error durante el login del usuario");
            return Result<AuthResponseDto>.Failure(new Error("LoginError", 
                "Error interno del servidor durante el login."));
        }
    }

    /// <inheritdoc />
    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(string refreshToken, string? ipAddress = null)
    {
        try
        {
            var storedToken = await _dbContext.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null)
            {
                _logger.LogWarning("Intento de refresh con token inexistente desde IP: {IpAddress}", ipAddress);
                return Result<AuthResponseDto>.Failure(new Error("InvalidToken", "Token inválido."));
            }

            if (!storedToken.IsActive)
            {
                _logger.LogWarning("Intento de refresh con token revocado o expirado para usuario: {UserId}", storedToken.UserId);
                return Result<AuthResponseDto>.Failure(new Error("TokenExpired", "El token ha expirado o fue revocado."));
            }

            // Revocar el token actual (rotación de tokens)
            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;

            // Generar nuevos tokens
            var user = storedToken.User;
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = await GenerateRefreshTokenAsync(user.Id, ipAddress);
            
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Token refrescado exitosamente para usuario: {Email}", user.Email);

            var response = CreateAuthResponse(user, newAccessToken, newRefreshToken.Token);
            return Result<AuthResponseDto>.Success(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el refresh del token");
            return Result<AuthResponseDto>.Failure(new Error("RefreshError", "Error interno del servidor durante el refresh."));
        }
    }

    /// <inheritdoc />
    public async Task<Result<bool>> RevokeTokenAsync(string refreshToken, string? ipAddress = null)
    {
        try
        {
            var storedToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

            if (storedToken == null)
            {
                return Result<bool>.Failure(new Error("InvalidToken", "Token no encontrado."));
            }

            if (!storedToken.IsActive)
            {
                return Result<bool>.Failure(new Error("TokenAlreadyRevoked", "El token ya fue revocado o expiró."));
            }

            storedToken.IsRevoked = true;
            storedToken.RevokedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Token revocado exitosamente para usuario: {UserId} desde IP: {IpAddress}", 
                storedToken.UserId, ipAddress);

            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al revocar el token");
            return Result<bool>.Failure(new Error("RevokeError", "Error interno del servidor al revocar el token."));
        }
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key not configured"));
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = jwtSettings.GetValue<int>("AccessTokenExpirationMinutes", 60);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private async Task<RefreshToken> GenerateRefreshTokenAsync(int userId, string? ipAddress)
    {
        var expirationDays = _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7);

        var refreshToken = new RefreshToken
        {
            Token = GenerateSecureToken(),
            ExpiresAt = DateTime.UtcNow.AddDays(expirationDays),
            UserId = userId,
            CreatedByIp = ipAddress,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.RefreshTokens.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return refreshToken;
    }

    private static string GenerateSecureToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private static AuthResponseDto CreateAuthResponse(User user, string accessToken, string refreshToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(accessToken) as JwtSecurityToken;

        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = jwtToken?.ValidTo ?? DateTime.UtcNow.AddHours(1),
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            }
        };
    }

    /// <summary>
    /// Sanitiza entrada de texto para prevenir XSS.
    /// </summary>
    private static string SanitizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        // Remover caracteres potencialmente peligrosos
        return System.Net.WebUtility.HtmlEncode(input.Trim());
    }

    /// <summary>
    /// Genera un hash del email para logging seguro (no revelar email completo en logs).
    /// </summary>
    private static string HashEmail(string email)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(email.ToLowerInvariant()));
        return Convert.ToHexString(hashBytes)[..16]; // Solo primeros 16 caracteres
    }

    /// <summary>
    /// Agrega un delay aleatorio para prevenir timing attacks.
    /// </summary>
    private static async Task AddRandomDelayAsync()
    {
        var random = RandomNumberGenerator.GetInt32(100, 300);
        await Task.Delay(random);
    }
}