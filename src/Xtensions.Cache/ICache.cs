namespace Xtensions.Cache
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a basic cache contract.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Reads the <see cref="CacheEntry{T}"/> at specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <returns>The <see cref="CacheEntry{T}"/> or <see langword="null"/>.</returns>
        Task<CacheEntry<T>> ReadEntry<T>(string cacheKey)
            where T : class;

        /// <summary>
        /// Writes the <see cref="CacheEntry{T}"/> at the specified cache key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="cacheEntry">The cache entry.</param>
        /// <returns>A <see cref="Task"/> that will complete when the entry is written.</returns>
        Task WriteEntry<T>(string cacheKey, CacheEntry<T> cacheEntry)
            where T : class;
    }
}
