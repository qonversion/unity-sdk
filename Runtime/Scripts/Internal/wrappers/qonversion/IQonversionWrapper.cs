using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    internal interface IQonversionWrapper
    {
        void Initialize(string gameObjectName);
        void InitializeSdk(string projectKey, string launchMode, [CanBeNull] string environment, [CanBeNull] string entitlementsCacheLifetime, [CanBeNull] string proxyUrl, bool kidsMode);
        void StoreSdkInfo(string version, string source);
        void SyncHistoricalData();
        void SyncStoreKit2Purchases();
        void SetAdvertisingID();
        void SetUserProperty(UserPropertyKey key, string value);
        void SetCustomUserProperty(string key, string value);
        void UserProperties(string callbackName);
        void SyncPurchases();
        void AddAttributionData(string conversionData, string providerName);
        void CheckEntitlements(string callbackName);
        void Purchase(PurchaseModel purchaseModel, string callbackName);
        void Restore(string callbackName);
        void UpdatePurchase(PurchaseUpdateModel purchaseUpdateModel, string callbackName);
        void Products(string callbackName);
        void Offerings(string callbackName);
        void RemoteConfig([CanBeNull] string contextKey, string callbackName);
        void RemoteConfigList(string callbackName);
        void RemoteConfigList(string contextKeysJson, bool includeEmptyContextKey, string callbackName);
        void AttachUserToExperiment(string experimentId, string groupId, string callbackName);
        void DetachUserFromExperiment(string experimentId, string callbackName);
        void AttachUserToRemoteConfiguration(string remoteConfigurationId, string callbackName);
        void DetachUserFromRemoteConfiguration(string remoteConfigurationId, string callbackName);
        void IsFallbackFileAccessible(string callbackName);
        void CheckTrialIntroEligibility(string productIdsJson, string callbackName);
        void SetAppleSearchAdsAttributionEnabled(bool enable);
        void Identify(string userID, string callbackName);
        void Logout();
        void UserInfo(string callbackName);
        void PromoPurchase(string storeProductId, string callbackName);
        void PresentCodeRedemptionSheet();
    }
}