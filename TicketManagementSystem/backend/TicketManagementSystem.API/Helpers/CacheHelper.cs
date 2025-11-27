using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TicketManagementSystem.API.Helpers
{
    /// <summary>
    /// Helper class for cache operations
    /// </summary>
    public class CacheHelper : ICacheHelper
    {
        private readonly IDistributedCache _cache;

        public CacheHelper(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Gets or sets a cached value with JSON serialization
        /// </summary>
        public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            var cached = await _cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cached))
            {
                return JsonSerializer.Deserialize<T>(cached);
            }

            var value = await factory();
            await SetAsync(key, value, expiration);
            return value;
        }

        /// <summary>
        /// Sets a cached value with JSON serialization
        /// </summary>
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var serialized = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions();

            if (expiration.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiration.Value;
            }

            await _cache.SetStringAsync(key, serialized, options);
        }

        /// <summary>
        /// Removes a cached value
        /// </summary>
        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        /// <summary>
        /// Invalidates cache keys matching a pattern
        /// </summary>
        public async Task InvalidatePatternAsync(string pattern)
        {
            // Note: This is a simplified implementation
            // In production, consider using Redis SCAN or cache tags
            await _cache.RemoveAsync(pattern);
        }
    }
}