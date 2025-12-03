namespace BookingSystemAPI.Api.Common.Results;

/// <summary>
/// Representa el resultado de una operación que puede fallar.
/// Implementa el Result Pattern para manejo de errores sin excepciones.
/// </summary>
/// <typeparam name="T">Tipo del valor en caso de éxito.</typeparam>
public class Result<T>
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica si la operación falló.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Valor retornado en caso de éxito.
    /// </summary>
    public T? Value { get; }

    /// <summary>
    /// Error en caso de fallo.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    /// Lista de errores de validación.
    /// </summary>
    public IReadOnlyList<Error> ValidationErrors { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
        ValidationErrors = Array.Empty<Error>();
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
        ValidationErrors = Array.Empty<Error>();
    }

    private Result(IEnumerable<Error> validationErrors)
    {
        IsSuccess = false;
        Value = default;
        Error = new Error("Validation", "Se encontraron errores de validación.");
        ValidationErrors = validationErrors.ToList().AsReadOnly();
    }

    /// <summary>
    /// Crea un resultado exitoso.
    /// </summary>
    /// <param name="value">Valor a retornar.</param>
    /// <returns>Resultado exitoso.</returns>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Crea un resultado fallido.
    /// </summary>
    /// <param name="error">Error ocurrido.</param>
    /// <returns>Resultado fallido.</returns>
    public static Result<T> Failure(Error error) => new(error);

    /// <summary>
    /// Crea un resultado fallido con múltiples errores de validación.
    /// </summary>
    /// <param name="errors">Errores de validación.</param>
    /// <returns>Resultado fallido.</returns>
    public static Result<T> ValidationFailure(IEnumerable<Error> errors) => new(errors);

    /// <summary>
    /// Crea un resultado de recurso no encontrado.
    /// </summary>
    /// <param name="resourceName">Nombre del recurso.</param>
    /// <param name="resourceId">ID del recurso.</param>
    /// <returns>Resultado fallido.</returns>
    public static Result<T> NotFound(string resourceName, object resourceId) =>
        Failure(Error.NotFound(resourceName, resourceId));

    /// <summary>
    /// Convierte implícitamente un valor a un Result exitoso.
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);

    /// <summary>
    /// Convierte implícitamente un Error a un Result fallido.
    /// </summary>
    public static implicit operator Result<T>(Error error) => Failure(error);
}

/// <summary>
/// Representa el resultado de una operación sin valor de retorno.
/// </summary>
public class Result
{
    /// <summary>
    /// Indica si la operación fue exitosa.
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Indica si la operación falló.
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Error en caso de fallo.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    /// Lista de errores de validación.
    /// </summary>
    public IReadOnlyList<Error> ValidationErrors { get; }

    private Result()
    {
        IsSuccess = true;
        Error = null;
        ValidationErrors = Array.Empty<Error>();
    }

    private Result(Error error)
    {
        IsSuccess = false;
        Error = error;
        ValidationErrors = Array.Empty<Error>();
    }

    private Result(IEnumerable<Error> validationErrors)
    {
        IsSuccess = false;
        Error = new Error("Validation", "Se encontraron errores de validación.");
        ValidationErrors = validationErrors.ToList().AsReadOnly();
    }

    /// <summary>
    /// Crea un resultado exitoso.
    /// </summary>
    /// <returns>Resultado exitoso.</returns>
    public static Result Success() => new();

    /// <summary>
    /// Crea un resultado fallido.
    /// </summary>
    /// <param name="error">Error ocurrido.</param>
    /// <returns>Resultado fallido.</returns>
    public static Result Failure(Error error) => new(error);

    /// <summary>
    /// Crea un resultado fallido con múltiples errores de validación.
    /// </summary>
    /// <param name="errors">Errores de validación.</param>
    /// <returns>Resultado fallido.</returns>
    public static Result ValidationFailure(IEnumerable<Error> errors) => new(errors);

    /// <summary>
    /// Convierte implícitamente un Error a un Result fallido.
    /// </summary>
    public static implicit operator Result(Error error) => Failure(error);
}

/// <summary>
/// Representa un error en una operación.
/// </summary>
/// <param name="Code">Código del error.</param>
/// <param name="Message">Mensaje descriptivo del error.</param>
public record Error(string Code, string Message)
{
    /// <summary>
    /// Error de recurso no encontrado.
    /// </summary>
    public static Error NotFound(string resourceName, object resourceId) =>
        new("NotFound", $"El recurso '{resourceName}' con identificador '{resourceId}' no fue encontrado.");

    /// <summary>
    /// Error de validación.
    /// </summary>
    public static Error Validation(string code, string message) =>
        new(code, message);

    /// <summary>
    /// Error de conflicto.
    /// </summary>
    public static Error Conflict(string message) =>
        new("Conflict", message);

    /// <summary>
    /// Error de regla de negocio.
    /// </summary>
    public static Error BusinessRule(string code, string message) =>
        new(code, message);
}
