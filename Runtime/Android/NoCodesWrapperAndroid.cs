using System;
using JetBrains.Annotations;
using QonversionUnity.MiniJSON;
using UnityEngine;

namespace QonversionUnity
{
    /// <summary>
    /// Android implementation of INoCodesWrapper.
    /// </summary>
    internal class NoCodesWrapperAndroid : INoCodesWrapper
    {
        private const string NoCodesWrapper = "com.qonversion.unitywrapper.NoCodesWrapper";

        public void Initialize(string gameObjectName)
        {
            CallNoCodes("initialize", gameObjectName);
        }

        public void InitializeNoCodes(string projectKey, string version, string source, [CanBeNull] string proxyUrl)
        {
            CallNoCodes("initializeNoCodes", projectKey, version, source, proxyUrl);
        }

        public void SetScreenPresentationConfig(string configJson, [CanBeNull] string contextKey)
        {
            CallNoCodes("setScreenPresentationConfig", configJson, contextKey);
        }

        public void ShowScreen(string contextKey)
        {
            CallNoCodes("showNoCodesScreen", contextKey);
        }

        public void Close()
        {
            CallNoCodes("closeNoCodes");
        }

        private static void CallNoCodes(string methodName, params object[] args)
        {
            try
            {
                using (var noCodes = new AndroidJavaClass(NoCodesWrapper))
                {
                    noCodes.CallStatic(methodName, args);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[NoCodes] Error calling {methodName}: {e.Message}");
            }
        }
    }
}




