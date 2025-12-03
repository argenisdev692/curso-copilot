using BookingSystemAPI.Api.Models;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Interfaz del repositorio de reservas con operaciones específicas del dominio.
/// </summary>
public interface IBookingRepository : IRepository<Booking>
{
    /// <summary>
    /// Obtiene una reserva por ID incluyendo la información de la sala.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva con información de sala.</returns>
    Task<Booking?> GetByIdWithRoomAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene todas las reservas incluyendo información de sala.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas con salas.</returns>
    Task<IEnumerable<Booking>> GetAllWithRoomAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si existe un conflicto de horario para una sala.
    /// </summary>
    /// <param name="roomId">ID de la sala.</param>
    /// <param name="startTime">Hora de inicio.</param>
    /// <param name="endTime">Hora de fin.</param>
    /// <param name="excludeBookingId">ID de reserva a excluir (para actualizaciones).</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si existe conflicto.</returns>
    Task<bool> HasScheduleConflictAsync(
        int roomId,
        DateTime startTime,
        DateTime endTime,
        int? excludeBookingId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las reservas de una sala en un rango de fechas.
    /// </summary>
    /// <param name="roomId">ID de la sala.</param>
    /// <param name="startDate">Fecha de inicio del rango.</param>
    /// <param name="endDate">Fecha de fin del rango.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas.</returns>
    Task<IEnumerable<Booking>> GetByRoomAndDateRangeAsync(
        int roomId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las reservas de un organizador por email.
    /// </summary>
    /// <param name="email">Email del organizador.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas del organizador.</returns>
    Task<IEnumerable<Booking>> GetByOrganizerEmailAsync(
        string email,
        CancellationToken cancellationToken = default);
}
