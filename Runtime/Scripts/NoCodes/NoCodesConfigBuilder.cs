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
        private NoCodesTheme? _theme;
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
        /// 
        /// <para>
        /// <b>Android Warning:</b> On Android, when a No-Codes screen is displayed, Unity's game loop
        /// is paused. All delegate events will be delivered with a delay - only after the No-Codes screen
        /// is closed and Unity resumes.
        /// </para>
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
        /// 
        /// <para>
        /// <b>Android Warning:</b> This delegate does NOT work on Android. When a No-Codes screen is displayed,
        /// Unity's game loop is paused, so the delegate methods will not be called while the screen is active.
        /// Use this delegate only on iOS, or use the default Qonversion SDK purchase flow on Android.
        /// </para>
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
        /// Set the theme mode for No-Code screens.
        /// Controls how screens adapt to light/dark themes.
        /// </summary>
        /// <param name="theme">The desired theme mode.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public NoCodesConfigBuilder SetTheme(NoCodesTheme theme)
        {
            _theme = theme;
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
                _theme,
                _noCodesDelegate,
                _purchaseDelegate
            );
        }
    }
}
