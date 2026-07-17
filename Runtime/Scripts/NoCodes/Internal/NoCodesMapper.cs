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
        internal static NoCodesScreen ScreenFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> screenDict))
            {
                Debug.LogError("Could not parse NoCodes screen");
                return null;
            }

            return new NoCodesScreen(screenDict);
        }

        internal static string CustomActionValueFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dict))
            {
                Debug.LogError("Could not parse NoCodes custom action");
                return "";
            }

            return dict.GetString("value");
        }

        [CanBeNull]
        internal static string ContextKeyFromJson(string jsonStr)
        {
            if (!(Json.Deserialize(jsonStr) is Dictionary<string, object> dict))
            {
                return null;
            }

            return dict.GetString("contextKey");
        }

        internal static NoCodesError ScreenParsingError()
        {
            return new NoCodesError(new Dictionary<string, object>
            {
                { "code", "Deserialization" },
                { "description", "Failed to parse the loaded No-Code screen" },
                { "additionalMessage", "Native payload parsing failed." }
            });
        }

        internal static NoCodesError UnsupportedPlatformError()
        {
            return new NoCodesError(new Dictionary<string, object>
            {
                { "code", "Unknown" },
                { "description", "No-Codes is not supported on this platform" },
                { "additionalMessage", "" }
            });
        }

        internal static NoCodesError LoadScreenErrorFromJson(string jsonStr)
        {
            if (Json.Deserialize(jsonStr) is Dictionary<string, object> dict &&
                dict.TryGetValue("error", out object errorValue) &&
                errorValue is Dictionary<string, object> errorDict)
            {
                return new NoCodesError(errorDict);
            }

            Debug.LogError("Could not parse NoCodes screen loading error");
            return new NoCodesError(new Dictionary<string, object>
            {
                { "code", "Unknown" },
                { "description", "Failed to load No-Code screen" },
                { "additionalMessage", "Native error parsing failed." }
            });
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
