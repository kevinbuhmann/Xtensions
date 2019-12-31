namespace Xtensions.ArgumentNullGuard.Fody.Tests
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentNullGuard.Fody.Tests.Helpers;
    using Xunit;

    public class ValueTypeParameters : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForValueTypeParameterWhenNullIsPassed(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod(int value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            // This is not possible without reflection, but it ensures that the null guard is not emitted.
            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForValueTypeParameterWhenDefaultValueIsPassed(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod(int value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { default(int) })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForValueTypeParameterWhenNonDefaultValueIsPassed(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public static class Target
                {
                    public static void TestMethod(int value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { 1 })));
        }
    }
}
