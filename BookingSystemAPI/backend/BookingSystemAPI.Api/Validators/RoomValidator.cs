using BookingSystemAPI.Api.Models;
using FluentValidation;

namespace BookingSystemAPI.Api.Validators;

/// <summary>
/// Validador para la entidad Room.
/// </summary>
public class RoomValidator : AbstractValidator<Room>
{
    /// <summary>
    /// Inicializa las reglas de validación para Room.
    /// </summary>
    public RoomValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
                .WithMessage("El nombre de la sala es requerido.")
            .MaximumLength(100)
                .WithMessage("El nombre no puede exceder 100 caracteres.")
            .Matches(@"^[a-zA-Z0-9\s\-áéíóúÁÉÍÓÚñÑ]+$")
                .WithMessage("El nombre solo puede contener letras, números, espacios y guiones.");

        RuleFor(r => r.Capacity)
            .GreaterThan(0)
                .WithMessage("La capacidad debe ser mayor a 0.")
            .LessThanOrEqualTo(1000)
                .WithMessage("La capacidad no puede exceder 1000 personas.");

        RuleFor(r => r.Equipment)
            .Must(e => e == null || e.Count <= 50)
                .WithMessage("No se pueden especificar más de 50 equipos.");

        RuleForEach(r => r.Equipment)
            .MaximumLength(100)
                .WithMessage("El nombre del equipo no puede exceder 100 caracteres.")
            .When(r => r.Equipment != null && r.Equipment.Any());

        RuleFor(r => r.Location)
            .NotEmpty()
                .WithMessage("La ubicación es requerida.")
            .MaximumLength(200)
                .WithMessage("La ubicación no puede exceder 200 caracteres.");

        RuleFor(r => r.Status)
            .IsInEnum()
                .WithMessage("El estado de la sala no es válido.");
    }
}
