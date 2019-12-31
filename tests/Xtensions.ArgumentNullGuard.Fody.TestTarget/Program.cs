namespace Xtensions.ArgumentNullGuard.Fody.TestTarget
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class Program
    {
        public static void Main()
        {
        }

        public static async Task TestAsyncMethod(string value)
        {
            Console.WriteLine(value);

            if (value == "a")
            {
                while (true)
                {
                    await Task.Delay(0);
                }
            }
        }

        public static IEnumerable<string> TestIteratorMethod(string value)
        {
            Console.WriteLine(value);

            yield return value;

            if (value == "a")
            {
                while (true)
                {
                    yield return value;
                }
            }
        }
    }
}
