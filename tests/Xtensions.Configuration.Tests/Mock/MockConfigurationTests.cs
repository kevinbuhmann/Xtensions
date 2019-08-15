namespace Xtensions.Configuration.Tests.Mock
{
    using System;
    using System.Linq;
    using Microsoft.Extensions.Primitives;
    using Xtensions.Configuration.Mock;
    using Xunit;

    public class MockConfigurationTests
    {
        [Fact]
        public void Indexer_ReturnsValue()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration["Key"], expected: "Value");
            }
        }

        [Fact]
        public void Indexer_SetsValue()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                configuration["Key"] = "New Value";

                Assert.Equal(actual: configuration["Key"], expected: "New Value");
            }
        }

        [Fact]
        public void Indexer_KeyMissing_ReturnsNull()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration["Key2"], expected: null);
            }
        }

        [Fact]
        public void Indexer_SectionKey_ReturnsNull()
        {
            dynamic configurationData = new
            {
                Key = new
                {
                    InnerKey = "Value",
                },
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration["Key"], expected: null);
            }
        }

        [Fact]
        public void GetSectionIndexer_ReturnsValue()
        {
            dynamic configurationData = new
            {
                Settings = new
                {
                    Key = "Value",
                },
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration.GetSection("Settings")["Key"], expected: "Value");
            }
        }

        [Fact]
        public void GetChildren_ReturnsExpectedNumberOfChildren()
        {
            dynamic configurationData = new
            {
                Settings = new
                {
                    Key = "Value",
                },
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration.GetChildren()?.Count(), expected: 1);
            }
        }

        [Fact]
        public void GetReloadToken_ConfigurationChanges_ReloadTokenFires()
        {
            dynamic configurationData = new
            {
                Settings = new
                {
                    Key = "Value",
                },
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                IChangeToken reloadToken = configuration.GetReloadToken();

                configurationData = new
                {
                    Key = "NewValue",
                };

                configuration.SetConfigurationData(configurationData);

                Assert.Equal(actual: reloadToken.HasChanged, expected: true);
            }
        }

        [Fact]
        public void GetReloadToken_ConfigurationChanges_ReturnsNewToken()
        {
            dynamic configurationData = new
            {
                Settings = new
                {
                    Key = "Value",
                },
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                IChangeToken reloadToken = configuration.GetReloadToken();

                configurationData = new
                {
                    Key = "NewValue",
                };

                configuration.SetConfigurationData(configurationData);

                Assert.True(configuration.GetReloadToken() != reloadToken);
            }
        }

        [Fact]
        public void SetConfigurationData_UpdatesConfigurationValues()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                configurationData = new
                {
                    Key = "NewValue",
                };

                configuration.SetConfigurationData(configurationData);

                Assert.Equal(actual: configuration["Key"], expected: "NewValue");
            }
        }

        [Fact]
        public void SetConfigurationData_AlreadyDisposed_Throws()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                configuration.Dispose();

                Assert.Throws<ObjectDisposedException>(() =>
                {
                    configuration.SetConfigurationData(configurationData);
                });
            }
        }
    }
}
