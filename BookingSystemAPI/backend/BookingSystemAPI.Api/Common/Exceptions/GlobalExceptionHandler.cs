using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace BookingSystemAPI.Api.Common.Exceptions;

/// <summary>
/// Manejador global de excepciones que implementa el estándar RFC 7807 (ProblemDetails).
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    /// <summary>
    /// Inicializa una nueva instancia del manejador de excepciones.
    /// </summary>
    /// <param name="logger">Logger para registro de errores.</param>
    /// <param name="environment">Información del entorno de ejecución.</param>
    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }

    /// <summary>
    /// Maneja las excepciones no controladas de la aplicación.
    /// </summary>
    /// <param name="httpContext">Contexto HTTP.</param>
    /// <param name="exception">Excepción a manejar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si la excepción fue manejada.</returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var correlationId = httpContext.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();

        _logger.LogError(exception,
            "Excepción no controlada. CorrelationId: {CorrelationId}, Path: {Path}, Method: {Method}",
            correlationId,
            httpContext.Request.Path,
            httpContext.Request.Method);

        var problemDetails = CreateProblemDetails(httpContext, exception, correlationId);

        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    /// <summary>
    /// Crea un objeto ProblemDetails basado en el tipo de excepción.
    /// </summary>
    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception, string correlationId)
    {
        var problemDetails = exception switch
        {
            ValidationException validationException => new ValidationProblemDetails(
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()))
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Error de validación",
                Detail = "Uno o más errores de validación ocurrieron.",
                Instance = context.Request.Path
            },

            NotFoundException notFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso no encontrado",
                Detail = notFoundException.Message,
                Instance = context.Request.Path
            },

            BusinessException businessException => new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Error de negocio",
                Detail = businessException.Message,
                Instance = context.Request.Path
            },

            UnauthorizedAccessException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "No autorizado",
                Detail = "No tiene permisos para realizar esta operación.",
                Instance = context.Request.Path
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = _environment.IsDevelopment()
                    ? exception.Message
                    : "Ha ocurrido un error interno. Por favor, contacte al administrador.",
                Instance = context.Request.Path
            }
        };

        problemDetails.Extensions["correlationId"] = correlationId;
        problemDetails.Extensions["timestamp"] = DateTime.UtcNow;

        if (_environment.IsDevelopment() && problemDetails.Status == StatusCodes.Status500InternalServerError)
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        return problemDetails;
    }
}

/// <summary>
/// Excepción para recursos no encontrados.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Nombre del recurso no encontrado.
    /// </summary>
    public string ResourceName { get; }

    /// <summary>
    /// Identificador del recurso no encontrado.
    /// </summary>
    public object? ResourceId { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la excepción.
    /// </summary>
    /// <param name="resourceName">Nombre del recurso.</param>
    /// <param name="resourceId">Identificador del recurso.</param>
    public NotFoundException(string resourceName, object? resourceId = null)
        : base($"El recurso '{resourceName}' con identificador '{resourceId}' no fue encontrado.")
    {
        ResourceName = resourceName;
        ResourceId = resourceId;
    }
}

/// <summary>
/// Excepción para errores de lógica de negocio.
/// </summary>
public class BusinessException : Exception
{
    /// <summary>
    /// Código de error de negocio.
    /// </summary>
    public string ErrorCode { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la excepción.
    /// </summary>
    /// <param name="message">Mensaje de error.</param>
    /// <param name="errorCode">Código de error opcional.</param>
    public BusinessException(string message, string errorCode = "BUSINESS_ERROR")
        : base(message)
    {
        ErrorCode = errorCode;
    }
}
