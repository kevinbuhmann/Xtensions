namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Xtensions.Testing.Tests.ValueObjects.TestObjects;
    using Xtensions.Testing.ValueObjects;
    using Xunit;

    public class ValueObjectTestCasesTests
    {
        [Fact]
        public void GetDistinctValues_ReturnsPairs()
        {
            IEnumerable<EmailAddress> expectedValues = new EmailAddress[]
            {
                new EmailAddress("bob@smith.com"),
                new EmailAddress("carol@smith.com"),
                new EmailAddress("bob@example.com"),
                new EmailAddress("carol@example.com"),
            };

            Assert.Equal(expected: expectedValues, actual: new EmailAddressValueObjectTestCases().GetDistinctValues());
        }

        [Fact]
        public void GetPermutatedEqualPairs_GetAdditionalEqualPairsReturnsEmptyCollection_ReturnsPairs()
        {
            RectangleValueObjectTestCases testCases = new RectangleValueObjectTestCases();

            IEnumerable<ValueObjectPair<Rectangle>> expectedPermutatedEqualPairs = new ValueObjectPair<Rectangle>[]
            {
                new ValueObjectPair<Rectangle>(new Rectangle(width: 5, height: 5), new Rectangle(width: 5, height: 5)),
                new ValueObjectPair<Rectangle>(new Rectangle(width: 5, height: 10), new Rectangle(width: 5, height: 10)),
                new ValueObjectPair<Rectangle>(new Rectangle(width: 5, height: 15), new Rectangle(width: 5, height: 15)),
                new ValueObjectPair<Rectangle>(new Rectangle(width: 10, height: 5), new Rectangle(width: 10, height: 5)),
                new ValueObjectPair<Rectangle>(new Rectangle(width: 10, height: 10), new Rectangle(width: 10, height: 10)),
            };

            Assert.Equal(expected: expectedPermutatedEqualPairs, actual: testCases.GetPermutatedEqualPairs());
            Assert.Equal(expected: Array.Empty<ValueObjectPair<Rectangle>>(), actual: testCases.GetAdditionalEqualPairs());
        }

        [Fact]
        public void GetPermutatedEqualPairs_ReturnsPairs()
        {
            IEnumerable<ValueObjectPair<EmailAddress>> expectedPermutatedEqualPairs = new ValueObjectPair<EmailAddress>[]
            {
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("bob@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("carol@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@example.com"), new EmailAddress("bob@example.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@example.com"), new EmailAddress("carol@example.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("BOB@SMITH.COM")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("BOB@SMITH.COM"), new EmailAddress("bob@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("Carol@Smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("Carol@Smith.com"), new EmailAddress("carol@smith.com")),
            };

            Assert.Equal(expected: expectedPermutatedEqualPairs, actual: new EmailAddressValueObjectTestCases().GetPermutatedEqualPairs());
        }

        [Fact]
        public void GetPermutatedNotEqualPairs_ReturnsPairs()
        {
            IEnumerable<ValueObjectPair<EmailAddress>> expectedPermutatedNotEqualPairs = new ValueObjectPair<EmailAddress>[]
            {
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("carol@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("bob@example.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), new EmailAddress("carol@example.com")),

                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("bob@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("bob@example.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@smith.com"), new EmailAddress("carol@example.com")),

                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@example.com"), new EmailAddress("bob@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@example.com"), new EmailAddress("carol@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("bob@example.com"),  new EmailAddress("carol@example.com")),

                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@example.com"), new EmailAddress("bob@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@example.com"), new EmailAddress("carol@smith.com")),
                new ValueObjectPair<EmailAddress>(new EmailAddress("carol@example.com"), new EmailAddress("bob@example.com")),
            };

            Assert.Equal(expected: expectedPermutatedNotEqualPairs, actual: new EmailAddressValueObjectTestCases().GetPermutatedNotEqualPairs());
        }
    }
}
