using System.Collections.Generic;
using System.Runtime.InteropServices;
using SimpleJSON;
using UnityEngine;

#if UNITY_IOS
namespace Qonversion.Scripts
{
    public class PurchasesWrapperIos : IPurchasesWrapper
    {
        [DllImport("__Internal")]
        private static extern void _QonvSetup(string gameObject, string projectKey, string userID);
        [DllImport("__Internal")]
        private static extern void _QonvPushAttribution(string data, int provider, string uid);
        
        //[DllImport("__Internal")]
        //private static extern void _QonvTrackPurchase(string receipt);

        public void Setup(string gameObject, string projectKey, string userID)
        {
            Debug.Log("wr Setup");
            _QonvSetup(gameObject, projectKey, userID);
        }

        public void TrackPurchase(string jsonSkuDetails, string jsonPurchaseInfo, string signature)
        {
            
        }

        public void PushAttribution(Dictionary<string, object> conversionData, string attributionSource, string conversionUid)
        {
            var data = JSONEncoder.Encode(conversionData);
            Debug.Log($"PushAttribution: data:{data}, attributionSource: {attributionSource}, conversionUid:{conversionUid}");
            int provider;
            switch (attributionSource)
            {                
                default:
                    provider = 0;
                    break;
            }
            _QonvPushAttribution(data, provider, conversionUid);
        }


        public void TrackPurchaseIos(string receipt)
        {
            // _QVRSTrackPurchase(receipt);
        }
    }
}
#endif
