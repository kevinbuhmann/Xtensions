namespace Xtensions.ArgumentGuard.Fody.Tests.NullGuardTests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class VirtualMethods : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentNullExceptionForNonNullableParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public class Target
                {
                    public virtual void TestMethod(string value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            Type targetType = assembly.GetType("Target")!;
            MethodInfo testMethod = targetType!.GetMethod("TestMethod")!;

            object target = Activator.CreateInstance(targetType)!;

            Assert.Throws<ArgumentNullException>(
                paramName: "value",
                testCode: () => InvokeMethod(method: testMethod, target: target, parameters: new object?[] { null }));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNonNullableParameterWhenNonNullValueIsPassed(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public class Target
                {
                    public virtual void TestMethod(string? value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            Type targetType = assembly.GetType("Target")!;
            MethodInfo testMethod = targetType!.GetMethod("TestMethod")!;

            object target = Activator.CreateInstance(targetType)!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, target: target, parameters: new object?[] { "test-value" })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentNullExceptionForNullableParameter(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public class Target
                {
                    public virtual void TestMethod(string? value)
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            Type targetType = assembly.GetType("Target")!;
            MethodInfo testMethod = targetType!.GetMethod("TestMethod")!;

            object target = Activator.CreateInstance(targetType)!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, target: target, parameters: new object?[] { null })));
        }
    }
}
