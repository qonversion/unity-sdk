using System.Runtime.InteropServices;
using UnityEngine;

#if UNITY_IOS
namespace Qonversion.Scripts
{
    public class PurchasesWrapperIos : IPurchasesWrapper
    {
        [DllImport("__Internal")]
        private static extern void _QonvSetup(string gameObject, string projectKey, string userID);

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

        public void TrackPurchaseIos(string receipt)
        {
            // _QVRSTrackPurchase(receipt);
        }
    }
}
#endif
