using System.Collections.Generic;

namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void SetDebugMode();
        void Launch(string projectKey, bool observerMode);
        void SetUserID(string userID);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
    }
}