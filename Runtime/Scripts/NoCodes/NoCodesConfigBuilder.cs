using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Builder class for creating NoCodesConfig instances.
    /// </summary>
    public class NoCodesConfigBuilder
    {
        private readonly string _projectKey;
        [CanBeNull] private string _proxyUrl;

        /// <summary>
        /// Creates a new instance of NoCodesConfigBuilder.
        /// </summary>
        /// <param name="projectKey">Qonversion project key.</param>
        public NoCodesConfigBuilder(string projectKey)
        {
            _projectKey = projectKey;
        }

        /// <summary>
        /// Set proxy URL for requests.
        /// </summary>
        /// <param name="proxyUrl">Proxy server URL.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public NoCodesConfigBuilder SetProxyUrl(string proxyUrl)
        {
            _proxyUrl = proxyUrl;
            return this;
        }

        /// <summary>
        /// Builds a NoCodesConfig instance with all provided configurations.
        /// </summary>
        /// <returns>The complete NoCodesConfig instance.</returns>
        public NoCodesConfig Build()
        {
            return new NoCodesConfig(_projectKey, _proxyUrl);
        }
    }
}




