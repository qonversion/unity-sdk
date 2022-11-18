using JetBrains.Annotations;

namespace QonversionUnity
{
    internal interface IAutomationsWrapper
    {
        void Initialize(string gameObjectName);
        void InitializeSdk();
        void SetNotificationsToken(string token);
        bool HandleNotification(string notification);
        [CanBeNull] string GetNotificationCustomPayload(string notification);
        void SubscribeOnAutomationEvents();
    }
}
