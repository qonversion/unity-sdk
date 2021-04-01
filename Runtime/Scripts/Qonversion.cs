using QonversionUnity.MiniJSON;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public class Qonversion : MonoBehaviour
    {
        public delegate void OnPermissionsReceived(Dictionary<string, QPermission> permissions, QError error);
        public delegate void OnProductsReceived(Dictionary<string, QProduct> products, QError error);
        public delegate void OnOfferingsReceived(QOfferings offerings, QError error);

        private const string gameObjectName = "QonvesrionRuntimeGameObject";

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

            GameObject go = new GameObject(gameObjectName);
            go.AddComponent<Qonversion>();
            DontDestroyOnLoad(go);

            return _Instance;
        }

        public static void Launch(string apiKey, bool observerMode)
        {
            IQonversionWrapper instance = getFinalInstance();
            instance.Launch(gameObjectName, apiKey, observerMode);
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
            instance.CheckPermissions();
        }

        private static OnPermissionsReceived PurchaseCallback { get; set; }

        public static void Purchase(string productId, OnPermissionsReceived callback)
        {
            PurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Purchase(productId);
        }

        private static OnPermissionsReceived RestoreCallback { get; set; }

        public static void Restore(OnPermissionsReceived callback)
        {
            RestoreCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Restore();
        }

        private static OnPermissionsReceived UpdatePurchaseCallback { get; set; }

        public static void UpdatePurchase(string productId, string oldProductId, OnPermissionsReceived callback, ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy)
        {
            UpdatePurchaseCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.UpdatePurchase(productId, oldProductId, prorationMode);
        }

        private static OnProductsReceived ProductsCallback { get; set; }

        public static void Products(OnProductsReceived callback)
        {
            ProductsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Products();
        }

        private static OnOfferingsReceived OfferingsCallback { get; set; }

        public static void Offerings(OnOfferingsReceived callback)
        {
            OfferingsCallback = callback;
            IQonversionWrapper instance = getFinalInstance();
            instance.Offerings();
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
                Dictionary<string, QPermission> permissions = Mapper.PermissionsFromJson(jsonString);
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
                Dictionary<string, QPermission> permissions = Mapper.PermissionsFromJson(jsonString);
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
                Dictionary<string, QPermission> permissions = Mapper.PermissionsFromJson(jsonString);
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
                Dictionary<string, QPermission> permissions = Mapper.PermissionsFromJson(jsonString);
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
                Dictionary<string, QProduct> products = Mapper.ProductsFromJson(jsonString);
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
                QOfferings offerings = Mapper.OfferingsFromJson(jsonString);
                OfferingsCallback(offerings, null);
            }

            OfferingsCallback = null;
        }
    }
}