namespace QonversionUnity
{
    /// <summary>
    /// Theme mode for No-Code screens.
    /// Use this to control how screens adapt to light/dark themes.
    /// </summary>
    public enum NoCodesTheme
    {
        /// <summary>
        /// Automatically follow the device's system appearance (default).
        /// The screen will use light theme in light mode and dark theme in dark mode.
        /// </summary>
        Auto,

        /// <summary>
        /// Force light theme regardless of device settings.
        /// </summary>
        Light,

        /// <summary>
        /// Force dark theme regardless of device settings.
        /// </summary>
        Dark,
    }

    internal static class NoCodesThemeExtensions
    {
        /// <summary>
        /// Converts NoCodesTheme enum value to native SDK string representation.
        /// </summary>
        public static string ToNativeString(this NoCodesTheme theme)
        {
            switch (theme)
            {
                case NoCodesTheme.Auto:
                    return "auto";
                case NoCodesTheme.Light:
                    return "light";
                case NoCodesTheme.Dark:
                    return "dark";
                default:
                    return "auto";
            }
        }
    }
}
