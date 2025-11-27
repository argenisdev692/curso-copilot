using TicketManagementSystem.API.DTOs;

namespace TicketManagementSystem.API.Helpers
{
    /// <summary>
    /// Helper class for pagination operations
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Creates a paginated response from a queryable source
        /// </summary>
        public static PagedResponse<T> CreatePagedResponse<T>(
            IEnumerable<T> items,
            int totalItems,
            int page,
            int pageSize)
        {
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return new PagedResponse<T>
            {
                Items = items.ToList(),
                TotalItems = totalItems,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                HasNextPage = page < totalPages,
                HasPreviousPage = page > 1
            };
        }

        /// <summary>
        /// Validates pagination parameters
        /// </summary>
        public static (bool IsValid, string? ErrorMessage) ValidatePagination(int page, int pageSize)
        {
            if (page < 1)
                return (false, "Page must be greater than or equal to 1");

            if (pageSize < 1)
                return (false, "PageSize must be greater than or equal to 1");

            if (pageSize > 100)
                return (false, "PageSize must not exceed 100");

            return (true, null);
        }

        /// <summary>
        /// Applies pagination to an IQueryable
        /// </summary>
        public static IQueryable<T> ApplyPagination<T>(IQueryable<T> query, int page, int pageSize)
        {
            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}