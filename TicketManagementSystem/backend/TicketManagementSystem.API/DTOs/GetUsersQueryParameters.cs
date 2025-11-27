namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Query parameters for GET /api/users endpoint
    /// </summary>
    public class GetUsersQueryParameters
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
        /// Filter by role: Admin, Agent, User
        /// </summary>
        public string? Role { get; set; }

        /// <summary>
        /// Filter by active status
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Search in email and fullName
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Sort by: createdAt, updatedAt, email, fullName (default createdAt)
        /// </summary>
        public string? SortBy { get; set; } = "createdAt";

        /// <summary>
        /// Sort order: asc or desc (default desc)
        /// </summary>
        public string? SortOrder { get; set; } = "desc";
    }
}