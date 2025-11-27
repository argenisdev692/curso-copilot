using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Business rules and validation for tickets
    /// </summary>
    public class TicketBusinessRules : BaseService, ITicketBusinessRules
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public TicketBusinessRules(
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            ILogger<TicketBusinessRules> logger) : base(logger)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task ValidateTicketCreationAsync(CreateTicketDto dto, int createdByUserId, CancellationToken ct)
        {
            // Validate creator exists and is active
            var creator = await _userRepository.GetByIdAsync(createdByUserId);
            if (creator == null || !creator.IsActive)
            {
                throw new InvalidOperationException("Ticket creator must be an active user");
            }

            // Validate creator has permission to create tickets (all active users can create)
            if (!creator.IsActive)
            {
                throw new UnauthorizedAccessException("User does not have permission to create tickets");
            }

            // If assigned user is specified, validate they exist and are agents
            if (dto.AssignedToId.HasValue)
            {
                var assignedUser = await _userRepository.GetByIdAsync(dto.AssignedToId.Value);
                if (assignedUser == null || !assignedUser.IsActive)
                {
                    throw new InvalidOperationException("Assigned user must be active");
                }

                if (assignedUser.Role != "Agent" && assignedUser.Role != "Admin")
                {
                    throw new InvalidOperationException("Tickets can only be assigned to Agents or Admins");
                }
            }

            LogInformation("Ticket creation validation passed for user {UserId}", createdByUserId);
        }

        /// <inheritdoc />
        public async Task ValidateTicketUpdateAsync(int ticketId, UpdateTicketDto dto, CancellationToken ct)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId, false, ct);
            if (ticket == null)
            {
                throw new KeyNotFoundException($"Ticket with ID {ticketId} not found");
            }

            // Validate status transition if status is being changed
            if (dto.Status.HasValue && dto.Status.Value != ticket.Status)
            {
                if (!IsValidStatusTransition(ticket.Status, dto.Status.Value))
                {
                    throw new InvalidOperationException(
                        $"Invalid status transition from {ticket.Status} to {dto.Status.Value}");
                }
            }

            // Validate assigned user if being changed
            if (dto.AssignedToId.HasValue)
            {
                var assignedUser = await _userRepository.GetByIdAsync(dto.AssignedToId.Value);
                if (assignedUser == null || !assignedUser.IsActive)
                {
                    throw new InvalidOperationException("Assigned user must be active");
                }

                if (assignedUser.Role != "Agent" && assignedUser.Role != "Admin")
                {
                    throw new InvalidOperationException("Tickets can only be assigned to Agents or Admins");
                }
            }

            LogInformation("Ticket update validation passed for ticket {TicketId}", ticketId);
        }

        /// <inheritdoc />
        public bool IsValidStatusTransition(Status currentStatus, Status newStatus)
        {
            return (currentStatus, newStatus) switch
            {
                (Status.Open, Status.InProgress) => true,
                (Status.Open, Status.Closed) => true,
                (Status.InProgress, Status.Resolved) => true,
                (Status.InProgress, Status.Open) => true,
                (Status.Resolved, Status.Closed) => true,
                (Status.Resolved, Status.InProgress) => true,
                _ => false
            };
        }

        /// <inheritdoc />
        public async Task CreateTicketHistoryAsync(Ticket ticket, string changeDescription, int changedByUserId, CancellationToken ct)
        {
            var history = new TicketHistory
            {
                TicketId = ticket.Id,
                ChangedById = changedByUserId,
                ChangedAt = DateTime.UtcNow,
                OldStatus = ticket.Status, // This would be set before the change
                NewStatus = ticket.Status,
                OldPriority = ticket.Priority, // This would be set before the change
                NewPriority = ticket.Priority,
                OldAssignedToId = ticket.AssignedToId, // This would be set before the change
                NewAssignedToId = ticket.AssignedToId,
                ChangeDescription = changeDescription
            };

            // Note: This would need to be saved via repository
            // For now, just log the history creation
            LogInformation("Ticket history created for ticket {TicketId}: {Description}",
                ticket.Id, changeDescription);

            await Task.CompletedTask;
        }
    }
}