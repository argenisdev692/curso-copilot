using BookingSystemAPI.Api.Common.Options;
using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.DTOs.Bookings;
using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Linq.Expressions;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Implementación del repositorio de reservas con consultas LINQ optimizadas.
/// </summary>
/// <remarks>
/// Incluye:
/// - Consultas con AsNoTracking para reads
/// - Proyecciones directas a DTOs (evita over-fetching)
/// - Paginación eficiente con Skip/Take
/// - Consultas compiladas para operaciones repetitivas
/// - Logging estructurado con métricas de performance
/// </remarks>
public class BookingRepository : Repository<Booking>, IBookingRepository
{
    private readonly QueryOptions _queryOptions;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de reservas.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="logger">Logger para registro.</param>
    /// <param name="queryOptions">Opciones de configuración de queries.</param>
    public BookingRepository(
        ApplicationDbContext context,
        ILogger<BookingRepository> logger,
        IOptions<QueryOptions> queryOptions)
        : base(context, logger)
    {
        _queryOptions = queryOptions?.Value ?? new QueryOptions();
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

    #region Consultas LINQ Optimizadas

    /// <inheritdoc/>
    public async Task<PagedResultDto<BookingSearchResultDto>> GetBookingsWithFiltersAsync(
        BookingQueryDto query,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Ejecutando GetBookingsWithFiltersAsync con filtros: RoomId={RoomId}, Status={Status}, StartDate={StartDate}, EndDate={EndDate}",
            correlationId, query.RoomId, query.Status, query.StartDate, query.EndDate);

        // 1. Construir query base con AsNoTracking (solo lectura)
        var baseQuery = _dbSet
            .AsNoTracking()
            .TagWith($"GetBookingsWithFilters-{correlationId}") // Tag para debugging en SQL Profiler
            .Include(b => b.Room)
            .AsSplitQuery() // Evita explosión cartesiana en joins complejos
            .AsQueryable();

        // 2. Aplicar filtros dinámicos usando expresiones lambda
        baseQuery = ApplyFilters(baseQuery, query);

        // 3. Obtener conteo total (ejecuta COUNT en servidor)
        var totalCount = await baseQuery.CountAsync(cancellationToken);

        // 4. Aplicar ordenamiento dinámico
        baseQuery = ApplySorting(baseQuery, query.SortBy, query.IsDescending);

        // 5. Proyectar directamente a DTO (evita cargar entidades completas)
        var items = await baseQuery
            .Skip(query.Skip)
            .Take(Math.Min(query.PageSize, _queryOptions.MaxPageSize))
            .Select(b => new BookingSearchResultDto
            {
                BookingId = b.Id,
                Title = b.Title,
                UserName = b.OrganizerName,
                RoomName = b.Room != null ? b.Room.Name : string.Empty,
                StartTime = b.StartTime,
                EndTime = b.EndTime,
                Status = b.Status.ToString()
            })
            .ToListAsync(cancellationToken);

        stopwatch.Stop();

        // 6. Logging de performance
        LogQueryPerformance(correlationId, "GetBookingsWithFiltersAsync", stopwatch.ElapsedMilliseconds, totalCount);

        return new PagedResultDto<BookingSearchResultDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = query.Page,
            PageSize = query.PageSize
        };
    }

    /// <inheritdoc/>
    public async Task<BookingSummaryDto> GetBookingsSummaryAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Ejecutando GetBookingsSummaryAsync para período {StartDate} - {EndDate}",
            correlationId, startDate, endDate);

        // Consulta optimizada con agregaciones en servidor
        var summary = await _dbSet
            .AsNoTracking()
            .TagWith($"GetBookingsSummary-{correlationId}")
            .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
            .GroupBy(_ => 1) // Agrupa todo en un solo grupo para agregación
            .Select(g => new BookingSummaryDto
            {
                TotalBookings = g.Count(),
                ConfirmedBookings = g.Count(b => b.Status == BookingStatus.Confirmed),
                CancelledBookings = g.Count(b => b.Status == BookingStatus.Cancelled),
                PendingBookings = g.Count(b => b.Status == BookingStatus.Pending),
                CompletedBookings = g.Count(b => b.Status == BookingStatus.Completed),
                TotalHoursBooked = g.Where(b => b.Status != BookingStatus.Cancelled)
                    .Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0,
                AverageDurationMinutes = g.Where(b => b.Status != BookingStatus.Cancelled)
                    .Average(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)),
                AverageAttendees = g.Where(b => b.Status != BookingStatus.Cancelled)
                    .Average(b => (double)b.AttendeeCount),
                PeriodStart = startDate,
                PeriodEnd = endDate
            })
            .FirstOrDefaultAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetBookingsSummaryAsync", stopwatch.ElapsedMilliseconds, 1);

        // Si no hay datos, retornar DTO vacío con fechas del período
        return summary ?? new BookingSummaryDto
        {
            PeriodStart = startDate,
            PeriodEnd = endDate
        };
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RoomBookingSummaryDto>> GetBookingsSummaryByRoomAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Ejecutando GetBookingsSummaryByRoomAsync para período {StartDate} - {EndDate}",
            correlationId, startDate, endDate);

        // Horas de negocio por día (8:00 - 20:00 = 12 horas)
        var businessHoursPerDay = 12.0;
        var totalDays = (endDate - startDate).TotalDays + 1;
        var totalAvailableHours = businessHoursPerDay * totalDays;

        var summaries = await _dbSet
            .AsNoTracking()
            .TagWith($"GetBookingsSummaryByRoom-{correlationId}")
            .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .GroupBy(b => new { b.RoomId, b.Room!.Name, b.Room.Capacity })
            .Select(g => new RoomBookingSummaryDto
            {
                RoomId = g.Key.RoomId,
                RoomName = g.Key.Name,
                RoomCapacity = g.Key.Capacity,
                TotalBookings = g.Count(),
                TotalHoursBooked = g.Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0,
                AverageAttendees = g.Average(b => (double)b.AttendeeCount),
                OccupancyRate = (g.Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0) 
                    / totalAvailableHours * 100,
                CapacityUtilization = g.Average(b => (double)b.AttendeeCount / g.Key.Capacity * 100)
            })
            .OrderByDescending(r => r.TotalBookings)
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetBookingsSummaryByRoomAsync", stopwatch.ElapsedMilliseconds, summaries.Count);

        return summaries;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<OrganizerBookingSummaryDto>> GetBookingsSummaryByOrganizerAsync(
        DateTime startDate,
        DateTime endDate,
        int topN = 10,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Ejecutando GetBookingsSummaryByOrganizerAsync para período {StartDate} - {EndDate}, Top {TopN}",
            correlationId, startDate, endDate, topN);

        var summaries = await _dbSet
            .AsNoTracking()
            .TagWith($"GetBookingsSummaryByOrganizer-{correlationId}")
            .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
            .GroupBy(b => new { b.OrganizerEmail, b.OrganizerName })
            .Select(g => new OrganizerBookingSummaryDto
            {
                OrganizerEmail = g.Key.OrganizerEmail,
                OrganizerName = g.Key.OrganizerName,
                TotalBookings = g.Count(),
                TotalHoursBooked = g.Where(b => b.Status != BookingStatus.Cancelled)
                    .Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0,
                CancelledBookings = g.Count(b => b.Status == BookingStatus.Cancelled),
                CancellationRate = g.Count(b => b.Status == BookingStatus.Cancelled) * 100.0 / g.Count()
            })
            .OrderByDescending(o => o.TotalBookings)
            .Take(topN)
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetBookingsSummaryByOrganizerAsync", stopwatch.ElapsedMilliseconds, summaries.Count);

        return summaries;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DailyBookingStatsDto>> GetDailyStatsAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Ejecutando GetDailyStatsAsync para período {StartDate} - {EndDate}",
            correlationId, startDate, endDate);

        var stats = await _dbSet
            .AsNoTracking()
            .TagWith($"GetDailyStats-{correlationId}")
            .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .GroupBy(b => b.StartTime.Date)
            .Select(g => new DailyBookingStatsDto
            {
                Date = DateOnly.FromDateTime(g.Key),
                TotalBookings = g.Count(),
                TotalHoursBooked = g.Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0,
                UniqueRoomsUsed = g.Select(b => b.RoomId).Distinct().Count(),
                TotalAttendees = g.Sum(b => b.AttendeeCount)
            })
            .OrderBy(s => s.Date)
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetDailyStatsAsync", stopwatch.ElapsedMilliseconds, stats.Count);

        return stats;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<TResult>> QueryWithProjectionAsync<TResult>(
        Expression<Func<Booking, bool>> predicate,
        Expression<Func<Booking, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogDebug(
            "[{CorrelationId}] Ejecutando QueryWithProjectionAsync con predicado personalizado",
            correlationId);

        var results = await _dbSet
            .AsNoTracking()
            .TagWith($"QueryWithProjection-{correlationId}")
            .Where(predicate)
            .Select(selector)
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "QueryWithProjectionAsync", stopwatch.ElapsedMilliseconds, results.Count);

        return results;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Booking>> GetActiveBookingsForRoomCompiledAsync(
        int roomId,
        DateTime fromDate,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogDebug(
            "[{CorrelationId}] Ejecutando consulta optimizada GetActiveBookingsForRoomCompiledAsync para sala {RoomId}",
            correlationId, roomId);

        // Consulta optimizada con AsNoTracking y filtros específicos
        // Nota: Las consultas compiladas estáticas tienen limitaciones con DbContext scoped,
        // por lo que usamos una consulta estándar optimizada
        var results = await _dbSet
            .AsNoTracking()
            .TagWith($"GetActiveBookingsForRoom-{correlationId}")
            .Where(b => b.RoomId == roomId)
            .Where(b => b.Status != BookingStatus.Cancelled)
            .Where(b => b.EndTime >= fromDate)
            .OrderBy(b => b.StartTime)
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetActiveBookingsForRoomCompiledAsync", stopwatch.ElapsedMilliseconds, results.Count);

        return results;
    }

    #endregion

    #region Métodos Privados de Ayuda

    /// <summary>
    /// Aplica filtros dinámicos a la consulta base.
    /// </summary>
    /// <remarks>
    /// Usa coalescencia nula (??) para manejar filtros opcionales.
    /// Los filtros se componen sobre IQueryable sin ejecutar la consulta.
    /// </remarks>
    private static IQueryable<Booking> ApplyFilters(IQueryable<Booking> query, BookingQueryDto filter)
    {
        // Filtro por RoomId (si se especifica)
        if (filter.RoomId.HasValue)
        {
            query = query.Where(b => b.RoomId == filter.RoomId.Value);
        }

        // Filtro por Status (si se especifica)
        if (!string.IsNullOrWhiteSpace(filter.Status) && 
            Enum.TryParse<BookingStatus>(filter.Status, ignoreCase: true, out var status))
        {
            query = query.Where(b => b.Status == status);
        }

        // Filtro por rango de fechas (coalescencia para opcionales)
        if (filter.StartDate.HasValue)
        {
            query = query.Where(b => b.StartTime >= filter.StartDate.Value);
        }

        if (filter.EndDate.HasValue)
        {
            query = query.Where(b => b.EndTime <= filter.EndDate.Value);
        }

        // Filtro por email del organizador (case-insensitive)
        if (!string.IsNullOrWhiteSpace(filter.OrganizerEmail))
        {
            var emailLower = filter.OrganizerEmail.ToLowerInvariant();
            query = query.Where(b => b.OrganizerEmail.ToLower() == emailLower);
        }

        // Búsqueda de texto en título o descripción
        if (!string.IsNullOrWhiteSpace(filter.SearchText))
        {
            var searchLower = filter.SearchText.ToLowerInvariant();
            query = query.Where(b => 
                b.Title.ToLower().Contains(searchLower) ||
                (b.Description != null && b.Description.ToLower().Contains(searchLower)));
        }

        return query;
    }

    /// <summary>
    /// Aplica ordenamiento dinámico a la consulta.
    /// </summary>
    /// <param name="query">Consulta base.</param>
    /// <param name="sortBy">Campo de ordenamiento.</param>
    /// <param name="descending">Si es descendente.</param>
    /// <returns>Consulta ordenada.</returns>
    private static IQueryable<Booking> ApplySorting(IQueryable<Booking> query, string? sortBy, bool descending)
    {
        return sortBy?.ToLowerInvariant() switch
        {
            "title" => descending 
                ? query.OrderByDescending(b => b.Title) 
                : query.OrderBy(b => b.Title),
            "organizer" or "organizername" => descending 
                ? query.OrderByDescending(b => b.OrganizerName) 
                : query.OrderBy(b => b.OrganizerName),
            "room" or "roomname" => descending 
                ? query.OrderByDescending(b => b.Room!.Name) 
                : query.OrderBy(b => b.Room!.Name),
            "status" => descending 
                ? query.OrderByDescending(b => b.Status) 
                : query.OrderBy(b => b.Status),
            "createdat" => descending 
                ? query.OrderByDescending(b => b.CreatedAt) 
                : query.OrderBy(b => b.CreatedAt),
            // Default: ordenar por StartTime
            _ => descending 
                ? query.OrderByDescending(b => b.StartTime) 
                : query.OrderBy(b => b.StartTime)
        };
    }

    /// <summary>
    /// Registra métricas de performance de la consulta.
    /// </summary>
    private void LogQueryPerformance(string correlationId, string methodName, long elapsedMs, int resultCount)
    {
        if (elapsedMs > _queryOptions.SlowQueryThresholdMs)
        {
            _logger.LogWarning(
                "[{CorrelationId}] ⚠️ SLOW QUERY: {MethodName} tardó {ElapsedMs}ms (umbral: {Threshold}ms). Resultados: {ResultCount}",
                correlationId, methodName, elapsedMs, _queryOptions.SlowQueryThresholdMs, resultCount);
        }
        else if (_queryOptions.EnableDetailedLogging)
        {
            _logger.LogInformation(
                "[{CorrelationId}] ✅ {MethodName} completado en {ElapsedMs}ms. Resultados: {ResultCount}",
                correlationId, methodName, elapsedMs, resultCount);
        }
    }

    #endregion
}

