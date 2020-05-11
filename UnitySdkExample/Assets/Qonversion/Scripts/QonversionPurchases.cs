using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Qonversion.Scripts
{
    public class QonversionPurchases : MonoBehaviour
    {
        private IPurchasesWrapper _wrapper;

        [Tooltip("Your Qonversion API Key. Get from https://qonversion.io/")]
        // ReSharper disable once InconsistentNaming
        public string qonversionAPIKey;


        public void Initialize(string appUserID)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _wrapper = new PurchasesWrapperAndroid();
#elif UNITY_IOS && !UNITY_EDITOR
            _wrapper = new PurchasesWrapperIos();
#else
            _wrapper = new PurchasesWrapperNoop();
#endif
            Setup(string.IsNullOrEmpty(appUserID) ? null : appUserID);
        }
        
        private void Setup(string newUserId)
        {            
            _wrapper.Setup(gameObject.name, qonversionAPIKey, newUserId);            
        }

        public void TrackPurchase(string jsonSkuDetails, string jsonPurchaseInfo, string signature)
        {
            _wrapper.TrackPurchase(jsonSkuDetails, jsonPurchaseInfo, signature);
        }

        public void PushAttribution(Dictionary<string, object> conversionData, string attributionSource, string conversionUid)
        {
            _wrapper.PushAttribution(conversionData, attributionSource, conversionUid);
        }
        

        private class PurchasesWrapperNoop : IPurchasesWrapper
        {
            public void Setup(string gameObject, string projectKey, string userID)
            {
            }

            public void TrackPurchase(string jsonSkuDetails, string jsonPurchaseInfo, string signature)
            {
            }

            public void PushAttribution(Dictionary<string, object> conversionData, string attributionSource, string conversionUid)
            {
                
            }

            public void TrackPurchaseIos(string receipt)
            {
                
            }
        }
    }
}