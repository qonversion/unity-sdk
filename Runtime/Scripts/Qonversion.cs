using JetBrains.Annotations;
using QonversionUnity.MiniJSON;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public class Qonversion : MonoBehaviour
    {
        public delegate void OnPermissionsReceived(Dictionary<string, Permission> permissions, QonversionError error);
        public delegate void OnProductsReceived(Dictionary<string, Product> products, QonversionError error);
        public delegate void OnOfferingsReceived(Offerings offerings, QonversionError error);
        public delegate void OnEligibilitiesReceived(Dictionary<string, Eligibility> eligibilities, QonversionError error);

        /// <summary>
        /// Delegate fires each time a promo purchase from the App Store happens.
        /// Be sure you define a delegate for the event <see cref="PromoPurchasesReceived"/>. 
        /// Call StartPromoPurchase in case of your app is ready to start promo purchase. 
        /// Or cache that delegate and call later when you need.
        /// </summary>
        /// <param name="productId">StoreKit product identifier</param>
        /// <param name="purchaseDelegate">A delegate that will start a promo purchase flow.
        /// <see cref="StartPromoPurchase"/>
        /// </param>
        public delegate void OnPromoPurchasesReceived(string productId, StartPromoPurchase purchaseDelegate);

        /// <summary>
        /// Call the function if your app can handle a promo purchase at the current time.
        /// Or you can cache the delegate, and call it when the app is ready to make the purchase.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received. Returns permissions or potentially a QonversionError.
        /// <see cref="OnPermissionsReceived"/>
        /// </param>
        public delegate void StartPromoPurchase(OnPermissionsReceived callback);

        /// <summary>
        /// Delegate fires each time a deferred transaction happens
        /// </summary>
        public delegate void OnUpdatedPurchasesReceived(Dictionary<string, Permission> permissions);
     
        private const string GameObjectName = "QonvesrionRuntimeGameObject";
        private const string OnCheckPermissionsMethodName = "OnCheckPermissions";
        private const string OnPurchaseMethodName = "OnPurchase";
        private const string OnPromoPurchaseMethodName = "OnPromoPurchase";
        private const string OnPurchaseProductMethodName = "OnPurchaseProduct";
        private const string OnUpdatePurchaseMethodName = "OnUpdatePurchase";
        private const string OnUpdatePurchaseWithProductMethodName = "OnUpdatePurchaseWithProduct";
        private const string OnRestoreMethodName = "OnRestore";
        private const string OnProductsMethodName = "OnProducts";
        private const string OnOfferingsMethodName = "OnOfferings";
        private const string OnEligibilitiesMethodName = "OnEligibilities";

        private const string SdkVersion = "3.3.1";
        private const string SdkSource = "unity";

        private static IQonversionWrapper _Instance;
        private static OnUpdatedPurchasesReceived _onUpdatedPurchasesReceived;

        private static OnPromoPurchasesReceived _onPromoPurchasesReceived;
        private static string _storedPromoProductId = null;
        private static AutomationsDelegate _automationsDelegate;

        private static IQonversionWrapper getFinalInstance()
        {
            if (_Instance == null)
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.Android:
                        _Instance = new QonversionWrapperAndroid();
                        break;
                    case RuntimePlatform.IPhonePlayer:
                        _Instance = new QonversionWrapperIOS();
                        break;
                    default:
                        _Instance = new QonversionWrapperNoop();
                        break;
                }
            }

            GameObject go = new GameObject(GameObjectName);
            go.AddComponent<Qonversion>();
            DontDestroyOnLoad(go);

            return _Instance;
        }

         /// <summary>
         /// This event will be fired when a user initiates a promotional in-app purchase from the App Store.
         /// Declare a delegate <see cref="OnPromoPurchasesReceived"/> for the event.
         /// If you are not using the PromoPurchasesReceived event promo purchases will proceed automatically.
         /// </summary>
         public static event OnPromoPurchasesReceived PromoPurchasesReceived
         {
             add
             {
                 _onPromoPurchasesReceived += value;

                 if (_onPromoPurchasesReceived.GetInvocationList().Length == 1)
                 {
                     IQonversionWrapper instance = getFinalInstance();
                     instance.AddPromoPurchasesDelegate();
                 }
             }
             remove
             {
                 _onPromoPurchasesReceived -= value;

                 if (_onPromoPurchasesReceived == null)
                 {
                     IQonversionWrapper instance = getFinalInstance();
                     instance.RemovePromoPurchasesDelegate();
                 }
             }
         }
         
         /// <summary>
         /// This event will be fired each time a deferred transaction happens.
         /// </summary>
         public static event OnUpdatedPurchasesReceived UpdatedPurchasesReceived
         {
             add
             {
                 _onUpdatedPurchasesReceived += value;

                 if (_onUpdatedPurchasesReceived.GetInvocationList().Length == 1)
                 {
                     IQonversionWrapper instance = getFinalInstance();
                     instance.AddUpdatedPurchasesDelegate();
                 }
             }
             remove
             {
                 _onUpdatedPurchasesReceived -= value;

                 if (_onUpdatedPurchasesReceived == null)
                 {
                     IQonversionWrapper instance = getFinalInstance();
                     instance.RemoveUpdatedPurchasesDelegate();
                 }
             }
         }

        internal static void SetAutomationsDelegate(AutomationsDelegate automationsDelegate)
        {
            _automationsDelegate = automationsDelegate;

            IQonversionWrapper instance = getFinalInstance();
            instance.AddAutomationsDelegate();
        }
    
        /// <summary>
        /// Initializes Qonversion SDK with the given API key.
        /// You can get one in your account on https://dash.qonversion.io.
        /// </summary>
        /// <param name="apiKey">Project key to setup the SDK.</param>
        /// <param name="observerMode">Set true if you are using observer mode only.</param>
        /// <see href="https://documentation.qonversion.io/docs/how-qonversion-works">Observer mode</see>
        /// <see href="https://qonversion.io/docs/google">Installing the Android SDK</see>
        public static void Launch(string apiKey, bool observerMode)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.StoreSdkInfo(SdkVersion, Constants.VersionKey, SdkSource, Constants.SourceKey);
            instance.Launch(GameObjectName, apiKey, observerMode);
        }

        /// <summary>
        /// You can set the flag to distinguish sandbox and production users.
        /// To see the sandbox users turn on the Viewing test Data toggle on Qonversion Dashboard
        /// </summary>
        public static void SetDebugMode()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetDebugMode();
        }

        /// <summary>
        /// iOS only.
        /// On iOS 14.5+, after requesting the app tracking permission using ATT, you need to notify Qonversion if tracking
        /// is allowed and IDFA is available.
        /// </summary>
        public static void SetAdvertisingID()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetAdvertisingID();
        }

        /// <summary>
        /// Qonversion SDK provides an asynchronous method to set your side User ID that can be used to match users in
        /// third-party integrations.
        /// </summary>
        /// <param name="userID">Your database user ID.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-identifiers">User Identifiers</see>
        [Obsolete("Deprecated. Will be removed in a future major release. Use SetProperty(UserProperty.CustomUserId, value) instead.")]
        public static void SetUserID(string userID)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetProperty(UserProperty.CustomUserId, userID);
        }

        /// <summary>
        /// Adds custom user property.
        ///
        /// User properties are attributes you can set on a user level.
        /// You can send user properties to third party platforms as well as use them in Qonversion for customer segmentation
        /// and analytics.
        /// </summary>
        /// <param name="key">Custom user property key.</param>
        /// <param name="value">Property value.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-properties">User Properties</see>
        public static void SetUserProperty(string key, string value)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetUserProperty(key, value);
        }
  
        /// <summary>
        /// Sets user property for pre-defined case property.
        ///
        /// User properties are attributes you can set on a user level.
        /// You can send user properties to third party platforms as well as use them in Qonversion for customer segmentation
        /// and analytics.
        /// </summary>
        /// <param name="key">Defined enum key that will be transformed to string.</param>
        /// <param name="value">Property value.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-properties">User Properties</see>
        public static void SetProperty(UserProperty key, string value)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetProperty(key, value);
        }

        /// <summary>
        /// This method will send all purchases to the Qonversion backend. Call this every time when purchase is handled
        /// by your own implementation.
        ///
        /// //////Warning!//////
        ///
        /// This method works for Android only.
        /// It should only be called if you're using Qonversion SDK in observer mode.
        /// </summary>
        /// <see href="https://documentation.qonversion.io/docs/observer-mode#android-sdk">Observer mode for Android SDK</see>
        public static void SyncPurchases()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SyncPurchases();
        }

        /// <summary>
        /// iOS only.
        /// On iOS 14.0+ shows up a sheet for users to redeem AppStore offer codes.
        /// </summary>
        public static void PresentCodeRedemptionSheet()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.PresentCodeRedemptionSheet();
        }

        /// <summary>
        /// Sends your attribution data to the attribution source.
        /// </summary>
        /// <param name="conversionData">An object containing your attribution data.</param>
        /// <param name="attributionSource">The attribution source to which the data will be sent.</param>
        public static void AddAttributionData(Dictionary<string, object> conversionData, AttributionSource attributionSource)
        {
            AddAttributionData(conversionData.toJson(), attributionSource);
        }

        /// <summary>
        /// Sends your attribution data to the attribution source.
        /// </summary>
        /// <param name="conversionData">A json string containing your attribution data.</param>
        /// <param name="attributionSource">The attribution source to which the data will be sent.</param>
        public static void AddAttributionData(string conversionData, AttributionSource attributionSource)
        {
            IQonversionWrapper instance = getFinalInstance();

            instance.AddAttributionData(conversionData, attributionSource);
        }

        /// <summary>
        /// Enable collecting Apple Search Ads attribution data. "false" by default.
        /// </summary>
        /// <param name="enable">A bool value indicating whether Qonversion should collect attribution from Apple Search Ads.</param>
        /// <see href="https://documentation.qonversion.io/docs/apple-search-ads#enable-data-collection"/>
        public static void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetAppleSearchAdsAttributionEnabled(enable);
        }

        /// <summary>
        /// Call this function to link a user to his unique ID in your system and share purchase data.
        /// </summary>
        /// <param name="userID">An unique user ID in your system.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-identifiers#3-user-identity"/>
        public static void Identify(string userID)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.Identify(userID);
        }

        /// <summary>
        /// Call this function to unlink a user from his unique ID in your system and his purchase data.
        /// </summary>
        /// <see href="https://documentation.qonversion.io/docs/user-identifiers#logging-out"/>
        public static void Logout()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.Logout();
        }

        private static OnPermissionsReceived CheckPermissionsCallback { get; set; }

        /// <summary>
        /// You need to call the CheckPermissions method at the start of your app to check if a user has the required
        /// permission.
        ///
        /// This method will check the user receipt and will return the current permissions.
        ///
        /// If Apple or Google servers are not responding at the time of the request, Qonversion provides the latest
        /// permissions data from its database.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received</param>
        public static void CheckPermissions(OnPermissionsReceived callback)
        {
            CheckPermissionsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.CheckPermissions(OnCheckPermissionsMethodName);
        }

        private static OnPermissionsReceived PurchaseCallback { get; set; }

        /// <summary>
        /// Make a purchase and validate that through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="productId">Qonversion product identifier for purchase.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#1-make-a-purchase"/>
        public static void Purchase(string productId, OnPermissionsReceived callback)
        {
            PurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Purchase(productId, OnPurchaseMethodName);
        }

        private static OnPermissionsReceived PurchaseProductCallback { get; set; }

        /// <summary>
        /// Make a purchase and validate that through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="product">Qonversion product for purchase.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#1-make-a-purchase"/>
        public static void PurchaseProduct([NotNull] Product product, OnPermissionsReceived callback)
        {
            if(product == null)
            {
                callback(null, new QonversionError("PurchaseInvalid", "Product is null"));
                return;
            }

            var productJson = product.OriginalJson;
          
            PurchaseProductCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.PurchaseProduct(productJson, OnPurchaseProductMethodName);
        }

        private static OnPermissionsReceived RestoreCallback { get; set; }

        /// <summary>
        /// Restoring purchases restores users purchases in your app, to maintain access to purchased content.
        /// Users sometimes need to restore purchased content, such as when they upgrade to a new phone.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received</param>
        public static void Restore(OnPermissionsReceived callback)
        {
            RestoreCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Restore(OnRestoreMethodName);
        }

        private static OnPermissionsReceived UpdatePurchaseCallback { get; set; }

        /// <summary>
        /// Update (upgrade/downgrade) subscription and validate that through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="productId">Qonversion product identifier for purchase</param>
        /// <param name="oldProductId">Qonversion product identifier from which the upgrade/downgrade will be initialized</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        /// <param name="prorationMode">Proration Mode</param>
        /// <see href="https://developer.android.com/google/play/billing/subscriptions#proration">Proration Mode</see>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#3-update-purchases-android-only">Update Purchase</see>
        public static void UpdatePurchase(string productId, string oldProductId, OnPermissionsReceived callback, ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy)
        {
            UpdatePurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.UpdatePurchase(productId, oldProductId, prorationMode, OnUpdatePurchaseMethodName);
        }

        private static OnPermissionsReceived UpdatePurchaseWithProductCallback { get; set; }

        /// <summary>
        /// Update (upgrade/downgrade) subscription and validate that through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="product">Qonversion product for purchase</param>
        /// <param name="oldProductId">Qonversion product identifier from which the upgrade/downgrade will be initialized</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        /// <param name="prorationMode">Proration Mode</param>
        /// <see href="https://developer.android.com/google/play/billing/subscriptions#proration">Proration Mode</see>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#3-update-purchases-android-only">Update Purchase</see>
        public static void UpdatePurchaseWithProduct([NotNull] Product product, string oldProductId, OnPermissionsReceived callback, ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy)
        {
            if (product == null)
            {
                callback(null, new QonversionError("PurchaseInvalid", "Product is null"));
                return;
            }

            var productJson = product.OriginalJson;

            UpdatePurchaseWithProductCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.UpdatePurchaseWithProduct(productJson, oldProductId, prorationMode, OnUpdatePurchaseWithProductMethodName);
        }

        private static OnProductsReceived ProductsCallback { get; set; }
  
        /// <summary>
        /// Returns Qonversion products in association with Apple and Google Play Store Products.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://qonversion.io/docs/product-center">Product Center</see>
        public static void Products(OnProductsReceived callback)
        {
            ProductsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Products(OnProductsMethodName);
        }

        private static OnOfferingsReceived OfferingsCallback { get; set; }

        /// <summary>
        /// Return Qonversion Offerings Object.
        ///
        /// An offering is a group of products that you can offer to a user on a given paywall based on your business logic.
        /// For example, you can offer one set of products on a paywall immediately after onboarding and another
        /// set of products with discounts later on if a user has not converted.
        /// Offerings allow changing the products offered remotely without releasing app updates.
        /// </summary>
        /// <see href="https://qonversion.io/docs/offerings">Offerings</see>
        /// <see href="https://qonversion.io/docs/product-center">Product Center</see>
        public static void Offerings(OnOfferingsReceived callback)
        {
            OfferingsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Offerings(OnOfferingsMethodName);
        }

        private static OnEligibilitiesReceived EligibilitiesCallback { get; set; }

        /// <summary>
        /// You can check if a user is eligible for an introductory offer, including a free trial.
        /// You can show only a regular price for users who are not eligible for an introductory offer.
        /// </summary>
        /// <param name="productIds">Products identifiers that must be checked.</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        public static void CheckTrialIntroEligibilityForProductIds(IList<string> productIds, OnEligibilitiesReceived callback)
        {
            var productIdsJson = Json.Serialize(productIds);

            EligibilitiesCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.CheckTrialIntroEligibilityForProductIds(productIdsJson, OnEligibilitiesMethodName);
        }

        /// <summary>
        /// Set push token to Qonversion to enable Qonversion push notifications
        /// </summary>
        /// <param name="token">Firebase device token on Android. APNs device token on iOS.</param>
        public static void SetNotificationsToken(string token)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetNotificationsToken(token);
        }

        public static bool HandleNotification(Dictionary<string, object> notification)
        {
            IQonversionWrapper instance = getFinalInstance();
            return instance.HandleNotification(notification.toJson());
        }

        // Called from the native SDK - Called when permissions received from the checkPermissions() method 
        private void OnCheckPermissions(string jsonString)
        {
            Debug.Log("OnCheckPermissions " + jsonString);
            HandlePermissions(CheckPermissionsCallback, jsonString);
            CheckPermissionsCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the purchase() method 
        private void OnPurchase(string jsonString)
        {
            Debug.Log("OnPurchase callback " + jsonString);
            HandlePermissions(PurchaseCallback, jsonString);
            PurchaseCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the purchaseProduct() method 
        private void OnPurchaseProduct(string jsonString)
        {
            Debug.Log("OnPurchaseProduct callback " + jsonString);
            HandlePermissions(PurchaseProductCallback, jsonString);
            PurchaseProductCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the restore() method 
        private void OnRestore(string jsonString)
        {
            Debug.Log("OnRestore " + jsonString);
            HandlePermissions(RestoreCallback, jsonString);
            RestoreCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the updatePurchase() method 
        private void OnUpdatePurchase(string jsonString)
        {
            Debug.Log("OnUpdatePurchase " + jsonString);
            HandlePermissions(UpdatePurchaseCallback, jsonString);
            UpdatePurchaseCallback = null;
        }
        
        // Called from the native SDK - Called when permissions received from the updatePurchaseWithProduct() method 
        private void OnUpdatePurchaseWithProduct(string jsonString)
        {
            Debug.Log("OnUpdatePurchaseWithProduct " + jsonString);
            HandlePermissions(UpdatePurchaseWithProductCallback, jsonString);
            UpdatePurchaseWithProductCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the promoPurchase() method 
        private void OnPromoPurchase(string jsonString)
        {
            Debug.Log("OnPromoPurchase callback " + jsonString);
            HandlePermissions(PromoPurchaseCallback, jsonString);
            PromoPurchaseCallback = null;
            _storedPromoProductId = null;
        }

        // Called from the native SDK - Called when products received from the products() method 
        private void OnProducts(string jsonString)
        {
            Debug.Log("OnProducts " + jsonString);

            if (ProductsCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                ProductsCallback(null, error);
            }
            else
            {
                Dictionary<string, Product> products = Mapper.ProductsFromJson(jsonString);
                ProductsCallback(products, null);
            }

            ProductsCallback = null;
        }

        // Called from the native SDK - Called when offerings received from the offerings() method 
        private void OnOfferings(string jsonString)
        {
            Debug.Log("OnOfferings " + jsonString);

            if (OfferingsCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                OfferingsCallback(null, error);
            }
            else
            {
                Offerings offerings = Mapper.OfferingsFromJson(jsonString);
                OfferingsCallback(offerings, null);
            }

            OfferingsCallback = null;
        }

        // Called from the native SDK - Called when eligibilities received from the checkTrialIntroEligibilityForProductIds() method 
        private void OnEligibilities(string jsonString)
        {
            Debug.Log("OnEligibilities " + jsonString);

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

        // Called from the native SDK - Called when deferred or pending purchase occured
        private void OnReceiveUpdatedPurchases(string jsonString)
        {
             if (_onUpdatedPurchasesReceived == null)
             {
                 return;
             }

             Debug.Log("OnReceiveUpdatedPurchases " + jsonString);
             Dictionary<string, Permission> permissions = Mapper.PermissionsFromJson(jsonString);
             _onUpdatedPurchasesReceived(permissions);
        }

        private void OnReceivePromoPurchase(string storeProductId)
        {
             if (_onPromoPurchasesReceived == null)
             {
                 return;
             }

             Debug.Log("OnReceivePromoPurchase " + storeProductId);
             _storedPromoProductId = storeProductId;
             _onPromoPurchasesReceived(storeProductId, PromoPurchase);
        }

        private static OnPermissionsReceived PromoPurchaseCallback { get; set; }

        private void PromoPurchase(OnPermissionsReceived callback)
        {
            PromoPurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.PromoPurchase(_storedPromoProductId, OnPromoPurchaseMethodName);
        }

        private void HandlePermissions(OnPermissionsReceived callback, string jsonString)
        {
            if (callback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                callback(null, error);
            }
            else
            {
                Dictionary<string, Permission> permissions = Mapper.PermissionsFromJson(jsonString);
                callback(permissions, null);
            }
        }

        private void OnAutomationsScreenShown(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            string screenId = Mapper.ScreenIdFromJson(jsonString);

            Debug.Log(screenId);
            _automationsDelegate.OnAutomationsScreenShown(screenId);
        }

        private void OnAutomationsActionStarted(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            ActionResult actionResult = Mapper.ActionResultFromJson(jsonString);
            _automationsDelegate.OnAutomationsActionStarted(actionResult);
        }

        private void OnAutomationsActionFailed(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            ActionResult actionResult = Mapper.ActionResultFromJson(jsonString);
            _automationsDelegate.OnAutomationsActionFailed(actionResult);
        }

        
        private void OnAutomationsActionFinished(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            ActionResult actionResult = Mapper.ActionResultFromJson(jsonString);
            _automationsDelegate.OnAutomationsActionFinished(actionResult);
        }

        private void OnAutomationsFinished(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            _automationsDelegate.OnAutomationsFinished();
        }
    }
}