using System.Collections;
using System.Collections.Generic;
public interface IPurchasesWrapper
{
    void Setup(string gameObject, string projectKey, string userID);
    void TrackPurchase(string jsonSkuDetails, string jsonPurchaseInfo, string signature);
    void PushAttribution(Dictionary<string, object> conversionData, string attributionSource, string conversionUid);
    void TrackPurchaseIos(string receipt);
}
