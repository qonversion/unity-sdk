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
        private const string OnPurchaseProductMethodName = "OnPurchaseProduct";
        private const string OnUpdatePurchaseMethodName = "OnUpdatePurchase";
        private const string OnUpdatePurchaseWithProductMethodName = "OnUpdatePurchaseWithProduct";
        private const string OnRestoreMethodName = "OnRestore";
        private const string OnProductsMethodName = "OnProducts";
        private const string OnOfferingsMethodName = "OnOfferings";
        private const string OnEligibilitiesMethodName = "OnEligibilities";
        private const string OnUserInfoMethodName = "OnUserInfo";

        private const string SdkVersion = "4.4.1";
        private const string SdkSource = "unity";

        private IQonversionWrapper _nativeWrapperInstance;
        private Qonversion.OnUpdatedEntitlementsReceived _onUpdatedEntitlementsReceived;

        private Qonversion.OnPromoPurchasesReceived _onPromoPurchasesReceived;
        private string _storedPromoProductId = null;

        private List<Qonversion.OnEntitlementsReceived> CheckEntitlementsCallbacks { get; } = new List<Qonversion.OnEntitlementsReceived>();
        private List<Qonversion.OnEntitlementsReceived> RestoreCallbacks { get; } = new List<Qonversion.OnEntitlementsReceived>();
        private Qonversion.OnPurchaseResultReceived PurchaseCallback { get; set; }
        private Qonversion.OnPurchaseResultReceived PurchaseProductCallback { get; set; }
        private Qonversion.OnPurchaseResultReceived UpdatePurchaseCallback { get; set; }
        private Qonversion.OnPurchaseResultReceived UpdatePurchaseWithProductCallback { get; set; }
        private List<Qonversion.OnProductsReceived> ProductsCallbacks { get; } = new List<Qonversion.OnProductsReceived>();
        private List<Qonversion.OnOfferingsReceived> OfferingsCallbacks { get; } = new List<Qonversion.OnOfferingsReceived>();
        private Qonversion.OnEligibilitiesReceived EligibilitiesCallback { get; set; }
        private Qonversion.OnEntitlementsReceived PromoPurchaseCallback { get; set; }
        private Qonversion.OnUserInfoReceived UserInfoCallback { get; set; }

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

        public void Purchase(string productId, Qonversion.OnPurchaseResultReceived callback)
        {
            PurchaseCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.Purchase(productId, OnPurchaseMethodName);
        }

        public void PurchaseProduct([NotNull] Product product, Qonversion.OnPurchaseResultReceived callback)
        {
            if (product == null)
            {
                callback(null, new QonversionError("PurchaseInvalid", "Product is null"), false);
                return;
            }

            PurchaseProductCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.PurchaseProduct(product.QonversionId, product.OfferingId, OnPurchaseProductMethodName);
        }

        public void UpdatePurchase(string productId, string oldProductId, Qonversion.OnPurchaseResultReceived callback, ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy)
        {
            UpdatePurchaseCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.UpdatePurchase(productId, oldProductId, prorationMode, OnUpdatePurchaseMethodName);
        }

        public void UpdatePurchaseWithProduct([NotNull] Product product, string oldProductId, Qonversion.OnPurchaseResultReceived callback, ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy)
        {
            if (product == null)
            {
                callback(null, new QonversionError("PurchaseInvalid", "Product is null"), false);
                return;
            }

            UpdatePurchaseWithProductCallback = callback;
            IQonversionWrapper instance = GetNativeWrapper();
            instance.UpdatePurchaseWithProduct(product.QonversionId, product.OfferingId, oldProductId, prorationMode, OnUpdatePurchaseWithProductMethodName);
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
            instance.Identify(userID);
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
  
        public void SetProperty(UserProperty key, string value)
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SetProperty(key, value);
        }

        public void SetUserProperty(string key, string value)
        {
            IQonversionWrapper instance = GetNativeWrapper();
            instance.SetUserProperty(key, value);
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

        // Called from the native SDK - Called when purchase result received from the purchaseProduct() method 
        private void OnPurchaseProduct(string jsonString)
        {
            HandlePurchaseResult(PurchaseProductCallback, jsonString);
            PurchaseProductCallback = null;
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
        
        // Called from the native SDK - Called when purchase result received from the updatePurchaseWithProduct() method 
        private void OnUpdatePurchaseWithProduct(string jsonString)
        {
            HandlePurchaseResult(UpdatePurchaseWithProductCallback, jsonString);
            UpdatePurchaseWithProductCallback = null;
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
