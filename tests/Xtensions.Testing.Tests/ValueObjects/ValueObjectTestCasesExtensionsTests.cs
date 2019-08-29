namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Xtensions.Testing.ValueObjects;
    using Xunit;

    public class ValueObjectTestCasesExtensionsTests
    {
        [Fact]
        public void GetPermutatedEqualPairs_NullTestCases_Throws()
        {
            IValueObjectTestCases<Rectangle> valueObjectTestCases = null;

            Assert.Throws<ArgumentNullException>(
                paramName: "valueObjectTestCases",
                testCode: () => valueObjectTestCases.GetPermutatedEqualPairs());
        }

        [Fact]
        public void GetPermutatedEqualPairs_ReturnsPairs()
        {
            IEnumerable<(string left, string right)> expectedPermutatedEqualPairs = new (string value1, string value2)[]
            {
                ("hello", "HELLO"),
                ("HELLO", "hello"),
                ("world", "WORLD"),
                ("WORLD", "world"),
                ("bob", "BOB"),
                ("BOB", "bob"),
                ("smith", "SMITH"),
                ("SMITH", "smith"),
            };

            Assert.Equal(expected: expectedPermutatedEqualPairs, actual: new StringValueObjectTestCases().GetPermutatedEqualPairs());
        }

        [Fact]
        public void GetPermutatedNotEqualPairs_NullTestCases_Throws()
        {
            IValueObjectTestCases<Rectangle> valueObjectTestCases = null;

            Assert.Throws<ArgumentNullException>(
                paramName: "valueObjectTestCases",
                testCode: () => valueObjectTestCases.GetPermutatedNotEqualPairs());
        }

        [Fact]
        public void GetPermutatedNotEqualPairs_ReturnsPairs()
        {
            IEnumerable<(string left, string right)> expectedPermutatedNotEqualPairs = new (string value1, string value2)[]
            {
                ("hello", "world"),
                ("hello", "bob"),
                ("hello", "smith"),

                ("world", "hello"),
                ("world", "bob"),
                ("world", "smith"),

                ("bob", "hello"),
                ("bob", "world"),
                ("bob", "smith"),

                ("smith", "hello"),
                ("smith", "world"),
                ("smith", "bob"),
            };

            Assert.Equal(expected: expectedPermutatedNotEqualPairs, actual: new StringValueObjectTestCases().GetPermutatedNotEqualPairs());
        }

        private class StringValueObjectTestCases : IValueObjectTestCases<string>
        {
            public IEnumerable<(string value1, string value2)> GetEqualPairs()
            {
                // These are obviously not equal pairs. It's just for testing that the pairs are permutated. Let's pretend case does not matter.
                return new (string value1, string value2)[]
                {
                    ("hello", "HELLO"),
                    ("world", "WORLD"),
                    ("bob", "BOB"),
                    ("smith", "SMITH"),
                };
            }

            public IEnumerable<string> GetNotEqualValues()
            {
                return new string[]
                {
                    "hello",
                    "world",
                    "bob",
                    "smith",
                };
            }
        }
    }
}
