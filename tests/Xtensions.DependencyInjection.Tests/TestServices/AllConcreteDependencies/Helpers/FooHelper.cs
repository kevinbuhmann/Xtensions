namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers
{
    using EnsureThat;
    using Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Clients;

    public class FooHelper
    {
        public FooHelper(IServiceClient serviceClient)
        {
            EnsureArg.IsNotNull(serviceClient, nameof(serviceClient));
        }
    }
}
