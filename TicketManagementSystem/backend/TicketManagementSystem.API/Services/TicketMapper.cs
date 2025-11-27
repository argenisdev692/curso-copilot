using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Handles ticket mapping operations
    /// </summary>
    public class TicketMapper : ITicketMapper
    {
        /// <inheritdoc />
        public TicketDto MapToDto(Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Description = ticket.Description,
                Status = ticket.Status.ToString(),
                Priority = ticket.Priority.ToString(),
                CreatedById = ticket.CreatedById,
                CreatedByName = ticket.CreatedBy?.FullName ?? string.Empty,
                AssignedToId = ticket.AssignedToId,
                AssignedToName = ticket.AssignedTo?.FullName,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                CommentCount = ticket.Comments?.Count ?? 0
            };
        }

        /// <inheritdoc />
        public IEnumerable<TicketDto> MapToDtos(IEnumerable<Ticket> tickets)
        {
            return tickets.Select(MapToDto);
        }

        /// <inheritdoc />
        public Ticket MapToEntity(CreateTicketDto dto, int createdByUserId)
        {
            return new Ticket
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = Status.Open, // Default status for new tickets
                Priority = dto.Priority,
                CreatedById = createdByUserId,
                AssignedToId = dto.AssignedToId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        /// <inheritdoc />
        public void ApplyUpdate(Ticket ticket, UpdateTicketDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Title))
                ticket.Title = dto.Title;

            if (!string.IsNullOrWhiteSpace(dto.Description))
                ticket.Description = dto.Description;

            if (dto.Status.HasValue)
                ticket.Status = dto.Status.Value;

            if (dto.Priority.HasValue)
                ticket.Priority = dto.Priority.Value;

            if (dto.AssignedToId.HasValue)
                ticket.AssignedToId = dto.AssignedToId;

            ticket.UpdatedAt = DateTime.UtcNow;
        }
    }
}