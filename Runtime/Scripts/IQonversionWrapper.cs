namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void SetDebugMode();
        void Launch(string gameObjectName, string projectKey, bool observerMode);
        void SetUserID(string userID);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
        void CheckPermissions();
        void Purchase(string productId);
        void Restore();
        void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode);
        void Products();
        void Offerings();
    }
}