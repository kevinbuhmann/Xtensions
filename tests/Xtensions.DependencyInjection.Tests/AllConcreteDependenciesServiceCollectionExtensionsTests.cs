namespace Xtensions.DependencyInjection.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies;
    using Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Clients;
    using Xunit;

    public class AllConcreteDependenciesServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSingletonIncludingAllConcreteDependencies_RegistersAllConcreteDependencies()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IServiceClient, ServiceClient>();
            services.AddSingletonIncludingAllConcreteDependencies<IService, Service>();

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                Assert.NotNull(serviceProvider.GetRequiredService<IService>());
            }
        }
    }
}
