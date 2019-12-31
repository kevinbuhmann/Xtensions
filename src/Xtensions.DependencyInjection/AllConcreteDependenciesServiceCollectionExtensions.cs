namespace Xtensions.DependencyInjection
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Extension methods for adding services to an <see cref="IServiceCollection"/>.
    /// </summary>
    public static class AllConcreteDependenciesServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the singleton including all concrete dependencies.
        /// </summary>
        /// <typeparam name="TService">The type of the service to add.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service(s) to.</param>
        public static void AddSingletonIncludingAllConcreteDependencies<TService, TImplementation>(this IServiceCollection services)
            where TService : class
            where TImplementation : class, TService
        {
            services.AddSingleton<TService, TImplementation>();
            services.AddAllConcreteDependenciesAsSingletons(typeof(TImplementation));
        }

        private static void AddAllConcreteDependenciesAsSingletons(this IServiceCollection services, Type serviceType)
        {
            foreach (ConstructorInfo constructor in serviceType.GetConstructors())
            {
                foreach (ParameterInfo parameter in constructor.GetParameters())
                {
                    Type dependencyType = parameter.ParameterType;

                    if (dependencyType.IsAbstract == false)
                    {
                        services.AddSingleton(dependencyType);
                        services.AddAllConcreteDependenciesAsSingletons(dependencyType);
                    }
                }
            }
        }
    }
}
