public interface IPurchasesWrapper
{
    void Setup(string gameObject, string projectKey, string userID);
    void TrackPurchase(string jsonSkuDetails, string jsonPurchaseInfo, string signature);
    void TrackPurchaseIos(string receipt);
}
