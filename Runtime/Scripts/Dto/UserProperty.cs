using System.Collections.Generic;
using UnityEngine.Scripting;

namespace QonversionUnity
{
    public class UserProperty
    {
        public readonly string Key;
        public readonly string Value;
        public readonly UserPropertyKey DefinedKey;

        // Informs compiler to save this method from removal via optimization,
        // as even if it has no direct calls, it is called via reflection while mapping.
        [Preserve]
        public UserProperty(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("key", out var key) && key != null)
            {
                Key = key as string;
                DefinedKey = ConvertUserPropertyKey(Key);
            }

            if (dict.TryGetValue("value", out var value) && value != null)
            {
                Value = value as string;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Key)}: {Key}, " +
                   $"{nameof(Value)}: {Value}";
        }
        
        private UserPropertyKey ConvertUserPropertyKey(string stringKey)
        {
            switch (stringKey)
            {
                case "_q_email":
                    return UserPropertyKey.Email;
                case "_q_name":
                    return UserPropertyKey.Name;
                case "_q_kochava_device_id":
                    return UserPropertyKey.KochavaDeviceId;
                case "_q_appsflyer_user_id":
                    return UserPropertyKey.AppsFlyerUserId;
                case "_q_adjust_adid":
                    return UserPropertyKey.AdjustAdId;
                case "_q_custom_user_id":
                    return UserPropertyKey.CustomUserId;
                case "_q_fb_attribution":
                    return UserPropertyKey.FacebookAttribution;
                case "_q_firebase_instance_id":
                    return UserPropertyKey.FirebaseAppInstanceId;
                case "_q_app_set_id":
                    return UserPropertyKey.AppSetId;
                case "_q_advertising_id":
                    return UserPropertyKey.AdvertisingId;
            }

            return UserPropertyKey.Custom;
        }
    }
}
