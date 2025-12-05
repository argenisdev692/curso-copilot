using BookingSystemAPI.Api.DTOs.Rooms;
using BookingSystemAPI.Api.Models;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Repositorio específico para operaciones de Room con consultas LINQ optimizadas.
/// </summary>
public interface IRoomRepository : IRepository<Room>
{
    /// <summary>
    /// Verifica si existe una sala con el nombre especificado.
    /// </summary>
    /// <param name="name">Nombre de la sala.</param>
    /// <param name="excludeId">ID a excluir de la búsqueda (para updates).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si existe, False en caso contrario.</returns>
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);

    #region Consultas LINQ Optimizadas

    /// <summary>
    /// Obtiene salas disponibles para un rango de tiempo con filtros opcionales.
    /// </summary>
    /// <remarks>
    /// Utiliza LEFT JOIN implícito para verificar disponibilidad.
    /// Proyecta directamente a DTO para evitar over-fetching.
    /// </remarks>
    /// <param name="query">Parámetros de búsqueda de disponibilidad.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de salas disponibles con detalles.</returns>
    Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(
        RoomAvailabilityQueryDto query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene métricas de ocupación de salas para un período.
    /// </summary>
    /// <remarks>
    /// Calcula ocupación usando agregaciones en servidor (GroupBy, Sum).
    /// </remarks>
    /// <param name="startDate">Fecha de inicio del período.</param>
    /// <param name="endDate">Fecha de fin del período.</param>
    /// <param name="roomIds">IDs de salas específicas (opcional, null = todas).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de métricas de ocupación por sala.</returns>
    Task<IEnumerable<RoomOccupancyDto>> GetRoomOccupancyAsync(
        DateTime startDate,
        DateTime endDate,
        IEnumerable<int>? roomIds = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene salas con sus próximas N reservas.
    /// </summary>
    /// <remarks>
    /// Usa Include con Take limitado para evitar cargar todas las reservas.
    /// </remarks>
    /// <param name="upcomingBookingsCount">Número de próximas reservas a incluir.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Salas con próximas reservas.</returns>
    Task<IEnumerable<Room>> GetRoomsWithUpcomingBookingsAsync(
        int upcomingBookingsCount = 5,
        CancellationToken cancellationToken = default);

    #endregion
}

