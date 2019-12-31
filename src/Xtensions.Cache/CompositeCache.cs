namespace Xtensions.Cache
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EnsureThat;

    /// <summary>
    /// An implementation of <see cref="ICache"/> that composes multiple source caches.
    /// </summary>
    /// <seealso cref="ICache" />
    public class CompositeCache : ICache
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeCache"/> class.
        /// </summary>
        /// <param name="sourceCaches">The source caches.</param>
        public CompositeCache(IEnumerable<ICache> sourceCaches)
        {
            EnsureArg.IsNotNull(sourceCaches, nameof(sourceCaches));

            List<ICache> sourceCachesList = sourceCaches.ToList();

            EnsureArg.IsTrue(
                value: sourceCachesList.Count > 1,
                paramName: nameof(sourceCaches),
                optsFn: opts => opts.WithMessage($"{nameof(CompositeCache)} requires at least two source caches."));

            EnsureArg.IsTrue(
                value: sourceCachesList.Count == sourceCachesList.Distinct().Count(),
                paramName: nameof(sourceCaches),
                optsFn: opts => opts.WithMessage($"{nameof(CompositeCache)} requires at least distinct two source caches."));

            this.SourceCaches = sourceCachesList;
        }

        /// <summary>
        /// Gets the source caches.
        /// </summary>
        /// <value>
        /// The source caches.
        /// </value>
        internal IReadOnlyCollection<ICache> SourceCaches { get; }

        /// <summary>
        /// Reads the <see cref="CacheEntry{T}" /> at specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>
        /// The <see cref="CacheEntry{T}" /> or <see langword="null" />.
        /// </returns>
        public async Task<CacheEntry<T>?> ReadEntry<T>(string cacheKey)
            where T : class
        {
            EnsureArg.IsNotNullOrEmpty(cacheKey, nameof(cacheKey));

            CacheEntry<T>? cacheEntry = null;
            List<ICache> missedCaches = new List<ICache>();

            foreach (ICache cache in this.SourceCaches)
            {
                cacheEntry = await cache.ReadEntry<T>(cacheKey).ConfigureAwait(false);

                if (cacheEntry == null)
                {
                    missedCaches.Add(cache);
                }
                else
                {
                    break;
                }
            }

            if (cacheEntry != null && missedCaches.Any())
            {
                await missedCaches.WriteEntry(cacheKey, cacheEntry).ConfigureAwait(false);
            }

            return cacheEntry;
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

            return this.SourceCaches.WriteEntry(cacheKey, cacheEntry);
        }
    }
}
