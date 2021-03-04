#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperIOS : IQonversionWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _setDebugMode();

        [DllImport("__Internal")]
        private static extern void _setUserID(string userID);

        [DllImport("__Internal")]
        private static extern void _launchWithKey(string key);

        [DllImport("__Internal")]
        private static extern void _addAttributionData(string conversionData, int provider);
#endif

        public void Launch(string projectKey)
        {
#if UNITY_IOS
            _launchWithKey(projectKey);
#endif
        }

        public void SyncPurchases()
        {
        }

        public void SetDebugMode()
        {
#if UNITY_IOS
            _setDebugMode();
#endif
        }

        public void SetUserID(string userID)
        {
#if UNITY_IOS
            _setUserID(userID);
#endif
        }

        public void AddAttributionData(string conversionData, AttributionSource source)
        {
#if UNITY_IOS
            _addAttributionData(conversionData, (int)source);
#endif
        }
    }
}