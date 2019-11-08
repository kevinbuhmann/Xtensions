namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies
{
    using EnsureThat;
    using Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers;

    public class Service : IService
    {
        public Service(ServiceHelper serviceHelper)
        {
            EnsureArg.IsNotNull(serviceHelper, nameof(serviceHelper));
        }
    }
}
