#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

namespace QonversionUnity
{
    internal class QonversionWrapperIOS : IQonversionWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _setDebugMode(bool debugMode);

        [DllImport("__Internal")]
        private static extern void _launchWithKey(string key, string userID, 
        QonversionSuccessInitCallback onSuccessInitCallback);

        [DllImport("__Internal")]
        private static extern void _addAttributionData(string conversionData, int provider);
#endif

        private delegate void QonversionSuccessInitCallback();

        private static InitDelegate onInitCompleteDelegate;

        public void Launch(string projectKey, string userID, bool debugMode, InitDelegate onInitComplete)
        {
            onInitCompleteDelegate = onInitComplete;

#if UNITY_IOS
            _setDebugMode(debugMode);

            _launchWithKey(projectKey, userID, onSuccessInit);
#endif
        }

        public void AddAttributionData(string conversionData, AttributionSource source, string conversionUid)
        {
#if UNITY_IOS
            _addAttributionData(conversionData, (int)source);
#endif
        }

#if UNITY_IOS
        [AOT.MonoPInvokeCallback(typeof(QonversionSuccessInitCallback))]
#endif
        private static void onSuccessInit()
        {
            onInitCompleteDelegate?.Invoke();
        }
    }
}