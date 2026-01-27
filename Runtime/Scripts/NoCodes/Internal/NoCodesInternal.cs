using UnityEngine;

namespace QonversionUnity
{
    internal class NoCodesInternal : MonoBehaviour, INoCodes
    {
        private const string GameObjectName = "QonversionNoCodesRuntimeGameObject";
        private const string SdkVersion = "8.2.1";

        private INoCodesWrapper _nativeWrapperInstance;
        private NoCodesDelegate _noCodesDelegate;
        private NoCodesPurchaseDelegate _purchaseDelegate;
        private NoCodesConfig _config;

        public static NoCodesInternal CreateInstance(NoCodesConfig config)
        {
            GameObject go = new GameObject(GameObjectName);
            NoCodesInternal instance = go.AddComponent<NoCodesInternal>();
            DontDestroyOnLoad(go);

            instance._config = config;
            instance._noCodesDelegate = config.NoCodesDelegate;
            instance._purchaseDelegate = config.PurchaseDelegate;
            instance.InitializeNative();

            return instance;
        }

        private void InitializeNative()
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            string theme = _config.Theme.HasValue ? _config.Theme.Value.ToNativeString() : null;
            wrapper.Initialize(GameObjectName, _config.ProjectKey, _config.ProxyUrl, _config.Locale, theme, SdkVersion);

            if (_noCodesDelegate != null)
            {
                LogAndroidNoCodesDelegateWarning();
                wrapper.SetDelegate();
            }

            if (_purchaseDelegate != null)
            {
                LogAndroidPurchaseDelegateWarning();
                wrapper.SetPurchaseDelegate();
            }
        }

        public void SetScreenPresentationConfig(ScreenPresentationConfig config, string contextKey = null)
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            wrapper.SetScreenPresentationConfig(config.ToJson(), contextKey);
        }

        public void ShowScreen(string contextKey)
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            wrapper.ShowScreen(contextKey);
        }

        public void Close()
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            wrapper.Close();
        }

        public void SetLocale(string locale)
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            wrapper.SetLocale(locale);
        }

        public void SetTheme(NoCodesTheme theme)
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            wrapper.SetTheme(theme.ToNativeString());
        }

        public void SetPurchaseDelegate(NoCodesPurchaseDelegate purchaseDelegate)
        {
            _purchaseDelegate = purchaseDelegate;
            
            if (purchaseDelegate != null)
            {
                LogAndroidPurchaseDelegateWarning();
                INoCodesWrapper wrapper = GetNativeWrapper();
                wrapper.SetPurchaseDelegate();
            }
        }

        private void LogAndroidNoCodesDelegateWarning()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.LogWarning(
                    "[Qonversion NoCodes] Android Warning: NoCodesDelegate events will be delivered with a delay. " +
                    "When a No-Codes screen is displayed, Unity's game loop is paused because the No-Codes screen " +
                    "runs as a separate Activity. Events will only be received after the No-Codes screen is closed."
                );
            }
        }

        private void LogAndroidPurchaseDelegateWarning()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                Debug.LogWarning(
                    "[Qonversion NoCodes] Android Warning: NoCodesPurchaseDelegate does NOT work on Android. " +
                    "When a No-Codes screen is displayed, Unity's game loop is paused, so Purchase() and Restore() " +
                    "methods will not be called while the screen is active. Use the default Qonversion SDK purchase " +
                    "flow on Android instead."
                );
            }
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

            return _nativeWrapperInstance;
        }

        // Called from native when NoCodes events occur
        private void OnNoCodesScreenShown(string jsonString)
        {
            if (_noCodesDelegate == null) return;

            string screenId = NoCodesMapper.ScreenIdFromJson(jsonString);
            if (screenId != null)
            {
                _noCodesDelegate.OnScreenShown(screenId);
            }
        }

        private void OnNoCodesActionStarted(string jsonString)
        {
            if (_noCodesDelegate == null) return;

            NoCodesAction action = NoCodesMapper.ActionFromJson(jsonString);
            if (action != null)
            {
                _noCodesDelegate.OnActionStarted(action);
            }
        }

        private void OnNoCodesActionFailed(string jsonString)
        {
            if (_noCodesDelegate == null) return;

            NoCodesAction action = NoCodesMapper.ActionFromJson(jsonString);
            if (action != null)
            {
                _noCodesDelegate.OnActionFailed(action);
            }
        }

        private void OnNoCodesActionFinished(string jsonString)
        {
            if (_noCodesDelegate == null) return;

            NoCodesAction action = NoCodesMapper.ActionFromJson(jsonString);
            if (action != null)
            {
                _noCodesDelegate.OnActionFinished(action);
            }
        }

        private void OnNoCodesFinished(string jsonString)
        {
            if (_noCodesDelegate == null) return;

            _noCodesDelegate.OnFinished();
        }

        private void OnNoCodesScreenFailedToLoad(string jsonString)
        {
            if (_noCodesDelegate == null) return;

            NoCodesError error = NoCodesMapper.ErrorFromJson(jsonString);
            if (error != null)
            {
                _noCodesDelegate.OnScreenFailedToLoad(error);
            }
        }

        // Called from native for purchase delegate
        private void OnNoCodesPurchase(string jsonString)
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            
            if (_purchaseDelegate == null)
            {
                Debug.LogError("PurchaseDelegate is not set but purchase event received");
                wrapper.DelegatedPurchaseFailed("PurchaseDelegate is not set");
                return;
            }

            Product product = NoCodesMapper.ProductFromJson(jsonString);
            if (product == null)
            {
                wrapper.DelegatedPurchaseFailed("Failed to parse product");
                return;
            }

            _purchaseDelegate.Purchase(
                product,
                onSuccess: () => wrapper.DelegatedPurchaseCompleted(),
                onError: (errorMessage) => wrapper.DelegatedPurchaseFailed(errorMessage)
            );
        }

        private void OnNoCodesRestore(string jsonString)
        {
            INoCodesWrapper wrapper = GetNativeWrapper();
            
            if (_purchaseDelegate == null)
            {
                Debug.LogError("PurchaseDelegate is not set but restore event received");
                wrapper.DelegatedRestoreFailed("PurchaseDelegate is not set");
                return;
            }

            _purchaseDelegate.Restore(
                onSuccess: () => wrapper.DelegatedRestoreCompleted(),
                onError: (errorMessage) => wrapper.DelegatedRestoreFailed(errorMessage)
            );
        }
    }
}
