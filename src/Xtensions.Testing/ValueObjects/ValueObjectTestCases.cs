namespace Xtensions.Testing.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CSharpFunctionalExtensions;

    /// <summary>
    /// Provides test cases for <see cref="ValueObjectTests{TValueObject, TValueObjectTestCases}"/>.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the value object.</typeparam>
    public abstract class ValueObjectTestCases<TValueObject>
        where TValueObject : ValueObject
    {
        /// <summary>
        /// Gets a value indicating whether the tests should assert that the constructor(s) populate properties properly.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the tests should assert that the constructor(s) populate properties properly; otherwise, <c>false</c>.
        /// </value>
        public virtual bool TestConstructorPropertyAssignment { get; } = true;

        /// <summary>
        /// Gets a collecton of <see cref="NewExpression"/>.
        /// </summary>
        /// <returns>A collection of <see cref="NewExpression"/>.</returns>
        public IEnumerable<NewExpression> GetNewExpressions()
        {
            return this.GetDistinctFactoryExpressions().Select(factoryExpression => factoryExpression.Body as NewExpression);
        }

        /// <summary>
        /// Gets distinct values.
        /// </summary>
        /// <returns>A collection of distinct values.</returns>
        public IEnumerable<TValueObject> GetDistinctValues()
        {
            return this.GetDistinctValueFactories().Select(valueFn => valueFn());
        }

        /// <summary>
        /// Gets permutated pairs of equal objects.
        /// </summary>
        /// <returns>A collection of permutated pairs of equal objects.</returns>
        public IEnumerable<ValueObjectPair<TValueObject>> GetPermutatedEqualPairs()
        {
            IEnumerable<ValueObjectPair<TValueObject>> distinctValuePairs = this.GetDistinctValueFactories()
                .Select(valueFn => new ValueObjectPair<TValueObject>(valueFn(), valueFn()));

            IEnumerable<ValueObjectPair<TValueObject>> permutatedAdditionalEqualPairs = this.GetAdditionalEqualPairs()
                .SelectMany(pair => (new ValueObjectPair<TValueObject>[]
                {
                    new ValueObjectPair<TValueObject>(pair.Value1, pair.Value2),
                    new ValueObjectPair<TValueObject>(pair.Value2, pair.Value1),
                }));

            return distinctValuePairs.Concat(permutatedAdditionalEqualPairs);
        }

        /// <summary>
        /// Gets permutated pairs of not equal objects.
        /// </summary>
        /// <returns>A collection of permutated pairs of not equal objects.</returns>
        public IEnumerable<ValueObjectPair<TValueObject>> GetPermutatedNotEqualPairs()
        {
            IReadOnlyCollection<TValueObject> values = this.GetDistinctValues().ToList();

            return values.SelectMany(
                collectionSelector: item => values.Where(innerItem => ReferenceEquals(item, innerItem) == false),
                resultSelector: (value1, value2) => new ValueObjectPair<TValueObject>(value1, value2));
        }

        /// <summary>
        /// Gets a collection of factory expressions that each generate an instance <typeparamref name="TValueObject" />.
        /// No two expressions should return "equal" instances, see <see cref="GetAdditionalEqualPairs" />.
        /// </summary>
        /// <returns>A collection of factory expressions for <typeparamref name="TValueObject" />.</returns>
        public abstract FactoryExpressionCollection<TValueObject> GetDistinctFactoryExpressions();

        /// <summary>
        /// Gets a collection of additional equal pairs of instances of <typeparamref name="TValueObject" />.
        /// Use this method when the "equal" values need to be constructed separately. It is not necessary
        /// to return pairs constructed identically in the method. This method can return an empty collection.
        /// Example use case: An email value object that considers "bob@smith.com" to equal "BOB@SMITH.COM".
        /// </summary>
        /// <returns>A collection of equal pairs of instances of <typeparamref name="TValueObject" />.</returns>
        public virtual IEnumerable<ValueObjectPair<TValueObject>> GetAdditionalEqualPairs()
        {
            return Enumerable.Empty<ValueObjectPair<TValueObject>>();
        }

        private IEnumerable<Func<TValueObject>> GetDistinctValueFactories()
        {
            return this.GetDistinctFactoryExpressions().Select(valueExpression => valueExpression.Compile());
        }
    }
}
