using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public interface IAutomations
    {
        /// <summary>
        /// The Automations delegate is responsible for handling in-app screens and actions when push notification is received.
        /// Make sure the method is called before Qonversion.handleNotification.
        /// </summary>
        /// <param name="automationsDelegate">The delegate to handle automations events</param>
        public void SetDelegate(AutomationsDelegate automationsDelegate);

        /// <summary>
        /// Set push token to Qonversion to enable Qonversion push notifications
        /// </summary>
        /// <param name="token">Firebase device token for Android. APNs device token for iOS.</param>
        public void SetNotificationsToken(string token);

        /// <summary>
        /// Call to handle push notifications sent by Qonversion Automations.
        /// </summary>
        /// <param name="notification">notification payload data</param>
        /// <returns>true when a push notification was received from Qonversion. Otherwise, returns false, so you need to handle the notification yourself</returns>
        /// <see href="https://pub.dev/documentation/firebase_messaging_platform_interface/latest/firebase_messaging_platform_interface/RemoteMessage/data.html">Firebase RemoteMessage data</see>
        /// <see href="https://developer.apple.com/documentation/usernotifications/unnotificationcontent/1649869-userinfo">APNs notification data</see>
        public bool HandleNotification(Dictionary<string, object> notification);

        /// <summary>
        /// Get parsed custom payload, which you added to the notification in the dashboard
        /// </summary>
        /// <param name="notification">notification payload data</param>
        /// <returns>a map with custom payload from the notification or null if it's not provided</returns>
        [CanBeNull]
        public Dictionary<string, object> GetNotificationCustomPayload(Dictionary<string, object> notification);

        /// <summary>
        /// Show the screen using its ID.
        /// </summary>
        /// <param name="screenId">identifier of the screen which must be shown</param>
        /// <param name="callback">callback that will be called when response is received</param>
        public void ShowScreen(string screenId, Automations.OnShowScreenResponseReceived callback);

        /// <summary>
        /// Set the configuration of screen representation.
        /// </summary>
        /// <param name="config">a configuration to apply</param>
        /// <param name="screenId">identifier of screen, to which a config should be applied.
        ///                        If not provided, the config is used for all the screens</param>
        public void SetScreenPresentationConfig(ScreenPresentationConfig config, [CanBeNull] string screenId = null);
    }
}
