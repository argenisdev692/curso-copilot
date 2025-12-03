using BookingSystemAPI.Api.DTOs.Bookings;
using FluentValidation;

namespace BookingSystemAPI.Api.Validators;

/// <summary>
/// Validador para CreateBookingDto.
/// </summary>
public class CreateBookingValidator : AbstractValidator<CreateBookingDto>
{
    /// <summary>
    /// Hora mínima permitida para reservas (8:00 AM).
    /// </summary>
    private static readonly TimeSpan MinAllowedTime = new(8, 0, 0);

    /// <summary>
    /// Hora máxima permitida para reservas (8:00 PM).
    /// </summary>
    private static readonly TimeSpan MaxAllowedTime = new(20, 0, 0);

    /// <summary>
    /// Duración mínima de una reserva (15 minutos).
    /// </summary>
    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(15);

    /// <summary>
    /// Duración máxima de una reserva (8 horas).
    /// </summary>
    private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(8);

    /// <summary>
    /// Inicializa las reglas de validación.
    /// </summary>
    public CreateBookingValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
                .WithMessage("El título es requerido.")
            .MaximumLength(200)
                .WithMessage("El título no puede exceder 200 caracteres.");

        RuleFor(x => x.Description)
            .MaximumLength(1000)
                .WithMessage("La descripción no puede exceder 1000 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.StartTime)
            .NotEmpty()
                .WithMessage("La fecha de inicio es requerida.")
            .Must(BeInTheFuture)
                .WithMessage("La fecha de inicio debe ser en el futuro.")
            .Must(BeWithinAllowedHours)
                .WithMessage($"La hora de inicio debe estar entre {MinAllowedTime:hh\\:mm} y {MaxAllowedTime:hh\\:mm}.");

        RuleFor(x => x.EndTime)
            .NotEmpty()
                .WithMessage("La fecha de fin es requerida.")
            .GreaterThan(x => x.StartTime)
                .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.")
            .Must(BeWithinAllowedHours)
                .WithMessage($"La hora de fin debe estar entre {MinAllowedTime:hh\\:mm} y {MaxAllowedTime:hh\\:mm}.");

        RuleFor(x => x)
            .Must(HaveValidDuration)
                .WithMessage($"La duración debe estar entre {MinDuration.TotalMinutes} minutos y {MaxDuration.TotalHours} horas.")
            .Must(EndOnSameDay)
                .WithMessage("La reserva debe iniciar y terminar el mismo día.");

        RuleFor(x => x.RoomId)
            .GreaterThan(0)
                .WithMessage("Debe seleccionar una sala válida.");

        RuleFor(x => x.OrganizerName)
            .NotEmpty()
                .WithMessage("El nombre del organizador es requerido.")
            .MaximumLength(100)
                .WithMessage("El nombre del organizador no puede exceder 100 caracteres.");

        RuleFor(x => x.OrganizerEmail)
            .NotEmpty()
                .WithMessage("El email del organizador es requerido.")
            .EmailAddress()
                .WithMessage("El email no tiene un formato válido.")
            .MaximumLength(150)
                .WithMessage("El email no puede exceder 150 caracteres.");

        RuleFor(x => x.AttendeeCount)
            .GreaterThan(0)
                .WithMessage("El número de asistentes debe ser mayor a 0.")
            .LessThanOrEqualTo(500)
                .WithMessage("El número de asistentes no puede exceder 500.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
                .WithMessage("Las notas no pueden exceder 500 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }

    /// <summary>
    /// Valida que la fecha sea en el futuro.
    /// </summary>
    private static bool BeInTheFuture(DateTime dateTime)
    {
        return dateTime > DateTime.UtcNow;
    }

    /// <summary>
    /// Valida que la hora esté dentro del horario permitido.
    /// </summary>
    private static bool BeWithinAllowedHours(DateTime dateTime)
    {
        var time = dateTime.TimeOfDay;
        return time >= MinAllowedTime && time <= MaxAllowedTime;
    }

    /// <summary>
    /// Valida que la duración esté dentro de los límites.
    /// </summary>
    private static bool HaveValidDuration(CreateBookingDto dto)
    {
        var duration = dto.EndTime - dto.StartTime;
        return duration >= MinDuration && duration <= MaxDuration;
    }

    /// <summary>
    /// Valida que la reserva inicie y termine el mismo día.
    /// </summary>
    private static bool EndOnSameDay(CreateBookingDto dto)
    {
        return dto.StartTime.Date == dto.EndTime.Date;
    }
}

/// <summary>
/// Validador para UpdateBookingDto.
/// </summary>
public class UpdateBookingValidator : AbstractValidator<UpdateBookingDto>
{
    private static readonly TimeSpan MinAllowedTime = new(8, 0, 0);
    private static readonly TimeSpan MaxAllowedTime = new(20, 0, 0);
    private static readonly TimeSpan MinDuration = TimeSpan.FromMinutes(15);
    private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(8);

    /// <summary>
    /// Inicializa las reglas de validación para actualización.
    /// </summary>
    public UpdateBookingValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
                .WithMessage("El título es requerido.")
            .MaximumLength(200)
                .WithMessage("El título no puede exceder 200 caracteres.");

        RuleFor(x => x.StartTime)
            .NotEmpty()
                .WithMessage("La fecha de inicio es requerida.")
            .Must(BeInTheFuture)
                .WithMessage("La fecha de inicio debe ser en el futuro.")
            .Must(BeWithinAllowedHours)
                .WithMessage($"La hora de inicio debe estar entre {MinAllowedTime:hh\\:mm} y {MaxAllowedTime:hh\\:mm}.");

        RuleFor(x => x.EndTime)
            .NotEmpty()
                .WithMessage("La fecha de fin es requerida.")
            .GreaterThan(x => x.StartTime)
                .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.")
            .Must(BeWithinAllowedHours)
                .WithMessage($"La hora de fin debe estar entre {MinAllowedTime:hh\\:mm} y {MaxAllowedTime:hh\\:mm}.");

        RuleFor(x => x)
            .Must(HaveValidDuration)
                .WithMessage($"La duración debe estar entre {MinDuration.TotalMinutes} minutos y {MaxDuration.TotalHours} horas.")
            .Must(EndOnSameDay)
                .WithMessage("La reserva debe iniciar y terminar el mismo día.");

        RuleFor(x => x.AttendeeCount)
            .GreaterThan(0)
                .WithMessage("El número de asistentes debe ser mayor a 0.");
    }

    private static bool BeInTheFuture(DateTime dateTime) => dateTime > DateTime.UtcNow;

    private static bool BeWithinAllowedHours(DateTime dateTime)
    {
        var time = dateTime.TimeOfDay;
        return time >= MinAllowedTime && time <= MaxAllowedTime;
    }

    private static bool HaveValidDuration(UpdateBookingDto dto)
    {
        var duration = dto.EndTime - dto.StartTime;
        return duration >= MinDuration && duration <= MaxDuration;
    }

    private static bool EndOnSameDay(UpdateBookingDto dto) =>
        dto.StartTime.Date == dto.EndTime.Date;
}
