using MediatR;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Commands
{
    /// <summary>
    /// Handler for DeleteTicketCommand
    /// </summary>
    public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand, Result>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public DeleteTicketCommandHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketService.DeleteAsync(request.Id, cancellationToken);
        }
    }
}