using BookingSystemAPI.Api.DTOs.Auth;
using FluentValidation;

namespace BookingSystemAPI.Api.Validators.Auth;

/// <summary>
/// Validador para el DTO de registro.
/// </summary>
public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .MaximumLength(256).WithMessage("El correo electrónico no puede exceder los 256 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
            .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número.")
            .Matches(@"[\W]").WithMessage("La contraseña debe contener al menos un carácter especial.");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("La confirmación de contraseña es obligatoria.")
            .Equal(x => x.Password).WithMessage("La confirmación de contraseña no coincide con la contraseña.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .MaximumLength(100).WithMessage("El apellido no puede exceder los 100 caracteres.");
    }
}