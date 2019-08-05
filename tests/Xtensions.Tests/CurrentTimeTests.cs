namespace Xtensions.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class CurrentTimeTests
    {
        [Fact]
        public void UtcNow_NotMocked_ReturnsCurrentTime()
        {
            for (int i = 0; i < 100; ++i)
            {
                Assert.InRange(actual: CurrentTime.UtcNow, low: DateTime.UtcNow.AddMilliseconds(-2), high: DateTime.UtcNow);
            }
        }

        [Fact]
        public void UtcNow_Mocked_ReturnsMockTime()
        {
            DateTime mockUtcNow = new DateTime(year: 2010, month: 3, day: 17, hour: 18, minute: 23, second: 48);

            using (CurrentTime.UseMockUtcNow(mockUtcNow))
            {
                for (int i = 0; i < 100; ++i)
                {
                    Assert.Equal(actual: CurrentTime.UtcNow, expected: mockUtcNow);
                }
            }
        }

        [Fact]
        public async Task UtcNow_Mocked_ReturnsMockTimeInAsyncCode()
        {
            DateTime mockUtcNow = new DateTime(year: 2010, month: 3, day: 17, hour: 18, minute: 23, second: 48);

            using (CurrentTime.UseMockUtcNow(mockUtcNow))
            {
                for (int i = 0; i < 100; ++i)
                {
                    await Task.Delay(1).ConfigureAwait(false);
                    Assert.Equal(actual: CurrentTime.UtcNow, expected: mockUtcNow);
                }
            }
        }

        [Fact]
        public void UtcNow_Mocked_SupportsParallelTests()
        {
            Parallel.ForEach(Enumerable.Range(1, 25), threadNumber =>
            {
                DateTime mockUtcNow = new DateTime(year: 2010, month: 3, day: threadNumber);

                using (CurrentTime.UseMockUtcNow(mockUtcNow))
                {
                    for (int i = 0; i < 100; ++i)
                    {
                        Assert.Equal(actual: CurrentTime.UtcNow, expected: mockUtcNow);
                    }
                }
            });
        }

        [Fact]
        public async Task UtcNow_Mocked_SupportsParallelAsyncTests()
        {
            IEnumerable<Task> tasks = Enumerable.Range(1, 10).Select(async threadNumber =>
            {
                DateTime mockUtcNow = new DateTime(year: 2010, month: 3, day: threadNumber);

                using (CurrentTime.UseMockUtcNow(mockUtcNow))
                {
                    for (int i = 0; i < 100; ++i)
                    {
                        await Task.Delay(1).ConfigureAwait(false);
                        Assert.Equal(actual: CurrentTime.UtcNow, expected: mockUtcNow);
                    }
                }
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [Fact]
        public void UtcNow_Mocked_ReturnsCurrentTimeAfterDisposal()
        {
            DateTime mockUtcNow = new DateTime(year: 2010, month: 3, day: 17, hour: 18, minute: 23, second: 48);

            using (CurrentTime.UseMockUtcNow(mockUtcNow))
            {
                Assert.Equal(actual: CurrentTime.UtcNow, expected: mockUtcNow);
            }

            Assert.InRange(actual: CurrentTime.UtcNow, low: DateTime.UtcNow.AddMilliseconds(-2), high: DateTime.UtcNow);
        }

        [Fact]
        public void UtcNow_MockedTwice_Throws()
        {
            DateTime mockUtcNow = new DateTime(year: 2010, month: 3, day: 17, hour: 18, minute: 23, second: 48);

            using (CurrentTime.UseMockUtcNow(mockUtcNow))
            {
                InvalidOperationException exception = Assert.Throws<InvalidOperationException>(() =>
                {
                    using (CurrentTime.UseMockUtcNow(mockUtcNow))
                    {
                    }
                });

                Assert.Equal(actual: exception.Message, expected: "Current time is already mocked.");
            }
        }
    }
}
