using QonversionUnity.MiniJSON;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public static class Qonversion
    {
        private static IQonversionWrapper _Instance;

        public static void Launch(string apiKey)
        {
            Launch(apiKey, null);
        }

        public static void Launch(string apiKey, bool debugMode)
        {
            Launch(apiKey, null, debugMode);
        }

        public static void Launch(string apiKey, string appUserID)
        {
            Launch(apiKey, appUserID, false);
        }

        public static void Launch(string apiKey, string appUserID, bool debugMode)
        {
            if(_Instance != null)
            {
                return;
            }

            Debug.Log(string.Format("[Qonversion] Launch userID={0}", appUserID));

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _Instance = new QonversionWrapperAndroid();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _Instance = new QonversionWrapperIOS();
                    break;
                default:
                    _Instance = new PurchasesWrapperNoop();
                    break;
            }

            _Instance.Launch(apiKey, string.IsNullOrEmpty(appUserID) ? null : appUserID, debugMode);
        }

        public static void AddAttributionData(Dictionary<string, object> conversionData, AttributionSource attributionSource, string conversionUid)
        {
            AddAttributionData(conversionData.toJson(), attributionSource, conversionUid);
        }

        public static void AddAttributionData(string conversionData, AttributionSource attributionSource, string conversionUid)
        {
            if(_Instance == null)
            {
                throw new System.InvalidOperationException("The SDK has not been initialized, make sure to call "
                            + "Qonversion.Launch() first.");
            }

            _Instance.AddAttributionData(conversionData, attributionSource, conversionUid);
        }

        private class PurchasesWrapperNoop : IQonversionWrapper
        {

            public void Launch(string projectKey, string userID, bool debugMode)
            {
            }

            public void AddAttributionData(string conversionData, AttributionSource source, string conversionUid)
            {

            }
        }
    }
}