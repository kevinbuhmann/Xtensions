namespace Xtensions.Cache.Tests
{
    using System;
    using System.Threading.Tasks;
    using Moq;
    using Xunit;

    public class CompositeCacheTests
    {
        [Fact]
        public void Constructor_ZeroSourceCaches_Throws()
        {
            Assert.Throws<ArgumentException>(
                paramName: "sourceCaches",
                testCode: () => new CompositeCache(sourceCaches: Array.Empty<ICache>()));
        }

        [Fact]
        public void Constructor_OneSourceCache_Throws()
        {
            ICache sourceCache = new Mock<ICache>(MockBehavior.Strict).Object;

            Assert.Throws<ArgumentException>(
                paramName: "sourceCaches",
                testCode: () => new CompositeCache(sourceCaches: new ICache[] { sourceCache }));
        }

        [Fact]
        public void Constructor_NonDistinctSourceCaches_Throws()
        {
            ICache sourceCache = new Mock<ICache>(MockBehavior.Strict).Object;

            Assert.Throws<ArgumentException>(
                paramName: "sourceCaches",
                testCode: () => new CompositeCache(sourceCaches: new ICache[] { sourceCache, sourceCache }));
        }

        [Fact]
        public void Constructor_TwoDistinctSourceCaches_DoesNotThrow()
        {
            ICache sourceCache1 = new Mock<ICache>(MockBehavior.Strict).Object;
            ICache sourceCache2 = new Mock<ICache>(MockBehavior.Strict).Object;

            Assert.NotNull(new CompositeCache(sourceCaches: new ICache[] { sourceCache1, sourceCache2 }));
        }

        [Fact]
        public async Task ReadEntry_EntryDoesNotExist_ReturnsNull()
        {
            const string cacheKey = "test-cache-key";

            Mock<ICache> sourceCache1Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache1Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(null as CacheEntry<string>);

            Mock<ICache> sourceCache2Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache2Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(null as CacheEntry<string>);

            CompositeCache compositeCache = new CompositeCache(new ICache[] { sourceCache1Mock.Object, sourceCache2Mock.Object });

            Assert.Null(await compositeCache.ReadEntry<string>(cacheKey));
        }

        [Fact]
        public async Task ReadEntry_EntryExistsInFirstCache_ReturnsEntry()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);
            CacheEntry<string> cacheEntry = new CacheEntry<string>(value, absoluteExpiration);

            Mock<ICache> sourceCache1Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache1Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(cacheEntry);

            Mock<ICache> sourceCache2Mock = new Mock<ICache>(MockBehavior.Strict);

            CompositeCache compositeCache = new CompositeCache(new ICache[] { sourceCache1Mock.Object, sourceCache2Mock.Object });

            Assert.Equal(
                actual: await compositeCache.ReadEntry<string>(cacheKey),
                expected: cacheEntry);
        }

        [Fact]
        public async Task ReadEntry_EntryExistsInSecondCache_ReturnsEntry()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);
            CacheEntry<string> cacheEntry = new CacheEntry<string>(value, absoluteExpiration);

            // This mock uses loose behavior because this test is not asserting that the entry is written to the missed cache.
            Mock<ICache> sourceCache1Mock = new Mock<ICache>(MockBehavior.Loose);

            sourceCache1Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(null as CacheEntry<string>);

            Mock<ICache> sourceCache2Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache2Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(cacheEntry);

            CompositeCache compositeCache = new CompositeCache(new ICache[] { sourceCache1Mock.Object, sourceCache2Mock.Object });

            Assert.Equal(
                actual: await compositeCache.ReadEntry<string>(cacheKey),
                expected: cacheEntry);
        }

        [Fact]
        public async Task ReadEntry_EntryExistsInSecondCache_EntryIsWrittenToTheFirstCache()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);
            CacheEntry<string> cacheEntry = new CacheEntry<string>(value, absoluteExpiration);

            Mock<ICache> sourceCache1Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache1Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(null as CacheEntry<string>);

            sourceCache1Mock
                .Setup(cache => cache.WriteEntry(cacheKey, cacheEntry))
                .Returns(Task.CompletedTask)
                .Verifiable();

            Mock<ICache> sourceCache2Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache2Mock
                .Setup(cache => cache.ReadEntry<string>(cacheKey))
                .ReturnsAsync(cacheEntry);

            CompositeCache compositeCache = new CompositeCache(new ICache[] { sourceCache1Mock.Object, sourceCache2Mock.Object });

            await compositeCache.ReadEntry<string>(cacheKey);

            sourceCache1Mock.Verify();
        }

        [Fact]
        public async Task WriteEntry_WritesEntryToAllSourceCaches()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);
            CacheEntry<string> cacheEntry = new CacheEntry<string>(value, absoluteExpiration);

            Mock<ICache> sourceCache1Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache1Mock
                .Setup(cache => cache.WriteEntry(cacheKey, cacheEntry))
                .Returns(Task.CompletedTask).Verifiable();

            Mock<ICache> sourceCache2Mock = new Mock<ICache>(MockBehavior.Strict);

            sourceCache2Mock
                .Setup(cache => cache.WriteEntry(cacheKey, cacheEntry))
                .Returns(Task.CompletedTask).Verifiable();

            CompositeCache compositeCache = new CompositeCache(new ICache[] { sourceCache1Mock.Object, sourceCache2Mock.Object });

            await compositeCache.WriteEntry(cacheKey, cacheEntry);

            sourceCache1Mock.Verify();
            sourceCache2Mock.Verify();
        }
    }
}
