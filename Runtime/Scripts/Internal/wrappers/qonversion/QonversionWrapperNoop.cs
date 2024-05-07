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

        public void SetUserProperty(UserPropertyKey key, string value)
        {
        }

        public void SetCustomUserProperty(string key, string value)
        {
        }

        public void UserProperties(string callbackName)
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

        public void Purchase(PurchaseModel purchaseModel, string callbackName)
        {
        }

        public void Restore(string callbackName)
        {
        }

        public void UpdatePurchase(PurchaseUpdateModel purchaseUpdateModel, string callbackName)
        {
        }

        public void Products(string callbackName)
        {
        }

        public void Offerings(string callbackName)
        {
        }

        public void RemoteConfig(string contextKey, string callbackName)
        {
        }

        public void RemoteConfigList(string callbackName)
        {
        }

        public void RemoteConfigList(string contextKeysJson, bool includeEmptyContextKey, string callbackName)
        {
        }

        public void AttachUserToExperiment(string experimentId, string groupId, string callbackName)
        {
        }

        public void DetachUserFromExperiment(string experimentId, string callbackName)
        {
        }

        public void AttachUserToRemoteConfiguration(string remoteConfigurationId, string callbackName)
        {
        }

        public void DetachUserFromRemoteConfiguration(string remoteConfigurationId, string callbackName)
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

        public void Identify(string userID, string callbackName)
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