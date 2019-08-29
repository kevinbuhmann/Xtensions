namespace Xtensions.Testing.ValueObjects
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using CSharpFunctionalExtensions;

    /// <summary>
    /// This exception is thrown when <see cref="ValueObjectPair{TValueObject}"/> is constructed with two values that are the same reference.
    /// </summary>
    [SuppressMessage("Design", "CA1032:Implement standard exception constructors", Justification = "Message is not customizable.")]
    public class ValueObjectPairNonDistinctReferenceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueObjectPairNonDistinctReferenceException"/> class.
        /// </summary>
        /// <param name="value">The value that was used to attempt constructing <see cref="ValueObjectPair{TValueObject}"/>.</param>
        public ValueObjectPairNonDistinctReferenceException(object value)
            : base($"{nameof(ValueObjectPair<ValueObject>)} was constructed with two values that are the same reference. See the {nameof(Value)} property.")
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the value that was used to attempt constructing <see cref="ValueObjectPair{TValueObject}"/>.
        /// </summary>
        /// <value>
        /// The value that was used to attempt constructing <see cref="ValueObjectPair{TValueObject}"/>.
        /// </value>
        public object Value { get; }
    }
}
