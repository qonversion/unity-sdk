using UnityEngine;

namespace QonversionUnity
{
    internal class NoCodesWrapperAndroid : INoCodesWrapper
    {
        private const string NoCodesWrapper = "com.qonversion.unitywrapper.NoCodesWrapper";

        public void Initialize(string gameObjectName, string projectKey, string proxyUrl, string locale, string sdkVersion)
        {
            CallNoCodes("initialize", gameObjectName, projectKey, proxyUrl ?? "", locale ?? "", sdkVersion);
        }

        public void SetDelegate()
        {
            CallNoCodes("setDelegate");
        }

        public void SetScreenPresentationConfig(string configJson, string contextKey)
        {
            CallNoCodes("setScreenPresentationConfig", configJson, contextKey ?? "");
        }

        public void ShowScreen(string contextKey)
        {
            CallNoCodes("showScreen", contextKey);
        }

        public void Close()
        {
            CallNoCodes("close");
        }

        public void SetLocale(string locale)
        {
            CallNoCodes("setLocale", locale ?? "");
        }

        public void SetPurchaseDelegate()
        {
            CallNoCodes("setPurchaseDelegate");
        }

        public void DelegatedPurchaseCompleted()
        {
            CallNoCodes("delegatedPurchaseCompleted");
        }

        public void DelegatedPurchaseFailed(string errorMessage)
        {
            CallNoCodes("delegatedPurchaseFailed", errorMessage);
        }

        public void DelegatedRestoreCompleted()
        {
            CallNoCodes("delegatedRestoreCompleted");
        }

        public void DelegatedRestoreFailed(string errorMessage)
        {
            CallNoCodes("delegatedRestoreFailed", errorMessage);
        }

        private static void CallNoCodes(string methodName, params object[] args)
        {
            using (var noCodes = new AndroidJavaClass(NoCodesWrapper))
            {
                noCodes.CallStatic(methodName, args);
            }
        }
    }
}
