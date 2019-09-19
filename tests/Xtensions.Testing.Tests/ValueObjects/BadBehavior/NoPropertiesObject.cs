namespace Xtensions.Testing.Tests.ValueObjects.BadBehavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using CSharpFunctionalExtensions;
    using Xtensions.Testing.ValueObjects;

    public class NoPropertiesObject : ValueObject
    {
        private readonly int value;

        public NoPropertiesObject(int value)
        {
            this.value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.value;
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class NoPropertiesObjectValueObjectTestCases : ValueObjectTestCases<NoPropertiesObject>
    {
        public override FactoryExpressionCollection<NoPropertiesObject> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<NoPropertiesObject>(new Expression<Func<NoPropertiesObject>>[]
            {
                () => new NoPropertiesObject(1),
            });
        }
    }

    internal class NoPropertiesObjectTests : ValueObjectTests<NoPropertiesObject, NoPropertiesObjectValueObjectTestCases>
    {
    }
#pragma warning restore SA1402 // File may only contain a single type
}
