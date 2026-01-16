#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace QonversionUnity
{
    internal class NoCodesWrapperIOS : INoCodesWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _initializeNoCodes(string gameObjectName, string projectKey, string proxyUrl, string locale, string sdkVersion);

        [DllImport("__Internal")]
        private static extern void _setNoCodesDelegate();

        [DllImport("__Internal")]
        private static extern void _setNoCodesScreenPresentationConfig(string configJson, string contextKey);

        [DllImport("__Internal")]
        private static extern void _showNoCodesScreen(string contextKey);

        [DllImport("__Internal")]
        private static extern void _closeNoCodes();

        [DllImport("__Internal")]
        private static extern void _setNoCodesLocale(string locale);

        [DllImport("__Internal")]
        private static extern void _setNoCodesPurchaseDelegate();

        [DllImport("__Internal")]
        private static extern void _noCodesDelegatedPurchaseCompleted();

        [DllImport("__Internal")]
        private static extern void _noCodesDelegatedPurchaseFailed(string errorMessage);

        [DllImport("__Internal")]
        private static extern void _noCodesDelegatedRestoreCompleted();

        [DllImport("__Internal")]
        private static extern void _noCodesDelegatedRestoreFailed(string errorMessage);
#endif

        public void Initialize(string gameObjectName, string projectKey, string proxyUrl, string locale, string sdkVersion)
        {
#if UNITY_IOS
            _initializeNoCodes(gameObjectName, projectKey, proxyUrl ?? "", locale ?? "", sdkVersion);
#endif
        }

        public void SetDelegate()
        {
#if UNITY_IOS
            _setNoCodesDelegate();
#endif
        }

        public void SetScreenPresentationConfig(string configJson, string contextKey)
        {
#if UNITY_IOS
            _setNoCodesScreenPresentationConfig(configJson, contextKey ?? "");
#endif
        }

        public void ShowScreen(string contextKey)
        {
#if UNITY_IOS
            _showNoCodesScreen(contextKey);
#endif
        }

        public void Close()
        {
#if UNITY_IOS
            _closeNoCodes();
#endif
        }

        public void SetLocale(string locale)
        {
#if UNITY_IOS
            _setNoCodesLocale(locale ?? "");
#endif
        }

        public void SetPurchaseDelegate()
        {
#if UNITY_IOS
            _setNoCodesPurchaseDelegate();
#endif
        }

        public void DelegatedPurchaseCompleted()
        {
#if UNITY_IOS
            _noCodesDelegatedPurchaseCompleted();
#endif
        }

        public void DelegatedPurchaseFailed(string errorMessage)
        {
#if UNITY_IOS
            _noCodesDelegatedPurchaseFailed(errorMessage);
#endif
        }

        public void DelegatedRestoreCompleted()
        {
#if UNITY_IOS
            _noCodesDelegatedRestoreCompleted();
#endif
        }

        public void DelegatedRestoreFailed(string errorMessage)
        {
#if UNITY_IOS
            _noCodesDelegatedRestoreFailed(errorMessage);
#endif
        }
    }
}
