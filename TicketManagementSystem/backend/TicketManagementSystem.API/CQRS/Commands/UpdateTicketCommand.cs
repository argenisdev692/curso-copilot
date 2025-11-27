using MediatR;
using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.CQRS.Commands
{
    /// <summary>
    /// Command to update an existing ticket
    /// </summary>
    public record UpdateTicketCommand(int Id, UpdateTicketDto Dto) : IRequest<Result<Ticket>>;
}