namespace Xtensions.ArgumentGuard.Fody.Tests.EnumGuardTests
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;
    using Xunit;

    public class OutParameters : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNonNullableOutParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(out Shape shape)
                    {
                        shape = Shape.Circle;
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNullableOutParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(out Shape? shape)
                    {
                        shape = Shape.Circle;
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNonNullableGenericOutParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class Target
                {
                    public static void TestMethod<T>(out T value)
                        where T : Enum
                    {
                        value = default!;
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape));

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNullableGenericOutParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;

                public static class Target
                {
                    public static void TestMethod<T>(out T? value)
                        where T : struct, Enum
                    {
                        value = default;
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape));

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
