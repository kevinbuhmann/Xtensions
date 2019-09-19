namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Xtensions.Testing.ValueObjects;

    public class EmailAddressValueObjectTestCases : ValueObjectTestCases<EmailAddress>
    {
        public override FactoryExpressionCollection<EmailAddress> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<EmailAddress>(new Expression<Func<EmailAddress>>[]
            {
                () => new EmailAddress("bob@smith.com"),
                () => new EmailAddress("carol@smith.com"),
                () => new EmailAddress("bob@example.com"),
                () => new EmailAddress("carol@example.com"),
            });
        }

        public override IEnumerable<ValueObjectPair<EmailAddress>> GetAdditionalEqualPairs()
        {
            return new ValueObjectPair<EmailAddress>[]
            {
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("BOB@SMITH.COM")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("Carol@Smith.com")),
            };
        }
    }
}
