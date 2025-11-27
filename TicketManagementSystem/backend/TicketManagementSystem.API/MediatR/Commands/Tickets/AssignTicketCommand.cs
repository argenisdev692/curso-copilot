using MediatR;
using TicketManagementSystem.API.Models;
using TicketManagementSystem.API.Repositories;
using TicketManagementSystem.API.Services;

namespace TicketManagementSystem.API.MediatR.Commands.Tickets;

/// <summary>
/// Command to assign a ticket to a user
/// </summary>
public class AssignTicketCommand : IRequest<Ticket>
{
    public int TicketId { get; set; }
    public int UserId { get; set; }
    public int AssignedByUserId { get; set; } // For history tracking
}

/// <summary>
/// Handler for AssignTicketCommand
/// </summary>
public class AssignTicketCommandHandler : IRequestHandler<AssignTicketCommand, Ticket>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITicketBusinessRules _businessRules;

    public AssignTicketCommandHandler(IUnitOfWork unitOfWork, ITicketBusinessRules businessRules)
    {
        _unitOfWork = unitOfWork;
        _businessRules = businessRules;
    }

    public async Task<Ticket> Handle(AssignTicketCommand request, CancellationToken cancellationToken)
    {
        // Validate ticket exists
        var ticket = await _unitOfWork.Tickets.GetByIdAsync(request.TicketId);
        if (ticket == null)
        {
            throw new KeyNotFoundException("Ticket not found.");
        }

        if (ticket.Status == Status.Closed)
        {
            throw new InvalidOperationException("Cannot assign a closed ticket.");
        }

        // Validate user exists and is Agent/Admin
        var user = await _unitOfWork.Users.GetByIdAsync(request.UserId);
        if (user == null || !user.IsActive || (user.Role != "Agent" && user.Role != "Admin"))
        {
            throw new InvalidOperationException("Invalid user or insufficient permissions.");
        }

        // Update assignment
        var oldAssignedToId = ticket.AssignedToId;
        ticket.AssignedToId = request.UserId;
        ticket.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.SaveChangesAsync();

        // Create history record
        await _businessRules.CreateTicketHistoryAsync(ticket, $"Ticket assigned to {user.FullName}", request.AssignedByUserId, cancellationToken);

        return ticket;
    }
}