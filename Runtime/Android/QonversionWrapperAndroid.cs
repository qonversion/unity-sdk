using System;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperAndroid : IQonversionWrapper
    {
        public void SetDebugMode(bool debugMode)
        {
            //Will be available soon
            Debug.LogWarning("[Qonversion] Not Supported SetDebugMode on Android platform.");
        }

        public void Initialize(string projectKey, string userID, bool debugMode)
        {
            if (debugMode)
            {
                SetDebugMode(debugMode);
            }

            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {
                purchases.CallStatic("initialize", projectKey, userID);
            }
        }

        public void AddAttributionData(string conversionData, AttributionSource source, string conversionUid)
        {

            string attibutionSource;

            switch(source)
            {
                case AttributionSource.AppsFlyer:
                    attibutionSource = "appsflyer";
                    break;
                default:
                    Debug.LogWarning(string.Format("[Qonversion] Not Supported AttributionSource.{0} on Android platform.", source));
                    return;
            }

            try
            {
                using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
                {
                    purchases.CallStatic("attribution", 
                        conversionData,
                        attibutionSource,
                        conversionUid);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Qonversion] AddAttributionData Marshalling Error: {e}");
            }
        }
    }
}