using MediatR;
using TicketManagementSystem.API.Helpers;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.CQRS.Commands
{
    /// <summary>
    /// Handler for CreateTicketCommand
    /// </summary>
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Result<Ticket>>
    {
        private readonly ITicketService _ticketService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ticketService">Ticket service</param>
        public CreateTicketCommandHandler(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        /// <inheritdoc />
        public async Task<Result<Ticket>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            return await _ticketService.CreateAsync(request.Dto, request.CreatedByUserId, cancellationToken);
        }
    }
}