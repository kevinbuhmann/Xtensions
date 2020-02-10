namespace Xtensions.ArgumentGuard.Fody.Tests.EmptyStringGuardTests
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class OutParameters : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNonNullableOutParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
  
                public static class Target
                {
                    public static void TestMethod(out string value)
                    {
                        value = ""value"";
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
                public static class Target
                {
                    public static void TestMethod(out string? value)
                    {
                        value = ""value"";
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
