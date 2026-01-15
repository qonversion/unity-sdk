namespace QonversionUnity
{
    /// <summary>
    /// Type of action that can be executed from No-Codes screens.
    /// </summary>
    public enum NoCodesActionType
    {
        /// <summary>
        /// Unspecified action type.
        /// </summary>
        Unknown,

        /// <summary>
        /// URL action that opens the URL using system browser.
        /// </summary>
        Url,

        /// <summary>
        /// Deeplink action that opens if Application can handle the specified deeplink.
        /// </summary>
        Deeplink,

        /// <summary>
        /// Navigation to another No-Codes screen.
        /// </summary>
        Navigation,

        /// <summary>
        /// Purchase the product.
        /// </summary>
        Purchase,

        /// <summary>
        /// Restore all purchases.
        /// </summary>
        Restore,

        /// <summary>
        /// Close current screen.
        /// </summary>
        Close,

        /// <summary>
        /// Close all No-Code screens.
        /// </summary>
        CloseAll
    }
}
