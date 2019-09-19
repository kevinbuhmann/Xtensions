namespace Xtensions.Testing.Tests.ValueObjects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using CSharpFunctionalExtensions;
    using Xtensions.Testing.ValueObjects;
    using Xunit;

    public class FactoryExpressionCollectionTests
    {
        [Fact]
        public void Constructor_NullExpressions_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                paramName: "expressions",
                testCode: () => new FactoryExpressionCollection<Person>(expressions: null));
        }

        [Fact]
        public void Constructor_NoExpressions_Throws()
        {
            IEnumerable<Expression<Func<Person>>> expressions = Enumerable.Empty<Expression<Func<Person>>>();

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "expressions",
                testCode: () => new FactoryExpressionCollection<Person>(expressions));

            Assert.Equal(
                expected: "At least one factory expression is required.\r\nParameter name: expressions",
                actual: exception.Message);
        }

        [Fact]
        public void Constructor_ExpressionIsNotConstructorInvocation_Throws()
        {
            IEnumerable<Expression<Func<Person>>> expressions = new Expression<Func<Person>>[]
            {
                () => new Person("Jane"),
                () => GetPerson(),
            };

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "expressions",
                testCode: () => new FactoryExpressionCollection<Person>(expressions));

            Assert.Equal(
                expected: "Expression 'GetPerson()' is not an invocation of a 'Person' constructor.\r\nParameter name: expressions",
                actual: exception.Message);
        }

        [Fact]
        public void Constructor_ExpressionConstructsAnDifferentType_Throws()
        {
            IEnumerable<Expression<Func<Person>>> expressions = new Expression<Func<Person>>[]
            {
                () => new Person("Jane"),
                () => new Employee("Bob"),
            };

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "expressions",
                testCode: () => new FactoryExpressionCollection<Person>(expressions));

            Assert.Equal(
                expected: "Expression 'new Employee(\"Bob\")' is not an invocation of a 'Person' constructor.\r\nParameter name: expressions",
                actual: exception.Message);
        }

        [Fact]
        public void Constructor_ValidExpressions_DoesNotThrow()
        {
            IEnumerable<Expression<Func<Person>>> expressions = new Expression<Func<Person>>[]
            {
                () => new Person("Jane"),
                () => new Person("Bob"),
            };

            _ = new FactoryExpressionCollection<Person>(expressions);
        }

        [Fact]
        public void Count_ReturnsCount()
        {
            IEnumerable<Expression<Func<Person>>> expressions = new Expression<Func<Person>>[]
            {
                () => new Person("Jane"),
                () => new Person("Bob"),
            };

            FactoryExpressionCollection<Person> collection = new FactoryExpressionCollection<Person>(expressions);

            Assert.Equal(expected: 2, actual: collection.Count);
        }

        [Fact]
        public void GetEnumerator_ReturnsWorkingEnumerator()
        {
            Expression<Func<Person>> expression1 = () => new Person("Jane");
            Expression<Func<Person>> expression2 = () => new Person("Bob");

            IEnumerable<Expression<Func<Person>>> expressions = new Expression<Func<Person>>[]
            {
                expression1,
                expression2,
            };

            FactoryExpressionCollection<Person> collection = new FactoryExpressionCollection<Person>(expressions);

            IEnumerator<Expression<Func<Person>>> enumerator = collection.GetEnumerator();

            Assert.True(enumerator.MoveNext());
            Assert.Same(expected: expression1, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Same(expected: expression2, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        [Fact]
        public void IEnumerableGetEnumerator_ReturnsWorkingEnumerator()
        {
            Expression<Func<Person>> expression1 = () => new Person("Jane");
            Expression<Func<Person>> expression2 = () => new Person("Bob");

            IEnumerable<Expression<Func<Person>>> expressions = new Expression<Func<Person>>[]
            {
                expression1,
                expression2,
            };

            FactoryExpressionCollection<Person> collection = new FactoryExpressionCollection<Person>(expressions);

            IEnumerator enumerator = (collection as IEnumerable).GetEnumerator();

            Assert.True(enumerator.MoveNext());
            Assert.Same(expected: expression1, enumerator.Current);
            Assert.True(enumerator.MoveNext());
            Assert.Same(expected: expression2, enumerator.Current);
            Assert.False(enumerator.MoveNext());
        }

        private static Person GetPerson()
        {
            return new Person("Bob");
        }

        private class Person : ValueObject
        {
            public Person(string name)
            {
                this.Name = name;
            }

            public string Name { get; }

            protected override IEnumerable<object> GetEqualityComponents()
            {
                return new object[] { this.Name };
            }
        }

        private class Employee : Person
        {
            public Employee(string name)
                : base(name: name)
            {
            }
        }
    }
}
