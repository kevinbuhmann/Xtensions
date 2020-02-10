namespace Xtensions.ArgumentGuard.Fody.Tests.EmptyStringGuardTests
{
    using global::Fody;
    using Microsoft.CodeAnalysis;
    using Xtensions.ArgumentGuard.Fody.Tests.Helpers;
    using Xunit;

    public class ValidateMetdata : BaseModuleWeaverTests
    {
        [Theory]
        [OptimizationLevelData]
        public void ThrowsWeavingExceptionIfNonStringParameterHasAllowEmptyAttribute(OptimizationLevel optimizationLevel)
        {
            string sourceCode = @"
                using Xtensions.ArgumentGuard;

                public class Target
                {
                    public static void TestMethod([AllowEmpty] Target value)
                    {
                    }
                }";

            WeavingException exception = Assert.Throws<WeavingException>(() => this.WeaveAssembly(sourceCode, optimizationLevel));

            Assert.Equal(
                expected: "Attribute 'Xtensions.ArgumentGuard.AllowEmptyAttribute' is not allowed on parameter 'value' of method 'Target.TestMethod'.",
                actual: exception.Message);
        }
    }
}
