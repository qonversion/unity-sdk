using System.Collections.Generic;

namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void Initialize(string projectKey, string userID, bool debugMode);
        void AddAttributionData(string conversionData, AttributionSource source, string conversionUid);
    }
}