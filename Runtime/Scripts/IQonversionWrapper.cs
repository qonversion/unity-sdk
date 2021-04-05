﻿namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void SetDebugMode();
        void Launch(string gameObjectName, string projectKey, bool observerMode);
        void SetUserID(string userID);
        void SyncPurchases();
        void AddAttributionData(string conversionData, AttributionSource source);
        void CheckPermissions(string callbackName);
        void Purchase(string productId, string callbackName);
        void Restore(string callbackName);
        void UpdatePurchase(string productId, string oldProductId, ProrationMode prorationMode, string callbackName);
        void Products(string callbackName);
        void Offerings(string callbackName);
    }
}