namespace Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies
{
    using Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies.Abstractions;

    public class OtherService : IOtherService
    {
        public string Name { get; } = nameof(OtherService);
    }
}
