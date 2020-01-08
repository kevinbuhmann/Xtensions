namespace Xtensions.ArgumentGuard.Fody.Tests.EnumGuardTests
{
    using System;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums;
    using Xunit;

    public class GenericMethods : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionForInvalidEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;
                
                public static class Target
                {
                    public static void TestMethod<T>(T value)
                        where T : Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape))!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "value",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { 10 }));

            Assert.Equal(
                expected: "10 is not a valid value for the Shape enum. (Parameter 'value')",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void ThrowsArgumentExceptionForInvalidNullableEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;
                
                public static class Target
                {
                    public static void TestMethod<T>(T? value)
                        where T : struct, Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape))!;

            ArgumentException exception = Assert.Throws<ArgumentException>(
                paramName: "value",
                testCode: () => InvokeMethod(method: testMethod, parameters: new object[] { (Shape)10 }));

            Assert.Equal(
                expected: "10 is not a valid value for the Shape enum. (Parameter 'value')",
                actual: exception.Message);
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionValidEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;
                
                public static class Target
                {
                    public static void TestMethod<T>(T value)
                        where T : Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape))!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { 2 })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionValidNullableEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;
                
                public static class Target
                {
                    public static void TestMethod<T>(T? value)
                        where T : struct, Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape))!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object[] { (Shape)2 })));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotThrowArgumentExceptionForNullNullableEnumArgument(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using System;
                
                public static class Target
                {
                    public static void TestMethod<T>(T? value)
                        where T : struct, Enum
                    {
                    }
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);
            MethodInfo testMethod = assembly.GetType("Target")!.GetMethod("TestMethod")!.MakeGenericMethod(typeof(Shape))!;

            Assert.Null(Record.Exception(() => InvokeMethod(method: testMethod, parameters: new object?[] { null })));
        }
    }
}
