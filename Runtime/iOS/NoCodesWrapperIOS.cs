#if UNITY_IOS
using System.Runtime.InteropServices;
#endif

using JetBrains.Annotations;
using UnityEngine;

namespace QonversionUnity
{
    /// <summary>
    /// iOS implementation of INoCodesWrapper.
    /// </summary>
    internal class NoCodesWrapperIOS : INoCodesWrapper
    {
#if UNITY_IOS
        [DllImport("__Internal")]
        private static extern void _initializeNoCodes(string gameObjectName);

        [DllImport("__Internal")]
        private static extern void _initializeNoCodesSdk(string projectKey, string proxyUrl);

        [DllImport("__Internal")]
        private static extern void _setScreenPresentationConfig(string configJson, string contextKey);

        [DllImport("__Internal")]
        private static extern void _showNoCodesScreen(string contextKey);

        [DllImport("__Internal")]
        private static extern void _closeNoCodes();
#endif

        public void Initialize(string gameObjectName)
        {
#if UNITY_IOS
            _initializeNoCodes(gameObjectName);
#endif
        }

        public void InitializeNoCodes(string projectKey, string version, string source, [CanBeNull] string proxyUrl)
        {
#if UNITY_IOS
            _initializeNoCodesSdk(projectKey, proxyUrl ?? "");
#endif
        }

        public void SetScreenPresentationConfig(string configJson, [CanBeNull] string contextKey)
        {
#if UNITY_IOS
            _setScreenPresentationConfig(configJson, contextKey ?? "");
#endif
        }

        public void ShowScreen(string contextKey)
        {
#if UNITY_IOS
            _showNoCodesScreen(contextKey);
#endif
        }

        public void Close()
        {
#if UNITY_IOS
            _closeNoCodes();
#endif
        }
    }
}




