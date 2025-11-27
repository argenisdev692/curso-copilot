using System;
using System.Threading.Tasks;

namespace TicketManagementSystem.API.Helpers
{
    /// <summary>
    /// Interface for cache operations
    /// </summary>
    public interface ICacheHelper
    {
        /// <summary>
        /// Gets or sets a cached value with JSON serialization
        /// </summary>
        Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);

        /// <summary>
        /// Sets a cached value with JSON serialization
        /// </summary>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Removes a cached value
        /// </summary>
        Task RemoveAsync(string key);

        /// <summary>
        /// Invalidates cache keys matching a pattern
        /// </summary>
        Task InvalidatePatternAsync(string pattern);
    }
}