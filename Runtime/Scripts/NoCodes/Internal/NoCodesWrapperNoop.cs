using UnityEngine;

namespace QonversionUnity
{
    /// <summary>
    /// No-op implementation for unsupported platforms (Editor, standalone, etc.)
    /// </summary>
    internal class NoCodesWrapperNoop : INoCodesWrapper
    {
        public void Initialize(string gameObjectName, string projectKey, string proxyUrl, string locale, string theme, string sdkVersion)
        {
            Debug.Log("NoCodes is not supported on this platform.");
        }

        public void SetDelegate()
        {
            // No-op
        }

        public void SetScreenPresentationConfig(string configJson, string contextKey)
        {
            // No-op
        }

        public void ShowScreen(string contextKey)
        {
            Debug.Log($"NoCodes.ShowScreen({contextKey}) called on unsupported platform.");
        }

        public void Close()
        {
            // No-op
        }

        public void SetLocale(string locale)
        {
            // No-op
        }

        public void SetTheme(string theme)
        {
            // No-op
        }

        public void SetPurchaseDelegate()
        {
            // No-op
        }

        public void DelegatedPurchaseCompleted()
        {
            // No-op
        }

        public void DelegatedPurchaseFailed(string errorMessage)
        {
            // No-op
        }

        public void DelegatedRestoreCompleted()
        {
            // No-op
        }

        public void DelegatedRestoreFailed(string errorMessage)
        {
            // No-op
        }
    }
}
