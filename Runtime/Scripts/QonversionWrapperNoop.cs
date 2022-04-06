namespace QonversionUnity
 {
    internal class QonversionWrapperNoop : IQonversionWrapper
    {
        public void Launch(string gameObjectName, string projectKey, bool observerMode)
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

        public void PurchaseProduct(string productJson, string callbackName)
        {
        }

        public void Restore(string callbackName)
        {
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
        }

        public void UpdatePurchaseWithProduct(string productJson, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
        }

        public void Products(string callbackName)
        {
        }

        public void Offerings(string callbackName)
        {
        }

        public void StoreSdkInfo(string version, string versionKey, string source, string sourceKey)
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

        public void AddPromoPurchasesDelegate()
        {  
        }

        public void RemovePromoPurchasesDelegate()
        {
        }
        public void AddUpdatedPurchasesDelegate()
        {
        }

        public void RemoveUpdatedPurchasesDelegate()
        {
        }

        public void SetNotificationsToken(string token)
        {
        }

        public bool HandleNotification(string notification)
        {
            return false;
        }

        public void AddAutomationsDelegate()
        {

        }

        public void PresentCodeRedemptionSheet()
        {

        }
    }
  }