#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperIOS : IQonversionWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _initialize(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _storeSdkInfo(string version, string source);

        [DllImport("__Internal")]
        private static extern void _setDebugMode();

        [DllImport("__Internal")]
        private static extern void _setAdvertisingID();

        [DllImport("__Internal")]
        private static extern void _setProperty(string propertyName, string value);

        [DllImport("__Internal")]
        private static extern void _setAppleSearchAdsAttributionEnabled(bool enable);

        [DllImport("__Internal")]
        private static extern void _identify(string userID);

        [DllImport("__Internal")]
        private static extern void _logout();

        [DllImport("__Internal")]
        private static extern void _setUserProperty(string key, string value);

        [DllImport("__Internal")]
        private static extern void _launchWithKey(string key, string callbackName);

        [DllImport("__Internal")]
        private static extern void _addAttributionData(string conversionData, int provider);

        [DllImport("__Internal")]
        private static extern void _checkPermissions(string callbackName);

        [DllImport("__Internal")]
        private static extern void _restore(string callbackName);

        [DllImport("__Internal")]
        private static extern void _purchase(string productID, string callbackName);

        [DllImport("__Internal")]
        private static extern void _purchaseProduct(string productId, string offeringId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _products(string callbackName);

        [DllImport("__Internal")]
        private static extern void _offerings(string callbackName);

        [DllImport("__Internal")]
        private static extern void _checkTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName);

        [DllImport("__Internal")]
        private static extern void _promoPurchase(string storeProductId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _setNotificationsToken(string token);

        [DllImport("__Internal")]
        private static extern bool _handleNotification(string notification);

        [DllImport("__Internal")]
        private static extern void _subscribeOnAutomationEvents();

        [DllImport("__Internal")]
        private static extern void _presentCodeRedemptionSheet();

        [DllImport("__Internal")]
        private static extern void _setPermissionsCacheLifetime(string lifetimeKey);
#endif

        public void Initialize(string gameObjectName)
        {
#if UNITY_IOS
            _initialize(gameObjectName);
#endif
        }

        public void StoreSdkInfo(string version, string source)
        {
#if UNITY_IOS
            _storeSdkInfo(version, source);
#endif
        }

        public void Launch(string projectKey, bool observerMode, string callbackName)
        {
#if UNITY_IOS
            _launchWithKey(projectKey, callbackName);
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

        public void SetUserProperty(string key, string value)
        {
#if UNITY_IOS
            _setUserProperty(key, value);
#endif
        }

        public void SetProperty(UserProperty key, string value)
        {
#if UNITY_IOS
            string propertyName = Enum.GetName(typeof(UserProperty), key);
            _setProperty(propertyName, value);
#endif
        }

        public void AddAttributionData(string conversionData, AttributionSource source)
        {
#if UNITY_IOS
            _addAttributionData(conversionData, (int)source);
#endif
        }

        public void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
#if UNITY_IOS
            _setAppleSearchAdsAttributionEnabled(enable);
#endif
        }

        public void Identify(string userID)
        {
#if UNITY_IOS
            _identify(userID);
#endif
        }

        public void Logout()
        {
#if UNITY_IOS
            _logout();
#endif
        }

        public void PresentCodeRedemptionSheet()
        {
#if UNITY_IOS
            _presentCodeRedemptionSheet();
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

        public void PurchaseProduct(string productId, string offeringId, string callbackName)
        {
#if UNITY_IOS
            _purchaseProduct(productId, offeringId, callbackName);
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

        public void UpdatePurchaseWithProduct(string productId, string offeringId, string oldProductId, ProrationMode prorationMode, string callbackName)
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

        public void CheckTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName)
        {
#if UNITY_IOS
            _checkTrialIntroEligibilityForProductIds(productIdsJson, callbackName);
#endif
        }

        public void PromoPurchase(string storeProductId, string callbackName)
        {
#if UNITY_IOS
            _promoPurchase(storeProductId, callbackName);
#endif
        }

        public void SetNotificationsToken(string token)
        {
#if UNITY_IOS
             _setNotificationsToken(token);
#endif
        }

        public bool HandleNotification(string notification)
        {
#if UNITY_IOS
             return _handleNotification(notification);
#else
            return false;
#endif
        }

        public void SubscribeOnAutomationEvents()
        {
#if UNITY_IOS
            _subscribeOnAutomationEvents();
#endif
        }
        
        public void SetPermissionsCacheLifetime(string lifetime)
        {
#if UNITY_IOS
            _setPermissionsCacheLifetime(lifetime);
#endif
        }
    }
}