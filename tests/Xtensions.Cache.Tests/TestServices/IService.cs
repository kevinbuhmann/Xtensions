namespace Xtensions.Cache.Tests.TestServices
{
    using Xtensions.Cache;

    public interface IService
    {
        ICache Cache { get; }
    }
}
