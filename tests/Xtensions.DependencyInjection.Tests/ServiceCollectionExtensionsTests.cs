namespace Xtensions.DependencyInjection.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Xtensions.DependencyInjection.Tests.TestServices;
    using Xtensions.DependencyInjection.Tests.TestServices.Abstractions;
    using Xunit;

    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSingletonWithCustomDependencies_DependencyServiceNotRegistered_InjectsCustomDependency()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IOtherService, OtherService>();
            services.AddSingleton<CustomInjectedService>();

            services.AddSingletonWithCustomDependencies<IRootService, RootService>(serviceProvider => new object[]
            {
                serviceProvider.GetRequiredService<CustomInjectedService>(),
            });

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IRootService rootService = serviceProvider.GetRequiredService<IRootService>();

                Assert.IsType<CustomInjectedService>(rootService.InjectedService);
            }
        }

        [Fact]
        public void AddSingletonWithCustomDependencies_DependencyServiceRegistered_InjectsCustomDependencyInstead()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IOtherService, OtherService>();
            services.AddSingleton<IInjectedService, DefaultInjectedService>();
            services.AddSingleton<CustomInjectedService>();

            services.AddSingletonWithCustomDependencies<IRootService, RootService>(serviceProvider => new object[]
            {
                serviceProvider.GetRequiredService<CustomInjectedService>(),
            });

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IRootService rootService = serviceProvider.GetRequiredService<IRootService>();

                Assert.IsType<CustomInjectedService>(rootService.InjectedService);
            }
        }

        [Fact]
        public void AddSingletonWithCustomDependencies_InjectsNonCustomDependencyUsingServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddSingleton<IOtherService, OtherService>();
            services.AddSingleton<CustomInjectedService>();

            services.AddSingletonWithCustomDependencies<IRootService, RootService>(serviceProvider => new object[]
            {
                serviceProvider.GetRequiredService<CustomInjectedService>(),
            });

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IRootService rootService = serviceProvider.GetRequiredService<IRootService>();

                Assert.IsType<OtherService>(rootService.OtherService);
            }
        }
    }
}
