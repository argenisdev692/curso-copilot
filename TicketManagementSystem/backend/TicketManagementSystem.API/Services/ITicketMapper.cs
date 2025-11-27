using TicketManagementSystem.API.DTOs;
using TicketManagementSystem.API.Models;

namespace TicketManagementSystem.API.Services
{
    /// <summary>
    /// Interface for ticket mapping operations
    /// </summary>
    public interface ITicketMapper
    {
        /// <summary>
        /// Maps a ticket to DTO
        /// </summary>
        TicketDto MapToDto(Ticket ticket);

        /// <summary>
        /// Maps multiple tickets to DTOs
        /// </summary>
        IEnumerable<TicketDto> MapToDtos(IEnumerable<Ticket> tickets);

        /// <summary>
        /// Maps create DTO to ticket entity
        /// </summary>
        Ticket MapToEntity(CreateTicketDto dto, int createdByUserId);

        /// <summary>
        /// Applies update DTO to existing ticket
        /// </summary>
        void ApplyUpdate(Ticket ticket, UpdateTicketDto dto);
    }
}