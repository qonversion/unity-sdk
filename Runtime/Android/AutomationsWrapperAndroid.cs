using UnityEngine;

namespace QonversionUnity
{
    internal class AutomationsWrapperAndroid : IAutomationsWrapper
    {
        public void Initialize(string gameObjectName)
        {
            CallQonversion("initialize", gameObjectName);
        }

        public void SetNotificationsToken(string token)
        {
            CallQonversion("setNotificationsToken", token);
        }

        public bool HandleNotification(string notification)
        {
            return CallQonversion<bool>("handleNotification", notification);
        }

        public string GetNotificationCustomPayload(string notification)
        {
            return CallQonversion<string>("getNotificationCustomPayload", notification);
        }

        public void SubscribeOnAutomationEvents()
        {
            CallQonversion("subscribeOnAutomationEvents");
        }

        private const string AutomationsWrapper = "com.qonversion.unitywrapper.AutomationsWrapper";

        private static T CallQonversion<T>(string methodName, params object[] args)
        {
            using (var automations = new AndroidJavaClass(AutomationsWrapper))
            {
                return automations.CallStatic<T>(methodName, args);
            }
        }

        private static void CallQonversion(string methodName, params object[] args)
        {
            using (var automations = new AndroidJavaClass(AutomationsWrapper))
            {
                automations.CallStatic(methodName, args);
            }
        }
    }
}