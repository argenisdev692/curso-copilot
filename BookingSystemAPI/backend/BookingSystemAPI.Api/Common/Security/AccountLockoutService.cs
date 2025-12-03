using System.Collections.Concurrent;

namespace BookingSystemAPI.Api.Common.Security;

/// <summary>
/// Servicio para gestionar bloqueo de cuentas tras intentos fallidos de login.
/// Implementa protección contra ataques de fuerza bruta (OWASP A07:2021).
/// </summary>
public interface IAccountLockoutService
{
    /// <summary>
    /// Registra un intento fallido de login.
    /// </summary>
    /// <param name="email">Email de la cuenta.</param>
    /// <param name="ipAddress">Dirección IP del intento.</param>
    /// <returns>True si la cuenta debe bloquearse.</returns>
    bool RegisterFailedAttempt(string email, string? ipAddress);

    /// <summary>
    /// Verifica si una cuenta está bloqueada.
    /// </summary>
    /// <param name="email">Email de la cuenta.</param>
    /// <returns>True si está bloqueada, junto con el tiempo restante.</returns>
    (bool IsLocked, TimeSpan? RemainingTime) IsAccountLocked(string email);

    /// <summary>
    /// Reinicia el contador de intentos tras un login exitoso.
    /// </summary>
    /// <param name="email">Email de la cuenta.</param>
    void ResetFailedAttempts(string email);

    /// <summary>
    /// Verifica si una IP está bloqueada por demasiados intentos.
    /// </summary>
    /// <param name="ipAddress">Dirección IP.</param>
    /// <returns>True si la IP está bloqueada.</returns>
    (bool IsLocked, TimeSpan? RemainingTime) IsIpLocked(string? ipAddress);
}

/// <summary>
/// Implementación en memoria del servicio de bloqueo de cuentas.
/// Para producción, considerar usar Redis o base de datos.
/// </summary>
public class AccountLockoutService : IAccountLockoutService
{
    private readonly ConcurrentDictionary<string, LockoutInfo> _accountAttempts = new();
    private readonly ConcurrentDictionary<string, LockoutInfo> _ipAttempts = new();
    private readonly ILogger<AccountLockoutService> _logger;
    private readonly AccountLockoutOptions _options;

    public AccountLockoutService(
        ILogger<AccountLockoutService> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _options = configuration.GetSection("Security:AccountLockout").Get<AccountLockoutOptions>() 
            ?? new AccountLockoutOptions();
    }

    public bool RegisterFailedAttempt(string email, string? ipAddress)
    {
        var normalizedEmail = email.ToLowerInvariant();
        var now = DateTime.UtcNow;

        // Registrar intento por cuenta
        var accountInfo = _accountAttempts.AddOrUpdate(
            normalizedEmail,
            _ => new LockoutInfo { FailedAttempts = 1, FirstAttempt = now, LastAttempt = now },
            (_, existing) =>
            {
                // Si el primer intento fue hace más tiempo que la ventana, reiniciar
                if (now - existing.FirstAttempt > TimeSpan.FromMinutes(_options.AttemptWindowMinutes))
                {
                    return new LockoutInfo { FailedAttempts = 1, FirstAttempt = now, LastAttempt = now };
                }

                existing.FailedAttempts++;
                existing.LastAttempt = now;

                // Bloquear si se excede el límite
                if (existing.FailedAttempts >= _options.MaxFailedAttempts && !existing.IsLocked)
                {
                    existing.IsLocked = true;
                    existing.LockoutEnd = now.AddMinutes(_options.LockoutDurationMinutes);
                    _logger.LogWarning(
                        "[SECURITY] Cuenta bloqueada por múltiples intentos fallidos. Email: {Email}, Intentos: {Attempts}",
                        normalizedEmail, existing.FailedAttempts);
                }

                return existing;
            });

        // Registrar intento por IP
        if (!string.IsNullOrEmpty(ipAddress))
        {
            var ipInfo = _ipAttempts.AddOrUpdate(
                ipAddress,
                _ => new LockoutInfo { FailedAttempts = 1, FirstAttempt = now, LastAttempt = now },
                (_, existing) =>
                {
                    if (now - existing.FirstAttempt > TimeSpan.FromMinutes(_options.IpAttemptWindowMinutes))
                    {
                        return new LockoutInfo { FailedAttempts = 1, FirstAttempt = now, LastAttempt = now };
                    }

                    existing.FailedAttempts++;
                    existing.LastAttempt = now;

                    if (existing.FailedAttempts >= _options.MaxIpFailedAttempts && !existing.IsLocked)
                    {
                        existing.IsLocked = true;
                        existing.LockoutEnd = now.AddMinutes(_options.IpLockoutDurationMinutes);
                        _logger.LogWarning(
                            "[SECURITY] IP bloqueada por múltiples intentos fallidos. IP: {IP}, Intentos: {Attempts}",
                            ipAddress, existing.FailedAttempts);
                    }

                    return existing;
                });
        }

        return accountInfo.IsLocked;
    }

    public (bool IsLocked, TimeSpan? RemainingTime) IsAccountLocked(string email)
    {
        var normalizedEmail = email.ToLowerInvariant();

        if (!_accountAttempts.TryGetValue(normalizedEmail, out var info))
        {
            return (false, null);
        }

        if (!info.IsLocked)
        {
            return (false, null);
        }

        var now = DateTime.UtcNow;
        if (info.LockoutEnd <= now)
        {
            // Desbloquear pero mantener el registro para auditoría
            info.IsLocked = false;
            return (false, null);
        }

        return (true, info.LockoutEnd - now);
    }

    public void ResetFailedAttempts(string email)
    {
        var normalizedEmail = email.ToLowerInvariant();
        _accountAttempts.TryRemove(normalizedEmail, out _);
    }

    public (bool IsLocked, TimeSpan? RemainingTime) IsIpLocked(string? ipAddress)
    {
        if (string.IsNullOrEmpty(ipAddress))
        {
            return (false, null);
        }

        if (!_ipAttempts.TryGetValue(ipAddress, out var info))
        {
            return (false, null);
        }

        if (!info.IsLocked)
        {
            return (false, null);
        }

        var now = DateTime.UtcNow;
        if (info.LockoutEnd <= now)
        {
            info.IsLocked = false;
            return (false, null);
        }

        return (true, info.LockoutEnd - now);
    }

    private class LockoutInfo
    {
        public int FailedAttempts { get; set; }
        public DateTime FirstAttempt { get; set; }
        public DateTime LastAttempt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LockoutEnd { get; set; }
    }
}

/// <summary>
/// Opciones de configuración para bloqueo de cuentas.
/// </summary>
public class AccountLockoutOptions
{
    /// <summary>
    /// Número máximo de intentos fallidos antes de bloquear la cuenta.
    /// </summary>
    public int MaxFailedAttempts { get; set; } = 5;

    /// <summary>
    /// Ventana de tiempo en minutos para contar intentos fallidos.
    /// </summary>
    public int AttemptWindowMinutes { get; set; } = 15;

    /// <summary>
    /// Duración del bloqueo en minutos.
    /// </summary>
    public int LockoutDurationMinutes { get; set; } = 15;

    /// <summary>
    /// Número máximo de intentos fallidos por IP.
    /// </summary>
    public int MaxIpFailedAttempts { get; set; } = 20;

    /// <summary>
    /// Ventana de tiempo para intentos por IP.
    /// </summary>
    public int IpAttemptWindowMinutes { get; set; } = 60;

    /// <summary>
    /// Duración del bloqueo de IP en minutos.
    /// </summary>
    public int IpLockoutDurationMinutes { get; set; } = 30;
}
