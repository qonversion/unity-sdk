using System.Collections.Generic;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    internal class AutomationsInternal : MonoBehaviour, IAutomations
    {
        private const string GameObjectName = "QonvesrionAutomationsRuntimeGameObject";
        private const string OnShowScreenMethodName = "OnShowScreen";

        private IAutomationsWrapper _nativeWrapperInstance;
        private AutomationsDelegate _automationsDelegate;

        private Automations.OnShowScreenResponseReceived ShowScreenResponseReceivedCallback { get; set; }

        public static AutomationsInternal CreateInstance()
        {
            GameObject go = new GameObject(GameObjectName);
            AutomationsInternal instance = go.AddComponent<AutomationsInternal>();
            DontDestroyOnLoad(go);

            return instance;
        }

        public void SetDelegate(AutomationsDelegate automationsDelegate)
        {
            _automationsDelegate = automationsDelegate;

            IAutomationsWrapper instance = GetNativeWrapper();
            instance.SubscribeOnAutomationEvents();
        }
        
        public void SetNotificationsToken(string token)
        {
            IAutomationsWrapper instance = GetNativeWrapper();
            instance.SetNotificationsToken(token);
        }

        public bool HandleNotification(Dictionary<string, object> notification)
        {
            IAutomationsWrapper instance = GetNativeWrapper();
            return instance.HandleNotification(notification.toJson());
        }

        public Dictionary<string, object> GetNotificationCustomPayload(Dictionary<string, object> notification)
        {
            IAutomationsWrapper instance = GetNativeWrapper();
            var payloadJson = instance.GetNotificationCustomPayload(notification.toJson());

            if (payloadJson == null)
            {
                return null;
            }

            if (!(Json.Deserialize(payloadJson) is Dictionary<string, object> response))
            {
                Debug.LogError("Could not parse custom notification payload.");
                return null;
            }

            return response;
        }

        public void ShowScreen(string screenId, Automations.OnShowScreenResponseReceived callback)
        {
            ShowScreenResponseReceivedCallback = callback;
            
            IAutomationsWrapper instance = GetNativeWrapper();
            instance.ShowScreen(screenId, OnShowScreenMethodName);
        }

        private IAutomationsWrapper GetNativeWrapper()
        {
            if (_nativeWrapperInstance != null)
            {
                return _nativeWrapperInstance;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _nativeWrapperInstance = new AutomationsWrapperAndroid();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _nativeWrapperInstance = new AutomationsWrapperIOS();
                    break;
                default:
                    _nativeWrapperInstance = new AutomationsWrapperNoop();
                    break;
            }
            _nativeWrapperInstance.Initialize(GameObjectName);

            return _nativeWrapperInstance;
        }
        
        // The below methods are called from native when Automations events occur
        private void OnAutomationsScreenShown(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            string screenId = Mapper.ScreenIdFromJson(jsonString);

            _automationsDelegate.OnAutomationsScreenShown(screenId);
        }

        private void OnAutomationsActionStarted(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            ActionResult actionResult = Mapper.ActionResultFromJson(jsonString);
            _automationsDelegate.OnAutomationsActionStarted(actionResult);
        }

        private void OnAutomationsActionFailed(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            ActionResult actionResult = Mapper.ActionResultFromJson(jsonString);
            _automationsDelegate.OnAutomationsActionFailed(actionResult);
        }

        
        private void OnAutomationsActionFinished(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            ActionResult actionResult = Mapper.ActionResultFromJson(jsonString);
            _automationsDelegate.OnAutomationsActionFinished(actionResult);
        }

        private void OnAutomationsFinished(string jsonString)
        {
            if (_automationsDelegate == null)
            {
                return;
            }

            _automationsDelegate.OnAutomationsFinished();
        }
        
        private void OnShowScreen(string jsonString)
        {
            var error = Mapper.ErrorFromJson(jsonString);
            ShowScreenResponseReceivedCallback(error);
            ShowScreenResponseReceivedCallback = null;
        }
    }
}
