namespace Xtensions.Testing.Tests.ValueObjects
{
    using Xtensions.Testing.ValueObjects;
    using Xunit;

    public class ValueObjectPairNonDistinctReferenceExceptionTests
    {
        [Fact]
        public void Constructor_PopulatesProperties()
        {
            ValueObjectPairNonDistinctReferenceException exception = new ValueObjectPairNonDistinctReferenceException("value");

            Assert.Equal(
                expected: "ValueObjectPair was constructed with two values that are the same reference. See the Value property.",
                actual: exception.Message);
            Assert.Equal(expected: "value", actual: exception.Value);
        }
    }
}
