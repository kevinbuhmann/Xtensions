namespace Xtensions.Configuration.Mock
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json;

    /// <summary>
    /// This class provides configuration based on data from an anonymous object.
    /// </summary>
    public sealed class MockConfiguration : IConfiguration, IDisposable
    {
        private readonly string tempFilePath;
        private readonly IConfigurationRoot configuration;

        private bool disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockConfiguration"/> class.
        /// </summary>
        /// <param name="configurationData">The initial configuration data.</param>
        public MockConfiguration(dynamic configurationData = null)
        {
            this.tempFilePath = Path.GetTempFileName();
            this.SetConfigurationData(configurationData);

            this.configuration = new ConfigurationBuilder()
                .AddJsonFile(path: this.tempFilePath, optional: false, reloadOnChange: true)
                .Build();
        }

        /// <summary>
        /// Gets or sets a configuration value.
        /// </summary>
        /// <param name="key">The configuration key.</param>
        /// <returns>The configuration value.</returns>
        public string this[string key]
        {
            get => this.configuration[key];
            set => this.configuration[key] = value;
        }

        /// <summary>
        /// Gets the immediate descendant configuration sub-sections.
        /// </summary>
        /// <returns>
        /// The configuration sub-sections.
        /// </returns>
        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return this.configuration.GetChildren();
        }

        /// <summary>
        /// Returns a <see cref="IChangeToken" /> that can be used to observe when this configuration is reloaded.
        /// </summary>
        /// <returns>
        /// A <see cref="IChangeToken" />.
        /// </returns>
        public IChangeToken GetReloadToken()
        {
            return this.configuration.GetReloadToken();
        }

        /// <summary>
        /// Gets a configuration sub-section with the specified key.
        /// </summary>
        /// <param name="key">The key of the configuration section.</param>
        /// <returns>
        /// The <see cref="IConfigurationSection" />.
        /// </returns>
        /// <remarks>
        /// This method will never return <c>null</c>. If no matching sub-section is found with the specified key,
        /// an empty <see cref="IConfigurationSection" /> will be returned.
        /// </remarks>
        public IConfigurationSection GetSection(string key)
        {
            return this.configuration.GetSection(key);
        }

        /// <summary>
        /// Sets the configuration data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <exception cref="ObjectDisposedException">This mock configuration instance has been disposed.</exception>
        public void SetConfigurationData(dynamic configurationData)
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("This mock configuration instance has been disposed.");
            }

            File.WriteAllText(
                path: this.tempFilePath,
                contents: JsonConvert.SerializeObject(configurationData ?? new { }));

            this.configuration?.Reload();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (this.disposed == false)
            {
                File.Delete(this.tempFilePath);
                this.disposed = true;
            }
        }
    }
}
