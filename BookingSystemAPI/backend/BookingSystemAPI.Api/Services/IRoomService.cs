using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Rooms;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Servicio para gestión de salas.
/// </summary>
public interface IRoomService
{
    /// <summary>
    /// Obtiene todas las salas.
    /// </summary>
    Task<Result<IEnumerable<RoomDto>>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene una sala por su ID.
    /// </summary>
    Task<Result<RoomDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Crea una nueva sala.
    /// </summary>
    Task<Result<RoomDto>> CreateAsync(CreateRoomDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Actualiza una sala existente.
    /// </summary>
    Task<Result<RoomDto>> UpdateAsync(int id, UpdateRoomDto dto, CancellationToken cancellationToken = default);

    /// <summary>
    /// Elimina una sala (soft delete).
    /// </summary>
    Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cambia el estado de una sala a mantenimiento.
    /// </summary>
    Task<Result> SetMaintenanceAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cambia el estado de una sala a disponible.
    /// </summary>
    Task<Result> SetAvailableAsync(int id, CancellationToken cancellationToken = default);
}
