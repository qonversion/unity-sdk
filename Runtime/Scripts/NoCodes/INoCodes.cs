using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Interface for NoCodes SDK functionality.
    /// </summary>
    public interface INoCodes
    {
        /// <summary>
        /// Event fired when any NoCodes event occurs.
        /// </summary>
        event Action<NoCodesEvent> EventReceived;

        /// <summary>
        /// Sets the screen presentation configuration.
        /// </summary>
        /// <param name="config">Presentation configuration.</param>
        /// <param name="contextKey">Optional context key for the configuration.</param>
        /// <returns>Task that completes when the configuration is set.</returns>
        Task SetScreenPresentationConfig(NoCodesPresentationConfig config, [CanBeNull] string contextKey = null);

        /// <summary>
        /// Shows a NoCodes screen with the specified context key.
        /// </summary>
        /// <param name="contextKey">Context key for the screen to show.</param>
        /// <returns>Task that completes when the screen is shown.</returns>
        Task ShowScreen(string contextKey);

        /// <summary>
        /// Closes the current NoCodes screen.
        /// </summary>
        /// <returns>Task that completes when the screen is closed.</returns>
        Task Close();

        /// <summary>
        /// Sets the delegate for handling NoCodes events.
        /// </summary>
        /// <param name="delegate">Delegate instance for handling NoCodes events.</param>
        void SetDelegate(NoCodesDelegate @delegate);

        internal void InitializeInstance(NoCodesConfig config);
    }
}

