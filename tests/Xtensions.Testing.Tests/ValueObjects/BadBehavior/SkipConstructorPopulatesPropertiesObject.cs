namespace Xtensions.Testing.Tests.ValueObjects.BadBehavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using CSharpFunctionalExtensions;
    using Xtensions.Testing.ValueObjects;

    public class SkipConstructorPopulatesPropertiesObject : ValueObject
    {
        private readonly int value;

        public SkipConstructorPopulatesPropertiesObject(int value)
        {
            this.value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.value;
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class SkipConstructorPopulatesPropertiesObjectValueObjectTestCases : ValueObjectTestCases<SkipConstructorPopulatesPropertiesObject>
    {
        public override bool TestConstructorPropertyAssignment { get; } = false;

        public override FactoryExpressionCollection<SkipConstructorPopulatesPropertiesObject> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<SkipConstructorPopulatesPropertiesObject>(new Expression<Func<SkipConstructorPopulatesPropertiesObject>>[]
            {
                () => new SkipConstructorPopulatesPropertiesObject(1),
            });
        }
    }

    internal class SkipConstructorPopulatesPropertiesObjectTests : ValueObjectTests<SkipConstructorPopulatesPropertiesObject, SkipConstructorPopulatesPropertiesObjectValueObjectTestCases>
    {
    }
#pragma warning restore SA1402 // File may only contain a single type
}
