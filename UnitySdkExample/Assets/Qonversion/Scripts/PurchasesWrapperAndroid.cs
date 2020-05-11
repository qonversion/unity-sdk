using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

        public void PushAttribution(Dictionary<string, object> conversionData, string attributionSource,
            string conversionUid)
        {
            try
            {
                using (var purchases = new AndroidJavaClass("com.qonversion.unitywrapper.QonversionWrapper"))
                {
                    purchases.CallStatic("pushAttribution", 
                        GetHashMapFromDictionary(conversionData), 
                        attributionSource,
                        conversionUid);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"PushAttribution Marshalling Error: {e}");
            }
        }

        private static AndroidJavaObject GetHashMapFromDictionary(Dictionary<string, object> dict)
        {
            // quick out if nothing in the dict param
            if (dict == null || dict.Count <= 0)
            {
                return null;
            }

            AndroidJavaObject hashMap = new AndroidJavaObject("java.util.HashMap");
            IntPtr putMethod = AndroidJNIHelper.GetMethodID(hashMap.GetRawClass(), "put",
                "(Ljava/lang/Object;Ljava/lang/Object;)Ljava/lang/Object;");
            object[] args = new object[2];
            foreach (KeyValuePair<string, object> kvp in dict)
            {
                using (var key = new AndroidJavaObject("java.lang.String", kvp.Key))
                {
                    using (var value = new AndroidJavaObject("java.lang.String", kvp.Value.ToString()))
                    {
                        args[0] = key;
                        args[1] = value;
                        AndroidJNI.CallObjectMethod(hashMap.GetRawObject(), putMethod,
                            AndroidJNIHelper.CreateJNIArgArray(args));
                    }
                }
            }

            return hashMap;
        }

        public void TrackPurchaseIos(string receipt)
        {
        }
    }
}