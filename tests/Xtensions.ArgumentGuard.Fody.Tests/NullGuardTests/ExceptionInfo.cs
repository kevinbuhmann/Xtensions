namespace Xtensions.ArgumentGuard.Fody.Tests.NullGuardTests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class ExceptionInfo : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData("foo", "Parameter 'foo' is null. (Parameter 'foo')")]
        [OptimizationLevelData("bar", "Parameter 'bar' is null. (Parameter 'bar')")]
        [OptimizationLevelData("baz", "Parameter 'baz' is null. (Parameter 'baz')")]
        public void ThrowsArgumentNullExceptionWithCorrectParameterNameAndMessage(OptimizationLevel optimizationLevel, string paramName, string expectedExceptionMessage)
        {
            string sourceCode = @$"
                public static class Target
                {{
                    public static void TestMethod(string {paramName})
                    {{
                    }}
                }}";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(
                paramName: paramName,
                testCode: () => InvokeMethod(method: testMethod, parameters: new object?[] { null }));

            Assert.Equal(expected: expectedExceptionMessage, actual: exception.Message);
        }
    }
}
