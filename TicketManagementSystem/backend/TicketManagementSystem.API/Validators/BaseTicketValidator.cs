using FluentValidation;
using System.Text.RegularExpressions;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Validators
{
    /// <summary>
    /// Base validator class providing common validation rules for ticket-related DTOs.
    /// Extends this class for specific DTO validations.
    /// </summary>
    /// <typeparam name="T">The DTO type to validate</typeparam>
    public abstract class BaseTicketValidator<T> : AbstractValidator<T> where T : class
    {
        protected BaseTicketValidator()
        {
        }

        /// <summary>
        /// Common validation rules for ticket titles
        /// </summary>
        protected IRuleBuilderOptions<T, string> ValidateTitle(IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Title is required")
                .Length(5, 200).WithMessage("Title must be between 5 and 200 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_.,!?()]+$").WithMessage("Title contains invalid characters");
        }

        /// <summary>
        /// Common validation rules for ticket descriptions
        /// </summary>
        protected IRuleBuilderOptions<T, string> ValidateDescription(IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");
        }

        /// <summary>
        /// Common validation rules for email addresses
        /// </summary>
        protected IRuleBuilderOptions<T, string> ValidateEmail(IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address")
                .MaximumLength(254).WithMessage("Email cannot exceed 254 characters");
        }

        /// <summary>
        /// Common validation rules for priority IDs
        /// </summary>
        protected IRuleBuilderOptions<T, int?> ValidatePriorityId(IRuleBuilder<T, int?> ruleBuilder)
        {
            return ruleBuilder
                .NotNull().WithMessage("Priority is required")
                .InclusiveBetween(1, 4).WithMessage("Priority must be between 1 and 4");
        }

        /// <summary>
        /// Common validation rules for user IDs (assigned to, created by)
        /// </summary>
        protected IRuleBuilderOptions<T, int?> ValidateUserId(IRuleBuilder<T, int?> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .GreaterThan(0).WithMessage($"{fieldName} must be a valid user ID");
        }

        /// <summary>
        /// Business rule: Ensure assigned user is different from creator (if both provided)
        /// </summary>
        protected void ValidateAssignedUserDifferentFromCreator(int? createdById, int? assignedToId)
        {
            if (createdById.HasValue && assignedToId.HasValue && createdById == assignedToId)
            {
                throw new ValidationException("Assigned user cannot be the same as the creator");
            }
        }

        /// <summary>
        /// Business rule: Validate ticket status transitions
        /// </summary>
        protected IRuleBuilderOptions<T, string> ValidateStatusTransition(IRuleBuilder<T, string> ruleBuilder, string currentStatus)
        {
            return ruleBuilder
                .Must(status => IsValidStatusTransition(currentStatus, status))
                .WithMessage($"Invalid status transition from {currentStatus}");
        }

        private bool IsValidStatusTransition(string currentStatus, string newStatus)
        {
            // Define allowed transitions
            var transitions = new Dictionary<string, string[]>
            {
                { "Open", new[] { "InProgress", "Resolved", "Closed" } },
                { "InProgress", new[] { "Resolved", "Closed" } },
                { "Resolved", new[] { "Closed", "Reopened" } },
                { "Closed", new[] { "Reopened" } },
                { "Reopened", new[] { "InProgress", "Resolved", "Closed" } }
            };

            return transitions.ContainsKey(currentStatus) &&
                   transitions[currentStatus].Contains(newStatus);
        }
    }

    /// <summary>
    /// Example validator for a custom DTO - easily extensible
    /// </summary>
    public class CustomTicketDtoValidator : BaseTicketValidator<CustomTicketDto>
    {
        public CustomTicketDtoValidator() : base()
        {
            // Add custom rules here
            RuleFor(x => x.CustomField)
                .NotEmpty().WithMessage("Custom field is required");

            // Reuse base validations
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .Length(5, 200).WithMessage("Title must be between 5 and 200 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_.,!?()]+$").WithMessage("Title contains invalid characters");
        }
    }

    /// <summary>
    /// Placeholder DTO for custom ticket validation example
    /// </summary>
    public class CustomTicketDto
    {
        public string Title { get; set; } = string.Empty;
        public string CustomField { get; set; } = string.Empty;
    }
}