namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Xtensions.Testing.Tests.ValueObjects.TestObjects;
    using Xtensions.Testing.ValueObjects;

    public class ValueObjectPairValueObjectTestCases : ValueObjectTestCases<ValueObjectPair<EmailAddress>>
    {
        public override IEnumerable<Func<ValueObjectPair<EmailAddress>>> GetDistinctValueFactories()
        {
            return new Func<ValueObjectPair<EmailAddress>>[]
            {
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("bob@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("carol@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("bob@example.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("carol@example.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("bob@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@example.com"), new EmailAddress("bob@smith.com")),
                () => new ValueObjectPair<EmailAddress>(new EmailAddress("carol@example.com"), new EmailAddress("bob@smith.com")),
            };
        }
    }
}
