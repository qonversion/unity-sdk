using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Internal interface for platform-specific NoCodes wrappers.
    /// </summary>
    internal interface INoCodesWrapper
    {
        /// <summary>
        /// Initializes the wrapper with the game object name.
        /// </summary>
        /// <param name="gameObjectName">Name of the Unity game object to receive callbacks.</param>
        void Initialize(string gameObjectName);

        /// <summary>
        /// Initializes NoCodes SDK.
        /// </summary>
        /// <param name="projectKey">Qonversion project key.</param>
        /// <param name="version">SDK version.</param>
        /// <param name="source">SDK source identifier.</param>
        /// <param name="proxyUrl">Optional proxy URL.</param>
        void InitializeNoCodes(string projectKey, string version, string source, [CanBeNull] string proxyUrl);

        /// <summary>
        /// Sets the screen presentation configuration.
        /// </summary>
        /// <param name="configJson">JSON string representation of the presentation config.</param>
        /// <param name="contextKey">Optional context key.</param>
        void SetScreenPresentationConfig(string configJson, [CanBeNull] string contextKey);

        /// <summary>
        /// Shows a NoCodes screen.
        /// </summary>
        /// <param name="contextKey">Context key for the screen to show.</param>
        void ShowScreen(string contextKey);

        /// <summary>
        /// Closes the current NoCodes screen.
        /// </summary>
        void Close();
    }
}




