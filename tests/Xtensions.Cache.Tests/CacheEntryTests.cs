namespace Xtensions.Cache.Tests
{
    using System;
    using Xtensions.Testing.ValueObjects;
    using Xunit;

    public class CacheEntryTests : ValueObjectTests<CacheEntry<string>, CacheEntryValueObjectTestCases>
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
