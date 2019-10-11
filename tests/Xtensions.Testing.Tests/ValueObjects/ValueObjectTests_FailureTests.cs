namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using System.Linq.Expressions;
    using Xtensions.Testing.Tests.ValueObjects.BadBehavior;
    using Xtensions.Testing.Tests.ValueObjects.Skipping;
    using Xunit;
    using Xunit.Sdk;

    public class ValueObjectTests_FailureTests
    {
        [Fact]
        public void Constructor_PopulatesProperties_Skipped()
        {
            Expression<Func<SkipConstructorPopulatesPropertiesObject>> factoryExpression = () => new SkipConstructorPopulatesPropertiesObject(1);
            NewExpression newExpression = factoryExpression.Body as NewExpression;

            SkipException exception = Assert.Throws<SkipException>(
                () => new SkipConstructorPopulatesPropertiesObjectTests().Constructor_PopulatesProperties(newExpression));

            Assert.Equal(
                expected: "The test cases for this value object opt out of this test.",
                actual: exception.Message);
        }

        [Fact]
        public void Constructor_PopulatesProperties_PropertyDoesNotExist()
        {
            Expression<Func<NoPropertiesObject>> factoryExpression = () => new NoPropertiesObject(1);
            NewExpression newExpression = factoryExpression.Body as NewExpression;

            XunitException exception = Assert.ThrowsAny<XunitException>(
                () => new NoPropertiesObjectTests().Constructor_PopulatesProperties(newExpression));

            Assert.Equal(
                expected: "No property matches the 'value' constructor parameter.",
                actual: exception.Message);
        }

        [Fact]
        public void Constructor_PopulatesProperties_PropertyNotAssignedProperly()
        {
            Expression<Func<PropertyValueIncrementedObject>> factoryExpression = () => new PropertyValueIncrementedObject(1);
            NewExpression newExpression = factoryExpression.Body as NewExpression;

            XunitException exception = Assert.ThrowsAny<XunitException>(
                () => new PropertyValueIncrementedObjectTests().Constructor_PopulatesProperties(newExpression));

            Assert.Equal(
                expected: "The 'Value' property was not assigned properly.\r\n'value' parameter value: 1\r\n'Value' property value:  2",
                actual: exception.Message);
        }

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
