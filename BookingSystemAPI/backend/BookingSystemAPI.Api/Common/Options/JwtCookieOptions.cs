namespace BookingSystemAPI.Api.Common.Options;

/// <summary>
/// Opciones de configuración para cookies de autenticación JWT.
/// Siguiendo mejores prácticas de seguridad OWASP.
/// </summary>
public class JwtCookieOptions
{
    /// <summary>
    /// Sección de configuración en appsettings.json.
    /// </summary>
    public const string SectionName = "JwtCookie";

    /// <summary>
    /// Nombre de la cookie para el access token.
    /// </summary>
    public string AccessTokenCookieName { get; set; } = "access_token";

    /// <summary>
    /// Nombre de la cookie para el refresh token.
    /// </summary>
    public string RefreshTokenCookieName { get; set; } = "refresh_token";

    /// <summary>
    /// Nombre de la cookie CSRF.
    /// </summary>
    public string CsrfTokenCookieName { get; set; } = "XSRF-TOKEN";

    /// <summary>
    /// Nombre del header esperado para el token CSRF.
    /// </summary>
    public string CsrfHeaderName { get; set; } = "X-XSRF-TOKEN";

    /// <summary>
    /// Indica si las cookies solo deben enviarse por HTTPS.
    /// Siempre true en producción.
    /// </summary>
    public bool SecureOnly { get; set; } = true;

    /// <summary>
    /// Política SameSite para las cookies.
    /// Strict: Cookie solo se envía en requests del mismo sitio.
    /// Lax: Cookie se envía en navegación de nivel superior.
    /// None: Cookie se envía en todos los requests (requiere Secure=true).
    /// </summary>
    public SameSiteMode SameSiteMode { get; set; } = SameSiteMode.Strict;

    /// <summary>
    /// Path para la cookie del access token.
    /// Por defecto "/" para que se envíe en todas las requests a la API.
    /// </summary>
    public string AccessTokenPath { get; set; } = "/";

    /// <summary>
    /// Path para la cookie del refresh token.
    /// Restringido a endpoints de auth para minimizar exposición.
    /// </summary>
    public string RefreshTokenPath { get; set; } = "/api/auth";

    /// <summary>
    /// Dominio de las cookies. Null para usar el dominio actual.
    /// </summary>
    public string? Domain { get; set; }

    /// <summary>
    /// Indica si las cookies son esenciales (bypass de consent).
    /// </summary>
    public bool IsEssential { get; set; } = true;
}

/// <summary>
/// Opciones extendidas para configuración JWT con soporte de claves RSA.
/// </summary>
public class JwtSecurityOptions
{
    /// <summary>
    /// Sección de configuración.
    /// </summary>
    public const string SectionName = "Jwt";

    /// <summary>
    /// Clave secreta para HS256 (solo desarrollo).
    /// </summary>
    public string? Key { get; set; }

    /// <summary>
    /// Clave privada RSA en formato PEM para RS256 (producción).
    /// </summary>
    public string? PrivateKey { get; set; }

    /// <summary>
    /// Clave pública RSA en formato PEM para RS256.
    /// </summary>
    public string? PublicKey { get; set; }

    /// <summary>
    /// URI del Azure Key Vault para obtener claves.
    /// </summary>
    public string? KeyVaultUri { get; set; }

    /// <summary>
    /// Nombre del secreto en Key Vault para la clave privada.
    /// </summary>
    public string? KeyVaultSecretName { get; set; }

    /// <summary>
    /// Emisor del token (iss claim).
    /// </summary>
    public string Issuer { get; set; } = "BookingSystemAPI";

    /// <summary>
    /// Audiencia del token (aud claim).
    /// </summary>
    public string Audience { get; set; } = "BookingSystemAPI";

    /// <summary>
    /// Duración del access token en minutos.
    /// Recomendación OWASP: 15 minutos o menos.
    /// </summary>
    public int AccessTokenExpirationMinutes { get; set; } = 15;

    /// <summary>
    /// Duración del refresh token en días.
    /// </summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;

    /// <summary>
    /// Algoritmo de firma a usar.
    /// HS256 para desarrollo, RS256 para producción.
    /// </summary>
    public string Algorithm { get; set; } = "HS256";

    /// <summary>
    /// Tolerancia de reloj para validación de tokens.
    /// Recomendación: TimeSpan.Zero para máxima seguridad.
    /// </summary>
    public int ClockSkewSeconds { get; set; } = 0;

    /// <summary>
    /// Indica si se debe validar la firma del token.
    /// </summary>
    public bool ValidateIssuerSigningKey { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar el emisor.
    /// </summary>
    public bool ValidateIssuer { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar la audiencia.
    /// </summary>
    public bool ValidateAudience { get; set; } = true;

    /// <summary>
    /// Indica si se debe validar el tiempo de vida del token.
    /// </summary>
    public bool ValidateLifetime { get; set; } = true;

    /// <summary>
    /// Indica si se debe requerir tiempo de expiración en el token.
    /// </summary>
    public bool RequireExpirationTime { get; set; } = true;
}
