namespace Xtensions.Cache
{
    using System;
    using System.Threading.Tasks;
    using EnsureThat;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// An implementation of <see cref="ICache"/> that wraps <see cref="IMemoryCache"/>.
    /// </summary>
    /// <seealso cref="ICache" />
    public class MemoryCacheAdapter : ICache
    {
        private readonly IMemoryCache memoryCache;
        private readonly TimeSpan slidingExpiration;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryCacheAdapter"/> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache for storing cached values.</param>
        /// <param name="slidingExpiration">The sliding expiration to use for all cached values.</param>
        public MemoryCacheAdapter(IMemoryCache memoryCache, TimeSpan slidingExpiration)
        {
            EnsureArg.IsNotNull(memoryCache, nameof(memoryCache));

            this.memoryCache = memoryCache;
            this.slidingExpiration = slidingExpiration;
        }

        /// <summary>
        /// Reads the <see cref="CacheEntry{T}" /> at specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        /// The <see cref="CacheEntry{T}" /> or <see langword="null" />.
        /// </returns>
        public Task<CacheEntry<T>> ReadEntry<T>(string cacheKey)
            where T : class
        {
            EnsureArg.IsNotNullOrEmpty(cacheKey, nameof(cacheKey));

            return Task.FromResult(this.memoryCache.Get<CacheEntry<T>>(cacheKey));
        }

        /// <summary>
        /// Writes the <see cref="CacheEntry{T}" /> at the specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheEntry">The cache entry.</param>
        /// <returns>
        /// A <see cref="Task" /> that will complete when the entry is written.
        /// </returns>
        public Task WriteEntry<T>(string cacheKey, CacheEntry<T> cacheEntry)
            where T : class
        {
            EnsureArg.IsNotNullOrEmpty(cacheKey, nameof(cacheKey));
            EnsureArg.IsNotNull(cacheEntry, nameof(cacheEntry));

            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = cacheEntry.AbsoluteExpiration,
                SlidingExpiration = this.slidingExpiration,
            };

            this.memoryCache.Set(key: cacheKey, value: cacheEntry, options: cacheEntryOptions);

            return Task.CompletedTask;
        }
    }
}
