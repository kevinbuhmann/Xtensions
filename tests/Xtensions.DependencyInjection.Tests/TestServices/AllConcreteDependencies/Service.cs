namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies
{
    using Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers;

    public class Service : IService
    {
        public Service(ServiceHelper serviceHelper)
        {
            this.ServiceHelper = serviceHelper;
        }

        public ServiceHelper ServiceHelper { get; }
    }
}
