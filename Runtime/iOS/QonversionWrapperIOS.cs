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
        private static extern void _launchWithKey(string key, string userID);

        [DllImport("__Internal")]
        private static extern void _addAttributionData(string conversionData, int provider);
#endif

        public void SetDebugMode(bool debugMode)
        {
#if UNITY_IOS
            _setDebugMode(debugMode);
#endif
        }

        public void Initialize(string projectKey, string userID, bool debugMode)
        {
            SetDebugMode(debugMode);

#if UNITY_IOS
            _launchWithKey(projectKey, userID);
#endif
        }

        public void AddAttributionData(string conversionData, AttributionSource source, string conversionUid)
        {
#if UNITY_IOS
            _addAttributionData(conversionData, (int)source);
#endif
        }
    }
}