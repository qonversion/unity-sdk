using System;
using System.Collections.Generic;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    internal class Mapper
    {
        internal static string GetLifetimeKey(PermissionsCacheLifetime lifetime) {
            var keys = new Dictionary<PermissionsCacheLifetime, string>() {
                {PermissionsCacheLifetime.WEEK, "Week"},
                {PermissionsCacheLifetime.TWO_WEEKS, "TwoWeeks"},
                {PermissionsCacheLifetime.MONTH, "Month"},
                {PermissionsCacheLifetime.TWO_MONTHS, "TwoMonths"},
                {PermissionsCacheLifetime.THREE_MONTHS, "ThreeMonths"},
                {PermissionsCacheLifetime.SIX_MONTHS, "SixMonths"},
                {PermissionsCacheLifetime.YEAR, "Year"},
                {PermissionsCacheLifetime.UNLIMITED, "Unlimited"}
            };
            return keys[lifetime];
        }
        
        internal static bool GetIsCancelledFromJson(string jsonStr)
        {
            if (Json.Deserialize(jsonStr) is not Dictionary<string, object> result)
            {
                Debug.LogError("Could not parse purchase result");
                return false;
            }
            
            var isCancelled = Convert.ToBoolean(result.GetValueOrDefault("isCancelled", 0));

            return isCancelled;
        }

        internal static Dictionary<string, Permission> PermissionsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, Permission>();

            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> permissions))
            {
                Debug.LogError("Could not parse QPermissions");
                return result;
            }

            foreach (KeyValuePair<string, object> permissionPair in permissions)
            {
                if (permissionPair.Value is Dictionary<string, object> permissionDict)
                {
                    Permission permission = new Permission(permissionDict);
                    result.Add(permissionPair.Key, permission);
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
    }
}