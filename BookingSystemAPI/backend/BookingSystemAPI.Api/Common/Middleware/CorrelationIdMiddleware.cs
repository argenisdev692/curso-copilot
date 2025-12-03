namespace BookingSystemAPI.Api.Common.Middleware;

/// <summary>
/// Middleware para gestión de CorrelationId en las peticiones HTTP.
/// Permite trazabilidad de las operaciones a través de los logs.
/// </summary>
public class CorrelationIdMiddleware
{
    private const string CorrelationIdHeader = "X-Correlation-Id";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del middleware.
    /// </summary>
    /// <param name="next">Siguiente middleware en el pipeline.</param>
    /// <param name="logger">Logger para registro de información.</param>
    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Procesa la petición HTTP y asigna un CorrelationId.
    /// </summary>
    /// <param name="context">Contexto HTTP de la petición.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        // Obtener o generar CorrelationId
        var correlationId = context.Request.Headers[CorrelationIdHeader].FirstOrDefault()
            ?? Guid.NewGuid().ToString();

        // Agregar al contexto y a la respuesta
        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers[CorrelationIdHeader] = correlationId;

        // Enriquecer el contexto de logging con el CorrelationId
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId,
            ["RequestPath"] = context.Request.Path.ToString(),
            ["RequestMethod"] = context.Request.Method
        }))
        {
            _logger.LogDebug("Procesando request con CorrelationId: {CorrelationId}", correlationId);
            await _next(context);
        }
    }
}
