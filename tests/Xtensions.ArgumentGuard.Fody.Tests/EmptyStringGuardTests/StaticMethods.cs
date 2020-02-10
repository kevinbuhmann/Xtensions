namespace Xtensions.ArgumentGuard.Fody.Tests.EmptyStringGuardTests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class StaticMethods : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionEmptyStringArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod(string value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { string.Empty }));

            Assert.Equal(
                expected: "Parameter 'value' is empty. (Parameter 'value')",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionEmptyNullableStringArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod(string? value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "value",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { string.Empty }));

            Assert.Equal(
                expected: "Parameter 'value' is empty. (Parameter 'value')",
                actual: exception.Message);
        }
    }
}
