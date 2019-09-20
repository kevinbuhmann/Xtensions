namespace Xtensions.Testing.Tests.ValueObjects.BadBehavior
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Xtensions.Testing.ValueObjects;

    public class AlwaysNotEqualObject : ValueObject
    {
        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { Guid.NewGuid() };
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class AlwaysNotEqualObjectValueObjectTestCases : ValueObjectTestCases<AlwaysNotEqualObject>
    {
        public override IEnumerable<Func<AlwaysNotEqualObject>> GetDistinctValueFactories()
        {
            throw new NotImplementedException();
        }
    }

    internal class AlwaysNotEqualObjectTests : ValueObjectTests<AlwaysNotEqualObject, AlwaysNotEqualObjectValueObjectTestCases>
    {
    }
#pragma warning restore SA1402 // File may only contain a single type
}
