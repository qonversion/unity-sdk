using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// No-op implementation of INoCodesWrapper for unsupported platforms.
    /// </summary>
    internal class NoCodesWrapperNoop : INoCodesWrapper
    {
        public void Initialize(string gameObjectName)
        {
        }

        public void InitializeNoCodes(string projectKey, string version, string source, [CanBeNull] string proxyUrl)
        {
        }

        public void SetScreenPresentationConfig(string configJson, [CanBeNull] string contextKey)
        {
        }

        public void ShowScreen(string contextKey)
        {
        }

        public void Close()
        {
        }
    }
}




