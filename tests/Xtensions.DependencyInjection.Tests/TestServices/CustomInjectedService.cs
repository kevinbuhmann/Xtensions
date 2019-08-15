namespace Xtensions.DependencyInjection.Tests.TestServices
{
    using Xtensions.DependencyInjection.Tests.TestServices.Abstractions;

    public class CustomInjectedService : IInjectedService
    {
        public string Name { get; } = nameof(CustomInjectedService);
    }
}
