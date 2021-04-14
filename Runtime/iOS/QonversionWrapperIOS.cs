#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using System;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperIOS : IQonversionWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _setDebugMode();

        [DllImport("__Internal")]
        private static extern void _setAdvertisingID();

        [DllImport("__Internal")]
        private static extern void _setUserID(string userID);

        [DllImport("__Internal")]
        private static extern void _setProperty(string propertyName, string value);

        [DllImport("__Internal")]
        private static extern void _setUserProperty(string key, string value);

        [DllImport("__Internal")]
        private static extern void _launchWithKey(string gameObjectName, string key);

        [DllImport("__Internal")]
        private static extern void _addAttributionData(string conversionData, int provider);

        [DllImport("__Internal")]
        private static extern void _checkPermissions(string callbackName);

        [DllImport("__Internal")]
        private static extern void _restore(string callbackName);

        [DllImport("__Internal")]
        private static extern void _purchase(string productID, string callbackName);

        [DllImport("__Internal")]
        private static extern void _products(string callbackName);

        [DllImport("__Internal")]
        private static extern void _offerings(string callbackName);
#endif

        public void Launch(string gameObjectName, string projectKey, bool observerMode)
        {
#if UNITY_IOS
            _launchWithKey(gameObjectName, projectKey);
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

        public void SetAdvertisingID()
        {
#if UNITY_IOS
            _setAdvertisingID();
#endif
        }


        public void SetUserID(string userID)
        {
#if UNITY_IOS
            _setUserID(userID);
#endif
        }

        public void SetUserProperty(string key, string value)
        {
#if UNITY_IOS
            _setUserProperty(key, value);
#endif
        }

        public void SetProperty(UserProperty key, string value)
        {
            string propertyName = Enum.GetName(typeof(UserProperty), key);
#if UNITY_IOS
            _setProperty(propertyName, value);
#endif
        }

    public void AddAttributionData(string conversionData, AttributionSource source)
        {
#if UNITY_IOS
            _addAttributionData(conversionData, (int)source);
#endif
        }

        public void CheckPermissions(string callbackName)
        {
#if UNITY_IOS
            _checkPermissions(callbackName);
#endif
        }

        public void Purchase(string productId, string callbackName)
        {
#if UNITY_IOS
            _purchase(productId, callbackName);
#endif
        }

        public void Restore(string callbackName)
        {
#if UNITY_IOS
            _restore(callbackName);
#endif
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
        }

        public void Products(string callbackName)
        {
#if UNITY_IOS
            _products(callbackName);
#endif
        }

        public void Offerings(string callbackName)
        {
#if UNITY_IOS
            _offerings(callbackName);
#endif
        }
    }
}