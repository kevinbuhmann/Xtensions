namespace Xtensions.Configuration.Tests.Mock
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using Xtensions.Configuration.Mock;
    using Xunit;

    public class MockConfigurationTests
    {
        [Fact]
        public void IConfiguration_ValueRead_ReturnsValue()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration.GetValue<string>("Key"), expected: "Value");
            }
        }

        [Fact]
        public void IConfiguration_MissingKeyRead_ReturnsNull()
        {
            dynamic configurationData = new
            {
                Key = "Value",
            };

            using (MockConfiguration configuration = new MockConfiguration(configurationData))
            {
                Assert.Equal(actual: configuration.GetValue<string>("Key2"), expected: null);
            }
        }

        [Fact]
        public void IConfiguration_SectionReadAsString_ReturnsNull()
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
                Assert.Equal(actual: configuration.GetValue<string>("Key"), expected: null);
            }
        }

        [Fact]
        public void IConfiguration_SectionValueRead_ReturnsValue()
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
                IConfigurationSection section = configuration.GetSection("Settings");

                Assert.Equal(actual: section.GetValue<string>("Key"), expected: "Value");
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

                Assert.Equal(actual: configuration.GetValue<string>("Key"), expected: "NewValue");
            }
        }
    }
}
