namespace Xtensions.Cache.Tests
{
    using System;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Internal;
    using Moq;
    using Xtensions.Time;
    using Xunit;

    public class MemoryCacheAdapterTests
    {
        [Fact]
        public void Constructor_NullMemoryCache_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                paramName: "memoryCache",
                testCode: () => new MemoryCacheAdapter(memoryCache: null, slidingExpiration: TimeSpan.Zero));
        }

        [Fact]
        public void Constructor_GivenMemoryCache_DoesNotThrow()
        {
            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                Assert.NotNull(new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1)));
            }
        }

        [Fact]
        public async void ReadEntry_NullCacheKey_Throws()
        {
            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1));

                await Assert.ThrowsAsync<ArgumentNullException>(
                    paramName: "cacheKey",
                    testCode: () => memoryCacheAdapter.ReadEntry<string>(cacheKey: null));
            }
        }

        [Fact]
        public async void ReadEntry_EntryDoesNotExist_ReturnsNull()
        {
            const string cacheKey = "test-cache-key";

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1));

                Assert.Null(await memoryCacheAdapter.ReadEntry<string>(cacheKey));
            }
        }

        [Fact]
        public async void ReadEntry_EntryExists_ReturnsEntry()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddHours(-1)))
            {
                memoryCache.Set(cacheKey, new CacheEntry<string>(value, absoluteExpiration));

                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1));
                CacheEntry<string> cacheEntry = await memoryCacheAdapter.ReadEntry<string>(cacheKey);

                Assert.NotNull(cacheEntry);
                Assert.Equal(actual: cacheEntry.Value, expected: value);
                Assert.Equal(actual: cacheEntry.AbsoluteExpiration, expected: absoluteExpiration);
            }
        }

        [Fact]
        public async void WriteEntry_NullCacheKey_Throws()
        {
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);
            CacheEntry<string> cacheEntry = new CacheEntry<string>(value, absoluteExpiration);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1));

                await Assert.ThrowsAsync<ArgumentNullException>(
                    paramName: "cacheKey",
                    testCode: () => memoryCacheAdapter.WriteEntry(cacheKey: null, cacheEntry));
            }
        }

        [Fact]
        public async void WriteEntry_NullCacheEntry_Throws()
        {
            const string cacheKey = "test-cache-key";

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1));

                await Assert.ThrowsAsync<ArgumentNullException>(
                    paramName: "cacheEntry",
                    testCode: () => memoryCacheAdapter.WriteEntry<string>(cacheKey, cacheEntry: null));
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntry()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddHours(-1)))
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromMinutes(1));
                await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));

                CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                Assert.NotNull(cacheEntry);
                Assert.Equal(actual: cacheEntry.Value, expected: value);
                Assert.Equal(actual: cacheEntry.AbsoluteExpiration, expected: absoluteExpiration);
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntryUsingCorrectAbsoluteExpiration_ReadBeforeExpirationTime()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromDays(10));

                using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddHours(-1)))
                {
                    await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));
                }

                using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddSeconds(-1)))
                {
                    CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                    Assert.NotNull(cacheEntry);
                    Assert.Equal(actual: cacheEntry.Value, expected: value);
                    Assert.Equal(actual: cacheEntry.AbsoluteExpiration, expected: absoluteExpiration);
                }
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntryUsingCorrectAbsoluteExpiration_ReadAtExpirationTime()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromDays(10));

                using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddHours(-1)))
                {
                    await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));
                }

                using (CurrentTime.UseMockUtcNow(absoluteExpiration))
                {
                    CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                    Assert.Null(cacheEntry);
                }
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntryUsingCorrectAbsoluteExpiration_ReadAfterExpirationTime()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: TimeSpan.FromDays(10));

                using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddHours(-1)))
                {
                    await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));
                }

                using (CurrentTime.UseMockUtcNow(absoluteExpiration.AddSeconds(1)))
                {
                    CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                    Assert.Null(cacheEntry);
                }
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntryUsingCorrectSlidingExpiration_ReadBeforeExpirationTime()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                TimeSpan slidingExpiration = TimeSpan.FromMinutes(10);
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: slidingExpiration);
                DateTime writeTime = absoluteExpiration.AddHours(-1);

                using (CurrentTime.UseMockUtcNow(writeTime))
                {
                    await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));
                }

                using (CurrentTime.UseMockUtcNow(writeTime.Add(slidingExpiration).AddSeconds(-1)))
                {
                    CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                    Assert.NotNull(cacheEntry);
                    Assert.Equal(actual: cacheEntry.Value, expected: value);
                    Assert.Equal(actual: cacheEntry.AbsoluteExpiration, expected: absoluteExpiration);
                }
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntryUsingCorrectSlidingExpiration_ReadAtExpirationTime()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                TimeSpan slidingExpiration = TimeSpan.FromMinutes(10);
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: slidingExpiration);
                DateTime writeTime = absoluteExpiration.AddHours(-1);

                using (CurrentTime.UseMockUtcNow(writeTime))
                {
                    await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));
                }

                using (CurrentTime.UseMockUtcNow(writeTime.Add(slidingExpiration)))
                {
                    CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                    Assert.Null(cacheEntry);
                }
            }
        }

        [Fact]
        public async void WriteEntry_WritesEntryUsingCorrectSlidingExpiration_ReadAfterExpirationTime()
        {
            const string cacheKey = "test-cache-key";
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            using (IMemoryCache memoryCache = GetMemoryCache())
            {
                TimeSpan slidingExpiration = TimeSpan.FromMinutes(10);
                MemoryCacheAdapter memoryCacheAdapter = new MemoryCacheAdapter(memoryCache, slidingExpiration: slidingExpiration);
                DateTime writeTime = absoluteExpiration.AddHours(-1);

                using (CurrentTime.UseMockUtcNow(writeTime))
                {
                    await memoryCacheAdapter.WriteEntry(cacheKey, new CacheEntry<string>(value, absoluteExpiration));
                }

                using (CurrentTime.UseMockUtcNow(writeTime.Add(slidingExpiration).AddSeconds(1)))
                {
                    CacheEntry<string> cacheEntry = memoryCache.Get<CacheEntry<string>>(cacheKey);

                    Assert.Null(cacheEntry);
                }
            }
        }

        private static IMemoryCache GetMemoryCache()
        {
            return new MemoryCache(new MemoryCacheOptions()
            {
                Clock = GetMockSystemClock(),
            });
        }

        private static ISystemClock GetMockSystemClock()
        {
            Mock<ISystemClock> systemClockMock = new Mock<ISystemClock>(MockBehavior.Strict);

            systemClockMock
                .SetupGet(systemClock => systemClock.UtcNow)
                .Returns(() => CurrentTime.UtcNow);

            return systemClockMock.Object;
        }
    }
}
