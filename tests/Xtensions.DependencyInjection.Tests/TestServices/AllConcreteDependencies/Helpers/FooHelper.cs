namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers
{
    using Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Clients;

    public class FooHelper
    {
        public FooHelper(IServiceClient serviceClient)
        {
            this.ServiceClient = serviceClient;
        }

        public IServiceClient ServiceClient { get; }
    }
}
