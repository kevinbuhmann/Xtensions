namespace Xtensions.Testing.Tests.ValueObjects
{
    using Xtensions.Testing.Tests.ValueObjects.BadBehavior;
    using Xunit;
    using Xunit.Sdk;

    public class ValueObjectTests_FailureTests
    {
        [Fact]
        public void Equals_EqualObjects_ReturnsTrue()
        {
            AlwaysNotEqualObject left = new AlwaysNotEqualObject();
            AlwaysNotEqualObject right = new AlwaysNotEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysNotEqualObjectTests().Equals_EqualObjects_ReturnsTrue(left, right));
        }

        [Fact]
        public void Equals_ReferenceEqualObjects_ReturnsTrue()
        {
            AlwaysNotEqualObject value = new AlwaysNotEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysNotEqualObjectTests().Equals_ReferenceEqualObjects_ReturnsTrue(value));
        }

        [Fact]
        public void Equals_NotEqualObjects_ReturnsFalse()
        {
            AlwaysEqualObject left = new AlwaysEqualObject();
            AlwaysEqualObject right = new AlwaysEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysEqualObjectTests().Equals_NotEqualObjects_ReturnsFalse(left, right));
        }

        [Fact]
        public void Equals_ValueEqualsNull_ReturnsFalse()
        {
            AlwaysEqualObject value = new AlwaysEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysEqualObjectTests().Equals_ValueEqualsNull_ReturnsFalse(value));
        }

        [Fact]
        public void EqualEqualOperator_ReferenceEqualObjects_ReturnsTrue()
        {
            AlwaysNotEqualObject value = new AlwaysNotEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysNotEqualObjectTests().EqualEqualOperator_ReferenceEqualObjects_ReturnsTrue(value));
        }

        [Fact]
        public void EqualEqualOperator_NotEqualObjects_ReturnsFalse()
        {
            AlwaysEqualObject left = new AlwaysEqualObject();
            AlwaysEqualObject right = new AlwaysEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysEqualObjectTests().EqualEqualOperator_NotEqualObjects_ReturnsFalse(left, right));
        }

        [Fact]
        public void NotEqualOperator_EqualObjects_ReturnFalse()
        {
            AlwaysNotEqualObject left = new AlwaysNotEqualObject();
            AlwaysNotEqualObject right = new AlwaysNotEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysNotEqualObjectTests().NotEqualOperator_EqualObjects_ReturnFalse(left, right));
        }

        [Fact]
        public void NotEqualOperator_NotEqualObjects_ReturnTrue()
        {
            AlwaysEqualObject left = new AlwaysEqualObject();
            AlwaysEqualObject right = new AlwaysEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysEqualObjectTests().NotEqualOperator_NotEqualObjects_ReturnTrue(left, right));
        }

        [Fact]
        public void GetHashCode_EqualObjects_ReturnsSameHashCode()
        {
            AlwaysNotEqualObject left = new AlwaysNotEqualObject();
            AlwaysNotEqualObject right = new AlwaysNotEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysNotEqualObjectTests().GetHashCode_EqualObjects_ReturnsSameHashCode(left, right));
        }

        [Fact]
        public void GetHashCode_NotEqualObjects_ReturnsDifferentHashCode()
        {
            AlwaysEqualObject left = new AlwaysEqualObject();
            AlwaysEqualObject right = new AlwaysEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysEqualObjectTests().GetHashCode_NotEqualObjects_ReturnsDifferentHashCode(left, right));
        }

        [Fact]
        public void Serialization_DeserializedValueEqualsSerializedValue()
        {
            AlwaysNotEqualObject value = new AlwaysNotEqualObject();

            Assert.ThrowsAny<XunitException>(() => new AlwaysNotEqualObjectTests().Serialization_DeserializedValueEqualsSerializedValue(value));
        }
    }
}
