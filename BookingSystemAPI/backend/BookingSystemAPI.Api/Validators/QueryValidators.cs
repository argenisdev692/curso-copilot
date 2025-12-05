using BookingSystemAPI.Api.DTOs.Bookings;
using BookingSystemAPI.Api.DTOs.Rooms;
using FluentValidation;

namespace BookingSystemAPI.Api.Validators;

/// <summary>
/// Validador para el DTO de consulta de reservas.
/// </summary>
public class BookingQueryDtoValidator : AbstractValidator<BookingQueryDto>
{
    /// <summary>
    /// Inicializa las reglas de validación para BookingQueryDto.
    /// </summary>
    public BookingQueryDtoValidator()
    {
        // Validar número de página
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("El número de página debe ser mayor a 0.");

        // Validar tamaño de página
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("El tamaño de página debe estar entre 1 y 100.");

        // Validar rango de fechas
        When(x => x.StartDate.HasValue && x.EndDate.HasValue, () =>
        {
            RuleFor(x => x)
                .Must(x => x.StartDate!.Value <= x.EndDate!.Value)
                .WithMessage("La fecha de inicio debe ser anterior o igual a la fecha de fin.")
                .OverridePropertyName("DateRange");
        });

        // Validar status si se proporciona
        When(x => !string.IsNullOrWhiteSpace(x.Status), () =>
        {
            RuleFor(x => x.Status)
                .Must(BeValidStatus)
                .WithMessage("El estado proporcionado no es válido. Estados válidos: Pending, Confirmed, Cancelled, Completed.");
        });

        // Validar email si se proporciona
        When(x => !string.IsNullOrWhiteSpace(x.OrganizerEmail), () =>
        {
            RuleFor(x => x.OrganizerEmail)
                .EmailAddress()
                .WithMessage("El email del organizador no tiene un formato válido.");
        });

        // Validar dirección de ordenamiento
        RuleFor(x => x.SortDirection)
            .Must(x => x.Equals("asc", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("desc", StringComparison.OrdinalIgnoreCase))
            .WithMessage("La dirección de ordenamiento debe ser 'asc' o 'desc'.");
    }

    /// <summary>
    /// Verifica si el estado es válido.
    /// </summary>
    private static bool BeValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status)) return true;

        return Enum.TryParse<Models.BookingStatus>(status, ignoreCase: true, out _);
    }
}

/// <summary>
/// Validador para el DTO de consulta de disponibilidad de salas.
/// </summary>
public class RoomAvailabilityQueryDtoValidator : AbstractValidator<RoomAvailabilityQueryDto>
{
    /// <summary>
    /// Inicializa las reglas de validación para RoomAvailabilityQueryDto.
    /// </summary>
    public RoomAvailabilityQueryDtoValidator()
    {
        // Fecha de inicio requerida
        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("La fecha de inicio es requerida.")
            .GreaterThan(DateTime.UtcNow.AddMinutes(-5))
            .WithMessage("La fecha de inicio debe ser en el futuro.");

        // Fecha de fin requerida
        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("La fecha de fin es requerida.")
            .GreaterThan(x => x.StartTime)
            .WithMessage("La fecha de fin debe ser posterior a la fecha de inicio.");

        // Duración máxima de 24 horas
        RuleFor(x => x)
            .Must(x => (x.EndTime - x.StartTime).TotalHours <= 24)
            .WithMessage("La duración máxima de búsqueda es de 24 horas.")
            .OverridePropertyName("Duration");

        // Capacidad mínima si se especifica
        When(x => x.MinCapacity.HasValue, () =>
        {
            RuleFor(x => x.MinCapacity)
                .GreaterThan(0)
                .WithMessage("La capacidad mínima debe ser mayor a 0.")
                .LessThanOrEqualTo(500)
                .WithMessage("La capacidad mínima no puede exceder 500 personas.");
        });

        // Ubicación preferida si se especifica
        When(x => !string.IsNullOrWhiteSpace(x.PreferredLocation), () =>
        {
            RuleFor(x => x.PreferredLocation)
                .MaximumLength(200)
                .WithMessage("La ubicación preferida no puede exceder 200 caracteres.");
        });
    }
}
