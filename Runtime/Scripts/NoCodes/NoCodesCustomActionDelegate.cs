namespace QonversionUnity
{
    /// <summary>
    /// Optional companion to <see cref="NoCodesDelegate"/> for receiving custom actions
    /// configured in the No-Codes builder.
    ///
    /// Implement this interface on the same class as your <see cref="NoCodesDelegate"/> —
    /// it is a separate interface (instead of a new <see cref="NoCodesDelegate"/> method)
    /// to keep existing delegate implementations source-compatible.
    /// </summary>
    public interface NoCodesCustomActionDelegate
    {
        /// <summary>
        /// Called when a custom action configured in the builder is triggered on the screen.
        /// The No-Codes SDK does not execute anything itself — handle the value in your app code.
        /// The screen stays open; close it using <see cref="INoCodes.Close"/> if needed.
        /// </summary>
        /// <param name="value">The string value configured for the custom action in the builder,
        /// or an empty string if no value was configured.</param>
        void OnCustomAction(string value);
    }
}
