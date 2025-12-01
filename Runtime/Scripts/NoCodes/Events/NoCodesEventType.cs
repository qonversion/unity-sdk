namespace QonversionUnity
{
    /// <summary>
    /// Types of NoCodes events.
    /// </summary>
    public enum NoCodesEventType
    {
        /// <summary>
        /// Unknown event type.
        /// </summary>
        Unknown,

        /// <summary>
        /// Event fired when a NoCodes screen is shown.
        /// </summary>
        ScreenShown,

        /// <summary>
        /// Event fired when a NoCodes flow is finished.
        /// </summary>
        Finished,

        /// <summary>
        /// Event fired when a NoCodes action is started.
        /// </summary>
        ActionStarted,

        /// <summary>
        /// Event fired when a NoCodes action fails.
        /// </summary>
        ActionFailed,

        /// <summary>
        /// Event fired when a NoCodes action is finished.
        /// </summary>
        ActionFinished,

        /// <summary>
        /// Event fired when a NoCodes screen fails to load.
        /// </summary>
        ScreenFailedToLoad
    }
}




