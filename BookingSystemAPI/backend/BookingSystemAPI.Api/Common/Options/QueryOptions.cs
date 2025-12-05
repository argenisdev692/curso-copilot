namespace BookingSystemAPI.Api.Common.Options;

/// <summary>
/// Opciones de configuración para consultas de base de datos.
/// </summary>
/// <remarks>
/// Permite configurar timeouts, logging y comportamiento de queries.
/// </remarks>
public class QueryOptions
{
    /// <summary>
    /// Nombre de la sección en appsettings.json.
    /// </summary>
    public const string SectionName = "QueryOptions";

    /// <summary>
    /// Timeout en segundos para consultas de base de datos.
    /// </summary>
    /// <example>30</example>
    public int TimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Habilita logging detallado de queries.
    /// </summary>
    public bool EnableDetailedLogging { get; set; }

    /// <summary>
    /// Umbral en milisegundos para considerar una query como lenta.
    /// </summary>
    /// <example>5000</example>
    public int SlowQueryThresholdMs { get; set; } = 5000;

    /// <summary>
    /// Tamaño máximo de página por defecto.
    /// </summary>
    public int DefaultPageSize { get; set; } = 20;

    /// <summary>
    /// Tamaño máximo de página permitido.
    /// </summary>
    public int MaxPageSize { get; set; } = 100;

    /// <summary>
    /// Habilita el uso de SplitQuery para consultas con múltiples Include.
    /// </summary>
    public bool UseSplitQuery { get; set; } = true;

    /// <summary>
    /// Número máximo de reintentos para queries fallidas.
    /// </summary>
    public int MaxRetryCount { get; set; } = 3;

    /// <summary>
    /// Intervalo base en milisegundos entre reintentos.
    /// </summary>
    public int RetryIntervalMs { get; set; } = 1000;
}
