#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using System;
using UnityEngine;
using System.IO;

namespace QonversionUnity
{
    internal class QonversionWrapperIOS : IQonversionWrapper
    {
#if UNITY_IOS
        private const string FallbackFileName = "qonversion_ios_fallbacks.json";

        [DllImport("__Internal")]
        private static extern void _initialize(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _initializeSdk(string projectKey, string launchMode, string environment, string entitlementsCacheLifetime, string proxyUrl);

        [DllImport("__Internal")]
        private static extern void _storeSdkInfo(string version, string source);

        [DllImport("__Internal")]
        private static extern void _syncHistoricalData();

        [DllImport("__Internal")]
        private static extern void _syncStoreKit2Purchases();

        [DllImport("__Internal")]
        private static extern void _setAdvertisingID();

        [DllImport("__Internal")]
        private static extern void _setUserProperty(string propertyName, string value);

        [DllImport("__Internal")]
        private static extern void _setCustomUserProperty(string key, string value);

        [DllImport("__Internal")]
        private static extern void _userProperties(string callbackName);

        [DllImport("__Internal")]
        private static extern void _setAppleSearchAdsAttributionEnabled(bool enable);

        [DllImport("__Internal")]
        private static extern void _identify(string userID, string callbackName);

        [DllImport("__Internal")]
        private static extern void _logout();

        [DllImport("__Internal")]
        private static extern void _addAttributionData(string conversionData, string providerName);

        [DllImport("__Internal")]
        private static extern void _checkPermissions(string callbackName);

        [DllImport("__Internal")]
        private static extern void _restore(string callbackName);

        [DllImport("__Internal")]
        private static extern void _userInfo(string callbackName);

        [DllImport("__Internal")]
        private static extern void _purchase(string productID, string callbackName);

        [DllImport("__Internal")]
        private static extern void _products(string callbackName);

        [DllImport("__Internal")]
        private static extern void _offerings(string callbackName);

        [DllImport("__Internal")]
        private static extern void _remoteConfig(string contextKey, string callbackName);

        [DllImport("__Internal")]
        private static extern void _remoteConfigList(string callbackName);

        [DllImport("__Internal")]
        private static extern void _remoteConfigListForContextKeys(string contextKeysJson, bool includeEmptyContextKey, string callbackName);

        [DllImport("__Internal")]
        private static extern void _attachUserToExperiment(string experimentId, string groupId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _detachUserFromExperiment(string experimentId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _attachUserToRemoteConfiguration(string remoteConfigurationId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _detachUserFromRemoteConfiguration(string remoteConfigurationId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _isFallbackFileAccessible(string callbackName);

        [DllImport("__Internal")]
        private static extern void _checkTrialIntroEligibility(string productIdsJson, string callbackName);

        [DllImport("__Internal")]
        private static extern void _promoPurchase(string storeProductId, string callbackName);

        [DllImport("__Internal")]
        private static extern void _presentCodeRedemptionSheet();
#endif

        public void Initialize(string gameObjectName)
        {
#if UNITY_IOS
            try
            {
                string filePath = Path.Combine(Application.streamingAssetsPath, FallbackFileName);

                if (File.Exists(filePath))
                {
                    string result = System.IO.File.ReadAllText(filePath);
                    File.WriteAllText(Application.persistentDataPath + "/" + FallbackFileName, result);
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Fallback file is not accessible");
            }
            
            _initialize(gameObjectName);
#endif
        }

        public void StoreSdkInfo(string version, string source)
        {
#if UNITY_IOS
            _storeSdkInfo(version, source);
#endif
        }

        public void InitializeSdk(string projectKey, string launchMode, string environment, string entitlementsCacheLifetime,
            string proxyUrl, bool kidsMode)
        {
#if UNITY_IOS
            _initializeSdk(projectKey, launchMode, environment, entitlementsCacheLifetime, proxyUrl);
#endif
        }

        public void SyncHistoricalData()
        {
#if UNITY_IOS
            _syncHistoricalData();
#endif
        }

        public void SyncStoreKit2Purchases()
        {
#if UNITY_IOS
            _syncStoreKit2Purchases();
#endif
        }

        public void SyncPurchases()
        {
        }

        public void SetAdvertisingID()
        {
#if UNITY_IOS
            _setAdvertisingID();
#endif
        }

        public void SetUserProperty(UserPropertyKey key, string value)
        {
#if UNITY_IOS
            string propertyName = Enum.GetName(typeof(UserPropertyKey), key);
            _setUserProperty(propertyName, value);
#endif
        }

        public void SetCustomUserProperty(string key, string value)
        {
#if UNITY_IOS
            _setCustomUserProperty(key, value);
#endif
        }

        public void UserProperties(string callbackName) {
#if UNITY_IOS
            _userProperties(callbackName);
#endif
        }

        public void AddAttributionData(string conversionData, string providerName)
        {
#if UNITY_IOS
            _addAttributionData(conversionData, providerName);
#endif
        }

        public void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
#if UNITY_IOS
            _setAppleSearchAdsAttributionEnabled(enable);
#endif
        }

        public void Identify(string userID, string callbackName)
        {
#if UNITY_IOS
            _identify(userID, callbackName);
#endif
        }

        public void Logout()
        {
#if UNITY_IOS
            _logout();
#endif
        }

        public void UserInfo(string callbackName)
        {
#if UNITY_IOS
            _userInfo(callbackName);
#endif
        }

        public void PresentCodeRedemptionSheet()
        {
#if UNITY_IOS
            _presentCodeRedemptionSheet();
#endif
        }

        public void CheckEntitlements(string callbackName)
        {
#if UNITY_IOS
            _checkPermissions(callbackName);
#endif
        }

        public void Purchase(PurchaseModel purchaseModel, string callbackName)
        {
#if UNITY_IOS
            _purchase(purchaseModel.ProductId, callbackName);
#endif
        }

        public void Restore(string callbackName)
        {
#if UNITY_IOS
            _restore(callbackName);
#endif
        }

        public void UpdatePurchase(PurchaseUpdateModel purchaseUpdateModel, string callbackName)
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

        public void RemoteConfig(string contextKey, string callbackName)
        {
#if UNITY_IOS
            _remoteConfig(contextKey, callbackName);
#endif
        }

        public void RemoteConfigList(string callbackName)
        {
#if UNITY_IOS
            _remoteConfigList(callbackName);
#endif
        }

        public void RemoteConfigList(string contextKeysJson, bool includeEmptyContextKey, string callbackName) 
        {
#if UNITY_IOS
            _remoteConfigListForContextKeys(contextKeysJson, includeEmptyContextKey, callbackName);
#endif
        }

        public void AttachUserToExperiment(string experimentId, string groupId, string callbackName)
        {
#if UNITY_IOS
            _attachUserToExperiment(experimentId, groupId,callbackName);
#endif
        }

        public void DetachUserFromExperiment(string experimentId, string callbackName)
        {
#if UNITY_IOS
            _detachUserFromExperiment(experimentId, callbackName);
#endif
        }

        public void AttachUserToRemoteConfiguration(string remoteConfigurationId, string callbackName)
        {
#if UNITY_IOS
            _attachUserToRemoteConfiguration(remoteConfigurationId, callbackName);
#endif
        }

        public void DetachUserFromRemoteConfiguration(string remoteConfigurationId, string callbackName)
        {
#if UNITY_IOS
            _detachUserFromRemoteConfiguration(remoteConfigurationId, callbackName);
#endif
        }

        public void IsFallbackFileAccessible(string callbackName)
        {
#if UNITY_IOS
            _isFallbackFileAccessible(callbackName);
#endif
        }

        public void CheckTrialIntroEligibility(string productIdsJson, string callbackName)
        {
#if UNITY_IOS
            _checkTrialIntroEligibility(productIdsJson, callbackName);
#endif
        }

        public void PromoPurchase(string storeProductId, string callbackName)
        {
#if UNITY_IOS
            _promoPurchase(storeProductId, callbackName);
#endif
        }
    }
}