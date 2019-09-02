namespace Xtensions.Cache.Tests
{
    using System;
    using System.Collections.Generic;
    using Xtensions.Testing.ValueObjects;

    public class CacheEntryValueObjectTestCases : ValueObjectTestCases<CacheEntry<string>>
    {
        public override IEnumerable<Func<CacheEntry<string>>> GetDistinctValueFactories()
        {
            DateTime expiration = new DateTime(2019, 03, 17, 18, 0, 0);
            DateTime otherExpiration = new DateTime(2019, 03, 17, 20, 0, 0);

            return new Func<CacheEntry<string>>[]
            {
                () => new CacheEntry<string>(value: "value", absoluteExpiration: expiration),
                () => new CacheEntry<string>(value: "other-value", absoluteExpiration: expiration),
                () => new CacheEntry<string>(value: "value", absoluteExpiration: otherExpiration),
                () => new CacheEntry<string>(value: "other-value", absoluteExpiration: otherExpiration),
            };
        }
    }
}
