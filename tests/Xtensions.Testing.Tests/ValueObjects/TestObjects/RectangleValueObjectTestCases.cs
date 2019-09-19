namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System;
    using System.Linq.Expressions;
    using Xtensions.Testing.ValueObjects;

    public class RectangleValueObjectTestCases : ValueObjectTestCases<Rectangle>
    {
        public override FactoryExpressionCollection<Rectangle> GetDistinctFactoryExpressions()
        {
            return new FactoryExpressionCollection<Rectangle>(new Expression<Func<Rectangle>>[]
            {
                () => new Rectangle(5, 5),
                () => new Rectangle(5, 10),
                () => new Rectangle(5, 15),
                () => new Rectangle(10, 5),
                () => new Rectangle(10, 10),
            });
        }
    }
}
