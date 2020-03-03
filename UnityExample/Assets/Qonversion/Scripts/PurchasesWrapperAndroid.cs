using UnityEngine;

namespace Qonversion.Scripts
{
    public class PurchasesWrapperAndroid : IPurchasesWrapper
    {
        public void Setup(string gameObject, string projectKey, string userID)
        {
            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {                
                purchases.CallStatic("setup", gameObject, projectKey, userID);
            }
        }

        public void TrackPurchase(string jsonSkuDetails, string jsonPurchaseInfo, string signature)
        {
            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {                
                purchases.CallStatic("trackPurchase", jsonSkuDetails, jsonPurchaseInfo, signature);
            }
        }
    }
}