using System;
using JetBrains.Annotations;
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

        public void Identify(string userID)
        {
            CallQonversion("identify", userID);
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

        public void Purchase(string productId, string callbackName)
        {
            CallQonversion("purchase", productId, callbackName);
        }

        public void PurchaseProduct(string productId, string offeringId, string callbackName)
        {
            CallQonversion("purchaseProduct", productId, offeringId, callbackName);
        }

        public void Restore(string callbackName)
        {
            CallQonversion("restore", callbackName);
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
            CallQonversion("updatePurchase", productId, oldProductId, (int)prorationMode, callbackName);
        }

        public void UpdatePurchaseWithProduct(string productId, string offeringId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
            CallQonversion("updatePurchaseWithProduct", productId, offeringId, oldProductId, (int)prorationMode, callbackName);
        }

        public void Products(string callbackName)
        {
            CallQonversion("products", callbackName);
        }

        public void Offerings(string callbackName)
        {
            CallQonversion("offerings", callbackName);
        }

        public void RemoteConfig(string callbackName)
        {
            CallQonversion("remoteConfig", callbackName);
        }

        public void AttachUserToExperiment(string experimentId, string groupId, string callbackName)
        {
            CallQonversion("attachUserToExperiment", experimentId, groupId, callbackName);
        }

        public void DetachUserFromExperiment(string experimentId, string callbackName)
        {
            CallQonversion("detachUserFromExperiment", experimentId, callbackName);
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