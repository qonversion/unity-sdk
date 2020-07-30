using System;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperAndroid : IQonversionWrapper
    {
        public void Launch(string projectKey, string userID, bool debugMode, InitDelegate onInitComplete)
        {
            var handler = new QonversionAndroidHandler(onInitComplete);

            if (debugMode)
            {
                //Will be available soon
                Debug.LogWarning("[Qonversion] Not Supported SetDebugMode on Android platform.");
            }

            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {
                purchases.CallStatic("Launch", projectKey, userID, handler);
            }
        }

        public void AddAttributionData(string conversionData, AttributionSource source, string conversionUid)
        {

            string attibutionSource;

            switch(source)
            {
                case AttributionSource.AppsFlyer:
                    attibutionSource = "APPSFLYER";
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
                Debug.LogError(string.Format("[Qonversion] AddAttributionData Marshalling Error: {0}", e));
            }
        }
    }
}