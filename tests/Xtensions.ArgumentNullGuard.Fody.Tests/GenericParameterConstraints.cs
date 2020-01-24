namespace Xtensions.ArgumentNullGuard.Fody.Tests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentNullGuard.Fody.Tests.Helpers;
    using Xunit;

    public class GenericParameterConstraints : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentNullExceptionForNonNullableGenericParameterWithReferenceTypeConstraint(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod<T>(T value)
                        where T : class
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(string));

            Assert.Throws<ArgumentNullException>(
                paramName: "value",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object?[] { null }));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNullableGenericParameterWithReferenceTypeConstraint(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod<T>(T? value)
                        where T : class
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(string));

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNonNullableGenericParameterWithValueTypeConstraint(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod<T>(T value)
                        where T : struct
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(int));

            // This is not possible without reflection, but it ensures that the null guard is not emitted.
            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNullableGenericParameterWithValueTypeConstraint(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod<T>(T? value)
                        where T : struct
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(int));

            // This is not possible without reflection, but it ensures that the null guard is not emitted.
            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNonNullableGenericParameterWithOnlyEnumConstraint(OptimizationLevel optimizationLevel)
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

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(StringComparison));

            // This is not possible without reflection, but it ensures that the null guard is not emitted.
            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNonNullableGenericParameterWithValueTypeAndEnumConstraints(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class Target
                {
                    public static void TestMethod<T>(T value)
                        where T : struct, Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(StringComparison));

            // This is not possible without reflection, but it ensures that the null guard is not emitted.
            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNullableGenericParameterWithValueTypeAndEnumConstraints(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class Target
                {
                    public static void TestMethod<T>(T? value)
                        where T : struct, Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(StringComparison));

            // This is not possible without reflection, but it ensures that the null guard is not emitted.
            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
