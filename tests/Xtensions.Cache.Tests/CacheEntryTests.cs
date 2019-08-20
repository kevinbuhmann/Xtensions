namespace Xtensions.Cache.Tests
{
    using System;
    using Xunit;

    public class CacheEntryTests
    {
        [Fact]
        public void Constructor_PopulatesProperties()
        {
            const string value = "test-value";
            DateTime absoluteExpiration = new DateTime(year: 2018, month: 3, day: 17, hour: 8, minute: 0, second: 0);

            CacheEntry<string> cacheEntry = new CacheEntry<string>(value, absoluteExpiration);

            Assert.Equal(actual: cacheEntry.Value, expected: value);
            Assert.Equal(actual: cacheEntry.AbsoluteExpiration, expected: absoluteExpiration);
        }
    }
}
