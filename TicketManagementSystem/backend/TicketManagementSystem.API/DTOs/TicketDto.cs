using System;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Data Transfer Object for Ticket entity
    /// </summary>
    public class TicketDto
    {
        /// <summary>
        /// Unique identifier of the ticket
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the ticket
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the ticket
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the ticket
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Priority level of the ticket
        /// </summary>
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// ID of the user who created the ticket
        /// </summary>
        public int CreatedById { get; set; }

        /// <summary>
        /// Name of the user who created the ticket
        /// </summary>
        public string CreatedByName { get; set; } = string.Empty;

        /// <summary>
        /// ID of the assigned user (nullable)
        /// </summary>
        public int? AssignedToId { get; set; }

        /// <summary>
        /// Name of the assigned user (nullable)
        /// </summary>
        public string? AssignedToName { get; set; }

        /// <summary>
        /// Creation timestamp in UTC
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last update timestamp in UTC
        /// </summary>
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Number of comments on the ticket
        /// </summary>
        public int CommentCount { get; set; }
    }
}