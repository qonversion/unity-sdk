namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void Initialize(string gameObjectName);
        void StoreSdkInfo(string version, string source);
        void SetDebugMode();
        void SetAdvertisingID();
        void Launch(string projectKey, bool observerMode, string callbackName);
        void SetUserProperty(string key, string value);
        void SetProperty(UserProperty key, string value);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
        void CheckPermissions(string callbackName);
        void Purchase(string productId, string callbackName);
        void PurchaseProduct(string productId, string offeringId, string callbackName);
        void Restore(string callbackName);
        void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void UpdatePurchaseWithProduct(string productId, string offeringId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void Products(string callbackName);
        void Offerings(string callbackName);
        void CheckTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName);
        void SetAppleSearchAdsAttributionEnabled(bool enable);
        void Identify(string userID);
        void Logout();
        void PromoPurchase(string storeProductId, string callbackName);
        void SetNotificationsToken(string token);
        bool HandleNotification(string notification);
        void SubscribeOnAutomationEvents();
        void PresentCodeRedemptionSheet();
        void SetPermissionsCacheLifetime(string lifetime);
    }
}