namespace Xtensions.ArgumentGuard
{
    using System;

    /// <summary>
    /// This attribute suppresses the empty string guard.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class AllowEmptyAttribute : Attribute
    {
    }
}
