namespace Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies
{
    using Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies.Abstractions;

    public class CustomInjectedService : IInjectedService
    {
        public string Name { get; } = nameof(CustomInjectedService);
    }
}
