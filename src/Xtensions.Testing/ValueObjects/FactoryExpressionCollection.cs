namespace Xtensions.Testing.ValueObjects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CSharpFunctionalExtensions;
    using EnsureThat;

    /// <summary>
    /// A collection of expressions that invoke a value object's constructor.
    /// </summary>
    /// <typeparam name="TValueObject">The type of the value object.</typeparam>
    public class FactoryExpressionCollection<TValueObject> : IReadOnlyCollection<Expression<Func<TValueObject>>>
        where TValueObject : ValueObject
    {
        private readonly IReadOnlyCollection<Expression<Func<TValueObject>>> expressions;

        /// <summary>
        /// Initializes a new instance of the <see cref="FactoryExpressionCollection{TValueObject}"/> class.
        /// </summary>
        /// <param name="expressions">The expressions.</param>
        public FactoryExpressionCollection(IEnumerable<Expression<Func<TValueObject>>> expressions)
        {
            EnsureArg.IsNotNull(expressions, nameof(expressions));

            Type valueObjectType = typeof(TValueObject);
            IReadOnlyCollection<Expression<Func<TValueObject>>> expressionsList = expressions.ToList();

            EnsureArg.IsTrue(
                value: expressionsList.Any(),
                paramName: nameof(expressions),
                optsFn: opts => opts.WithMessage("At least one factory expression is required."));

            foreach (Expression<Func<TValueObject>> expression in expressionsList)
            {
                EnsureArg.IsTrue(
                    value: expression.Body is NewExpression newExpression && newExpression.Type == valueObjectType,
                    paramName: nameof(expressions),
                    optsFn: opts => opts.WithMessage($"Expression '{expression.Body}' is not an invocation of a '{valueObjectType.Name}' constructor."));
            }

            this.expressions = expressionsList;
        }

        /// <summary>
        /// Gets the number of elements in the collection.
        /// </summary>
        /// <value>
        /// The number of elements in the collection.
        /// </value>
        public int Count => this.expressions.Count;

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<Expression<Func<TValueObject>>> GetEnumerator()
        {
            return this.expressions.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.expressions.GetEnumerator();
        }
    }
}
