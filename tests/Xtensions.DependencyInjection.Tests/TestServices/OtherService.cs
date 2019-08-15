namespace Xtensions.DependencyInjection.Tests.TestServices
{
    using Xtensions.DependencyInjection.Tests.TestServices.Abstractions;

    public class OtherService : IOtherService
    {
        public string Name { get; } = nameof(OtherService);
    }
}
