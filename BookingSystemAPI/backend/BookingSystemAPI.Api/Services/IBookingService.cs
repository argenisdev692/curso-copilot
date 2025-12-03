using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Bookings;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Interfaz del servicio de reservas.
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Obtiene todas las reservas.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas.</returns>
    Task<Result<IEnumerable<BookingDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una reserva por su ID.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva encontrada o error.</returns>
    Task<Result<BookingDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea una nueva reserva.
    /// </summary>
    /// <param name="dto">Datos de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva creada o error.</returns>
    Task<Result<BookingDto>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una reserva existente.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="dto">Nuevos datos de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reserva actualizada o error.</returns>
    Task<Result<BookingDto>> UpdateAsync(int id, UpdateBookingDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancela una reserva.
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una reserva (soft delete).
    /// </summary>
    /// <param name="id">ID de la reserva.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado de la operación.</returns>
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene las reservas de una sala en un rango de fechas.
    /// </summary>
    /// <param name="roomId">ID de la sala.</param>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de reservas.</returns>
    Task<Result<IEnumerable<BookingDto>>> GetByRoomAndDateRangeAsync(
        int roomId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);
}
