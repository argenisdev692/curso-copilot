using FluentValidation;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Validators
{
    /// <summary>
    /// Validator for CreateTicketDto
    /// </summary>
    public class CreateTicketDtoValidator : AbstractValidator<CreateTicketDto>
    {
        public CreateTicketDtoValidator(IUserRepository userRepository)
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MinimumLength(5).WithMessage("Title must be at least 5 characters long")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters")
                .Matches(@"^[^<>&]*$").WithMessage("Title cannot contain HTML characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MinimumLength(10).WithMessage("Description must be at least 10 characters long")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .Matches(@"^[^<>&]*$").WithMessage("Description cannot contain HTML characters");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value");

            RuleFor(x => x.AssignedToId)
                .MustAsync(async (assignedToId, cancellation) =>
                {
                    if (!assignedToId.HasValue) return true; // Optional field
                    var user = await userRepository.GetByIdAsync(assignedToId.Value);
                    return user != null && user.IsActive && user.Role == "Agent";
                })
                .WithMessage("Assigned user must be an active Agent");
        }
    }
}