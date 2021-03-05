using QonversionUnity.MiniJSON;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{

    public static class Qonversion
    {
        private static IQonversionWrapper _Instance;

        private static IQonversionWrapper getFinalInstance()
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

        public static void Launch(string apiKey, bool observerMode)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.Launch(apiKey, observerMode);
        }

        public static void SetDebugMode()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetDebugMode();
        }

        public static void SetUserID(string userID)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetUserID(userID);
        }

        public static void SyncPurchases()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SyncPurchases();
        }

        public static void AddAttributionData(Dictionary<string, object> conversionData, AttributionSource attributionSource)
        {
            AddAttributionData(conversionData.toJson(), attributionSource);
        }

        public static void AddAttributionData(string conversionData, AttributionSource attributionSource)
        {
            IQonversionWrapper instance = getFinalInstance();

            instance.AddAttributionData(conversionData, attributionSource);
        }

        private class PurchasesWrapperNoop : IQonversionWrapper
        {

            public void Launch(string projectKey, bool observerMode)
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
        }
    }
}