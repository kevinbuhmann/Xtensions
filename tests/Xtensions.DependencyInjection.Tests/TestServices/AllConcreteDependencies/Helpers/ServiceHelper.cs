namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers
{
    using EnsureThat;

    public class ServiceHelper
    {
        public ServiceHelper(BarHelper barHelper)
        {
            EnsureArg.IsNotNull(barHelper, nameof(barHelper));
        }
    }
}
