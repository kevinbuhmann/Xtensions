namespace Xtensions.ArgumentGuard.Fody.Tests.Helpers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.CodeAnalysis;
    using Xunit.Sdk;

    public sealed class OptimizationLevelDataAttribute : DataAttribute
    {
        private static readonly IReadOnlyCollection<OptimizationLevel> OptimizationLevels = new[]
        {
            OptimizationLevel.Debug,
            OptimizationLevel.Release,
        };

        public OptimizationLevelDataAttribute(params object[] otherParams)
        {
            this.OtherParams = otherParams;
        }

        public IReadOnlyCollection<object> OtherParams { get; }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            return OptimizationLevels
                .Select(optimizationLevel => new object[] { optimizationLevel }.Concat(this.OtherParams)
                .ToArray());
        }
    }
}
