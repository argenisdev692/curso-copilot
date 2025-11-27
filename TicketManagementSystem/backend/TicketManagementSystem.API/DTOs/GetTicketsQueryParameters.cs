namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Query parameters for GET /api/tickets endpoint
    /// </summary>
    public class GetTicketsQueryParameters
    {
        /// <summary>
        /// Page number (default 1, minimum 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Items per page (default 10, minimum 1, maximum 100)
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Filter by status: Open, InProgress, Resolved, Closed
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Filter by priority ID
        /// </summary>
        public int? PriorityId { get; set; }

        /// <summary>
        /// Filter by assigned user ID
        /// </summary>
        public int? AssignedTo { get; set; }

        /// <summary>
        /// Search in title and description
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Sort by: createdAt, updatedAt, priority (default createdAt)
        /// </summary>
        public string? SortBy { get; set; } = "createdAt";

        /// <summary>
        /// Sort order: asc or desc (default desc)
        /// </summary>
        public string? SortOrder { get; set; } = "desc";
    }
}