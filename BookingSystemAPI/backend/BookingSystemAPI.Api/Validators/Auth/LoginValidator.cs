using BookingSystemAPI.Api.DTOs.Auth;
using FluentValidation;

namespace BookingSystemAPI.Api.Validators.Auth;

/// <summary>
/// Validador para el DTO de login.
/// </summary>
public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.");
    }
}