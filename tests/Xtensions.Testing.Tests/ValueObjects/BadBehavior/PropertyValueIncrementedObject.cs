namespace Xtensions.Testing.Tests.ValueObjects.BadBehavior
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using CSharpFunctionalExtensions;
    using Xtensions.Testing.ValueObjects;

    public class PropertyValueIncrementedObject : ValueObject
    {
        public PropertyValueIncrementedObject(int value)
        {
            this.Value = value + 1;
        }

        public int Value { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value;
        }
    }

#pragma warning disable SA1402 // File may only contain a single type
    public class PropertyValueIncrementedObjectValueObjectTestCases : ValueObjectTestCases<PropertyValueIncrementedObject>
    {
        public override FactoryExpressionCollection<PropertyValueIncrementedObject> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<PropertyValueIncrementedObject>(new Expression<Func<PropertyValueIncrementedObject>>[]
            {
                () => new PropertyValueIncrementedObject(1),
            });
        }
    }

    internal class PropertyValueIncrementedObjectTests : ValueObjectTests<PropertyValueIncrementedObject, PropertyValueIncrementedObjectValueObjectTestCases>
    {
    }
#pragma warning restore SA1402 // File may only contain a single type
}
