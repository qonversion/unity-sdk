using System;
using UnityEngine;

namespace Qonversion.Scripts
{
    public class QonversionPurchases : MonoBehaviour
    {
        private IPurchasesWrapper _wrapper;

        [Tooltip("Your Qonversion API Key. Get from https://qonversion.io/")]
        // ReSharper disable once InconsistentNaming
        public string qonversionAPIKey;
        
        [Tooltip("An optional bool.")]
        public bool autoTrackingMode;
        
        [Tooltip("An optional bool.")]
        public bool useQonversionBilling;

        private Action<string> _onMakePurchase;

        public void Initialize(string appUserID, Action<string> onMakePurchase)
        {
            _onMakePurchase = onMakePurchase;
#if UNITY_ANDROID && !UNITY_EDITOR
            _wrapper = new PurchasesWrapperAndroid();
#elif UNITY_IPHONE && !UNITY_EDITOR
//        _wrapper = new PurchasesWrapperiOS();
#else
            _wrapper = new PurchasesWrapperNoop();
#endif
            Setup(string.IsNullOrEmpty(appUserID) ? null : appUserID);
//        GetProducts(productIdentifiers, null);
        }
        
        private void Setup(string newUserId)
        {            
            _wrapper.Setup(gameObject.name, qonversionAPIKey, newUserId, autoTrackingMode, useQonversionBilling);            
        }
        
        
        private void _makePurchase(string data)
        {
            _onMakePurchase?.Invoke(data);
        }

        private class PurchasesWrapperNoop : IPurchasesWrapper
        {
            public void Setup(string gameObject, string projectKey, string userID, bool autoTracking,
                bool useQonversionBilling)
            {
            }
        }
    }
}