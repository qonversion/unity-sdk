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

        private const string GameObjectName = "QonvesrionRuntimeGameObject";
        private const string OnCheckPermissionsMethodName = "OnCheckPermissions";
        private const string OnPurchaseMethodName = "OnPurchase";
        private const string OnUpdatePurchaseMethodName = "OnUpdatePurchase";
        private const string OnRestoreMethodName = "OnRestore";
        private const string OnProductsMethodName = "OnProducts";
        private const string OnOfferingsMethodName = "OnOfferings";
        private const string OnEligibilitiesMethodName = "OnEligibilities";

        private const string SdkVersion = "2.2.2";
        private const string SdkSource = "unity";

        private static IQonversionWrapper _Instance;

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
        /// iOS only. Returns `null` if called on Android.
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

        // Enable attribution collection from Apple Search Ads. NO by default.
        public static void SetAppleSearchAdsAttributionEnabled(bool enable)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetAppleSearchAdsAttributionEnabled(enable);
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

        // Called from the native SDK - Called when permissions received from the checkPermissions() method 
        private void OnCheckPermissions(string jsonString)
        {
            Debug.Log("OnCheckPermissions " + jsonString);

            if (CheckPermissionsCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                CheckPermissionsCallback(null, error);
            }
            else
            {
                Dictionary<string, Permission> permissions = Mapper.PermissionsFromJson(jsonString);
                CheckPermissionsCallback(permissions, null);
            }

            CheckPermissionsCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the purchase() method 
        private void OnPurchase(string jsonString)
        {
            Debug.Log("OnPurchase callback " + jsonString);

            if (PurchaseCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                PurchaseCallback(null, error);
            }
            else
            {
                Dictionary<string, Permission> permissions = Mapper.PermissionsFromJson(jsonString);
                PurchaseCallback(permissions, null);
            }

            PurchaseCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the restore() method 
        private void OnRestore(string jsonString)
        {
            Debug.Log("OnRestore " + jsonString);

            if (RestoreCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                RestoreCallback(null, error);
            }
            else
            {
                Dictionary<string, Permission> permissions = Mapper.PermissionsFromJson(jsonString);
                RestoreCallback(permissions, null);
            }

            RestoreCallback = null;
        }

        // Called from the native SDK - Called when permissions received from the updatePurchase() method 
        private void OnUpdatePurchase(string jsonString)
        {
            Debug.Log("OnUpdatePurchase " + jsonString);

            if (UpdatePurchaseCallback == null) return;

            var error = Mapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                UpdatePurchaseCallback(null, error);
            }
            else
            {
                Dictionary<string, Permission> permissions = Mapper.PermissionsFromJson(jsonString);
                UpdatePurchaseCallback(permissions, null);
            }

            UpdatePurchaseCallback = null;
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
    }
}