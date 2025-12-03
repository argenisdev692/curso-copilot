using System.Text.RegularExpressions;

namespace BookingSystemAPI.Api.Common.Middleware;

/// <summary>
/// Middleware para validar y sanitizar requests entrantes.
/// Protege contra ataques de inyección y XSS (OWASP A03:2021).
/// </summary>
public partial class RequestValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestValidationMiddleware> _logger;

    // Patrones peligrosos comunes (SQL Injection, XSS, Path Traversal)
    private static readonly string[] DangerousPatterns = new[]
    {
        @"<script\b[^>]*>",                    // Script tags
        @"javascript:",                         // JavaScript protocol
        @"on\w+\s*=",                          // Event handlers (onclick, onload, etc.)
        @"\.\.\/",                             // Path traversal
        @"\.\.\\",                             // Path traversal Windows
        @";\s*(drop|delete|truncate|alter)\s+", // SQL injection keywords
        @"'\s*or\s+'?\d+'?\s*=\s*'?\d+",       // SQL injection OR 1=1
        @"union\s+select",                     // SQL UNION injection
        @"<iframe",                            // Iframe injection
        @"<object",                            // Object injection
        @"<embed",                             // Embed injection
    };

    private static readonly Regex DangerousPatternRegex = new(
        string.Join("|", DangerousPatterns),
        RegexOptions.IgnoreCase | RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100)); // Timeout para evitar ReDoS

    // Límites de tamaño
    private const int MaxRequestBodySize = 1024 * 1024; // 1MB
    private const int MaxQueryStringLength = 2048;
    private const int MaxHeaderValueLength = 8192;

    public RequestValidationMiddleware(RequestDelegate next, ILogger<RequestValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? "unknown";

        // Validar Content-Type para POST/PUT/PATCH
        if (!ValidateContentType(context))
        {
            _logger.LogWarning(
                "[SECURITY] Content-Type inválido o faltante. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}",
                correlationId, context.Request.Path, context.Request.Method);

            context.Response.StatusCode = StatusCodes.Status415UnsupportedMediaType;
            await context.Response.WriteAsJsonAsync(new { error = "Content-Type debe ser application/json" });
            return;
        }

        // Validar longitud de query string
        if (context.Request.QueryString.HasValue && 
            context.Request.QueryString.Value?.Length > MaxQueryStringLength)
        {
            _logger.LogWarning(
                "[SECURITY] Query string excede límite. CorrelationId: {CorrelationId}, Length: {Length}",
                correlationId, context.Request.QueryString.Value?.Length);

            context.Response.StatusCode = StatusCodes.Status414UriTooLong;
            return;
        }

        // Validar patrones peligrosos en query string
        if (context.Request.QueryString.HasValue && 
            ContainsDangerousPatterns(context.Request.QueryString.Value))
        {
            LogSecurityThreat(context, "Query string contiene patrones peligrosos", correlationId);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(new { error = "Solicitud inválida" });
            return;
        }

        // Validar headers sospechosos
        foreach (var header in context.Request.Headers)
        {
            if (header.Value.ToString().Length > MaxHeaderValueLength)
            {
                _logger.LogWarning(
                    "[SECURITY] Header excede límite. CorrelationId: {CorrelationId}, Header: {Header}",
                    correlationId, header.Key);

                context.Response.StatusCode = StatusCodes.Status431RequestHeaderFieldsTooLarge;
                return;
            }

            // No validar patrones en ciertos headers (Authorization puede tener caracteres especiales)
            if (!IsExcludedHeader(header.Key) && ContainsDangerousPatterns(header.Value.ToString()))
            {
                LogSecurityThreat(context, $"Header '{header.Key}' contiene patrones peligrosos", correlationId);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }
        }

        await _next(context);
    }

    private bool ValidateContentType(HttpContext context)
    {
        var method = context.Request.Method;
        
        // Solo validar para métodos con body
        if (method != "POST" && method != "PUT" && method != "PATCH")
        {
            return true;
        }

        // Si no hay body, permitir
        if (!context.Request.HasJsonContentType() && context.Request.ContentLength == 0)
        {
            return true;
        }

        // Verificar que sea JSON
        var contentType = context.Request.ContentType;
        if (string.IsNullOrEmpty(contentType))
        {
            return false;
        }

        return contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase);
    }

    private static bool ContainsDangerousPatterns(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        try
        {
            return DangerousPatternRegex.IsMatch(input);
        }
        catch (RegexMatchTimeoutException)
        {
            // Si el regex timeout, considerarlo sospechoso
            return true;
        }
    }

    private static bool IsExcludedHeader(string headerName)
    {
        var excludedHeaders = new[] { "Authorization", "Cookie", "Set-Cookie" };
        return excludedHeaders.Contains(headerName, StringComparer.OrdinalIgnoreCase);
    }

    private void LogSecurityThreat(HttpContext context, string threat, string correlationId)
    {
        _logger.LogWarning(
            "[SECURITY THREAT] {Threat}. CorrelationId: {CorrelationId}, IP: {IP}, Path: {Path}, Method: {Method}, UserAgent: {UserAgent}",
            threat,
            correlationId,
            context.Connection.RemoteIpAddress,
            context.Request.Path,
            context.Request.Method,
            context.Request.Headers.UserAgent.ToString());
    }
}

/// <summary>
/// Extensiones para registrar el middleware de validación de requests.
/// </summary>
public static class RequestValidationMiddlewareExtensions
{
    /// <summary>
    /// Agrega el middleware de validación de requests al pipeline.
    /// </summary>
    public static IApplicationBuilder UseRequestValidation(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestValidationMiddleware>();
    }
}
