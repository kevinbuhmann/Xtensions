namespace Xtensions.ArgumentNullGuard.Fody.Tests
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentNullGuard.Fody.Tests.Helpers;
    using Xunit;

    public class GeneratedCode : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData("System.CodeDom.Compiler.GeneratedCodeAttribute(null, null)")]
        [OptimizationLevelData("System.Runtime.CompilerServices.CompilerGeneratedAttribute")]
        public void DoesNotThrowArgumentNullExceptionForNonNullableParameterWhenClassIsGenerated(OptimizationLevel optimizationLevel, string attribute)
        {
            string sourceCode = @$"
                [{attribute}]
                public static class Target
                {{
                    public static void TestMethod(string value)
                    {{
                    }}
                }}";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }

        [Theory]
        [OptimizationLevelData("System.CodeDom.Compiler.GeneratedCodeAttribute(null, null)")]
        [OptimizationLevelData("System.Runtime.CompilerServices.CompilerGeneratedAttribute")]
        public void DoesNotThrowArgumentNullExceptionForNonNullableParameterWhenMethodIsGenerated(OptimizationLevel optimizationLevel, string attribute)
        {
            string sourceCode = @$"
                public static class Target
                {{
                    [{attribute}]
                    public static void TestMethod(string value)
                    {{
                    }}
                }}";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
