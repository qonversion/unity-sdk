using QonversionUnity.MiniJSON;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public class Qonversion : MonoBehaviour
    {
        public delegate void OnPermissionsReceived(Dictionary<string, Permission> permissions, QonversionError error);
        public delegate void OnProductsReceived(Dictionary<string, Product> products, QonversionError error);
        public delegate void OnOfferingsReceived(Offerings offerings, QonversionError error);

        private const string GameObjectName = "QonvesrionRuntimeGameObject";
        private const string OnCheckPermissionsMethodName = "OnCheckPermissions";
        private const string OnPurchaseMethodName = "OnPurchase";
        private const string OnUpdatePurchaseMethodName = "OnUpdatePurchase";
        private const string OnRestoreMethodName = "OnRestore";
        private const string OnProductsMethodName = "OnProducts";
        private const string OnOfferingsMethodName = "OnOfferings";

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

        public static void Launch(string apiKey, bool observerMode)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.Launch(GameObjectName, apiKey, observerMode);
        }

        public static void SetDebugMode()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetDebugMode();
        }

        public static void SetUserID(string userID)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SetUserID(userID);
        }

        public static void SyncPurchases()
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.SyncPurchases();
        }

        public static void AddAttributionData(Dictionary<string, object> conversionData, AttributionSource attributionSource)
        {
            AddAttributionData(conversionData.toJson(), attributionSource);
        }

        public static void AddAttributionData(string conversionData, AttributionSource attributionSource)
        {
            IQonversionWrapper instance = getFinalInstance();

            instance.AddAttributionData(conversionData, attributionSource);
        }

        private static OnPermissionsReceived CheckPermissionsCallback { get; set; }

        public static void CheckPermissions(OnPermissionsReceived callback)
        {
            CheckPermissionsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.CheckPermissions(OnCheckPermissionsMethodName);
        }

        private static OnPermissionsReceived PurchaseCallback { get; set; }

        public static void Purchase(string productId, OnPermissionsReceived callback)
        {
            PurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Purchase(productId, OnPurchaseMethodName);
        }

        private static OnPermissionsReceived RestoreCallback { get; set; }

        public static void Restore(OnPermissionsReceived callback)
        {
            RestoreCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Restore(OnRestoreMethodName);
        }

        private static OnPermissionsReceived UpdatePurchaseCallback { get; set; }

        public static void UpdatePurchase(string productId, string oldProductId, OnPermissionsReceived callback, ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy)
        {
            UpdatePurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.UpdatePurchase(productId, oldProductId, prorationMode, OnUpdatePurchaseMethodName);
        }

        private static OnProductsReceived ProductsCallback { get; set; }

        public static void Products(OnProductsReceived callback)
        {
            ProductsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Products(OnProductsMethodName);
        }

        private static OnOfferingsReceived OfferingsCallback { get; set; }

        public static void Offerings(OnOfferingsReceived callback)
        {
            OfferingsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Offerings(OnOfferingsMethodName);
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
    }
}