namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers
{
    public class ServiceHelper
    {
        public ServiceHelper(BarHelper barHelper)
        {
            this.BarHelper = barHelper;
        }

        public BarHelper BarHelper { get; }
    }
}
