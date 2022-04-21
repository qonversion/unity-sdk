namespace QonversionUnity
 {
    internal class QonversionWrapperNoop : IQonversionWrapper
    {
        public void Initialize(string gameObjectName)
        {
        }
        public void Launch(string projectKey, bool observerMode, string callbackName)
        {
        }

        public void SetDebugMode()
        {
        }

        public void SetUserProperty(string key, string value)
        {
        }

        public void SetProperty(UserProperty key, string value)
        {
        }

        public void AddAttributionData(string conversionData, AttributionSource source)
        {
        }

        public void SyncPurchases()
        {
        }

        public void SetAdvertisingID()
        {
        }

        public void CheckPermissions(string callbackName)
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

        public void StoreSdkInfo(string version, string source)
        {
        }

        public void CheckTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName)
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

        public void PromoPurchase(string storeProductId, string callbackName)
        { 
        }

        public void SetNotificationsToken(string token)
        {
        }

        public bool HandleNotification(string notification)
        {
            return false;
        }

        public void SubscribeOnAutomationEvents()
        {
        }

        public void PresentCodeRedemptionSheet()
        {
        }
    }
  }