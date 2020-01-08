namespace Xtensions.ArgumentGuard.Fody.Tests.EnumGuardTests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;
    using Xunit;

    public class NormalEnums : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionForInvalidEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(Shape shape)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "shape",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { 10 }));

            Assert.Equal(
                expected: "10 is not a valid value for the Shape enum. (Parameter 'shape')",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionForInvalidNullableEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(Shape? shape)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "shape",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { (Shape)10 }));

            Assert.Equal(
                expected: "10 is not a valid value for the Shape enum. (Parameter 'shape')",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionValidEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(Shape shape)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { 2 })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNullNullableEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(Shape? shape)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
