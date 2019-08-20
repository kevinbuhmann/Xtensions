namespace Xtensions.Cache.Tests.TestServices
{
    using EnsureThat;

    public class Service : IService
    {
        public Service(ICache cache)
        {
            EnsureArg.IsNotNull(cache, nameof(cache));

            this.Cache = cache;
        }

        public ICache Cache { get; }
    }
}
