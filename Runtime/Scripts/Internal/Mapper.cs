using System;
using System.Collections.Generic;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    internal class Mapper
    {
        internal static Dictionary<string, Entitlement> EntitlementsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, Entitlement>();

            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> entitlements))
            {
                Debug.LogError("Could not parse QEntitlements");
                return result;
            }

            foreach (KeyValuePair<string, object> entitlementPair in entitlements)
            {
                if (entitlementPair.Value is Dictionary<string, object> entitlementDict)
                {
                    Entitlement entitlement = new Entitlement(entitlementDict);
                    result.Add(entitlementPair.Key, entitlement);
                }
            }

            return result;
        }

        internal static Dictionary<string, Product> ProductsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, Product>();

            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> products))
            {
                Debug.LogError("Could not parse QProducts");
                return result;
            }

            foreach (KeyValuePair<string, object> productPair in products)
            {
                if (productPair.Value is Dictionary<string, object> productDict)
                {
                    Product product = new Product(productDict);
                    result.Add(productPair.Key, product);
                }
            }

            return result;
        }

        internal static Offerings OfferingsFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> offerings))
            {
                Debug.LogError("Could not parse QOfferings");
                return null;
            }

            return new Offerings(offerings);
        }

        internal static RemoteConfig RemoteConfigFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> remoteConfig))
            {
                Debug.LogError("Could not parse RemoteConfig");
                return null;
            }

            return new RemoteConfig(remoteConfig);
        }

        internal static RemoteConfigList RemoteConfigListFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> remoteConfigList))
            {
                Debug.LogError("Could not parse RemoteConfigList");
                return null;
            }

            return new RemoteConfigList(remoteConfigList);
        }

        internal static ActionResult ActionResultFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> actionResult))
            {
                Debug.LogError("Could not parse Automations ActionResult");
                return null;
            }

            return new ActionResult(actionResult);
        }

        internal static string ScreenIdFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> screenResult))
            {
                Debug.LogError("Could not parse Automations screen id");
                return null;
            }

            return screenResult.GetString("screenId", "");
        }

        internal static bool IsFallbackFileAccessibleFromJson(string jsonStr)
        {
            var isAccessible = false;
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> rawResult))
            {
                Debug.LogError("Could not parse fallback file accessibility");
                return false;
            }
            if (rawResult.TryGetValue("success", out object value)) isAccessible = (bool)value;

            return isAccessible;
        }

        internal static Dictionary<string, Eligibility> EligibilitiesFromJson(string jsonStr)
        {
            var result = new Dictionary<string, Eligibility>();

            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> elibilities))
            {
                Debug.LogError("Could not parse Eligibilities");
                return result;
            }

            foreach (KeyValuePair<string, object> eligibilityPair in elibilities)
            {
                if (eligibilityPair.Value is Dictionary<string, object> eligibilityDict)
                {
                    Eligibility eligibility = new Eligibility(eligibilityDict);
                    result.Add(eligibilityPair.Key, eligibility);
                }
            }

            return result;
        }

        internal static User UserFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> userInfo))
            {
                Debug.LogError("Could not parse User");
                return new User("", null);
            }

            return new User(userInfo);
        }

        internal static UserProperties UserPropertiesFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> userProperties))
            {
                Debug.LogError("Could not parse user properties");
                return null;
            }

            return new UserProperties(userProperties);
        }

        internal static QonversionError ErrorFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dict)) return null;
            
            QonversionError error = null;
            if (dict.TryGetValue("error", out object errorObj))
            {
                error = new QonversionError((Dictionary<string, object>)errorObj);
            }

            return error;
        }

        internal static List<T> ConvertObjectsList<T>(List<object> objects)
        {
            var result = new List<T>();

            foreach (Dictionary<string, object> objectDict in objects)
            {
                T obj = (T)Activator.CreateInstance(typeof(T), objectDict);
                result.Add(obj);
            }

            return result;
        }

        internal static QProductType FormatType(object productType)
        {
            string value = productType as string;

            switch (value) {
                case "Trial": return QProductType.Trial;
                case "Intro": return QProductType.Intro;
                case "Subscription": return QProductType.Subscription;
                case "InApp": return QProductType.InApp;
                default: return QProductType.Unknown;
            }
        }
    }
}