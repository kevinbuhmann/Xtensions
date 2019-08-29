namespace Xtensions.Testing.ValueObjects
{
    using System.Collections.Generic;

    /// <summary>
    /// Provides test cases for <see cref="ValueObjectTests{TValueObject, TValueObjectTestCases}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the value object.</typeparam>
    public interface IValueObjectTestCases<T>
    {
        /// <summary>
        /// Gets a collection of distinct and equal instances of <typeparamref name="T" />.
        /// </summary>
        /// <returns>An collection of equal pairs of instances of <typeparamref name="T" />.</returns>
        IEnumerable<(T value1, T value2)> GetEqualPairs();

        /// <summary>
        /// Gets a collection of not equal instances of <typeparamref name="T" />.
        /// </summary>
        /// <returns>An collection of not equal instances of <typeparamref name="T" />.</returns>
        IEnumerable<T> GetNotEqualValues();
    }
}
