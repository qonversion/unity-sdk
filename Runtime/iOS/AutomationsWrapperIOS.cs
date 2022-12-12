#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    internal class AutomationsWrapperIOS : IAutomationsWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _initialize(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _setNotificationsToken(string token);

        [DllImport("__Internal")]
        private static extern bool _handleNotification(string notification);

        [DllImport("__Internal")]
        private static extern string _getNotificationCustomPayload(string notification);

        [DllImport("__Internal")]
        private static extern void _subscribeOnAutomationEvents();

        [DllImport("__Internal")]
        private static extern void _showScreen(string screenId, string callbackName);
#endif

        public void Initialize(string gameObjectName)
        {
#if UNITY_IOS
            _initialize(gameObjectName);
#endif
        }

        public void SetNotificationsToken(string token)
        {
#if UNITY_IOS
             _setNotificationsToken(token);
#endif
        }

        public bool HandleNotification(string notification)
        {
#if UNITY_IOS
             return _handleNotification(notification);
#else
            return false;
#endif
        }

        public string GetNotificationCustomPayload(string notification)
        {
#if UNITY_IOS
             return _getNotificationCustomPayload(notification);
#else
            return null;
#endif
        }

        public void SubscribeOnAutomationEvents()
        {
#if UNITY_IOS
            _subscribeOnAutomationEvents();
#endif
        }
        
        public void ShowScreen(string screenId, string callbackName)
        {
#if UNITY_IOS
            _showScreen(screenId, callbackName);
#endif
        }
    }
}