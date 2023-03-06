using JetBrains.Annotations;

namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void Initialize(string gameObjectName);
        void InitializeSdk(string projectKey, string launchMode, [CanBeNull] string environment, [CanBeNull] string entitlementsCacheLifetime, [CanBeNull] string proxyUrl, bool kidsMode);
        void StoreSdkInfo(string version, string source);
        void SetAdvertisingID();
        void SetUserProperty(string key, string value);
        void SetProperty(UserProperty key, string value);
        void SyncPurchases();
        void AddAttributionData(string conversionData, string providerName);
        void CheckEntitlements(string callbackName);
        void Purchase(string productId, string callbackName);
        void PurchaseProduct(string productId, string offeringId, string callbackName);
        void Restore(string callbackName);
        void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void UpdatePurchaseWithProduct(string productId, string offeringId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void Products(string callbackName);
        void Offerings(string callbackName);
        void CheckTrialIntroEligibility(string productIdsJson, string callbackName);
        void SetAppleSearchAdsAttributionEnabled(bool enable);
        void Identify(string userID);
        void Logout();
        void UserInfo(string callbackName);
        void PromoPurchase(string storeProductId, string callbackName);
        void PresentCodeRedemptionSheet();
    }
}