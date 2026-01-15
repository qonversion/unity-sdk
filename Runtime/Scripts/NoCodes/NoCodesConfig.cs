using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Configuration for No-Codes SDK initialization.
    /// Use <see cref="NoCodesConfigBuilder"/> to create instances.
    /// </summary>
    public class NoCodesConfig
    {
        /// <summary>
        /// Project key from Qonversion Dashboard.
        /// </summary>
        public readonly string ProjectKey;

        /// <summary>
        /// Optional proxy URL for API requests.
        /// </summary>
        [CanBeNull] public readonly string ProxyUrl;

        /// <summary>
        /// Optional locale to override device locale for No-Codes screens.
        /// </summary>
        [CanBeNull] public readonly string Locale;

        /// <summary>
        /// Delegate for receiving No-Codes events.
        /// </summary>
        [CanBeNull] public readonly NoCodesDelegate NoCodesDelegate;

        /// <summary>
        /// Delegate for custom purchase and restore handling.
        /// </summary>
        [CanBeNull] public readonly NoCodesPurchaseDelegate PurchaseDelegate;

        internal NoCodesConfig(
            string projectKey,
            string proxyUrl,
            string locale,
            NoCodesDelegate noCodesDelegate,
            NoCodesPurchaseDelegate purchaseDelegate)
        {
            ProjectKey = projectKey;
            ProxyUrl = proxyUrl;
            Locale = locale;
            NoCodesDelegate = noCodesDelegate;
            PurchaseDelegate = purchaseDelegate;
        }
    }
}
