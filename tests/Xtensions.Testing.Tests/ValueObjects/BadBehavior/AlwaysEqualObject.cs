namespace Xtensions.Testing.Tests.ValueObjects.BadBehavior
{
    using System;
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using Xtensions.Testing.ValueObjects;

    public class AlwaysEqualObject : ValueObject
    {
        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { 1 };
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class AlwaysEqualObjectValueObjectTestCases : ValueObjectTestCases<AlwaysEqualObject>
    {
        public override FactoryExpressionCollection<AlwaysEqualObject> GetDistinctFactoryExpressions()
        {
            throw new NotImplementedException();
        }
    }

    internal class AlwaysEqualObjectTests : ValueObjectTests<AlwaysEqualObject, AlwaysEqualObjectValueObjectTestCases>
    {
    }
#pragma warning restore SA1402 // File may only contain a single type
}
