namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers
{
    using EnsureThat;

    public class BarHelper
    {
        public BarHelper(FooHelper fooHelper)
        {
            EnsureArg.IsNotNull(fooHelper, nameof(fooHelper));
        }
    }
}
