namespace Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies
{
    using Xtensions.DependencyInjection.Tests.TestServices.CustomDependencies.Abstractions;

    public class RootService : IRootService
    {
        public RootService(IOtherService otherService, IInjectedService injectedService)
        {
            this.OtherService = otherService;
            this.InjectedService = injectedService;
        }

        public IOtherService OtherService { get; }

        public IInjectedService InjectedService { get; }
    }
}
