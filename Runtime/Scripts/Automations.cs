using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace QonversionUnity
{
    public abstract class Automations : MonoBehaviour
    {
        [CanBeNull] private static Automations _backingInstance;

        /// <summary>
        /// Use this variable to get a current initialized instance of the Qonversion Automations.
        /// Please, use the property only after calling <see cref="Automations.Initialize"/>.
        /// Otherwise, trying to access the variable will cause an exception.
        /// </summary>
        /// <returns>Current initialized instance of the Qonversion Automations.</returns>
        /// <exception cref="Exception">throws exception if the instance has not been initialized</exception>
        public static Automations GetSharedInstance()
        {
            if (_backingInstance == null)
            {
                throw new Exception(
                    "Automations has not been initialized. You should call " +
                    "the initialize method before accessing the shared instance of Automations."
                );
            }

            return _backingInstance;
        }

        /// <summary>
        /// An entry point to use Qonversion Automations. Call to initialize Automations.
        /// Make sure you have initialized <see cref="Qonversion"/> first.
        /// </summary>
        /// <returns>Initialized instance of the Qonversion Automations.</returns>
        /// <exception cref="Exception">throws exception if <see cref="Qonversion"/> has not been initialized</exception>
        public static Automations Initialize()
        {
            try
            {
                Qonversion.GetSharedInstance();
            }
            catch (Exception e)
            {
                throw new Exception("Qonversion has not been initialized. " + 
                                    "Automations initialization should be called after Qonversion is initialized.");
            }

            _backingInstance = new AutomationsInternal();
            return _backingInstance;
        }

        /// <summary>
        /// The Automations delegate is responsible for handling in-app screens and actions when push notification is received.
        /// Make sure the method is called before Qonversion.handleNotification.
        /// </summary>
        /// <param name="automationsDelegate">The delegate to handle automations events</param>
        public abstract void SetDelegate(AutomationsDelegate automationsDelegate);

        /// <summary>
        /// Set push token to Qonversion to enable Qonversion push notifications
        /// </summary>
        /// <param name="token">Firebase device token on Android. APNs device token on iOS.</param>
        public abstract void SetNotificationsToken(string token);

        /// <summary>
        /// Call to handle push notifications sent by Qonversion Automations.
        /// </summary>
        /// <param name="notification">notification payload data</param>
        /// <returns>true when a push notification was received from Qonversion. Otherwise, returns false, so you need to handle a notification yourself</returns>
        /// <see href="https://pub.dev/documentation/firebase_messaging_platform_interface/latest/firebase_messaging_platform_interface/RemoteMessage/data.html">Firebase RemoteMessage data</see>
        /// <see href="https://developer.apple.com/documentation/usernotifications/unnotificationcontent/1649869-userinfo">APNs notification data</see>
        public abstract bool HandleNotification(Dictionary<string, object> notification);

        /// <summary>
        /// Get parsed custom payload, which you added to the notification in the dashboard
        /// </summary>
        /// <param name="notification">notification payload data</param>
        /// <returns>a map with custom payload from the notification or null if it's not provided</returns>
        [CanBeNull]
        public abstract Dictionary<string, object> GetNotificationCustomPayload(Dictionary<string, object> notification);
    }
}
