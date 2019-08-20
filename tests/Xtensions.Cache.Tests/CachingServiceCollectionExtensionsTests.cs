namespace Xtensions.Cache.Tests
{
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xtensions.Cache.Tests.TestServices;
    using Xunit;

    public class CachingServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSingletonWithCache_CacheNotRegistered_InjectsTheGivenCache()
        {
            ICache cache = new Mock<ICache>(MockBehavior.Strict).Object;

            ServiceCollection services = new ServiceCollection();

            services.AddSingletonWithCache<IService, Service>(serviceProvider => cache);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IService service = serviceProvider.GetRequiredService<IService>();

                Assert.Equal(actual: service.Cache, expected: cache);
            }
        }

        [Fact]
        public void AddSingletonWithCache_CacheRegistered_InjectsTheGivenCacheInsteadOfTheRegisteredCache()
        {
            ICache cache = new Mock<ICache>(MockBehavior.Strict).Object;
            ICache registeredCache = new Mock<ICache>(MockBehavior.Strict).Object;

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton(registeredCache);
            services.AddSingletonWithCache<IService, Service>(serviceProvider => cache);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IService service = serviceProvider.GetRequiredService<IService>();

                Assert.Equal(actual: service.Cache, expected: cache);
            }
        }

        [Fact]
        public void AddSingletonWithCompositeCache_CacheNotRegistered_InjectsTheGivenCache()
        {
            ICache[] sourceCaches = new ICache[]
            {
                new Mock<ICache>(MockBehavior.Strict).Object,
                new Mock<ICache>(MockBehavior.Strict).Object,
            };

            ServiceCollection services = new ServiceCollection();

            services.AddSingletonWithCompositeCache<IService, Service>(serviceProvider => sourceCaches);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IService service = serviceProvider.GetRequiredService<IService>();

                Assert.IsType<CompositeCache>(service.Cache);
                Assert.Equal(actual: (service.Cache as CompositeCache).SourceCaches, expected: sourceCaches);
            }
        }

        [Fact]
        public void AddSingletonWithCompositeCache_CacheRegistered_InjectsTheGivenCacheInsteadOfTheRegisteredCache()
        {
            ICache[] sourceCaches = new ICache[]
            {
                new Mock<ICache>(MockBehavior.Strict).Object,
                new Mock<ICache>(MockBehavior.Strict).Object,
            };

            ServiceCollection services = new ServiceCollection();

            services.AddSingleton(new Mock<ICache>(MockBehavior.Strict).Object);
            services.AddSingletonWithCompositeCache<IService, Service>(serviceProvider => sourceCaches);

            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                IService service = serviceProvider.GetRequiredService<IService>();

                Assert.IsType<CompositeCache>(service.Cache);
                Assert.Equal(actual: (service.Cache as CompositeCache).SourceCaches, expected: sourceCaches);
            }
        }
    }
}
