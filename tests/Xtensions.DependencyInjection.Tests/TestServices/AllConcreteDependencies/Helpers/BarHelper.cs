namespace Xtensions.DependencyInjection.Tests.TestServices.AllConcreteDependencies.Helpers
{
    public class BarHelper
    {
        public BarHelper(FooHelper fooHelper)
        {
            this.FooHelper = fooHelper;
        }

        public FooHelper FooHelper { get; }
    }
}
