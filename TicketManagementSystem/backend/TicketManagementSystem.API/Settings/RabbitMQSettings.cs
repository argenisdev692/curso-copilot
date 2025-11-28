namespace TicketManagementSystem.API.Settings;

/// <summary>
/// Configuración para la conexión a RabbitMQ.
/// Se mapea desde appsettings.json usando la sección "RabbitMQ".
/// </summary>
public class RabbitMQSettings
{
    /// <summary>
    /// Nombre del host de RabbitMQ.
    /// </summary>
    public string HostName { get; set; } = string.Empty;

    /// <summary>
    /// Puerto de conexión a RabbitMQ.
    /// </summary>
    public int Port { get; set; } = 5672;

    /// <summary>
    /// Nombre de usuario para autenticación.
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña para autenticación.
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Virtual host de RabbitMQ.
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Nombre del exchange para publicar eventos.
    /// </summary>
    public string ExchangeName { get; set; } = string.Empty;

    /// <summary>
    /// Número máximo de reintentos en caso de fallo.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Retraso entre reintentos en milisegundos.
    /// </summary>
    public int RetryDelayMs { get; set; } = 1000;
}