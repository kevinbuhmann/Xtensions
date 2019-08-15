namespace Xtensions.DependencyInjection.Tests.TestServices
{
    using EnsureThat;
    using Xtensions.DependencyInjection.Tests.TestServices.Abstractions;

    public class RootService : IRootService
    {
        public RootService(IOtherService otherService, IInjectedService injectedService)
        {
            EnsureArg.IsNotNull(otherService, nameof(otherService));
            EnsureArg.IsNotNull(injectedService, nameof(injectedService));

            this.OtherService = otherService;
            this.InjectedService = injectedService;
        }

        public IOtherService OtherService { get; }

        public IInjectedService InjectedService { get; }
    }
}
