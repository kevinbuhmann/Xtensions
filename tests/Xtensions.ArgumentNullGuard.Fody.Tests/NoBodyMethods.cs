namespace Xtensions.ArgumentNullGuard.Fody.Tests
{
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentNullGuard.Fody.Tests.Helpers;
    using Xunit;

    public class NoBodyMethods : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void DoesNotProcessInterfaces(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public interface ITarget
                {
                    void TestMethod(string value);
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);

            Assert.Null(assembly.GetType(HelperMethods.ClassName));
        }

        [Theory]
        [OptimizationLevelData]
        public void DoesNotProcessAbstactMethods(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                public abstract class BaseTarget
                {
                    public abstract void TestMethod(string value);
                }";

            Assembly assembly = this.WeaveAssembly(sourceCode, optimizationLevel);

            Assert.Null(assembly.GetType(HelperMethods.ClassName));
        }
    }
}
