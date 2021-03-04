using System.Collections.Generic;

namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void SetDebugMode();
        void Launch(string projectKey);
        void SetUserID(string userID);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
    }
}