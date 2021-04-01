using System;
using System.Collections.Generic;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    internal class Mapper
    {
        internal static Dictionary<string, QPermission> PermissionsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, QPermission>();

            if (!(Json.Deserialize(jsonStr) is List<object> permissions))
            {
                Debug.LogError("Could not parse QPermissions");
                return result;
            }

            foreach (Dictionary<string, object> permissionDict in permissions)
            {
                QPermission permission = new QPermission(permissionDict);
                result.Add(permission.PermissionID, permission);
            }

            return result;
        }

        internal static Dictionary<string, QProduct> ProductsFromJson(string jsonStr)
        {
            var result = new Dictionary<string, QProduct>();

            if (!(Json.Deserialize(jsonStr) is List<object> products))
            {
                Debug.LogError("Could not parse QProducts");
                return result;
            }

            foreach (Dictionary<string, object> productDict in products)
            {
                QProduct product = new QProduct(productDict);
                result.Add(product.QonversionId, product);
            }

            return result;
        }

        internal static QOfferings OfferingsFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> offerings))
            {
                Debug.LogError("Could not parse QOfferings");
                return null;
            }

            return new QOfferings(offerings);
        }
        

        internal static QError ErrorFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dict)) return null;
            
            QError error = null;
            if (dict.TryGetValue("error", out object errorObj))
            {
                error = new QError((Dictionary<string, object>)errorObj);
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