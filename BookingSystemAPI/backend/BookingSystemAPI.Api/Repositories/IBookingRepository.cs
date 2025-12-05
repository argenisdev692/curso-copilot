using BookingSystemAPI.Api.DTOs.Bookings;
using BookingSystemAPI.Api.Models;
using System.Linq.Expressions;

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

    #region Consultas LINQ Optimizadas

    /// <summary>
    /// Obtiene reservas con filtros dinámicos, paginación y proyección optimizada.
    /// </summary>
    /// <remarks>
    /// Utiliza AsNoTracking, proyección directa a DTO y paginación eficiente.
    /// Todos los filtros son opcionales y se aplican mediante composición de IQueryable.
    /// </remarks>
    /// <param name="query">DTO con los filtros y parámetros de paginación.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resultado paginado con reservas filtradas.</returns>
    Task<PagedResultDto<BookingSearchResultDto>> GetBookingsWithFiltersAsync(
        BookingQueryDto query,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene un resumen agregado de reservas para un período.
    /// </summary>
    /// <remarks>
    /// Utiliza GroupBy y agregaciones en servidor para optimizar performance.
    /// </remarks>
    /// <param name="startDate">Fecha de inicio del período.</param>
    /// <param name="endDate">Fecha de fin del período.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Resumen con métricas agregadas.</returns>
    Task<BookingSummaryDto> GetBookingsSummaryAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene resumen de reservas agrupado por sala.
    /// </summary>
    /// <param name="startDate">Fecha de inicio del período.</param>
    /// <param name="endDate">Fecha de fin del período.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de resúmenes por sala.</returns>
    Task<IEnumerable<RoomBookingSummaryDto>> GetBookingsSummaryByRoomAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene resumen de reservas agrupado por organizador.
    /// </summary>
    /// <param name="startDate">Fecha de inicio del período.</param>
    /// <param name="endDate">Fecha de fin del período.</param>
    /// <param name="topN">Número máximo de organizadores a retornar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Lista de resúmenes por organizador.</returns>
    Task<IEnumerable<OrganizerBookingSummaryDto>> GetBookingsSummaryByOrganizerAsync(
        DateTime startDate,
        DateTime endDate,
        int topN = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene estadísticas diarias de reservas.
    /// </summary>
    /// <param name="startDate">Fecha de inicio.</param>
    /// <param name="endDate">Fecha de fin.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Estadísticas por día.</returns>
    Task<IEnumerable<DailyBookingStatsDto>> GetDailyStatsAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Busca reservas usando una expresión lambda personalizada con proyección.
    /// </summary>
    /// <typeparam name="TResult">Tipo del resultado proyectado.</typeparam>
    /// <param name="predicate">Expresión de filtro.</param>
    /// <param name="selector">Expresión de proyección.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Colección de resultados proyectados.</returns>
    Task<IEnumerable<TResult>> QueryWithProjectionAsync<TResult>(
        Expression<Func<Booking, bool>> predicate,
        Expression<Func<Booking, TResult>> selector,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Ejecuta una consulta compilada para obtener reservas activas de una sala.
    /// </summary>
    /// <remarks>
    /// Utiliza EF.CompileAsyncQuery para máxima performance en consultas repetitivas.
    /// </remarks>
    /// <param name="roomId">ID de la sala.</param>
    /// <param name="fromDate">Fecha desde la cual buscar.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>Reservas activas de la sala.</returns>
    Task<IEnumerable<Booking>> GetActiveBookingsForRoomCompiledAsync(
        int roomId,
        DateTime fromDate,
        CancellationToken cancellationToken = default);

    #endregion
}

