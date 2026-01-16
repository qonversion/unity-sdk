using JetBrains.Annotations;

namespace QonversionUnity
{
    internal interface INoCodesWrapper
    {
        void Initialize(string gameObjectName, string projectKey, [CanBeNull] string proxyUrl, [CanBeNull] string locale, string sdkVersion);
        void SetDelegate();
        void SetScreenPresentationConfig(string configJson, [CanBeNull] string contextKey);
        void ShowScreen(string contextKey);
        void Close();
        void SetLocale([CanBeNull] string locale);
        void SetPurchaseDelegate();
        void DelegatedPurchaseCompleted();
        void DelegatedPurchaseFailed(string errorMessage);
        void DelegatedRestoreCompleted();
        void DelegatedRestoreFailed(string errorMessage);
    }
}
