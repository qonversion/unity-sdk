using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    /// <summary>
    /// Internal implementation of NoCodes SDK.
    /// </summary>
    internal class NoCodesInternal : MonoBehaviour, INoCodes
    {
        private const string GameObjectName = "NoCodesRuntimeGameObject";
        private const string SdkVersion = "10.0.3";
        private const string SdkSource = "unity";

        // Event method names for Unity callbacks
        private const string OnScreenShownMethodName = "OnNoCodesScreenShown";
        private const string OnFinishedMethodName = "OnNoCodesFinished";
        private const string OnActionStartedMethodName = "OnNoCodesActionStarted";
        private const string OnActionFailedMethodName = "OnNoCodesActionFailed";
        private const string OnActionFinishedMethodName = "OnNoCodesActionFinished";
        private const string OnScreenFailedToLoadMethodName = "OnNoCodesScreenFailedToLoad";

        private INoCodesWrapper _nativeWrapperInstance;

        // Event handler
        private Action<NoCodesEvent> _onEventReceived;

        // Delegate for handling NoCodes events
        [CanBeNull]
        private NoCodesDelegate _delegate;

        public event Action<NoCodesEvent> EventReceived
        {
            add
            {
                _onEventReceived += value;
            }
            remove
            {
                _onEventReceived -= value;
            }
        }

        public static NoCodesInternal CreateInstance()
        {
            GameObject go = new GameObject(GameObjectName);
            NoCodesInternal instance = go.AddComponent<NoCodesInternal>();
            DontDestroyOnLoad(go);

            return instance;
        }

        void INoCodes.InitializeInstance(NoCodesConfig config)
        {
            INoCodesWrapper instance = GetNativeWrapper();
            instance.InitializeNoCodes(config.ProjectKey, SdkVersion, SdkSource, config.ProxyUrl);
        }

        public void SetDelegate(NoCodesDelegate @delegate)
        {
            _delegate = @delegate;
        }

        public Task SetScreenPresentationConfig(NoCodesPresentationConfig config, [CanBeNull] string contextKey = null)
        {
            try
            {
                var configDict = config.ToDictionary();
                string configJson = Json.Serialize(configDict);

                INoCodesWrapper instance = GetNativeWrapper();
                instance.SetScreenPresentationConfig(configJson, contextKey);

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"[NoCodes] Error setting screen presentation config: {e.Message}");
                return Task.FromException<bool>(e);
            }
        }

        public Task ShowScreen(string contextKey)
        {
            try
            {
                INoCodesWrapper instance = GetNativeWrapper();
                instance.ShowScreen(contextKey);

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"[NoCodes] Error showing screen: {e.Message}");
                return Task.FromException<bool>(e);
            }
        }

        public Task Close()
        {
            try
            {
                INoCodesWrapper instance = GetNativeWrapper();
                instance.Close();

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"[NoCodes] Error closing screen: {e.Message}");
                return Task.FromException<bool>(e);
            }
        }

        // Called from native SDK - Screen shown event
        public void OnNoCodesScreenShown(string jsonString)
        {
            var payload = ParsePayload(jsonString);
            payload["type"] = "nocodes_screen_shown";
            var evt = new NoCodesEvent(payload);
            
            // Invoke event handler
            if (_onEventReceived != null)
            {
                _onEventReceived(evt);
            }
            
            // Invoke delegate if set
            if (_delegate == null)
            {
                return;
            }

            string screenId = Mapper.ScreenIdFromJson(jsonString);
            _delegate.OnNoCodesScreenShown(screenId);
        }

        // Called from native SDK - Finished event
        public void OnNoCodesFinished(string jsonString)
        {
            var payload = ParsePayload(jsonString);
            payload["type"] = "nocodes_finished";
            var evt = new NoCodesEvent(payload);
            
            // Invoke event handler
            if (_onEventReceived != null)
            {
                _onEventReceived(evt);
            }
            
            // Invoke delegate if set
            if (_delegate == null)
            {
                return;
            }

            _delegate.OnNoCodesFinished(jsonString);
        }

        // Called from native SDK - Action started event
        public void OnNoCodesActionStarted(string jsonString)
        {
            var payload = ParsePayload(jsonString);
            payload["type"] = "nocodes_action_started";
            var evt = new NoCodesEvent(payload);
            
            // Invoke event handler
            if (_onEventReceived != null)
            {
                _onEventReceived(evt);
            }
            
            // Invoke delegate if set
            if (_delegate == null)
            {
                return;
            }

            _delegate.OnNoCodesActionStarted(jsonString);
        }

        // Called from native SDK - Action failed event
        public void OnNoCodesActionFailed(string jsonString)
        {
            var payload = ParsePayload(jsonString);
            payload["type"] = "nocodes_action_failed";
            var evt = new NoCodesEvent(payload);
            
            // Invoke event handler
            if (_onEventReceived != null)
            {
                _onEventReceived(evt);
            }
            
            // Invoke delegate if set
            if (_delegate == null)
            {
                return;
            }

            _delegate.OnNoCodesActionFailed(jsonString);
        }

        // Called from native SDK - Action finished event
        public void OnNoCodesActionFinished(string jsonString)
        {
            var payload = ParsePayload(jsonString);
            payload["type"] = "nocodes_action_finished";
            var evt = new NoCodesEvent(payload);
            
            // Invoke event handler
            if (_onEventReceived != null)
            {
                _onEventReceived(evt);
            }
            
            // Invoke delegate if set
            if (_delegate == null)
            {
                return;
            }

            _delegate.OnNoCodesActionFinished(jsonString);
        }

        // Called from native SDK - Screen failed to load event
        public void OnNoCodesScreenFailedToLoad(string jsonString)
        {
            var payload = ParsePayload(jsonString);
            payload["type"] = "nocodes_screen_failed_to_load";
            var evt = new NoCodesEvent(payload);
            
            // Invoke event handler
            if (_onEventReceived != null)
            {
                _onEventReceived(evt);
            }
            
            // Invoke delegate if set
            if (_delegate == null)
            {
                return;
            }

            _delegate.OnNoCodesScreenFailedToLoad(jsonString);
        }

        private Dictionary<string, object> ParsePayload(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                return new Dictionary<string, object>();
            }

            try
            {
                var deserialized = Json.Deserialize(jsonString);
                if (deserialized is Dictionary<string, object> dict)
                {
                    return dict;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[NoCodes] Failed to parse payload: {e.Message}");
            }

            return new Dictionary<string, object>();
        }

        private INoCodesWrapper GetNativeWrapper()
        {
            if (_nativeWrapperInstance != null)
            {
                return _nativeWrapperInstance;
            }

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    _nativeWrapperInstance = new NoCodesWrapperAndroid();
                    break;
                case RuntimePlatform.IPhonePlayer:
                    _nativeWrapperInstance = new NoCodesWrapperIOS();
                    break;
                default:
                    _nativeWrapperInstance = new NoCodesWrapperNoop();
                    break;
            }
            _nativeWrapperInstance.Initialize(GameObjectName);

            return _nativeWrapperInstance;
        }
    }
}

