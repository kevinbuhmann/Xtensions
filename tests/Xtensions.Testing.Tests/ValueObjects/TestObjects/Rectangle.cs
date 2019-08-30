namespace Xtensions.Testing.Tests.ValueObjects.TestObjects
{
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using EnsureThat;

    public class Rectangle : ValueObject
    {
        public Rectangle(int width, int height)
        {
            EnsureArg.IsGt(value: width, limit: 0, nameof(width));
            EnsureArg.IsGt(value: height, limit: 0, nameof(height));

            this.Width = width;
            this.Height = height;
        }

        public int Width { get; }

        public int Height { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[] { this.Width, this.Height };
        }
    }
}
