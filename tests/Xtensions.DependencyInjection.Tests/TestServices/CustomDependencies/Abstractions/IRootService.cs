namespace Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies.Abstractions
{
    public interface IRootService
    {
        IOtherService OtherService { get; }

        IInjectedService InjectedService { get; }
    }
}
