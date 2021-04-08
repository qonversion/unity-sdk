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

        public void SetUserID(string userID)
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

        public void CheckPermissions(string callbackName)
        {
        }

        public void Purchase(string productId, string callbackName)
        {
        }

        public void Restore(string callbackName)
        {
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName)
        {
        }

        public void Products(string callbackName)
        {
        }

        public void Offerings(string callbackName)
        {
        }
    }

  }