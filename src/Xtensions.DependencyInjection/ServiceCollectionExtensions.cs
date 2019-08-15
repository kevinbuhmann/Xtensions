namespace Xtensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using EnsureThat;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for adding services to an <see cref="IServiceCollection" />.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a singleton service of the type specified in TService with an implementation
        /// type specified in TImplementation using the custom dependencies specified in
        /// customDependenciesFactory to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the service to.</param>
        /// <param name="customDependenciesFactory">The factory that creates the custom dependencies.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddSingletonWithCustomDependencies<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, IEnumerable<object>> customDependenciesFactory)
            where TService : class
            where TImplementation : class, TService
        {
            EnsureArg.IsNotNull(services, nameof(services));
            EnsureArg.IsNotNull(customDependenciesFactory, nameof(customDependenciesFactory));

            return services.AddSingleton<TService, TImplementation>(GetImplementationFactory<TImplementation>(customDependenciesFactory));
        }

        private static Func<IServiceProvider, TImplementation> GetImplementationFactory<TImplementation>(
            Func<IServiceProvider, IEnumerable<object>> customDependenciesFactory)
            where TImplementation : class
        {
            return serviceProvider =>
            {
                ConstructorInfo constructor = typeof(TImplementation).GetConstructors().Single();
                IEnumerable<object> customDependencies = customDependenciesFactory(serviceProvider);

                IEnumerable<object> dependencies = constructor.GetParameters()
                    .Select(parameter => GetDependency(parameter, customDependencies, serviceProvider));

                return constructor.Invoke(dependencies.ToArray()) as TImplementation;
            };
        }

        private static object GetDependency(ParameterInfo parameter, IEnumerable<object> customDependencies, IServiceProvider serviceProvider)
        {
            return GetDependency(parameter, customDependencies) ?? serviceProvider.GetRequiredService(parameter.ParameterType);
        }

        private static object GetDependency(ParameterInfo parameter, IEnumerable<object> customDependencies)
        {
            return customDependencies.SingleOrDefault(customDependency => parameter.ParameterType.IsAssignableFrom(customDependency.GetType()));
        }
    }
}
