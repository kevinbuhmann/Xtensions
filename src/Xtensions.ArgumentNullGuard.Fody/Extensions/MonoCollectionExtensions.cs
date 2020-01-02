namespace Xtensions.ArgumentNullGuard.Fody.Extensions
{
    using System.Collections.Generic;
    using Mono.Collections.Generic;

    internal static class MonoCollectionExtensions
    {
        public static void AddRange<T>(this Collection<T> self, IReadOnlyCollection<T> items)
        {
            foreach (T item in items)
            {
                self.Add(item);
            }
        }

        public static void Prepend<T>(this Collection<T> self, IReadOnlyCollection<T> items)
        {
            int index = 0;

            foreach (T item in items)
            {
                self.Insert(index, item);
                index++;
            }
        }
    }
}
