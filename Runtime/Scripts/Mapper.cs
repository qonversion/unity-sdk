using System;
using System.Collections.Generic;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    internal class Mapper
    {
        internal static Dictionary<string, Permission> PermissionsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, Permission>();

            if (!(Json.Deserialize(jsonStr) is List<object> permissions))
            {
                Debug.LogError("Could not parse QPermissions");
                return result;
            }

            foreach (Dictionary<string, object> permissionDict in permissions)
            {
                Permission permission = new Permission(permissionDict);
                result.Add(permission.PermissionID, permission);
            }

            return result;
        }

        internal static Dictionary<string, Product> ProductsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, Product>();

            if (!(Json.Deserialize(jsonStr) is List<object> products))
            {
                Debug.LogError("Could not parse QProducts");
                return result;
            }

            foreach (Dictionary<string, object> productDict in products)
            {
                Product product = new Product(productDict);
                result.Add(product.QonversionId, product);
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