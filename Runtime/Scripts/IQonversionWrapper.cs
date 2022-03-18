﻿namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void StoreSdkInfo(string version, string versionKey, string source, string sourceKey);
        void SetDebugMode();
        void SetAdvertisingID();
        void Launch(string gameObjectName, string projectKey, bool observerMode);
        void SetUserProperty(string key, string value);
        void SetProperty(UserProperty key, string value);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
        void CheckPermissions(string callbackName);
        void Purchase(string productId, string callbackName);
        void PurchaseProduct(string productJson, string callbackName);
        void Restore(string callbackName);
        void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void UpdatePurchaseWithProduct(string productJson, string oldProductId, ProrationMode prorationMode, string callbackName);
        void Products(string callbackName);
        void Offerings(string callbackName);
        void CheckTrialIntroEligibilityForProductIds(string productIdsJson, string callbackName);
        void SetAppleSearchAdsAttributionEnabled(bool enable);
        void Identify(string userID);
        void Logout();
        void PromoPurchase(string storeProductId, string callbackName);
        void AddPromoPurchasesDelegate();
        void RemovePromoPurchasesDelegate();
        void AddUpdatedPurchasesDelegate();
        void RemoveUpdatedPurchasesDelegate();
        void SetNotificationsToken(string token);
        bool HandleNotification(string notification);
        void AddAutomationsDelegate();
    }
}