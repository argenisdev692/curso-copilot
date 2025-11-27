using FluentValidation;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Validators
{
    /// <summary>
    /// Validador para los parámetros de consulta de prioridades.
    /// </summary>
    public class GetPrioritiesQueryParametersValidator : AbstractValidator<GetPrioritiesQueryParameters>
    {
        /// <summary>
        /// Constructor del validador con reglas de validación.
        /// </summary>
        public GetPrioritiesQueryParametersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0)
                .WithMessage("La página debe ser mayor que 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                .WithMessage("El tamaño de página debe estar entre 1 y 100.");

            RuleFor(x => x.Name)
                .MaximumLength(50)
                .WithMessage("El nombre no puede exceder 50 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.SortBy)
                .Must(x => new[] { "name", "level", "createdat" }.Contains(x?.ToLower()))
                .WithMessage("SortBy debe ser uno de: name, level, createdAt.")
                .When(x => !string.IsNullOrWhiteSpace(x.SortBy));

            RuleFor(x => x.SortOrder)
                .Must(x => x == null || x.ToLower() == "asc" || x.ToLower() == "desc")
                .WithMessage("SortOrder debe ser 'asc' o 'desc'.")
                .When(x => !string.IsNullOrWhiteSpace(x.SortOrder));
        }
    }
}