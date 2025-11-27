using FluentValidation;
using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Validators
{
    /// <summary>
    /// Validator for CreateCommentDto
    /// </summary>
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required")
                .MaximumLength(1000).WithMessage("Content cannot exceed 1000 characters");
        }
    }
}