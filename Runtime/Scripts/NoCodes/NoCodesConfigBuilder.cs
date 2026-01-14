using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Builder for creating <see cref="NoCodesConfig"/> instances.
    /// </summary>
    public class NoCodesConfigBuilder
    {
        private readonly string _projectKey;
        private string _proxyUrl;
        private string _locale;
        private NoCodesDelegate _noCodesDelegate;
        private NoCodesPurchaseDelegate _purchaseDelegate;

        /// <summary>
        /// Creates a new builder with the specified project key.
        /// </summary>
        /// <param name="projectKey">Project key from Qonversion Dashboard.</param>
        public NoCodesConfigBuilder(string projectKey)
        {
            _projectKey = projectKey;
        }

        /// <summary>
        /// Provide a delegate to be notified about No-Codes events.
        /// 
        /// Make sure you provide this delegate for being up-to-date with the No-Codes events.
        /// Else you can lose some important updates. Also, please, consider that this delegate
        /// should live for the whole lifetime of the application.
        /// </summary>
        /// <param name="noCodesDelegate">Delegate to be called when No-Codes events occur.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public NoCodesConfigBuilder SetNoCodesDelegate(NoCodesDelegate noCodesDelegate)
        {
            _noCodesDelegate = noCodesDelegate;
            return this;
        }

        /// <summary>
        /// Provide a delegate for custom purchase and restore handling.
        /// When this delegate is provided, it replaces the default Qonversion SDK purchase flow.
        /// </summary>
        /// <param name="purchaseDelegate">Delegate to handle purchase and restore operations.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public NoCodesConfigBuilder SetPurchaseDelegate(NoCodesPurchaseDelegate purchaseDelegate)
        {
            _purchaseDelegate = purchaseDelegate;
            return this;
        }

        /// <summary>
        /// Set proxy URL for No-Codes SDK.
        /// </summary>
        /// <param name="proxyUrl">Proxy URL to use for API requests.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public NoCodesConfigBuilder SetProxyUrl(string proxyUrl)
        {
            _proxyUrl = proxyUrl;
            return this;
        }

        /// <summary>
        /// Set the locale for No-Code screens.
        /// Use this to override the device locale for the No-Codes SDK.
        /// </summary>
        /// <param name="locale">The locale to use (e.g. "en", "de", "fr").</param>
        /// <returns>Builder instance for chain calls.</returns>
        public NoCodesConfigBuilder SetLocale(string locale)
        {
            _locale = locale;
            return this;
        }

        /// <summary>
        /// Generate <see cref="NoCodesConfig"/> instance with all the provided configurations.
        /// </summary>
        /// <returns>The complete <see cref="NoCodesConfig"/> instance.</returns>
        public NoCodesConfig Build()
        {
            return new NoCodesConfig(
                _projectKey,
                _proxyUrl,
                _locale,
                _noCodesDelegate,
                _purchaseDelegate
            );
        }
    }
}
