using FluentValidation;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Validators
{
    /// <summary>
    /// Validator for GetUsersQueryParameters
    /// </summary>
    public class GetUsersQueryParametersValidator : AbstractValidator<GetUsersQueryParameters>
    {
        public GetUsersQueryParametersValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Page must be greater than or equal to 1");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageSize must be greater than or equal to 1")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize must not exceed 100");

            RuleFor(x => x.Role)
                .Must(BeAValidRole)
                .When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Role must be a valid value: Admin, Agent, User");

            RuleFor(x => x.SortBy)
                .Must(BeAValidSortBy)
                .When(x => !string.IsNullOrEmpty(x.SortBy))
                .WithMessage("SortBy must be one of: createdAt, updatedAt, email, fullName");

            RuleFor(x => x.SortOrder)
                .Must(BeAValidSortOrder)
                .When(x => !string.IsNullOrEmpty(x.SortOrder))
                .WithMessage("SortOrder must be 'asc' or 'desc'");
        }

        private bool BeAValidRole(string? role)
        {
            return role?.ToLower() switch
            {
                "admin" or "agent" or "user" => true,
                _ => false
            };
        }

        private bool BeAValidSortBy(string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "createdat" or "updatedat" or "email" or "fullname" => true,
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

    /// <summary>
    /// Validator for CreateUserDto
    /// </summary>
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

            RuleFor(x => x.Role)
                .Must(BeAValidRole)
                .WithMessage("Role must be one of: Admin, Agent, User");
        }

        private bool BeAValidRole(string? role)
        {
            return role?.ToLower() switch
            {
                "admin" or "agent" or "user" => true,
                _ => false
            };
        }
    }

    /// <summary>
    /// Validator for UpdateUserDto
    /// </summary>
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("Email must be a valid email address");

            RuleFor(x => x.FullName)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.FullName))
                .WithMessage("Full name must not exceed 100 characters");

            RuleFor(x => x.Role)
                .Must(BeAValidRole)
                .When(x => !string.IsNullOrEmpty(x.Role))
                .WithMessage("Role must be one of: Admin, Agent, User");
        }

        private bool BeAValidRole(string? role)
        {
            return role?.ToLower() switch
            {
                "admin" or "agent" or "user" => true,
                _ => false
            };
        }
    }
}