namespace QonversionUnity
{
    internal class AutomationsWrapperNoop : IAutomationsWrapper
    {
        public void Initialize(string gameObjectName)
        {
        }

        public void SetNotificationsToken(string token)
        {
        }

        public bool HandleNotification(string notification)
        {
            return false;
        }

        public string GetNotificationCustomPayload(string notification)
        {
            return null;
        }

        public void SubscribeOnAutomationEvents()
        {
        }
    }
}