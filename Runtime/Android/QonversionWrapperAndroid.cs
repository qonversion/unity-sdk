using System;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionWrapperAndroid : AndroidJavaProxy, IQonversionWrapper, IQonversionResultHandler
    {
        private const string QONVERSION_WRAPPER_INTERFACE_PATH = "com.qonversion.unitywrapper.IQonversionResultHandler";

        private InitDelegate onInitCompleteDelegate;

        public QonversionWrapperAndroid() : base(QONVERSION_WRAPPER_INTERFACE_PATH)
        {

        }

        public void Launch(string projectKey, string userID, bool debugMode, InitDelegate onInitComplete)
        {
            onInitCompleteDelegate = onInitComplete;

            if (debugMode)
            {
                //Will be available soon
                Debug.LogWarning("[Qonversion] Not Supported SetDebugMode on Android platform.");
            }

            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {
                purchases.CallStatic("Launch", projectKey, userID, this);
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

        public void onSuccessInit(string uid)
        {
            onInitCompleteDelegate?.Invoke();
        }

        public void onErrorInit(string errorMessage)
        {
            Debug.LogError(string.Format("[Qonversion] onErrorInit Error: {0}", errorMessage));
        }
    }
}