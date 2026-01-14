using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Interface for No-Codes SDK functionality.
    /// </summary>
    public interface INoCodes
    {
        /// <summary>
        /// Set screen presentation configuration.
        /// </summary>
        /// <param name="config">A configuration to apply.</param>
        /// <param name="contextKey">The context key of the screen, to which a config should be applied.
        ///                          If not provided, the config is used for all the screens.</param>
        void SetScreenPresentationConfig(ScreenPresentationConfig config, [CanBeNull] string contextKey = null);

        /// <summary>
        /// Show the screen using its context key.
        /// </summary>
        /// <param name="contextKey">The context key of the screen which must be shown.</param>
        void ShowScreen(string contextKey);

        /// <summary>
        /// Close the current opened No-Code screen.
        /// </summary>
        void Close();

        /// <summary>
        /// Set the locale for No-Code screens.
        /// Use this to override the device locale for the No-Codes SDK.
        /// Pass null to reset to the device default locale.
        /// </summary>
        /// <param name="locale">The locale to use (e.g. "en", "de", "fr"), or null to reset to device default.</param>
        void SetLocale([CanBeNull] string locale);

        /// <summary>
        /// Set or update the purchase delegate for custom purchase handling.
        /// When this delegate is set, it replaces the default Qonversion SDK purchase flow.
        /// This can be called at any time after initialization.
        /// </summary>
        /// <param name="purchaseDelegate">The delegate to handle purchase and restore operations.</param>
        void SetPurchaseDelegate(NoCodesPurchaseDelegate purchaseDelegate);
    }
}
