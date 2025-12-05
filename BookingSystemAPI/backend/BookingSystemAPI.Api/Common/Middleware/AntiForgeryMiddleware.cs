using Microsoft.AspNetCore.Antiforgery;

namespace BookingSystemAPI.Api.Common.Middleware;

/// <summary>
/// Middleware para manejar tokens CSRF/XSRF.
/// Genera un token CSRF para cada request GET y lo envía como cookie accesible por JavaScript.
/// Los requests POST/PUT/DELETE deben incluir este token en el header X-XSRF-TOKEN.
/// </summary>
public class AntiForgeryMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAntiforgery _antiforgery;
    private readonly ILogger<AntiForgeryMiddleware> _logger;

    /// <summary>
    /// Inicializa el middleware de anti-falsificación.
    /// </summary>
    /// <param name="next">Siguiente middleware en el pipeline.</param>
    /// <param name="antiforgery">Servicio de antiforgery.</param>
    /// <param name="logger">Logger para diagnóstico.</param>
    public AntiForgeryMiddleware(
        RequestDelegate next,
        IAntiforgery antiforgery,
        ILogger<AntiForgeryMiddleware> logger)
    {
        _next = next;
        _antiforgery = antiforgery;
        _logger = logger;
    }

    /// <summary>
    /// Procesa la solicitud HTTP.
    /// </summary>
    /// <param name="context">Contexto HTTP.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // Generar token CSRF para requests GET (páginas/recursos)
        if (HttpMethods.IsGet(context.Request.Method) && 
            !context.Request.Path.StartsWithSegments("/api/health"))
        {
            try
            {
                var tokens = _antiforgery.GetAndStoreTokens(context);
                
                if (!string.IsNullOrEmpty(tokens.RequestToken))
                {
                    context.Response.Cookies.Append(
                        "XSRF-TOKEN",
                        tokens.RequestToken,
                        new CookieOptions
                        {
                            HttpOnly = false, // JavaScript debe poder leerlo para enviarlo en headers
                            Secure = true,    // Solo HTTPS
                            SameSite = SameSiteMode.Strict,
                            Path = "/",
                            IsEssential = true
                        });
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "[SECURITY] Error al generar token CSRF");
            }
        }

        await _next(context);
    }
}

/// <summary>
/// Extensión para registrar el middleware de anti-falsificación.
/// </summary>
public static class AntiForgeryMiddlewareExtensions
{
    /// <summary>
    /// Agrega el middleware de anti-falsificación al pipeline.
    /// </summary>
    public static IApplicationBuilder UseAntiForgeryMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AntiForgeryMiddleware>();
    }
}
