namespace QonversionUnity
{
    /// <summary>
    /// Delegate interface for receiving No-Codes events.
    /// Implement this interface to handle events from No-Codes screens.
    /// 
    /// <para>
    /// <b>Android Warning:</b> On Android, when a No-Codes screen is displayed, Unity's game loop
    /// is paused because the No-Codes screen runs as a separate Activity on top of Unity's Activity.
    /// This means that all delegate events will be delivered with a delay - they will only be received
    /// after the No-Codes screen is closed and Unity resumes.
    /// </para>
    /// </summary>
    public interface NoCodesDelegate
    {
        /// <summary>
        /// Called when a No-Codes screen is shown.
        /// </summary>
        /// <param name="screenId">The identifier of the screen that was shown.</param>
        void OnScreenShown(string screenId);

        /// <summary>
        /// Called when No-Codes starts executing an action.
        /// </summary>
        /// <param name="action">The action that started executing.</param>
        void OnActionStarted(NoCodesAction action);

        /// <summary>
        /// Called when No-Codes fails to execute an action.
        /// </summary>
        /// <param name="action">The action that failed to execute.</param>
        void OnActionFailed(NoCodesAction action);

        /// <summary>
        /// Called when No-Codes finishes executing an action.
        /// </summary>
        /// <param name="action">The action that finished executing.</param>
        void OnActionFinished(NoCodesAction action);

        /// <summary>
        /// Called when No-Codes flow is finished (all screens are closed).
        /// </summary>
        void OnFinished();

        /// <summary>
        /// Called when a No-Codes screen fails to load.
        /// </summary>
        /// <param name="error">The error that occurred.</param>
        void OnScreenFailedToLoad(NoCodesError error);
    }
}
