namespace Xtensions.ArgumentNullGuard.Fody.Tests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentNullGuard.Fody.Tests.Helpers;
    using Xunit;

    public class StaticMethods : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentNullExceptionForNonNullableParameter(OptimizationLevel optimizationLevel)
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

            Assert.Throws<ArgumentNullException>(
                paramName: "value",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object?[] { null }));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNonNullableParameterWhenNonNullValueIsPassed(OptimizationLevel optimizationLevel)
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

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { "test-value" })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNullableParameter(OptimizationLevel optimizationLevel)
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

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
