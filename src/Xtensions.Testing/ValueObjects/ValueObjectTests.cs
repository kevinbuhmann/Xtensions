namespace Xtensions.Testing.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using CSharpFunctionalExtensions;
    using Newtonsoft.Json;
    using Xunit;

    /// <summary>
    /// Provides equality, inequality, hash code, and serialization tests for value objects.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the value object.</typeparam>
    /// <typeparam name="TValueObjectTestCases">The type that implements <see cref="IValueObjectTestCases{TValueObject}"/>.</typeparam>
    public abstract class ValueObjectTests<TValueObject, TValueObjectTestCases>
        where TValueObject : ValueObject
        where TValueObjectTestCases : IValueObjectTestCases<TValueObject>, new()
    {
#pragma warning disable CA1000 // Do not declare static members on generic types
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
        private static readonly TValueObjectTestCases ValueObjectTestCases = new TValueObjectTestCases();

        public static IEnumerable<object[]> EqualPairsValueObjectData { get; } = ValueObjectTestCases.GetPermutatedEqualPairs()
            .Select(pair => new object[] { pair.left, pair.right })
            .ToList();

        public static IEnumerable<object[]> NotEqualPairsValueObjectData { get; } = ValueObjectTestCases.GetPermutatedNotEqualPairs()
            .Select(pair => new object[] { pair.left, pair.right })
            .ToList();

        public static IEnumerable<object[]> ValueObjectData { get; } = ValueObjectTestCases.GetNotEqualValues()
            .Select(value => new object[] { value })
            .ToList();
#pragma warning restore CA1000 // Do not declare static members on generic types

#pragma warning disable CA1062 // Validate arguments of public methods
#pragma warning disable CA1707 // Identifiers should not contain underscores
        [Theory]
        [MemberData(nameof(EqualPairsValueObjectData))]
        public void Equals_EqualObjects_ReturnsTrue(TValueObject left, TValueObject right)
        {
            Assert.True(left.Equals(right));
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void Equals_ReferenceEqualObjects_ReturnsTrue(TValueObject value)
        {
            Assert.True(value.Equals(value));
        }

        [Theory]
        [MemberData(nameof(NotEqualPairsValueObjectData))]
        public void Equals_NotEqualObjects_ReturnsFalse(TValueObject left, TValueObject right)
        {
            Assert.False(left.Equals(right));
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void Equals_ValueEqualsNull_ReturnsFalse(TValueObject value)
        {
            Assert.False(value.Equals(null));
        }

        [Theory]
        [MemberData(nameof(EqualPairsValueObjectData))]
        public void EqualEqualOperator_EqualObjects_ReturnsTrue(TValueObject left, TValueObject right)
        {
            Assert.True(left == right);
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void EqualEqualOperator_ReferenceEqualObjects_ReturnsTrue(TValueObject value)
        {
            TValueObject sameReference = value;

            Assert.True(value == sameReference);
        }

        [Theory]
        [MemberData(nameof(NotEqualPairsValueObjectData))]
        public void EqualEqualOperator_NotEqualObjects_ReturnsFalse(TValueObject left, TValueObject right)
        {
            Assert.False(left == right);
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void EqualEqualOperator_ValueEqualEqualNull_ReturnsFalse(TValueObject value)
        {
            TValueObject nullValue = null;

            Assert.False(value == nullValue);
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void EqualEqualOperator_NullEqualEqualValue_ReturnsFalse(TValueObject value)
        {
            TValueObject nullValue = null;

            Assert.False(nullValue == value);
        }

        [Theory]
        [MemberData(nameof(EqualPairsValueObjectData))]
        public void NotEqualOperator_EqualObjects_ReturnFalse(TValueObject left, TValueObject right)
        {
            Assert.False(left != right);
        }

        [Theory]
        [MemberData(nameof(NotEqualPairsValueObjectData))]
        public void NotEqualOperator_NotEqualObjects_ReturnTrue(TValueObject left, TValueObject right)
        {
            Assert.True(left != right);
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void EqualEqualOperator_ValueNotEqualNull_ReturnsTrue(TValueObject value)
        {
            TValueObject nullValue = null;

            Assert.True(value != nullValue);
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void EqualEqualOperator_NullNotEqualValue_ReturnsTrue(TValueObject value)
        {
            TValueObject nullValue = null;

            Assert.True(nullValue != value);
        }

        [Theory]
        [MemberData(nameof(EqualPairsValueObjectData))]
        public void GetHashCode_EqualObjects_ReturnsSameHashCode(TValueObject left, TValueObject right)
        {
            Assert.Equal(expected: left.GetHashCode(), actual: right.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(NotEqualPairsValueObjectData))]
        public void GetHashCode_NotEqualObjects_ReturnsDifferentHashCode(TValueObject left, TValueObject right)
        {
            Assert.NotEqual(expected: left.GetHashCode(), actual: right.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(ValueObjectData))]
        public void Serialization_DeserializedValueEqualsSerializedValue(TValueObject value)
        {
            Assert.Equal(
                expected: value,
                actual: JsonConvert.DeserializeObject<TValueObject>(JsonConvert.SerializeObject(value)));
        }

        [Theory]
        [MemberData(nameof(EqualPairsValueObjectData))]
        public void TestCaseVerification_GetEqualPairs_NoPairConsistsOfSameReference(TValueObject left, TValueObject right)
        {
            Assert.False(ReferenceEquals(left, right));
        }
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CA1707 // Identifiers should not contain underscores
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning restore CA1062 // Validate arguments of public methods
    }
}
