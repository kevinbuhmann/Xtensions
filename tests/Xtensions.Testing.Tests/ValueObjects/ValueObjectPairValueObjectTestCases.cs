namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using System.Linq.Expressions;
    using Xtensions.Testing.Tests.ValueObjects.TestObjects;
    using Xtensions.Testing.ValueObjects;

    public class ValueObjectPairValueObjectTestCases : ValueObjectTestCases<ValueObjectPair<EmailAddress>>
    {
        public override FactoryExpressionCollection<ValueObjectPair<EmailAddress>> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<ValueObjectPair<EmailAddress>>(new Expression<Func<ValueObjectPair<EmailAddress>>>[]
            {
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("bob@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("carol@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("bob@example.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("carol@example.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("bob@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@example.com"), new EmailAddress("bob@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("carol@example.com"), new EmailAddress("bob@smith.com")),
            });
        }
    }
}
