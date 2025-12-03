using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Implementación del repositorio de reservas.
/// </summary>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    /// <summary>
    /// Inicializa una nueva instancia del repositorio de reservas.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="logger">Logger para registro.</param>
    public BookingRepository(
        ApplicationDbContext context,
        ILogger<BookingRepository> logger)
        : base(context, logger)
    {
    }

    /// <inheritdoc/>
    public async Task<Booking?> GetByIdWithRoomAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Obteniendo reserva {BookingId} con información de sala", id);

        return await _dbSet
            .Include(b => b.Room)
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetAllWithRoomAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Obteniendo todas las reservas con información de sala");

        return await _dbSet
            .Include(b => b.Room)
            .AsNoTracking()
            .OrderByDescending(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<bool> HasScheduleConflictAsync(
        int roomId,
        DateTime startTime,
        DateTime endTime,
        int? excludeBookingId = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug(
            "Verificando conflictos de horario para sala {RoomId} entre {StartTime} y {EndTime}",
            roomId, startTime, endTime);

        var query = _dbSet
            .Where(b => b.RoomId == roomId)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Where(b =>
                // Nuevo inicio está dentro de una reserva existente
                (startTime >= b.StartTime && startTime < b.EndTime) ||
                // Nuevo fin está dentro de una reserva existente
                (endTime > b.StartTime && endTime <= b.EndTime) ||
                // Nueva reserva envuelve completamente una existente
                (startTime <= b.StartTime && endTime >= b.EndTime));

        if (excludeBookingId.HasValue)
        {
            query = query.Where(b => b.Id != excludeBookingId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetByRoomAndDateRangeAsync(
        int roomId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug(
            "Obteniendo reservas de sala {RoomId} entre {StartDate} y {EndDate}",
            roomId, startDate, endDate);

        return await _dbSet
            .Include(b => b.Room)
            .AsNoTracking()
            .Where(b => b.RoomId == roomId)
            .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .OrderBy(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetByOrganizerEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Obteniendo reservas del organizador {Email}", email);

        return await _dbSet
            .Include(b => b.Room)
            .AsNoTracking()
            .Where(b => b.OrganizerEmail.ToLower() == email.ToLower())
            .OrderByDescending(b => b.StartTime)
            .ToListAsync(cancellationToken);
    }
}
