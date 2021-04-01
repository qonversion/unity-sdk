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

        public void AddAttributionData(string conversionData, AttributionSource source)
        {
        }

        public void SyncPurchases()
        {
        }

        public void CheckPermissions()
        {
        }

        public void Purchase(string productId)
        {
        }

        public void Restore()
        {
        }

        public void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode)
        {
        }

        public void Products()
        {
        }

        public void Offerings()
        {
        }
    }

  }