namespace Xtensions.ArgumentGuard.Fody.Tests.GeneralTests
{
    using global::Fody;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class EnumValueTypeConstraint : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsWeavingExceptionWhenValueTypeConstraintIsMissingOnGenericMethod(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class Target
                {
                    public static void TestMethod<T>(T value)
                        where T : Enum
                    {
                    }
                }";

            WeavingException exception = Assert.Throws<WeavingException>(() => this.WeaveAssembly(sourceCode, optimizationLevel));

            Assert.Equal(
                expected: "Generic parameter 'T' of method (or one of its containing types) 'Target.TestMethod' should be constrained to value types (enums are always value types).",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void ThrowsWeavingExceptionWhenValueTypeConstraintIsMissingOnGenericType(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class Target<T>
                    where T : Enum
                {
                    public static void TestMethod(T value)
                    {
                    }
                }";

            WeavingException exception = Assert.Throws<WeavingException>(() => this.WeaveAssembly(sourceCode, optimizationLevel));

            Assert.Equal(
                expected: "Generic parameter 'T' of method (or one of its containing types) 'Target`1.TestMethod' should be constrained to value types (enums are always value types).",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void ThrowsWeavingExceptionWhenValueTypeConstraintIsMissingOnParentGenericType(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class ParentTarget<T>
                    where T : Enum
                {
                    public static class NestedTarget
                    {
                        public static void TestMethod(T value)
                        {
                        }
                    }
                }";

            WeavingException exception = Assert.Throws<WeavingException>(() => this.WeaveAssembly(sourceCode, optimizationLevel));

            Assert.Equal(
                expected: "Generic parameter 'T' of method (or one of its containing types) 'ParentTarget`1/NestedTarget.TestMethod' should be constrained to value types (enums are always value types).",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void ThrowsWeavingExceptionWhenValueTypeConstraintIsMissingOnNestedGenericType(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class ParentTarget
                {
                    public static class NestedTarget<T>
                        where T : Enum
                    {
                        public static void TestMethod(T value)
                        {
                        }
                    }
                }";

            WeavingException exception = Assert.Throws<WeavingException>(() => this.WeaveAssembly(sourceCode, optimizationLevel));

            Assert.Equal(
                expected: "Generic parameter 'T' of method (or one of its containing types) 'ParentTarget/NestedTarget`1.TestMethod' should be constrained to value types (enums are always value types).",
                actual: exception.Message);
        }
    }
}
