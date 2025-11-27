using FluentValidation;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Validators
{
    /// <summary>
    /// Validator for GetTicketsQueryParameters
    /// </summary>
    public class GetTicketsQueryParametersValidator : AbstractValidator<GetTicketsQueryParameters>
    {
        public GetTicketsQueryParametersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be greater than or equal to 1");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be greater than or equal to 1")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must not exceed 100");

            RuleFor(x => x.Status)
                .Must(BeAValidStatus)
                .When(x => !string.IsNullOrEmpty(x.Status))
                .WithMessage("Status must be a valid value: Open, InProgress, Resolved, Closed");

            RuleFor(x => x.Priority)
                .Must(BeAValidPriority)
                .When(x => !string.IsNullOrEmpty(x.Priority))
                .WithMessage("Priority must be a valid value: Low, Medium, High, Critical");

            RuleFor(x => x.SortBy)
                .Must(BeAValidSortBy)
                .When(x => !string.IsNullOrEmpty(x.SortBy))
                .WithMessage("SortBy must be one of: createdAt, updatedAt, priority");

            RuleFor(x => x.SortOrder)
                .Must(BeAValidSortOrder)
                .When(x => !string.IsNullOrEmpty(x.SortOrder))
                .WithMessage("SortOrder must be 'asc' or 'desc'");
        }

        private bool BeAValidStatus(string? status)
        {
            return Enum.TryParse<Status>(status, true, out _);
        }

        private bool BeAValidPriority(string? priority)
        {
            return Enum.TryParse<Priority>(priority, true, out _);
        }

        private bool BeAValidSortBy(string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "createdat" or "updatedat" or "priority" => true,
                _ => false
            };
        }

        private bool BeAValidSortOrder(string? sortOrder)
        {
            return sortOrder?.ToLower() switch
            {
                "asc" or "desc" => true,
                _ => false
            };
        }
    }
}