using System;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperAndroid : IQonversionWrapper
    {
        public void StoreSdkInfo(string version, string versionKey, string source, string sourceKey)
        {
            CallQonversion("storeSdkInfo", version, versionKey, source, sourceKey);
        }

        public void Launch(string gameObjectName, string projectKey, bool observerMode)
        {
            CallQonversion("launch", gameObjectName, projectKey, observerMode);
        }

        public void SetDebugMode()
        {
            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {
                purchases.CallStatic("setDebugMode");
            }
        }

        public void SetUserProperty(string key, string value)
        {
            CallQonversion("setUserProperty", key, value);
        }

        public void SetProperty(UserProperty key, string value)
        {
            string propertyName = Enum.GetName(typeof(UserProperty), key);
            CallQonversion("setProperty", propertyName, value);
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

        public void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
        }

        public void AddAttributionData(string conversionData, AttributionSource source)
        {
            string attibutionSource;

            switch (source)
            {
                case AttributionSource.AppsFlyer:
                    attibutionSource = "AppsFlyer";
                    break;
                case AttributionSource.Branch:
                    attibutionSource = "Branch";
                    break;
                case AttributionSource.Adjust:
                    attibutionSource = "Adjust";
                    break;
                default:
                    Debug.LogWarning(string.Format("[Qonversion] Not Supported AttributionSource.{0} on Android platform.", source));
                    return;
            }

            try
            {
                CallQonversion("attribution", conversionData, attibutionSource);
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

        public void CheckPermissions(string callbackName)
        {
            CallQonversion("checkPermissions", callbackName);
        }

        public void Purchase(string productId, string callbackName)
        {
            CallQonversion("purchase", productId, callbackName);
        }

        public void PurchaseProduct(string productJson, string callbackName)
        {
            CallQonversion("purchaseProduct", productJson, callbackName);
        }

        public void Restore(string callbackName)
        {
            CallQonversion("restore", callbackName);
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
            CallQonversion("updatePurchase", productId, oldProductId, (int)prorationMode, callbackName);
        }

        public void UpdatePurchaseWithProduct(string productJson, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
            CallQonversion("updatePurchaseWithProduct", productJson, oldProductId, (int)prorationMode, callbackName);
        }

        public void Products(string callbackName)
        {
            CallQonversion("products", callbackName);
        }

        public void Offerings(string callbackName)
        {
            CallQonversion("offerings", callbackName);
        }

        public void CheckTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName)
        {
            CallQonversion("checkTrialIntroEligibilityForProductIds", productIdsJson, callbackName);
        }

        private const string QonversionWrapper = "com.qonversion.unitywrapper.QonversionWrapper";

        private static void CallQonversion(string methodName, params object[] args)
        {
            using (var qonversion = new AndroidJavaClass(QonversionWrapper))
            {
                qonversion.CallStatic(methodName, args);
            }
        }

        public void PromoPurchase(string storeProductId, string callbackName)
        {
        }

        public void AddPromoPurchaseDelegate()
        {
        }

        public void RemovePromoPurchaseDelegate()
        {
        }
    }
}