using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    // ReSharper disable once InconsistentNaming
    public interface Automations
    {
        [CanBeNull] private static Automations _backingInstance;

        /// <summary>
        /// Use this variable to get a current initialized instance of the Qonversion Automations.
        /// Please, use Automations only after calling <see cref="Qonversion.Initialize"/>.
        /// Otherwise, trying to access the variable will cause an error.
        /// </summary>
        /// <returns>Current initialized instance of the Qonversion Automations.</returns>
        /// <exception cref="Exception">throws exception if Qonversion has not been initialized.</exception>
        public static Automations GetSharedInstance()
        {
            if (_backingInstance == null)
            {
                try
                {
                    Qonversion.GetSharedInstance();
                }
                catch (Exception e)
                {
                    throw new Exception("Qonversion has not been initialized. " + 
                                        "Automations should be used after Qonversion is initialized.");
                }

                _backingInstance = AutomationsInternal.CreateInstance();
            }

            return _backingInstance;
        }

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
    }
}
