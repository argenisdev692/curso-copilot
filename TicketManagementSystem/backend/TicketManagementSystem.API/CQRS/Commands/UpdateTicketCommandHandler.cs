using MediatR;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Commands
{
    /// <summary>
    /// Handler for UpdateTicketCommand
    /// </summary>
    public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, Result<Ticket>>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public UpdateTicketCommandHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result<Ticket>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketService.UpdateAsync(request.Id, request.Dto, cancellationToken);
        }
    }
}