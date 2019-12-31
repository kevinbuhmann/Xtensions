namespace Xtensions.Cache
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Extensions.DependencyInjection;
    using Xtensions.DependencyInjection;

    /// <summary>
    /// Extension methods for adding services to an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a singleton service of the type specified in TService with an implementation
        /// type specified in TImplementation. The given cache will be injected into TImplementation as <see cref="ICache"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="cacheFactory">A function that returns an implementation of <see cref="ICache"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSingletonWithCache<TService, TImplementation>(
           this IServiceCollection services,
           Func<IServiceProvider, ICache> cacheFactory)
           where TService : class
           where TImplementation : class, TService
        {
            return services.AddSingletonWithCustomDependencies<TService, TImplementation>(serviceProvider => new object[]
            {
                cacheFactory(serviceProvider),
            });
        }

        /// <summary>
        /// Adds a singleton service of the type specified in TService with an implementation
        /// type specified in TImplementation. A composite cache using the given source caches will be
        /// injected into TImplementation as <see cref="ICache"/>.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="cacheFactory">A function that returns the source caches for <see cref="CompositeCache"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSingletonWithCompositeCache<TService, TImplementation>(
           this IServiceCollection services,
           Func<IServiceProvider, IEnumerable<ICache>> cacheFactory)
           where TService : class
           where TImplementation : class, TService
        {
            return services.AddSingletonWithCache<TService, TImplementation>(serviceProvider => new CompositeCache(cacheFactory(serviceProvider)));
        }
    }
}
