using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.CQRS.Commands
{
    /// <summary>
    /// Command to create a new ticket
    /// </summary>
    public record CreateTicketCommand(CreateTicketDto Dto, int CreatedByUserId) : IRequest<Result<Ticket>>;
}