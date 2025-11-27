using System.Threading;
using System.Threading.Tasks;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for ticket business rules and validation
    /// </summary>
    public interface ITicketBusinessRules
    {
        /// <summary>
        /// Validates ticket creation business rules
        /// </summary>
        Task ValidateTicketCreationAsync(CreateTicketDto dto, int createdByUserId, CancellationToken ct);

        /// <summary>
        /// Validates ticket update business rules
        /// </summary>
        Task ValidateTicketUpdateAsync(int ticketId, UpdateTicketDto dto, CancellationToken ct);

        /// <summary>
        /// Validates status transition
        /// </summary>
        bool IsValidStatusTransition(Status currentStatus, Status newStatus);

        /// <summary>
        /// Creates ticket history record
        /// </summary>
        Task CreateTicketHistoryAsync(Ticket ticket, string changeDescription, int changedByUserId, CancellationToken ct);
    }
}