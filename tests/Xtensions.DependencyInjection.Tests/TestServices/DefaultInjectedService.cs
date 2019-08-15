namespace Xtensions.DependencyInjection.Tests.TestServices
{
    using Xtensions.DependencyInjection.Tests.TestServices.Abstractions;

    public class DefaultInjectedService : IInjectedService
    {
        public string Name { get; } = nameof(DefaultInjectedService);
    }
}
