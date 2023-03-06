using JetBrains.Annotations;

namespace QonversionUnity
{
    public class QonversionConfig
    {
        public readonly string ProjectKey;
        public readonly LaunchMode LaunchMode;
        public readonly Environment Environment;
        public readonly EntitlementsCacheLifetime EntitlementsCacheLifetime;
        [CanBeNull] public readonly string ProxyUrl;
        public readonly bool KidsMode = false;

        public QonversionConfig(
            string projectKey,
            LaunchMode launchMode,
            Environment environment,
            EntitlementsCacheLifetime entitlementsCacheLifetime,
            [CanBeNull] string proxyUrl,
            bool kidsMode
        )
        {
            ProjectKey = projectKey;
            LaunchMode = launchMode;
            Environment = environment;
            EntitlementsCacheLifetime = entitlementsCacheLifetime;
            ProxyUrl = proxyUrl;
            KidsMode = kidsMode;
        }
    }
}