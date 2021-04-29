namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void StoreSdkInfo(string version, string versionKey, string source, string sourceKey);
        void SetDebugMode();
        void SetAdvertisingID();
        void Launch(string gameObjectName, string projectKey, bool observerMode);
        void SetUserID(string userID);
        void SetUserProperty(string key, string value);
        void SetProperty(UserProperty key, string value);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
        void CheckPermissions(string callbackName);
        void Purchase(string productId, string callbackName);
        void Restore(string callbackName);
        void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void Products(string callbackName);
        void Offerings(string callbackName);
        void CheckTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName);
    }
}