namespace Xtensions.Cache.Tests.TestServices
{
    public class Service : IService
    {
        public Service(ICache cache)
        {
            this.Cache = cache;
        }

        public ICache Cache { get; }
    }
}
