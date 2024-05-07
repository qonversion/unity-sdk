using System;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperAndroid : IQonversionWrapper
    {
        public void Initialize(string gameObjectName)
        {
            CallQonversion("initialize", gameObjectName);
        }

        public void StoreSdkInfo(string version, string source)
        {
            CallQonversion("storeSdkInfo", version, source);
        }

        public void InitializeSdk(string projectKey, string launchMode, string environment, string entitlementsCacheLifetime,
            string proxyUrl, bool kidsMode)
        {
            CallQonversion("initializeSdk", projectKey, launchMode, environment, entitlementsCacheLifetime, proxyUrl, kidsMode);
        }

        public void SyncHistoricalData() 
        {
            CallQonversion("syncHistoricalData");
        }

        public void SyncStoreKit2Purchases()
        {
        }

        public void SetDebugMode()
        {
            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {
                purchases.CallStatic("setDebugMode");
            }
        }

        public void SetUserProperty(UserPropertyKey key, string value)
        {
            string propertyName = Enum.GetName(typeof(UserPropertyKey), key);
            CallQonversion("setUserProperty", propertyName, value);
        }

        public void SetCustomUserProperty(string key, string value)
        {
            CallQonversion("setCustomUserProperty", key, value);
        }

        public void UserProperties(string callbackName)
        {
            CallQonversion("userProperties", callbackName);
        }

        public void SyncPurchases()
        {
            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {
                purchases.CallStatic("syncPurchases");
            }
        }

        public void SetAdvertisingID()
        {
        }

        public void PresentCodeRedemptionSheet()
        {
        }

        public void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
        }

        public void AddAttributionData(string conversionData, string providerName)
        {
            try
            {
                CallQonversion("attribution", conversionData, providerName);
            }
            catch (Exception e)
            {
                Debug.LogError(string.Format("[Qonversion] AddAttributionData Marshalling Error: {0}", e));
            }
        }

        public void Identify(string userID, string callbackName)
        {
            CallQonversion("identify", userID, callbackName);
        }

        public void Logout()
        {
            CallQonversion("logout");
        }

        public void UserInfo(string callbackName)
        {
            CallQonversion("userInfo", callbackName);
        }

        public void CheckEntitlements(string callbackName)
        {
            CallQonversion("checkEntitlements", callbackName);
        }

        public void Purchase(PurchaseModel purchaseModel, string callbackName)
        {
            CallQonversion(
                "purchase",
                purchaseModel.ProductId,
                purchaseModel.OfferId,
                purchaseModel.ApplyOffer,
                callbackName
            );
        }

        public void Restore(string callbackName)
        {
            CallQonversion("restore", callbackName);
        }

        public void UpdatePurchase(PurchaseUpdateModel purchaseUpdateModel, string callbackName) {
            string updatePolicyKey = purchaseUpdateModel.UpdatePolicy == null
                ? null
                : Enum.GetName(typeof(PurchaseUpdatePolicy), purchaseUpdateModel.UpdatePolicy);

            CallQonversion(
                "updatePurchase",
                purchaseUpdateModel.ProductId,
                purchaseUpdateModel.OfferId,
                purchaseUpdateModel.ApplyOffer,
                purchaseUpdateModel.OldProductId,
                updatePolicyKey,
                callbackName
            );
        }

        public void Products(string callbackName)
        {
            CallQonversion("products", callbackName);
        }

        public void Offerings(string callbackName)
        {
            CallQonversion("offerings", callbackName);
        }

        public void RemoteConfig(string contextKey, string callbackName)
        {
            CallQonversion("remoteConfig", contextKey, callbackName);
        }

        public void RemoteConfigList(string callbackName)
        {
            CallQonversion("remoteConfigList", callbackName);
        }

        public void RemoteConfigList(string contextKeysJson, bool includeEmptyContextKey, string callbackName)
        {
            CallQonversion("remoteConfigList", contextKeysJson, includeEmptyContextKey, callbackName);
        }

        public void AttachUserToExperiment(string experimentId, string groupId, string callbackName)
        {
            CallQonversion("attachUserToExperiment", experimentId, groupId, callbackName);
        }

        public void DetachUserFromExperiment(string experimentId, string callbackName)
        {
            CallQonversion("detachUserFromExperiment", experimentId, callbackName);
        }

        public void AttachUserToRemoteConfiguration(string remoteConfigurationId, string callbackName)
        {
            CallQonversion("attachUserToRemoteConfiguration", remoteConfigurationId, callbackName);
        }

        public void DetachUserFromRemoteConfiguration(string remoteConfigurationId, string callbackName)
        {
            CallQonversion("detachUserFromRemoteConfiguration", remoteConfigurationId, callbackName);
        }

        public void CheckTrialIntroEligibility(string productIdsJson, string callbackName)
        {
            CallQonversion("checkTrialIntroEligibility", productIdsJson, callbackName);
        }

        public void PromoPurchase(string storeProductId, string callbackName)
        {
        }

        private const string QonversionWrapper = "com.qonversion.unitywrapper.QonversionWrapper";

        private static T CallQonversion<T>(string methodName, params object[] args)
        {
            using (var qonversion = new AndroidJavaClass(QonversionWrapper))
            {
                return qonversion.CallStatic<T>(methodName, args);
            }
        }

        private static void CallQonversion(string methodName, params object[] args)
        {
            using (var qonversion = new AndroidJavaClass(QonversionWrapper))
            {
                qonversion.CallStatic(methodName, args);
            }
        }
    }
}