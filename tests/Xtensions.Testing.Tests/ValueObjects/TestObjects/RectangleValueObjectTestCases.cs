namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System;
    using System.Collections.Generic;
    using Xtensions.Testing.ValueObjects;

    public class RectangleValueObjectTestCases : ValueObjectTestCases<Rectangle>
    {
        public override IEnumerable<Func<Rectangle>> GetDistinctValueFactories()
        {
            return new Func<Rectangle>[]
            {
                () => new Rectangle(width: 5, height: 5),
                () => new Rectangle(width: 5, height: 10),
                () => new Rectangle(width: 5, height: 15),
                () => new Rectangle(width: 10, height: 5),
                () => new Rectangle(width: 10, height: 10),
            };
        }
    }
}
