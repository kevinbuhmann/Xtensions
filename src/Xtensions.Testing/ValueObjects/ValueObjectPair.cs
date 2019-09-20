namespace Xtensions.Testing.ValueObjects
{
    using System.Collections.Generic;
    using CSharpFunctionalExtensions;
    using EnsureThat;

    /// <summary>
    /// Represents a pair of instances of <typeparamref name="TValueObject"/>.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the value object.</typeparam>
    public class ValueObjectPair<TValueObject> : ValueObject
        where TValueObject : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectPair{TValueObject}"/> class.
        /// </summary>
        /// <param name="value1">An instance of <typeparamref name="TValueObject"/>.</param>
        /// <param name="value2">An instance of <typeparamref name="TValueObject"/> that is equal to <paramref name="value1"/> but not the same reference.</param>
        public ValueObjectPair(TValueObject value1, TValueObject value2)
        {
            EnsureArg.IsNotNull(value1, nameof(value1));
            EnsureArg.IsNotNull(value2, nameof(value2));
            EnsureArg.IsFalse(
                value: ReferenceEquals(value1, value2),
                optsFn: opts => opts.WithException(new ValueObjectPairNonDistinctReferenceException(value1)));

            this.Value1 = value1;
            this.Value2 = value2;
        }

        /// <summary>
        /// Gets the first instance of <typeparamref name="TValueObject"/>.
        /// </summary>
        /// <value>
        /// The first instance of <typeparamref name="TValueObject"/>.
        /// </value>
        public TValueObject Value1 { get; }

        /// <summary>
        /// Gets the second instance of <typeparamref name="TValueObject"/>.
        /// </summary>
        /// <value>
        /// The second instance of <typeparamref name="TValueObject"/>.
        /// </value>
        public TValueObject Value2 { get; }

        /// <summary>
        /// Returns the values as a array of two objects. This is used for xunit theory data.
        /// </summary>
        /// <returns>The values as a array of two objects.</returns>
        public object[] ToObjectArray()
        {
            return new object[] { this.Value1, this.Value2 };
        }

        /// <summary>
        /// Gets the values to check when determining if two instances are equal.
        /// </summary>
        /// <returns>The values to check when determining if two instances are equal.</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return this.Value1;
            yield return this.Value2;
        }
    }
}
