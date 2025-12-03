using AutoMapper;
using BookingSystemAPI.Api.Common.Results;
using BookingSystemAPI.Api.DTOs.Bookings;
using BookingSystemAPI.Api.Events;
using BookingSystemAPI.Api.Models;
using BookingSystemAPI.Api.Repositories;

namespace BookingSystemAPI.Api.Services;

/// <summary>
/// Implementación del servicio de reservas con validaciones de negocio.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRepository<Room> _roomRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<BookingService> _logger;
    private readonly IEventPublisher _eventPublisher;

    /// <summary>
    /// Errores de dominio específicos de reservas.
    /// </summary>
    private static class BookingErrors
    {
        public static Error RoomNotFound(int roomId) =>
            Error.NotFound("Room", roomId);

        public static Error BookingNotFound(int bookingId) =>
            Error.NotFound("Booking", bookingId);

        public static Error RoomInMaintenance(string roomName) =>
            Error.BusinessRule("RoomMaintenance", $"La sala '{roomName}' está en mantenimiento y no está disponible para reservas.");

        public static Error ScheduleConflict() =>
            Error.Conflict("Ya existe una reserva en la sala para el horario seleccionado.");

        public static Error CapacityExceeded(int roomCapacity, int attendeeCount) =>
            Error.BusinessRule("CapacityExceeded", $"La sala tiene capacidad para {roomCapacity} personas, pero se solicitaron {attendeeCount}.");

        public static Error BookingInPast() =>
            Error.BusinessRule("BookingInPast", "No se puede crear una reserva en el pasado.");

        public static Error InvalidTimeRange() =>
            Error.BusinessRule("InvalidTimeRange", "La hora de inicio debe ser anterior a la hora de fin.");

        public static Error OutsideBusinessHours() =>
            Error.BusinessRule("OutsideBusinessHours", "Las reservas solo están permitidas entre 8:00 y 20:00 horas.");

        public static Error DurationTooShort() =>
            Error.BusinessRule("DurationTooShort", "La duración mínima de una reserva es de 15 minutos.");

        public static Error DurationTooLong() =>
            Error.BusinessRule("DurationTooLong", "La duración máxima de una reserva es de 8 horas.");

        public static Error CannotModifyCancelledBooking() =>
            Error.BusinessRule("CannotModifyCancelled", "No se puede modificar una reserva cancelada.");

        public static Error CannotModifyPastBooking() =>
            Error.BusinessRule("CannotModifyPast", "No se puede modificar una reserva que ya ha comenzado.");

        public static Error CannotCancelPastBooking() =>
            Error.BusinessRule("CannotCancelPast", "No se puede cancelar una reserva que ya ha comenzado.");
    }

    /// <summary>
    /// Inicializa una nueva instancia del servicio de reservas.
    /// </summary>
    public BookingService(
        IBookingRepository bookingRepository,
        IRepository<Room> roomRepository,
        IMapper mapper,
        ILogger<BookingService> logger,
        IEventPublisher eventPublisher)
    {
        _bookingRepository = bookingRepository;
        _roomRepository = roomRepository;
        _mapper = mapper;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    /// <inheritdoc/>
    public async Task<Result<IEnumerable<BookingDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo todas las reservas");

        var bookings = await _bookingRepository.GetAllWithRoomAsync(cancellationToken);
        var dtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);

        return Result<IEnumerable<BookingDto>>.Success(dtos);
    }

    /// <inheritdoc/>
    public async Task<Result<BookingDto>> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo reserva con ID: {BookingId}", id);

        var booking = await _bookingRepository.GetByIdWithRoomAsync(id, cancellationToken);

        if (booking is null)
        {
            _logger.LogWarning("Reserva no encontrada: {BookingId}", id);
            return Result<BookingDto>.NotFound("Booking", id);
        }

        return Result<BookingDto>.Success(_mapper.Map<BookingDto>(booking));
    }

    /// <inheritdoc/>
    public async Task<Result<BookingDto>> CreateAsync(CreateBookingDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creando nueva reserva para sala {RoomId}", dto.RoomId);

        // Validar reglas de negocio básicas
        var timeValidation = ValidateTimeRules(dto.StartTime, dto.EndTime);
        if (timeValidation.IsFailure)
        {
            return Result<BookingDto>.Failure(timeValidation.Error!);
        }

        // Verificar que la sala existe
        var room = await _roomRepository.GetByIdAsync(dto.RoomId, cancellationToken);
        if (room is null)
        {
            _logger.LogWarning("Sala no encontrada: {RoomId}", dto.RoomId);
            return Result<BookingDto>.Failure(BookingErrors.RoomNotFound(dto.RoomId));
        }

        // Verificar que la sala no está en mantenimiento
        if (room.Status == RoomStatus.Maintenance)
        {
            _logger.LogWarning("Sala en mantenimiento: {RoomId} - {RoomName}", room.Id, room.Name);
            return Result<BookingDto>.Failure(BookingErrors.RoomInMaintenance(room.Name));
        }

        // Verificar capacidad
        if (dto.AttendeeCount > room.Capacity)
        {
            _logger.LogWarning("Capacidad excedida. Sala: {Capacity}, Solicitado: {Attendees}",
                room.Capacity, dto.AttendeeCount);
            return Result<BookingDto>.Failure(BookingErrors.CapacityExceeded(room.Capacity, dto.AttendeeCount));
        }

        // Verificar conflictos de horario
        var hasConflict = await _bookingRepository.HasScheduleConflictAsync(
            dto.RoomId, dto.StartTime, dto.EndTime, cancellationToken: cancellationToken);

        if (hasConflict)
        {
            _logger.LogWarning("Conflicto de horario detectado para sala {RoomId}", dto.RoomId);
            return Result<BookingDto>.Failure(BookingErrors.ScheduleConflict());
        }

        // Crear la reserva
        var booking = _mapper.Map<Booking>(dto);
        booking.Status = BookingStatus.Confirmed;

        await _bookingRepository.AddAsync(booking, cancellationToken);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        // Recargar con la información de la sala
        booking = await _bookingRepository.GetByIdWithRoomAsync(booking.Id, cancellationToken);

        _logger.LogInformation("Reserva creada exitosamente: {BookingId}", booking!.Id);

        // Publicar evento de reserva creada
        await PublishBookingCreatedEventAsync(booking, cancellationToken);

        return Result<BookingDto>.Success(_mapper.Map<BookingDto>(booking));
    }

    /// <inheritdoc/>
    public async Task<Result<BookingDto>> UpdateAsync(int id, UpdateBookingDto dto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Actualizando reserva {BookingId}", id);

        var booking = await _bookingRepository.GetByIdWithRoomAsync(id, cancellationToken);

        if (booking is null)
        {
            return Result<BookingDto>.NotFound("Booking", id);
        }

        // Verificar que no está cancelada
        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result<BookingDto>.Failure(BookingErrors.CannotModifyCancelledBooking());
        }

        // Verificar que no ha comenzado
        if (booking.StartTime <= DateTime.UtcNow)
        {
            return Result<BookingDto>.Failure(BookingErrors.CannotModifyPastBooking());
        }

        // Validar reglas de tiempo
        var timeValidation = ValidateTimeRules(dto.StartTime, dto.EndTime);
        if (timeValidation.IsFailure)
        {
            return Result<BookingDto>.Failure(timeValidation.Error!);
        }

        // Verificar capacidad si cambia el número de asistentes
        if (dto.AttendeeCount > booking.Room.Capacity)
        {
            return Result<BookingDto>.Failure(BookingErrors.CapacityExceeded(booking.Room.Capacity, dto.AttendeeCount));
        }

        // Verificar conflictos de horario (excluyendo esta reserva)
        var hasConflict = await _bookingRepository.HasScheduleConflictAsync(
            booking.RoomId, dto.StartTime, dto.EndTime, id, cancellationToken);

        if (hasConflict)
        {
            return Result<BookingDto>.Failure(BookingErrors.ScheduleConflict());
        }

        // Actualizar propiedades
        booking.Title = dto.Title;
        booking.Description = dto.Description;
        booking.StartTime = dto.StartTime;
        booking.EndTime = dto.EndTime;
        booking.AttendeeCount = dto.AttendeeCount;
        booking.Notes = dto.Notes;

        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Reserva actualizada exitosamente: {BookingId}", id);

        return Result<BookingDto>.Success(_mapper.Map<BookingDto>(booking));
    }

    /// <inheritdoc/>
    public async Task<Result> CancelAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Cancelando reserva {BookingId}", id);

        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking is null)
        {
            return Result.Failure(BookingErrors.BookingNotFound(id));
        }

        // No se puede cancelar una reserva que ya empezó
        if (booking.StartTime <= DateTime.UtcNow)
        {
            return Result.Failure(BookingErrors.CannotCancelPastBooking());
        }

        // No se puede cancelar una reserva ya cancelada
        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure(BookingErrors.CannotModifyCancelledBooking());
        }

        booking.Status = BookingStatus.Cancelled;
        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Reserva cancelada exitosamente: {BookingId}", id);

        // Publicar evento de reserva cancelada
        await PublishBookingCancelledEventAsync(booking, cancellationToken);

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Eliminando reserva {BookingId} (soft delete)", id);

        var booking = await _bookingRepository.GetByIdAsync(id, cancellationToken);

        if (booking is null)
        {
            return Result.Failure(BookingErrors.BookingNotFound(id));
        }

        // Soft delete
        booking.IsDeleted = true;
        booking.DeletedAt = DateTime.UtcNow;

        _bookingRepository.Update(booking);
        await _bookingRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Reserva eliminada exitosamente: {BookingId}", id);

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result<IEnumerable<BookingDto>>> GetByRoomAndDateRangeAsync(
        int roomId,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Obteniendo reservas de sala {RoomId} entre {Start} y {End}",
            roomId, startDate, endDate);

        var bookings = await _bookingRepository.GetByRoomAndDateRangeAsync(
            roomId, startDate, endDate, cancellationToken);

        return Result<IEnumerable<BookingDto>>.Success(_mapper.Map<IEnumerable<BookingDto>>(bookings));
    }

    /// <summary>
    /// Valida las reglas de tiempo para una reserva.
    /// </summary>
    private static Result ValidateTimeRules(DateTime startTime, DateTime endTime)
    {
        var minTime = new TimeSpan(8, 0, 0);
        var maxTime = new TimeSpan(20, 0, 0);
        var minDuration = TimeSpan.FromMinutes(15);
        var maxDuration = TimeSpan.FromHours(8);

        // Verificar que no sea en el pasado
        if (startTime <= DateTime.UtcNow)
        {
            return Result.Failure(BookingErrors.BookingInPast());
        }

        // Verificar que el rango es válido
        if (startTime >= endTime)
        {
            return Result.Failure(BookingErrors.InvalidTimeRange());
        }

        // Verificar horario permitido
        if (startTime.TimeOfDay < minTime || startTime.TimeOfDay > maxTime ||
            endTime.TimeOfDay < minTime || endTime.TimeOfDay > maxTime)
        {
            return Result.Failure(BookingErrors.OutsideBusinessHours());
        }

        // Verificar duración mínima
        var duration = endTime - startTime;
        if (duration < minDuration)
        {
            return Result.Failure(BookingErrors.DurationTooShort());
        }

        // Verificar duración máxima
        if (duration > maxDuration)
        {
            return Result.Failure(BookingErrors.DurationTooLong());
        }

        return Result.Success();
    }

    /// <summary>
    /// Publica el evento de reserva creada.
    /// </summary>
    private async Task PublishBookingCreatedEventAsync(Booking booking, CancellationToken cancellationToken)
    {
        try
        {
            var @event = new BookingCreatedEvent
            {
                BookingId = booking.Id,
                Title = booking.Title,
                Description = booking.Description,
                RoomId = booking.RoomId,
                RoomName = booking.Room?.Name ?? string.Empty,
                UserId = 0, // TODO: Agregar propiedad UserId al modelo Booking cuando se implemente
                UserEmail = string.Empty, // TODO: Obtener del usuario cuando esté disponible
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                AttendeeCount = booking.AttendeeCount,
                Status = booking.Status.ToString()
            };

            await _eventPublisher.PublishAsync(@event, cancellationToken: cancellationToken);

            // Programar recordatorio 30 minutos antes
            var reminderTime = booking.StartTime.AddMinutes(-30);
            if (reminderTime > DateTime.UtcNow)
            {
                var reminderEvent = new BookingReminderEvent
                {
                    BookingId = booking.Id,
                    Title = booking.Title,
                    Description = booking.Description,
                    RoomId = booking.RoomId,
                    RoomName = booking.Room?.Name ?? string.Empty,
                    RoomLocation = booking.Room?.Location,
                    UserId = 0, // TODO: Agregar propiedad UserId al modelo Booking cuando se implemente
                    UserEmail = string.Empty,
                    UserName = string.Empty,
                    StartTime = booking.StartTime,
                    EndTime = booking.EndTime,
                    MinutesBefore = 30,
                    ReminderType = ReminderType.Email
                };

                await _eventPublisher.SchedulePublishAsync(reminderEvent, reminderTime, cancellationToken);
                _logger.LogInformation("Recordatorio programado para reserva {BookingId} a las {ReminderTime}", 
                    booking.Id, reminderTime);
            }
        }
        catch (Exception ex)
        {
            // No fallar la operación principal si no se puede publicar el evento
            _logger.LogWarning(ex, "No se pudo publicar evento BookingCreatedEvent para reserva {BookingId}", booking.Id);
        }
    }

    /// <summary>
    /// Publica el evento de reserva cancelada.
    /// </summary>
    private async Task PublishBookingCancelledEventAsync(Booking booking, CancellationToken cancellationToken, string? reason = null)
    {
        try
        {
            var @event = new BookingCancelledEvent
            {
                BookingId = booking.Id,
                Title = booking.Title,
                RoomId = booking.RoomId,
                RoomName = booking.Room?.Name ?? string.Empty,
                UserId = 0, // TODO: Agregar propiedad UserId al modelo Booking cuando se implemente
                UserEmail = string.Empty, // TODO: Obtener del usuario cuando esté disponible
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                CancellationReason = reason
            };

            await _eventPublisher.PublishAsync(@event, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            // No fallar la operación principal si no se puede publicar el evento
            _logger.LogWarning(ex, "No se pudo publicar evento BookingCancelledEvent para reserva {BookingId}", booking.Id);
        }
    }
}
