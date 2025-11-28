using RabbitMQ.Client;

namespace TicketManagementSystem.API.Infrastructure.RabbitMQ;

/// <summary>
/// Interfaz para la gestión de conexiones a RabbitMQ.
/// Proporciona acceso a canales y maneja la reconexión automática.
/// </summary>
public interface IRabbitMQConnection : IDisposable
{
    /// <summary>
    /// Indica si la conexión está activa.
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// Intenta conectar a RabbitMQ si no está conectado.
    /// </summary>
    /// <returns>True si la conexión fue exitosa, false en caso contrario.</returns>
    bool TryConnect();

    /// <summary>
    /// Crea un nuevo canal de RabbitMQ.
    /// </summary>
    /// <returns>Un objeto IModel que representa el canal.</returns>
    IModel CreateModel();
}