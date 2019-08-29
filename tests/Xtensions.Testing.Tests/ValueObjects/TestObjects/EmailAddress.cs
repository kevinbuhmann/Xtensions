namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;

    public class EmailAddress : ValueObject
    {
        public EmailAddress(string value)
        {
            this.Value = value;
        }

        public string Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { this.Value.ToUpperInvariant() };
        }
    }
}
