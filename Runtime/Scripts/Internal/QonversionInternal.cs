using JetBrains.Annotations;
using QonversionUnity.MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    internal class QonversionInternal : MonoBehaviour, IQonversion
    {
        private const string GameObjectName = "QonvesrionRuntimeGameObject";
        private const string OnCheckEntitlementsMethodName = "OnCheckEntitlements";
        private const string OnPurchaseMethodName = "OnPurchase";
        private const string OnPromoPurchaseMethodName = "OnPromoPurchase";
        private const string OnUpdatePurchaseMethodName = "OnUpdatePurchase";
        private const string OnRestoreMethodName = "OnRestore";
        private const string OnProductsMethodName = "OnProducts";
        private const string OnOfferingsMethodName = "OnOfferings";
        private const string OnRemoteConfigMethodName = "OnRemoteConfig";
        private const string OnRemoteConfigListMethodName = "OnRemoteConfigList";
        private const string OnRemoteConfigListForContextKeysMethodName = "OnRemoteConfigListForContextKeys";
        private const string OnEligibilitiesMethodName = "OnEligibilities";
        private const string OnIdentityMethodName = "OnIdentity";
        private const string OnUserInfoMethodName = "OnUserInfo";
        private const string OnUserPropertiesMethodName = "OnUserProperties";
        private const string OnAttachUserMethodName = "OnAttachUser";
        private const string OnDetachUserMethodName = "OnDetachUser";

        private const string SdkVersion = "7.4.0";
        private const string SdkSource = "unity";

        private const string DefaultRemoteConfigContextKey = "";

        private IQonversionWrapper _nativeWrapperInstance;
        private Qonversion.OnUpdatedEntitlementsReceived _onUpdatedEntitlementsReceived;

        private Qonversion.OnPromoPurchasesReceived _onPromoPurchasesReceived;
        private string _storedPromoProductId = null;

        private List<Qonversion.OnEntitlementsReceived> CheckEntitlementsCallbacks { get; } = new List<Qonversion.OnEntitlementsReceived>();
        private List<Qonversion.OnEntitlementsReceived> RestoreCallbacks { get; } = new List<Qonversion.OnEntitlementsReceived>();
        private Qonversion.OnPurchaseResultReceived PurchaseCallback { get; set; }
        private Qonversion.OnPurchaseResultReceived UpdatePurchaseCallback { get; set; }
        private List<Qonversion.OnProductsReceived> ProductsCallbacks { get; } = new List<Qonversion.OnProductsReceived>();
        private List<Qonversion.OnOfferingsReceived> OfferingsCallbacks { get; } = new List<Qonversion.OnOfferingsReceived>();
        private Dictionary<string, List<Qonversion.OnRemoteConfigReceived>> RemoteConfigCallbacks { get; } = new Dictionary<string, List<Qonversion.OnRemoteConfigReceived>>();
        private Qonversion.OnRemoteConfigListReceived RemoteConfigListCallback { get; set; }
        private Qonversion.OnRemoteConfigListReceived RemoteConfigListForContextKeysCallback { get; set; }
        private Qonversion.OnEligibilitiesReceived EligibilitiesCallback { get; set; }
        private Qonversion.OnEntitlementsReceived PromoPurchaseCallback { get; set; }
        private Qonversion.OnUserInfoReceived IdentityCallback { get; set; }
        private Qonversion.OnUserInfoReceived UserInfoCallback { get; set; }
        private Qonversion.OnUserPropertiesReceived UserPropertiesCallback { get; set; }
        private Qonversion.OnAttachUserResponseReceived AttachUserCallback { get; set; }
        private Qonversion.OnAttachUserResponseReceived DetachUserCallback { get; set; }

        public event Qonversion.OnPromoPurchasesReceived PromoPurchasesReceived
        {
            add
            {
                _onPromoPurchasesReceived += value;
            }
            remove
            {
                _onPromoPurchasesReceived -= value;
            }
        }

        public event Qonversion.OnUpdatedEntitlementsReceived UpdatedEntitlementsReceived
        {
            add
            {
                _onUpdatedEntitlementsReceived += value;
            }
            remove
            {
                _onUpdatedEntitlementsReceived -= value;
            }
        }

        public static QonversionInternal CreateInstance()
        {
            GameObject go = new GameObject(GameObjectName);
            QonversionInternal instance = go.AddComponent<QonversionInternal>();
            DontDestroyOnLoad(go);

            return instance;
        }

        void IQonversion.InitializeInstance(QonversionConfig config)
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.StoreSdkInfo(SdkVersion, SdkSource);

            string launchModeKey = Enum.GetName(typeof(LaunchMode), config.LaunchMode);
            string environmentKey = Enum.GetName(typeof(Environment), config.Environment);
            string cacheLifetimeKey = Enum.GetName(typeof(EntitlementsCacheLifetime), config.EntitlementsCacheLifetime);

            instance.InitializeSdk(config.ProjectKey, launchModeKey, environmentKey, cacheLifetimeKey, config.ProxyUrl, config.KidsMode);
        }

        public void SyncHistoricalData()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SyncHistoricalData();
        }

        public void SyncStoreKit2Purchases()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SyncStoreKit2Purchases();
        }

        public void Purchase(PurchaseModel purchaseModel, Qonversion.OnPurchaseResultReceived callback)
        {
            PurchaseCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Purchase(purchaseModel, OnPurchaseMethodName);
        }

        public void UpdatePurchase(PurchaseUpdateModel purchaseUpdateModel, Qonversion.OnPurchaseResultReceived callback)
        {
            UpdatePurchaseCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.UpdatePurchase(purchaseUpdateModel, OnUpdatePurchaseMethodName);
        }

        public void Products(Qonversion.OnProductsReceived callback)
        {
            ProductsCallbacks.Add(callback);
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Products(OnProductsMethodName);
        }

        public void Offerings(Qonversion.OnOfferingsReceived callback)
        {
            OfferingsCallbacks.Add(callback);
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Offerings(OnOfferingsMethodName);
        }

        public void RemoteConfig(Qonversion.OnRemoteConfigReceived callback)
        {
            LoadRemoteConfig(null, callback);
        }

        public void RemoteConfig(string contextKey, Qonversion.OnRemoteConfigReceived callback)
        {
            LoadRemoteConfig(contextKey, callback);
        }

        private void LoadRemoteConfig([CanBeNull] string contextKey, Qonversion.OnRemoteConfigReceived callback)
        {
            var key = contextKey ?? DefaultRemoteConfigContextKey;
            if (!RemoteConfigCallbacks.ContainsKey(key)) {
                RemoteConfigCallbacks[key] = new List<Qonversion.OnRemoteConfigReceived>();
            }

            RemoteConfigCallbacks[key].Add(callback);
            var instance = GetNativeWrapper();
            instance.RemoteConfig(contextKey, OnRemoteConfigMethodName);
        }

        public void RemoteConfigList(Qonversion.OnRemoteConfigListReceived callback)
        {
            RemoteConfigListCallback = callback;
            var instance = GetNativeWrapper();
            instance.RemoteConfigList(OnRemoteConfigListMethodName);
        }

        public void RemoteConfigList(string[] contextKeys, bool includeEmptyContextKey, Qonversion.OnRemoteConfigListReceived callback)
        {
            RemoteConfigListForContextKeysCallback = callback;
            var contextKeysJson = contextKeys.toJson();
            
            var instance = GetNativeWrapper();
            instance.RemoteConfigList(contextKeysJson, includeEmptyContextKey, OnRemoteConfigListForContextKeysMethodName);
        }

        public void AttachUserToExperiment(string experimentId, string groupId, Qonversion.OnAttachUserResponseReceived callback)
        {
            AttachUserCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.AttachUserToExperiment(experimentId, groupId, OnAttachUserMethodName);
        }

        public void DetachUserFromExperiment(string experimentId, Qonversion.OnAttachUserResponseReceived callback)
        {
            DetachUserCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.DetachUserFromExperiment(experimentId, OnDetachUserMethodName);
        }

        public void AttachUserToRemoteConfiguration(string remoteConfigurationId, Qonversion.OnAttachUserResponseReceived callback)
        {
            AttachUserCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.AttachUserToRemoteConfiguration(remoteConfigurationId, OnAttachUserMethodName);
        }

        public void DetachUserFromRemoteConfiguration(string remoteConfigurationId, Qonversion.OnAttachUserResponseReceived callback)
        {
            DetachUserCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.DetachUserFromRemoteConfiguration(remoteConfigurationId, OnDetachUserMethodName);
        }

        public void CheckTrialIntroEligibility(IList<string> productIds, Qonversion.OnEligibilitiesReceived callback)
        {
            var productIdsJson = Json.Serialize(productIds);

            EligibilitiesCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.CheckTrialIntroEligibility(productIdsJson, OnEligibilitiesMethodName);
        }

        public void CheckEntitlements(Qonversion.OnEntitlementsReceived callback)
        {
            CheckEntitlementsCallbacks.Add(callback);
            IQonversionWrapper instance = GetNativeWrapper();
            instance.CheckEntitlements(OnCheckEntitlementsMethodName);
        }

        public void Restore(Qonversion.OnEntitlementsReceived callback)
        {
            RestoreCallbacks.Add(callback);
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Restore(OnRestoreMethodName);
        }

        public void SyncPurchases()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SyncPurchases();
        }

        public void Identify(string userID)
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Identify(userID, OnIdentityMethodName);
        }

        public void Identify(string userID, Qonversion.OnUserInfoReceived callback)
        {
            IdentityCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Identify(userID, OnIdentityMethodName);
        }

        public void Logout()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Logout();
        }

        public void UserInfo(Qonversion.OnUserInfoReceived callback)
        {
            UserInfoCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.UserInfo(OnUserInfoMethodName);
        }

        public void Attribution(Dictionary<string, object> conversionData, AttributionProvider attributionProvider)
        {
            Attribution(conversionData.toJson(), attributionProvider);
        }

        public void Attribution(string conversionData, AttributionProvider attributionProvider)
        {
            string providerName = Enum.GetName(typeof(AttributionProvider), attributionProvider);

            IQonversionWrapper instance = GetNativeWrapper();

            instance.AddAttributionData(conversionData, providerName);
        }

        public void SetUserProperty(UserPropertyKey key, string value)
        {
            if (key == UserPropertyKey.Custom)
            {
                Debug.LogWarning("Can not set user property with the key `UserPropertyKey.Custom`. " +
                                 "To set custom user property, use the `SetCustomUserProperty` method.");
                return;
            }

            IQonversionWrapper instance = GetNativeWrapper();
            instance.SetUserProperty(key, value);
        }

        public void SetCustomUserProperty(string key, string value)
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SetCustomUserProperty(key, value);
        }

        public void UserProperties(Qonversion.OnUserPropertiesReceived callback)
        {
            UserPropertiesCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.UserProperties(OnUserPropertiesMethodName);
        }

        public void CollectAdvertisingId()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SetAdvertisingID();
        }

        public void CollectAppleSearchAdsAttribution()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SetAppleSearchAdsAttributionEnabled(true);
        }

        public void PresentCodeRedemptionSheet()
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.PresentCodeRedemptionSheet();
        }

        // Called from the native SDK - Called when entitlements received from the checkEntitlements() method 
        private void OnCheckEntitlements(string jsonString)
        {
            HandleEntitlements(CheckEntitlementsCallbacks, jsonString);
            CheckEntitlementsCallbacks.Clear();
        }

        // Called from the native SDK - Called when purchase result received from the purchase() method
        private void OnPurchase(string jsonString)
        {
            HandlePurchaseResult(PurchaseCallback, jsonString);
            PurchaseCallback = null;
        }

        // Called from the native SDK - Called when entitlements received from the restore() method 
        private void OnRestore(string jsonString)
        {
            HandleEntitlements(RestoreCallbacks, jsonString);
            RestoreCallbacks.Clear();
        }

        // Called from the native SDK - Called when purchase result received from the updatePurchase() method 
        private void OnUpdatePurchase(string jsonString)
        {
            HandlePurchaseResult(UpdatePurchaseCallback, jsonString);
            UpdatePurchaseCallback = null;
        }

        // Called from the native SDK - Called when entitlements received from the promoPurchase() method 
        private void OnPromoPurchase(string jsonString)
        {
            if (PromoPurchaseCallback != null) {
                var callbacks = new List<Qonversion.OnEntitlementsReceived> { PromoPurchaseCallback };
                HandleEntitlements(callbacks, jsonString);
            }

            PromoPurchaseCallback = null;
            _storedPromoProductId = null;
        }

        // Called from the native SDK - Called when products received from the products() method 
        private void OnProducts(string jsonString)
        {
            if (ProductsCallbacks.Count == 0) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                ProductsCallbacks.ForEach(callback => callback(null, error));
            }
            else
            {
                var products = Mapper.ProductsFromJson(jsonString);
                ProductsCallbacks.ForEach(callback => callback(products, null));
            }

            ProductsCallbacks.Clear();
        }

        // Called from the native SDK - Called when offerings received from the offerings() method 
        private void OnOfferings(string jsonString)
        {
            if (OfferingsCallbacks.Count == 0) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                OfferingsCallbacks.ForEach(callback => callback(null, error));
            }
            else
            {
                var offerings = Mapper.OfferingsFromJson(jsonString);
                OfferingsCallbacks.ForEach(callback => callback(offerings, null));
            }

            OfferingsCallbacks.Clear();
        }

        // Called from the native SDK - Called when remoteConfig received from the remoteConfig() method 
        private void OnRemoteConfig(string jsonString)
        {
            if (RemoteConfigCallbacks.Count == 0) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                if (
                    Json.Deserialize(jsonString) is not Dictionary<string, object> dict ||
                    !dict.TryGetValue("contextKey", out var contextKey)
                ) {
                    foreach (var (_, callbacks) in RemoteConfigCallbacks)
                    {
                        callbacks.ForEach(callback => callback(null, error));
                    }

                    RemoteConfigCallbacks.Clear();
                    return;
                }

                var key = contextKey == null ? DefaultRemoteConfigContextKey : contextKey as string;

                if (key != null && RemoteConfigCallbacks.ContainsKey(key))
                {
                    RemoteConfigCallbacks[key].ForEach(callback => callback(null, error));
                    RemoteConfigCallbacks[key].Clear();
                }
            }
            else
            {
                var remoteConfig = Mapper.RemoteConfigFromJson(jsonString);

                var key = remoteConfig.Source?.ContextKey ?? DefaultRemoteConfigContextKey;
                if (RemoteConfigCallbacks.ContainsKey(key))
                {
                    RemoteConfigCallbacks[key].ForEach(callback => callback(remoteConfig, null));
                    RemoteConfigCallbacks[key].Clear();
                }
            }
        }

        // Called from the native SDK - Called when remoteConfigList received from the remoteConfigList() method without context keys 
        private void OnRemoteConfigList(string jsonString)
        {
            OnRemoteConfigListResult(jsonString, RemoteConfigListCallback);
        }

        // Called from the native SDK - Called when remoteConfigList received from the remoteConfigList() method with context keys 
        private void OnRemoteConfigListForContextKeys(string jsonString)
        {
            OnRemoteConfigListResult(jsonString, RemoteConfigListForContextKeysCallback);
        }

        private void OnRemoteConfigListResult(string jsonString, Qonversion.OnRemoteConfigListReceived callback)
        {
            if (callback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                callback(null, error);
            }
            else
            {
                var remoteConfigList = Mapper.RemoteConfigListFromJson(jsonString);
                callback(remoteConfigList, null);
            }
        }

        private void OnAttachUser(string jsonString)
        {
            if (AttachUserCallback == null) return;
            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                AttachUserCallback(false, error);
            }
            else
            {
                AttachUserCallback(true, null);
            }
        }

        private void OnDetachUser(string jsonString)
        {
            if (DetachUserCallback == null) return;
            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                DetachUserCallback(false, error);
            }
            else
            {
                DetachUserCallback(true, null);
            }
        }

        // Called from the native SDK - Called when eligibilities received from the checkTrialIntroEligibilityForProductIds() method 
        private void OnEligibilities(string jsonString)
        {
            if (EligibilitiesCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                EligibilitiesCallback(null, error);
            }
            else
            {
                Dictionary<string, Eligibility> eligibilities = Mapper.EligibilitiesFromJson(jsonString);
                EligibilitiesCallback(eligibilities, null);
            }

            EligibilitiesCallback = null;
        }

        // Called from the native SDK - Called when user info received from the Identify() method 
        private void OnIdentity(string jsonString)
        {
            if (IdentityCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                IdentityCallback(null, error);
            }
            else
            {
                User userInfo = Mapper.UserFromJson(jsonString);
                IdentityCallback(userInfo, null);
            }

            IdentityCallback = null;
        }

        // Called from the native SDK - Called when user info received from the UserInfo() method 
        private void OnUserInfo(string jsonString)
        {
            if (UserInfoCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                UserInfoCallback(null, error);
            }
            else
            {
                User userInfo = Mapper.UserFromJson(jsonString);
                UserInfoCallback(userInfo, null);
            }

            UserInfoCallback = null;
        }

        // Called from the native SDK - Called when user properties received from the UserProperties() method 
        private void OnUserProperties(string jsonString)
        {
            if (UserPropertiesCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                UserPropertiesCallback(null, error);
            }
            else
            {
                UserProperties userProperties = Mapper.UserPropertiesFromJson(jsonString);
                UserPropertiesCallback(userProperties, null);
            }

            UserPropertiesCallback = null;
        }

        // Called from the native SDK - Called when entitlements update. For example, when pending purchases like SCA, Ask to buy, etc., happen.
        private void OnReceivedUpdatedEntitlements(string jsonString)
        {
            if (_onUpdatedEntitlementsReceived == null)
            {
                return;
            }

            Dictionary<string, Entitlement> entitlements = Mapper.EntitlementsFromJson(jsonString);
            _onUpdatedEntitlementsReceived(entitlements);
        }

        private void OnReceivePromoPurchase(string storeProductId)
        {
			if (_onPromoPurchasesReceived == null)
            {
                return;
            }

            _storedPromoProductId = storeProductId;
            _onPromoPurchasesReceived(storeProductId, PromoPurchase);
        }

        private void PromoPurchase(Qonversion.OnEntitlementsReceived callback)
        {
            PromoPurchaseCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.PromoPurchase(_storedPromoProductId, OnPromoPurchaseMethodName);
        }

        private void HandleEntitlements(List<Qonversion.OnEntitlementsReceived> callbacks, string jsonString)
        {
            if (callbacks.Count == 0) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                callbacks.ForEach(callback => callback(null, error));
            }
            else
            {
                var entitlements = Mapper.EntitlementsFromJson(jsonString);
                callbacks.ForEach(callback => callback(entitlements, null));
            }
        }

        private void HandlePurchaseResult(Qonversion.OnPurchaseResultReceived callback, string jsonString)
        {
            if (callback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                var isCancelled = Mapper.GetIsCancelledFromJson(jsonString);
                callback(null, error, isCancelled);
            }
            else
            {
                var entitlements = Mapper.EntitlementsFromJson(jsonString);
                callback(entitlements, null, false);
            }
        }

        private IQonversionWrapper GetNativeWrapper()
        {
            if (_nativeWrapperInstance != null)
            {
                return _nativeWrapperInstance;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _nativeWrapperInstance = new QonversionWrapperAndroid();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _nativeWrapperInstance = new QonversionWrapperIOS();
                    break;
                default:
                    _nativeWrapperInstance = new QonversionWrapperNoop();
                    break;
            }
            _nativeWrapperInstance.Initialize(GameObjectName);

            return _nativeWrapperInstance;
        }
    }
}
