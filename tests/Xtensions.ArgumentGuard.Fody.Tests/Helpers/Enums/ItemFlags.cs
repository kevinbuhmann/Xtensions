namespace Xtensions.ArgumentGuard.Fody.Tests.Helpers.Enums
{
    using System;

    [Flags]
    public enum ItemFlags
    {
        Child = 1,
        Adult = 2,
        Toy = 4,
        Tool = 8,
    }
}
