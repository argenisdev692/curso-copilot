using System.Collections.Generic;

namespace TicketManagementSystem.API.DTOs
{
    /// <summary>
    /// Generic response wrapper for paginated data
    /// </summary>
    /// <typeparam name="T">The type of items in the response</typeparam>
    public class PagedResponse<T>
    {
        /// <summary>
        /// List of items for the current page
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Total number of items in the database
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Current page number
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items per page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages calculated with Math.Ceiling
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Indicates if there is a next page available
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Indicates if there is a previous page available
        /// </summary>
        public bool HasPreviousPage { get; set; }
    }
}