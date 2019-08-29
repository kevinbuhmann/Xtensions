namespace Xtensions.Testing.ValueObjects
{
    using System.Collections.Generic;
    using System.Linq;
    using EnsureThat;

    /// <summary>
    /// Extension methods permutating test cases provided by an implementation of <see cref="IValueObjectTestCases{T}"/>.
    /// </summary>
    public static class ValueObjectTestCasesExtensions
    {
        /// <summary>
        /// Gets permutated pairs of equal objects.
        /// </summary>
        /// <typeparam name="T">The type of the value object.</typeparam>
        /// <param name="valueObjectTestCases">The value object test cases.</param>
        /// <returns>A collection of permutated pairs of equal objects.</returns>
        public static IEnumerable<(T left, T right)> GetPermutatedEqualPairs<T>(this IValueObjectTestCases<T> valueObjectTestCases)
        {
            EnsureArg.IsNotNull(valueObjectTestCases, nameof(valueObjectTestCases));

            return valueObjectTestCases.GetEqualPairs()
                .SelectMany(pair => new (T left, T right)[] { (pair.value1, pair.value2), (pair.value2, pair.value1) });
        }

        /// <summary>
        /// Gets permutated pairs of not equal objects.
        /// </summary>
        /// <typeparam name="T">The type of the value object.</typeparam>
        /// <param name="valueObjectTestCases">The value object test cases.</param>
        /// <returns>A collection of permutated pairs of not equal objects.</returns>
        public static IEnumerable<(T left, T right)> GetPermutatedNotEqualPairs<T>(this IValueObjectTestCases<T> valueObjectTestCases)
        {
            EnsureArg.IsNotNull(valueObjectTestCases, nameof(valueObjectTestCases));

            IReadOnlyCollection<T> values = valueObjectTestCases.GetNotEqualValues().ToList();

            return values.SelectMany(
                collectionSelector: item => values.Where(innerItem => ReferenceEquals(item, innerItem) == false),
                resultSelector: (left, right) => (left, right));
        }
    }
}
