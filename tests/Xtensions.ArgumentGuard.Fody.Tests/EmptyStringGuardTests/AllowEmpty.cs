namespace Xtensions.ArgumentGuard.Fody.Tests.EmptyStringGuardTests
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class AllowEmpty : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForIfParameterHasAllowEmptyAttribute(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard;

                public static class Target
                {
                    public static void TestMethod([AllowEmpty] string value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { string.Empty })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForIfNullableParameterHasAllowEmptyAttribute(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard;

                public static class Target
                {
                    public static void TestMethod([AllowEmpty] string? value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { string.Empty })));
        }
    }
}
