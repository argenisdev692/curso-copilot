namespace BookingSystemAPI.Api.Common.Middleware;

/// <summary>
/// Middleware que agrega headers de seguridad HTTP según recomendaciones OWASP.
/// Protege contra ataques comunes como XSS, Clickjacking, MIME sniffing, etc.
/// </summary>
/// <remarks>
/// Headers implementados:
/// - X-Content-Type-Options: Previene MIME sniffing
/// - X-Frame-Options: Previene Clickjacking
/// - X-XSS-Protection: Capa adicional anti-XSS (legacy browsers)
/// - Content-Security-Policy: Política de seguridad de contenido
/// - Strict-Transport-Security: Fuerza HTTPS (HSTS)
/// - Referrer-Policy: Controla información de referrer
/// - Permissions-Policy: Restringe APIs del navegador
/// - Cache-Control: Control de caché para datos sensibles
/// </remarks>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(
        RequestDelegate next,
        IHostEnvironment environment,
        ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next;
        _environment = environment;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // ================================================
        // HEADERS DE SEGURIDAD - OWASP RECOMENDACIONES
        // ================================================

        // Prevenir MIME type sniffing (OWASP)
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

        // Prevenir Clickjacking
        context.Response.Headers.Append("X-Frame-Options", "DENY");

        // XSS Protection (para navegadores legacy)
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

        // Referrer Policy - limitar información enviada
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        // Permissions Policy - restringir APIs del navegador
        context.Response.Headers.Append("Permissions-Policy", 
            "accelerometer=(), camera=(), geolocation=(), gyroscope=(), magnetometer=(), microphone=(), payment=(), usb=()");

        // Content Security Policy - diferente para Swagger y API
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
        if (_environment.IsDevelopment() && IsSwaggerPath(path))
        {
            // CSP permisivo para Swagger UI en desarrollo
            // Nota: Swagger UI requiere 'unsafe-inline' y 'unsafe-eval' para funcionar correctamente
            context.Response.Headers.Append("Content-Security-Policy", 
                "default-src 'self'; " +
                "script-src 'self' 'unsafe-inline' 'unsafe-eval'; " +
                "style-src 'self' 'unsafe-inline'; " +
                "img-src 'self' data: https:; " +
                "font-src 'self' data:; " +
                "connect-src 'self';");
        }
        else
        {
            // CSP estricto para API
            context.Response.Headers.Append("Content-Security-Policy", 
                "default-src 'none'; frame-ancestors 'none'; form-action 'self'");
        }

        // Cache Control para respuestas con datos sensibles
        if (IsAuthEndpoint(context.Request.Path))
        {
            context.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
            context.Response.Headers.Append("Pragma", "no-cache");
            context.Response.Headers.Append("Expires", "0");
        }

        // HSTS - Solo en producción y HTTPS
        if (!_environment.IsDevelopment() && context.Request.IsHttps)
        {
            // max-age=31536000 (1 año), includeSubDomains, preload
            context.Response.Headers.Append("Strict-Transport-Security", 
                "max-age=31536000; includeSubDomains; preload");
        }

        // Remover headers que exponen información del servidor
        context.Response.Headers.Remove("Server");
        context.Response.Headers.Remove("X-Powered-By");
        context.Response.Headers.Remove("X-AspNet-Version");

        await _next(context);
    }

    private static bool IsAuthEndpoint(PathString path)
    {
        var authPaths = new[] { "/api/auth", "/api/token" };
        return authPaths.Any(p => path.StartsWithSegments(p, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica si la ruta es de Swagger UI.
    /// Incluye todas las rutas necesarias para el funcionamiento correcto de Swagger.
    /// </summary>
    private static bool IsSwaggerPath(string path)
    {
        // Rutas de Swagger cuando RoutePrefix = string.Empty
        var swaggerPaths = new[]
        {
            "/swagger",          // API JSON endpoint
            "/index.html",       // Swagger UI principal
            "/swagger-ui",       // Recursos de Swagger UI
            "/favicon",          // Favicon
            "/"                  // Root cuando es Swagger
        };

        // Extensiones de archivos estáticos de Swagger
        var swaggerExtensions = new[] { ".js", ".css", ".png", ".html", ".json", ".map" };

        return path == "" 
            || path == "/" 
            || swaggerPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase))
            || swaggerExtensions.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));
    }
}

/// <summary>
/// Extensiones para registrar el middleware de headers de seguridad.
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    /// <summary>
    /// Agrega el middleware de headers de seguridad al pipeline.
    /// </summary>
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}
