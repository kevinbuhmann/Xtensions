namespace Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies
{
    using Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies.Abstractions;

    public class DefaultInjectedService : IInjectedService
    {
        public string Name { get; } = nameof(DefaultInjectedService);
    }
}
