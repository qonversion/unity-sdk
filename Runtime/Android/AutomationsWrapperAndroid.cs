using UnityEngine;

namespace QonversionUnity
{
    internal class AutomationsWrapperAndroid : IAutomationsWrapper
    {
        public void Initialize(string gameObjectName)
        {
            CallAutomations("initialize", gameObjectName);
        }

        public void SetNotificationsToken(string token)
        {
            CallAutomations("setNotificationsToken", token);
        }

        public bool HandleNotification(string notification)
        {
            return CallAutomations<bool>("handleNotification", notification);
        }

        public string GetNotificationCustomPayload(string notification)
        {
            return CallAutomations<string>("getNotificationCustomPayload", notification);
        }

        public void SubscribeOnAutomationEvents()
        {
            CallAutomations("subscribeOnAutomationEvents");
        }

        public void ShowScreen(string screenId, string callbackName)
        {
            CallAutomations("showScreen", screenId, callbackName);
        }

        private const string AutomationsWrapper = "com.qonversion.unitywrapper.AutomationsWrapper";

        private static T CallAutomations<T>(string methodName, params object[] args)
        {
            using (var automations = new AndroidJavaClass(AutomationsWrapper))
            {
                return automations.CallStatic<T>(methodName, args);
            }
        }

        private static void CallAutomations(string methodName, params object[] args)
        {
            using (var automations = new AndroidJavaClass(AutomationsWrapper))
            {
                automations.CallStatic(methodName, args);
            }
        }
    }
}