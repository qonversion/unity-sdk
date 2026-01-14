using System.Collections.Generic;
using JetBrains.Annotations;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    internal static class NoCodesMapper
    {
        [CanBeNull]
        internal static string ScreenIdFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> screenResult))
            {
                Debug.LogError("Could not parse NoCodes screen id");
                return null;
            }

            return screenResult.GetString("screenId", "");
        }

        [CanBeNull]
        internal static NoCodesAction ActionFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> actionResult))
            {
                Debug.LogError("Could not parse NoCodes action");
                return null;
            }

            return new NoCodesAction(actionResult);
        }

        [CanBeNull]
        internal static NoCodesError ErrorFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dict))
            {
                Debug.LogError("Could not parse NoCodes error");
                return null;
            }

            return new NoCodesError(dict);
        }

        [CanBeNull]
        internal static Product ProductFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> productDict))
            {
                Debug.LogError("Could not parse NoCodes product for purchase");
                return null;
            }

            return new Product(productDict);
        }
    }
}
