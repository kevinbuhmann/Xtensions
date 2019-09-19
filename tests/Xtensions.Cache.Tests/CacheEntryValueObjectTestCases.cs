namespace Xtensions.Cache.Tests
{
    using System;
    using System.Linq.Expressions;
    using Xtensions.Testing.ValueObjects;

    public class CacheEntryValueObjectTestCases : ValueObjectTestCases<CacheEntry<string>>
    {
        public override FactoryExpressionCollection<CacheEntry<string>> GetDistinctFactoryExpressions()
        {
            DateTime expiration = new DateTime(2019, 03, 17, 18, 0, 0);
            DateTime otherExpiration = new DateTime(2019, 03, 17, 20, 0, 0);

            return new FactoryExpressionCollection<CacheEntry<string>>(new Expression<Func<CacheEntry<string>>>[]
            {
                () => new CacheEntry<string>("value", expiration),
                () => new CacheEntry<string>("other-value", expiration),
                () => new CacheEntry<string>("value", otherExpiration),
                () => new CacheEntry<string>("other-value", otherExpiration),
            });
        }
    }
}
