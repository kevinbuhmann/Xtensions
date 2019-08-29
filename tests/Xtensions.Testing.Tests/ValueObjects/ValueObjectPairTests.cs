namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using Xtensions.Testing.Tests.ValueObjects.TestObjects;
    using Xtensions.Testing.ValueObjects;
    using Xunit;

    public class ValueObjectPairTests : ValueObjectTests<ValueObjectPair<EmailAddress>, ValueObjectPairValueObjectTestCases>
    {
        [Fact]
        public void Constructor_NullValue1_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                paramName: "value1",
                testCode: () => new ValueObjectPair<EmailAddress>(value1: null, new EmailAddress("bob@smith.com")));
        }

        [Fact]
        public void Constructor_NullValue2_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                paramName: "value2",
                testCode: () => new ValueObjectPair<EmailAddress>(new EmailAddress("bob@smith.com"), value2: null));
        }

        [Fact]
        public void Constructor_Value1ReferenceEqualsValue2_Throws()
        {
            EmailAddress emailAddress = new EmailAddress("bob@smith.com");

            Assert.Throws<ValueObjectPairNonDistinctReferenceException>(
                testCode: () => new ValueObjectPair<EmailAddress>(emailAddress, emailAddress));
        }

        [Fact]
        public void Constructor_ValidParameters_PopulatesProperties()
        {
            EmailAddress emailAddress1 = new EmailAddress("bob@smith.com");
            EmailAddress emailAddress2 = new EmailAddress("carol@smith.com");

            ValueObjectPair<EmailAddress> pair = new ValueObjectPair<EmailAddress>(emailAddress1, emailAddress2);

            Assert.Equal(expected: emailAddress1, actual: pair.Value1);
            Assert.Equal(expected: emailAddress2, actual: pair.Value2);
        }

        [Fact]
        public void ToObjectArray_ReturnsArrayOfValues()
        {
            EmailAddress emailAddress1 = new EmailAddress("bob@smith.com");
            EmailAddress emailAddress2 = new EmailAddress("carol@smith.com");

            ValueObjectPair<EmailAddress> pair = new ValueObjectPair<EmailAddress>(emailAddress1, emailAddress2);

            Assert.Equal(expected: new object[] { emailAddress1, emailAddress2 }, actual: pair.ToObjectArray());
        }
    }
}
