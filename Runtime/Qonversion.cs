using QonversionUnity.MiniJSON;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public static class Qonversion
    {
        private static IQonversionWrapper _Instance;

        internal static IQonversionWrapper CurrentInstance
        {
            get
            {
                if (_Instance == null)
                {
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
                }

                return _Instance;
            }
        }

        public static void Initialize(string apiKey)
        {
            Initialize(apiKey, null);
        }

        public static void Initialize(string apiKey, bool debugMode)
        {
            Initialize(apiKey, null, debugMode);
        }

        public static void Initialize(string apiKey, string appUserID)
        {
            Initialize(apiKey, appUserID, false);
        }

        public static void Initialize(string apiKey, string appUserID, bool debugMode)
        {
            Debug.Log(string.Format("[Qonversion] Initialize userID={0}", appUserID));

            CurrentInstance.Initialize(apiKey, string.IsNullOrEmpty(appUserID) ? null : appUserID, debugMode);
        }

        public static void AddAttributionData(Dictionary<string, object> conversionData, AttributionSource attributionSource, string conversionUid)
        {
            AddAttributionData(conversionData.toJson(), attributionSource, conversionUid);
        }

        public static void AddAttributionData(string conversionData, AttributionSource attributionSource, string conversionUid)
        {
            CurrentInstance.AddAttributionData(conversionData, attributionSource, conversionUid);
        }

        private class PurchasesWrapperNoop : IQonversionWrapper
        {

            public void Initialize(string projectKey, string userID, bool debugMode)
            {
            }

            public void AddAttributionData(string conversionData, AttributionSource source, string conversionUid)
            {

            }
        }
    }
}