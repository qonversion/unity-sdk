using JetBrains.Annotations;

namespace QonversionUnity
{
    public class QonversionConfigBuilder
    {
        private readonly string _projectKey;
        private readonly LaunchMode _launchMode;
        private Environment _environment = Environment.Production;
        private EntitlementsCacheLifetime _entitlementsCacheLifetime = EntitlementsCacheLifetime.Month;
        [CanBeNull] private string _proxyUrl = null;

        public QonversionConfigBuilder(string projectKey, LaunchMode launchMode)
        {
            _projectKey = projectKey;
            _launchMode = launchMode;
        }

        /// <summary>
        /// Set current application <see cref="Environment"/>. Used to distinguish sandbox and production users.
        /// </summary>
        /// <param name="environment">current environment.</param>
        /// <returns>builder instance for chain calls.</returns>
        public QonversionConfigBuilder SetEnvironment(Environment environment)
        {
            _environment = environment;
            return this;
        }

        /// <summary>
        /// Entitlements cache is used when there are problems with the Qonversion API
        /// or internet connection. If so, Qonversion will return the last successfully loaded
        /// entitlements. The current method allows you to configure how long that cache may be used.
        /// The default value is <see cref="EntitlementsCacheLifetime.Month"/>.
        /// </summary>
        /// <param name="lifetime">desired entitlements cache lifetime duration.</param>
        /// <returns>builder instance for chain calls.</returns>
        public QonversionConfigBuilder SetEntitlementsCacheLifetime(EntitlementsCacheLifetime lifetime)
        {
            _entitlementsCacheLifetime = lifetime;
            return this;
        }

        /// <summary>
        /// Provide a URL to your proxy server which will redirect all the requests from the app
        /// to our API. Please, contact us before using this feature.
        /// </summary>
        /// <param name="url">your proxy server url.</param>
        /// <returns>builder instance for chain calls.</returns>
        /// <see href="https://documentation.qonversion.io/docs/custom-proxy-server-for-sdks" />
        public QonversionConfigBuilder SetProxyURL(string url)
        {
            _proxyUrl = url;
            return this;
        }

        /// <summary>
        /// Generate <see cref="QonversionConfig"/> instance with all the provided configurations.
        /// </summary>
        /// <returns>the complete <see cref="QonversionConfig"/> instance.</returns>
        public QonversionConfig Build()
        {
            return new QonversionConfig(
                _projectKey,
                _launchMode,
                _environment,
                _entitlementsCacheLifetime,
                _proxyUrl
            );
        }
    }
}