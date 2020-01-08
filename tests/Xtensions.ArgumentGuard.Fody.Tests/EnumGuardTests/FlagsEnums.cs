namespace Xtensions.ArgumentGuard.Fody.Tests.EnumGuardTests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;
    using Xunit;

    public class FlagsEnums : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionForInvalidEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(ItemFlags itemFlags)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "itemFlags",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { 25 }));

            Assert.Equal(
                expected: "25 is not a valid value for the ItemFlags enum. (Parameter 'itemFlags')",
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
                    public static void TestMethod(ItemFlags? itemFlags)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "itemFlags",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { (ItemFlags)25 }));

            Assert.Equal(
                expected: "25 is not a valid value for the ItemFlags enum. (Parameter 'itemFlags')",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionValidSingleBitEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(ItemFlags itemFlags)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { 2 })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionValidMultipleBitEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(ItemFlags itemFlags)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { 10 })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNullNullableEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;

                public static class Target
                {
                    public static void TestMethod(ItemFlags? itemFlags)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
