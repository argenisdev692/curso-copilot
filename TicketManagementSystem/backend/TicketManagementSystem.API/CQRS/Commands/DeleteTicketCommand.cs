using MediatR;
using TicketManagementSystem.API.Helpers;

namespace TicketManagementSystem.API.CQRS.Commands
{
    /// <summary>
    /// Command to delete a ticket
    /// </summary>
    public record DeleteTicketCommand(int Id) : IRequest<Result>;
}