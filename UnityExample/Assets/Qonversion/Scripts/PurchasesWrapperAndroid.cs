using UnityEngine;

namespace Qonversion.Scripts
{
    public class PurchasesWrapperAndroid : IPurchasesWrapper
    {
        public void Setup(string gameObject, string projectKey, string userID, bool autoTracking, bool useQonversionBilling)
        {
            using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
            {                
                purchases.CallStatic("setup", gameObject, projectKey, userID, autoTracking, useQonversionBilling);
            }
        }
    }
}