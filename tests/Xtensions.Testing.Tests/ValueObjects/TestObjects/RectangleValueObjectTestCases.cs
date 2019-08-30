namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Xtensions.Testing.ValueObjects;

    public class RectangleValueObjectTestCases : IValueObjectTestCases<Rectangle>
    {
        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Interface")]
        public IEnumerable<(Rectangle value1, Rectangle value2)> GetEqualPairs()
        {
            return new (Rectangle value1, Rectangle value2)[]
            {
                (new Rectangle(width: 5, height: 5), new Rectangle(width: 5, height: 5)),
                (new Rectangle(width: 5, height: 10), new Rectangle(width: 5, height: 10)),
                (new Rectangle(width: 10, height: 5), new Rectangle(width: 10, height: 5)),
            };
        }

        [SuppressMessage("Design", "CA1024:Use properties where appropriate", Justification = "Interface")]
        public IEnumerable<Rectangle> GetNotEqualValues()
        {
            return new Rectangle[]
            {
                new Rectangle(width: 5, height: 5),
                new Rectangle(width: 5, height: 10),
                new Rectangle(width: 5, height: 15),
                new Rectangle(width: 10, height: 5),
                new Rectangle(width: 10, height: 10),
            };
        }
    }
}
