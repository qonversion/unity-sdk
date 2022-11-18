namespace QonversionUnity
{
    public class QonversionConfig
    {
        public readonly string ProjectKey;
        public readonly LaunchMode LaunchMode;
        public readonly Environment Environment;
        public readonly EntitlementsCacheLifetime EntitlementsCacheLifetime;

        public QonversionConfig(string projectKey, LaunchMode launchMode, Environment environment, EntitlementsCacheLifetime entitlementsCacheLifetime)
        {
            ProjectKey = projectKey;
            LaunchMode = launchMode;
            Environment = environment;
            EntitlementsCacheLifetime = entitlementsCacheLifetime;
        }
    }
}