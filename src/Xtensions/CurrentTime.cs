namespace Xtensions
{
    using System;
    using System.Threading;

    /// <summary>
    /// This class provides a mechanism to mock the current UTC time in unit tests.
    /// </summary>
    public sealed class CurrentTime : IDisposable
    {
        private static readonly AsyncLocal<DateTime?> MockUtcNow = new AsyncLocal<DateTime?>();

        private CurrentTime(DateTime mockUtcNow)
        {
            if (MockUtcNow.Value != null)
            {
                throw new InvalidOperationException("Current time is already mocked.");
            }

            MockUtcNow.Value = mockUtcNow;
        }

        /// <summary>
        /// Gets either the current UTC time or the mocked UTC time for the current async context.
        /// </summary>
        /// <value>The current UTC time or the mocked UTC time for the current async context.</value>
        public static DateTime UtcNow => MockUtcNow.Value ?? DateTime.UtcNow;

        /// <summary>
        /// Sets the mock UTC time to the given value for the current async context.
        /// </summary>
        /// <param name="mockUtcNow">The mock UTC time to use for the current async context.</param>
        /// <returns>An instance of <see cref="CurrentTime" />. Dispose to return the current async context to current time.</returns>
        public static CurrentTime UseMockUtcNow(DateTime mockUtcNow)
        {
            return new CurrentTime(mockUtcNow);
        }

        /// <summary>
        /// Returns the current async context to current time.
        /// </summary>
        public void Dispose()
        {
            MockUtcNow.Value = null;
        }
    }
}
