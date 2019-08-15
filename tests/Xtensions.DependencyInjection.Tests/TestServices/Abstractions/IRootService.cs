namespace Xtensions.DependencyInjection.Tests.TestServices.Abstractions
{
    public interface IRootService
    {
        IOtherService OtherService { get; }

        IInjectedService InjectedService { get; }
    }
}
