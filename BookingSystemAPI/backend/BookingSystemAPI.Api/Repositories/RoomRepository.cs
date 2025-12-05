using BookingSystemAPI.Api.Common.Options;
using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.DTOs.Rooms;
using BookingSystemAPI.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace BookingSystemAPI.Api.Repositories;

/// <summary>
/// Implementación del repositorio de Room con consultas LINQ optimizadas.
/// </summary>
/// <remarks>
/// Incluye consultas de disponibilidad y ocupación con proyecciones eficientes.
/// </remarks>
public class RoomRepository : Repository<Room>, IRoomRepository
{
    private readonly QueryOptions _queryOptions;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de salas.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="logger">Logger para registro.</param>
    /// <param name="queryOptions">Opciones de configuración de queries.</param>
    public RoomRepository(
        ApplicationDbContext context,
        ILogger<RoomRepository> logger,
        IOptions<QueryOptions> queryOptions)
        : base(context, logger)
    {
        _queryOptions = queryOptions?.Value ?? new QueryOptions();
    }

    /// <inheritdoc />
    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _dbSet.Where(r => r.Name == name);

        if (excludeId.HasValue)
        {
            query = query.Where(r => r.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    #region Consultas LINQ Optimizadas

    /// <inheritdoc/>
    public async Task<IEnumerable<AvailableRoomDto>> GetAvailableRoomsAsync(
        RoomAvailabilityQueryDto query,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Buscando salas disponibles entre {StartTime} y {EndTime}",
            correlationId, query.StartTime, query.EndTime);

        // 1. Query base: salas activas
        var roomsQuery = _dbSet
            .AsNoTracking()
            .TagWith($"GetAvailableRooms-{correlationId}")
            .Where(r => r.Status == RoomStatus.Available)
            .AsQueryable();

        // 2. Filtro por capacidad mínima
        if (query.MinCapacity.HasValue)
        {
            roomsQuery = roomsQuery.Where(r => r.Capacity >= query.MinCapacity.Value);
        }

        // 3. Filtro por ubicación preferida
        if (!string.IsNullOrWhiteSpace(query.PreferredLocation))
        {
            var locationLower = query.PreferredLocation.ToLowerInvariant();
            roomsQuery = roomsQuery.Where(r => r.Location.ToLower().Contains(locationLower));
        }

        // 4. Obtener salas que NO tienen conflictos en el rango de tiempo
        // Usando LEFT JOIN implícito con navegación y condición negativa
        var availableRooms = await roomsQuery
            .Where(r => !r.Bookings.Any(b =>
                b.Status != BookingStatus.Cancelled &&
                // Verificar solapamiento: nueva reserva se superpone con existente
                ((query.StartTime >= b.StartTime && query.StartTime < b.EndTime) ||
                 (query.EndTime > b.StartTime && query.EndTime <= b.EndTime) ||
                 (query.StartTime <= b.StartTime && query.EndTime >= b.EndTime))))
            .Select(r => new AvailableRoomDto
            {
                Id = r.Id,
                Name = r.Name,
                Capacity = r.Capacity,
                Location = r.Location,
                Equipment = r.Equipment,
                HasAllRequiredEquipment = query.RequiredEquipment == null ||
                    query.RequiredEquipment.All(eq => r.Equipment.Contains(eq)),
                NextBookingStart = r.Bookings
                    .Where(b => b.StartTime > query.EndTime && b.Status != BookingStatus.Cancelled)
                    .OrderBy(b => b.StartTime)
                    .Select(b => (DateTime?)b.StartTime)
                    .FirstOrDefault()
            })
            .OrderBy(r => r.Name)
            .ToListAsync(cancellationToken);

        // 5. Filtrar por equipamiento requerido (post-query si es necesario)
        if (query.RequiredEquipment?.Any() == true)
        {
            availableRooms = availableRooms
                .Where(r => r.HasAllRequiredEquipment)
                .ToList();
        }

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetAvailableRoomsAsync", stopwatch.ElapsedMilliseconds, availableRooms.Count);

        return availableRooms;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RoomOccupancyDto>> GetRoomOccupancyAsync(
        DateTime startDate,
        DateTime endDate,
        IEnumerable<int>? roomIds = null,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{CorrelationId}] Calculando ocupación de salas entre {StartDate} y {EndDate}",
            correlationId, startDate, endDate);

        // Calcular horas disponibles (asumiendo 12 horas de negocio por día: 8:00-20:00)
        var businessHoursPerDay = 12.0;
        var totalDays = Math.Max(1, (endDate - startDate).TotalDays);
        var totalAvailableHours = businessHoursPerDay * totalDays;

        // Query base
        var query = _dbSet
            .AsNoTracking()
            .TagWith($"GetRoomOccupancy-{correlationId}")
            .AsQueryable();

        // Filtrar por IDs específicos si se proporcionan
        if (roomIds?.Any() == true)
        {
            var roomIdList = roomIds.ToList();
            query = query.Where(r => roomIdList.Contains(r.Id));
        }

        // Calcular ocupación con agregación en servidor
        var occupancy = await query
            .Select(r => new RoomOccupancyDto
            {
                RoomId = r.Id,
                RoomName = r.Name,
                TotalAvailableHours = totalAvailableHours,
                TotalBookedHours = r.Bookings
                    .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
                    .Where(b => b.Status != BookingStatus.Cancelled)
                    .Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0,
                BookingCount = r.Bookings
                    .Count(b => b.StartTime >= startDate && b.StartTime <= endDate &&
                           b.Status != BookingStatus.Cancelled),
                PeriodStart = startDate,
                PeriodEnd = endDate,
                OccupancyPercentage = r.Bookings
                    .Where(b => b.StartTime >= startDate && b.StartTime <= endDate)
                    .Where(b => b.Status != BookingStatus.Cancelled)
                    .Sum(b => EF.Functions.DateDiffMinute(b.StartTime, b.EndTime)) / 60.0 
                    / totalAvailableHours * 100
            })
            .OrderByDescending(o => o.OccupancyPercentage)
            .ToListAsync(cancellationToken);

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetRoomOccupancyAsync", stopwatch.ElapsedMilliseconds, occupancy.Count);

        return occupancy;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<Room>> GetRoomsWithUpcomingBookingsAsync(
        int upcomingBookingsCount = 5,
        CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var correlationId = Guid.NewGuid().ToString("N")[..8];
        var now = DateTime.UtcNow;

        _logger.LogDebug(
            "[{CorrelationId}] Obteniendo salas con próximas {Count} reservas",
            correlationId, upcomingBookingsCount);

        // Usar proyección intermedia para limitar reservas cargadas
        var rooms = await _dbSet
            .AsNoTracking()
            .TagWith($"GetRoomsWithUpcomingBookings-{correlationId}")
            .AsSplitQuery() // Evita explosión cartesiana
            .Where(r => r.Status == RoomStatus.Available)
            .Select(r => new
            {
                Room = r,
                UpcomingBookings = r.Bookings
                    .Where(b => b.StartTime >= now && b.Status != BookingStatus.Cancelled)
                    .OrderBy(b => b.StartTime)
                    .Take(upcomingBookingsCount)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        // Mapear a entidades con reservas limitadas
        var result = rooms.Select(x =>
        {
            x.Room.Bookings = x.UpcomingBookings;
            return x.Room;
        }).ToList();

        stopwatch.Stop();
        LogQueryPerformance(correlationId, "GetRoomsWithUpcomingBookingsAsync", stopwatch.ElapsedMilliseconds, result.Count);

        return result;
    }

    #endregion

    #region Métodos Privados

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

