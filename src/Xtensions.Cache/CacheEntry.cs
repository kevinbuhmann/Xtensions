namespace Xtensions.Cache
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;

    /// <summary>
    /// Represents an entry that is stored in cache.
    /// </summary>
    /// <typeparam name="T">The type of the value that is cached.</typeparam>
    public class CacheEntry<T> : ValueObject
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheEntry{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">The absolute expiration.</param>
        public CacheEntry(T value, DateTime absoluteExpiration)
        {
            this.Value = value;
            this.AbsoluteExpiration = absoluteExpiration;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public T Value { get; }

        /// <summary>
        /// Gets the absolute expiration.
        /// </summary>
        /// <value>
        /// The absolute expiration.
        /// </value>
        public DateTime AbsoluteExpiration { get; }

        /// <summary>
        /// Gets the values to check when determining if two instances are equal.
        /// </summary>
        /// <returns>The values to check when determining if two instances are equal.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
            yield return this.AbsoluteExpiration;
        }
    }
}
