using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.Data;
using BookingSystemAPI.Api.DTOs.Reports;
using BookingSystemAPI.Api.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Implementación del servicio de reportes de uso de salas.
/// </summary>
public class RoomReportService : IRoomReportService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<RoomReportService> _logger;

    /// <summary>
    /// Límite máximo de filas en el reporte Excel.
    /// </summary>
    private const int MaxReportRows = 10000;

    /// <summary>
    /// Horas de trabajo por día para calcular ocupación (9:00 - 18:00).
    /// </summary>
    private const int WorkHoursPerDay = 9;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de reportes.
    /// </summary>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="logger">Logger para registro de operaciones.</param>
    public RoomReportService(ApplicationDbContext context, ILogger<RoomReportService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<RoomUsageReportResultDto>> GetRoomUsageReportAsync(
        RoomUsageQueryDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Generando reporte de uso de salas. Período: {StartDate} - {EndDate}, RoomId: {RoomId}",
            query.StartDate, query.EndDate, query.RoomId);

        // Validar fechas
        if (query.StartDate > query.EndDate)
        {
            return Error.Validation("DateRange", "La fecha de inicio no puede ser posterior a la fecha de fin.");
        }

        var roomStats = await GetRoomStatsAsync(query, cancellationToken);

        var result = new RoomUsageReportResultDto
        {
            PeriodStart = query.StartDate,
            PeriodEnd = query.EndDate,
            GeneratedAt = DateTime.UtcNow,
            TotalRooms = roomStats.Count,
            RoomStats = roomStats
        };

        _logger.LogInformation("Reporte generado exitosamente. Total salas: {TotalRooms}", roomStats.Count);

        return result;
    }

    /// <inheritdoc />
    public async Task<Result<Stream>> GenerateRoomUsageExcelAsync(
        RoomUsageQueryDto query,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Generando Excel de uso de salas. Período: {StartDate} - {EndDate}",
            query.StartDate, query.EndDate);

        // Obtener datos del reporte
        var reportResult = await GetRoomUsageReportAsync(query, cancellationToken);

        if (reportResult.IsFailure)
        {
            return Result<Stream>.Failure(reportResult.Error!);
        }

        var report = reportResult.Value!;

        // Limitar filas si excede el máximo
        var roomStats = report.RoomStats.Take(MaxReportRows).ToList();

        if (report.RoomStats.Count > MaxReportRows)
        {
            _logger.LogWarning(
                "El reporte excede el límite de {MaxRows} filas. Se truncará a {MaxRows} registros.",
                MaxReportRows, MaxReportRows);
        }

        // Crear workbook de Excel
        var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Uso de Salas");

        // Agregar encabezado del reporte
        AddReportHeader(worksheet, report);

        // Agregar tabla de datos
        AddDataTable(worksheet, roomStats);

        // Ajustar columnas y estilos
        ApplyStyles(worksheet);

        // Escribir a stream
        var stream = new MemoryStream();
        workbook.SaveAs(stream);
        stream.Position = 0;

        _logger.LogInformation("Excel generado exitosamente. Tamaño: {Size} bytes", stream.Length);

        return stream;
    }

    /// <summary>
    /// Obtiene las estadísticas de uso por sala.
    /// </summary>
    private async Task<List<RoomUsageReportDto>> GetRoomStatsAsync(
        RoomUsageQueryDto query,
        CancellationToken cancellationToken)
    {
        // Query base de salas
        var roomsQuery = _context.Rooms
            .AsNoTracking()
            .Where(r => !r.IsDeleted);

        if (query.RoomId.HasValue)
        {
            roomsQuery = roomsQuery.Where(r => r.Id == query.RoomId.Value);
        }

        var rooms = await roomsQuery.ToListAsync(cancellationToken);

        // Query de reservas en el período
        var bookingsQuery = _context.Bookings
            .AsNoTracking()
            .Where(b => !b.IsDeleted)
            .Where(b => b.StartTime >= query.StartDate && b.EndTime <= query.EndDate);

        if (query.RoomId.HasValue)
        {
            bookingsQuery = bookingsQuery.Where(b => b.RoomId == query.RoomId.Value);
        }

        var bookings = await bookingsQuery.ToListAsync(cancellationToken);

        // Calcular días laborales en el período
        var totalWorkingDays = CalculateWorkingDays(query.StartDate, query.EndDate);
        var totalAvailableHours = totalWorkingDays * WorkHoursPerDay;

        // Agrupar estadísticas por sala
        var stats = rooms.Select(room =>
        {
            var roomBookings = bookings.Where(b => b.RoomId == room.Id).ToList();
            var totalHours = roomBookings.Sum(b => (b.EndTime - b.StartTime).TotalHours);
            var topUser = roomBookings
                .GroupBy(b => b.OrganizerEmail)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault() ?? "N/A";

            return new RoomUsageReportDto
            {
                RoomId = room.Id,
                RoomName = room.Name,
                Location = room.Location,
                Capacity = room.Capacity,
                TotalBookings = roomBookings.Count,
                TotalHours = Math.Round(totalHours, 2),
                OccupancyRate = totalAvailableHours > 0
                    ? Math.Round((totalHours / totalAvailableHours) * 100, 2)
                    : 0,
                AverageAttendees = roomBookings.Count > 0
                    ? Math.Round(roomBookings.Average(b => b.AttendeeCount), 2)
                    : 0,
                TopUser = topUser,
                ConfirmedBookings = roomBookings.Count(b => b.Status == BookingStatus.Confirmed),
                CancelledBookings = roomBookings.Count(b => b.Status == BookingStatus.Cancelled),
                CompletedBookings = roomBookings.Count(b => b.Status == BookingStatus.Completed)
            };
        }).OrderByDescending(s => s.TotalBookings).ToList();

        return stats;
    }

    /// <summary>
    /// Calcula los días laborales entre dos fechas (lunes a viernes).
    /// </summary>
    private static int CalculateWorkingDays(DateTime start, DateTime end)
    {
        var days = 0;
        var current = start.Date;

        while (current <= end.Date)
        {
            if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
            {
                days++;
            }
            current = current.AddDays(1);
        }

        return days;
    }

    /// <summary>
    /// Agrega el encabezado del reporte al worksheet.
    /// </summary>
    private static void AddReportHeader(IXLWorksheet worksheet, RoomUsageReportResultDto report)
    {
        // Título
        worksheet.Cell("A1").Value = "Reporte de Uso de Salas";
        worksheet.Cell("A1").Style.Font.Bold = true;
        worksheet.Cell("A1").Style.Font.FontSize = 16;
        worksheet.Range("A1:J1").Merge();

        // Período
        worksheet.Cell("A2").Value = $"Período: {report.PeriodStart:dd/MM/yyyy} - {report.PeriodEnd:dd/MM/yyyy}";
        worksheet.Range("A2:D2").Merge();

        // Fecha de generación
        worksheet.Cell("A3").Value = $"Generado: {report.GeneratedAt:dd/MM/yyyy HH:mm} UTC";
        worksheet.Range("A3:D3").Merge();

        // Total de salas
        worksheet.Cell("A4").Value = $"Total de salas: {report.TotalRooms}";
        worksheet.Range("A4:D4").Merge();
    }

    /// <summary>
    /// Agrega la tabla de datos al worksheet.
    /// </summary>
    private static void AddDataTable(IXLWorksheet worksheet, List<RoomUsageReportDto> roomStats)
    {
        const int headerRow = 6;

        // Encabezados
        var headers = new[]
        {
            "ID Sala", "Nombre", "Ubicación", "Capacidad", "Total Reservas",
            "Horas Totales", "Ocupación (%)", "Prom. Asistentes", "Top Usuario",
            "Confirmadas", "Canceladas", "Completadas"
        };

        for (var i = 0; i < headers.Length; i++)
        {
            var cell = worksheet.Cell(headerRow, i + 1);
            cell.Value = headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.LightBlue;
            cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
        }

        // Datos
        for (var rowIndex = 0; rowIndex < roomStats.Count; rowIndex++)
        {
            var room = roomStats[rowIndex];
            var row = headerRow + rowIndex + 1;

            worksheet.Cell(row, 1).Value = room.RoomId;
            worksheet.Cell(row, 2).Value = room.RoomName;
            worksheet.Cell(row, 3).Value = room.Location;
            worksheet.Cell(row, 4).Value = room.Capacity;
            worksheet.Cell(row, 5).Value = room.TotalBookings;
            worksheet.Cell(row, 6).Value = room.TotalHours;
            worksheet.Cell(row, 7).Value = room.OccupancyRate;
            worksheet.Cell(row, 8).Value = room.AverageAttendees;
            worksheet.Cell(row, 9).Value = room.TopUser;
            worksheet.Cell(row, 10).Value = room.ConfirmedBookings;
            worksheet.Cell(row, 11).Value = room.CancelledBookings;
            worksheet.Cell(row, 12).Value = room.CompletedBookings;

            // Bordes para las celdas de datos
            for (var col = 1; col <= 12; col++)
            {
                worksheet.Cell(row, col).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }
        }

        // Si no hay datos, agregar mensaje
        if (roomStats.Count == 0)
        {
            worksheet.Cell(headerRow + 1, 1).Value = "No hay datos disponibles para el período seleccionado.";
            worksheet.Range(headerRow + 1, 1, headerRow + 1, 12).Merge();
            worksheet.Cell(headerRow + 1, 1).Style.Font.Italic = true;
        }
    }

    /// <summary>
    /// Aplica estilos finales al worksheet.
    /// </summary>
    private static void ApplyStyles(IXLWorksheet worksheet)
    {
        // Ajustar ancho de columnas automáticamente
        worksheet.Columns().AdjustToContents();

        // Establecer ancho mínimo para algunas columnas
        worksheet.Column(2).Width = 25; // Nombre
        worksheet.Column(3).Width = 20; // Ubicación
        worksheet.Column(9).Width = 25; // Top Usuario

        // Formato de números
        worksheet.Column(6).Style.NumberFormat.Format = "#,##0.00"; // Horas
        worksheet.Column(7).Style.NumberFormat.Format = "#,##0.00"; // Ocupación
        worksheet.Column(8).Style.NumberFormat.Format = "#,##0.00"; // Promedio asistentes
    }
}
