namespace Xtensions.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EnsureThat;

    /// <summary>
    /// Extension methods for <see cref="ICache"/>.
    /// </summary>
    public static class CacheExtensions
    {
        /// <summary>
        /// Reads the value at the specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>The cached value.</returns>
        public static async Task<T> Read<T>(this ICache cache, string cacheKey)
            where T : class
        {
            EnsureArg.IsNotNull(cache, nameof(cache));
            EnsureArg.IsNotNullOrEmpty(cacheKey, nameof(cacheKey));

            CacheEntry<T> cacheEntry = await cache.ReadEntry<T>(cacheKey).ConfigureAwait(false);

            return cacheEntry?.Value;
        }

        /// <summary>
        /// Writes a value at the specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cache">The cache.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        /// <returns>A <see cref="Task"/> that will complete when the entry is written.</returns>
        public static Task Write<T>(this ICache cache, string cacheKey, T value, DateTime absoluteExpiration)
            where T : class
        {
            EnsureArg.IsNotNull(cache, nameof(cache));
            EnsureArg.IsNotNullOrEmpty(cacheKey, nameof(cacheKey));

            return cache.WriteEntry(cacheKey, new CacheEntry<T>(value, absoluteExpiration));
        }

        /// <summary>
        /// Writes a <see cref="CacheEntry{T}"/> to multiple <see cref="ICache"/> instances.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="caches">The caches.</param>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheEntry">The cache entry.</param>
        /// <returns>A <see cref="Task"/> that will complete when the entry is written to all caches.</returns>
        public static Task WriteEntry<T>(this IEnumerable<ICache> caches, string cacheKey, CacheEntry<T> cacheEntry)
            where T : class
        {
            EnsureArg.IsNotNull(caches, nameof(caches));
            EnsureArg.IsNotNullOrEmpty(cacheKey, nameof(cacheKey));
            EnsureArg.IsNotNull(cacheEntry, nameof(cacheEntry));

            return Task.WhenAll(caches.Select(cache => cache.WriteEntry(cacheKey, cacheEntry)));
        }
    }
}
