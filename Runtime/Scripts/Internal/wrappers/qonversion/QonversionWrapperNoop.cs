namespace QonversionUnity
{
    internal class QonversionWrapperNoop : IQonversionWrapper
    {
        public void Initialize(string gameObjectName)
        {
        }

        public void InitializeSdk(string projectKey, string launchMode, string environment, string entitlementsCacheLifetime,
            string proxyUrl, bool kidsMode)
        {
        }

        public void SyncHistoricalData()
        {
        }

        public void SyncStoreKit2Purchases()
        {
        }

        public void SetUserProperty(string key, string value)
        {
        }

        public void SetProperty(UserProperty key, string value)
        {
        }

        public void AddAttributionData(string conversionData, string providerName)
        {
        }

        public void SyncPurchases()
        {
        }

        public void SetAdvertisingID()
        {
        }

        public void CheckEntitlements(string callbackName)
        {
        }

        public void Purchase(string productId, string callbackName)
        {
        }

        public void PurchaseProduct(string productId, string offeringId, string callbackName)
        {
        }

        public void Restore(string callbackName)
        {
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
        }

        public void UpdatePurchaseWithProduct(string productId, string offeringId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
        }

        public void Products(string callbackName)
        {
        }

        public void Offerings(string callbackName)
        {
        }

        public void RemoteConfig(string callbackName)
        {
        }

        public void AttachUserToExperiment(string experimentId, string groupId, string callbackName)
        {
        }

        public void DetachUserFromExperiment(string experimentId, string callbackName)
        {
        }

        public void StoreSdkInfo(string version, string source)
        {
        }

        public void CheckTrialIntroEligibility(string productIdsJson, string callbackName)
        {
        }

        public void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
        }

        public void Identify(string userID)
        {
        }

        public void Logout()
        {
        }

        public void UserInfo(string callbackName)
        {
        }

        public void PromoPurchase(string storeProductId, string callbackName)
        { 
        }

        public void PresentCodeRedemptionSheet()
        {
        }
    }
}