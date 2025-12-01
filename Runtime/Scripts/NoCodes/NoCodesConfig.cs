using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Configuration class for NoCodes SDK initialization.
    /// </summary>
    public class NoCodesConfig
    {
        /// <summary>
        /// Qonversion project key.
        /// </summary>
        public string ProjectKey { get; }

        /// <summary>
        /// Optional proxy URL for requests.
        /// </summary>
        [CanBeNull]
        public string ProxyUrl { get; }

        /// <summary>
        /// Creates a new instance of NoCodesConfig.
        /// </summary>
        /// <param name="projectKey">Qonversion project key.</param>
        /// <param name="proxyUrl">Optional proxy URL.</param>
        public NoCodesConfig(string projectKey, string proxyUrl = null)
        {
            ProjectKey = projectKey;
            ProxyUrl = proxyUrl;
        }
    }
}




