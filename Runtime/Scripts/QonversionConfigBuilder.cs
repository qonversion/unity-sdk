namespace QonversionUnity
{
    public class QonversionConfigBuilder
    {
        public readonly string ProjectKey;
        public readonly LaunchMode LaunchMode;
        public Environment Environment;
        public EntitlementsCacheLifetime EntitlementsCacheLifetime;

        public QonversionConfigBuilder(string projectKey, LaunchMode launchMode)
        {
            ProjectKey = projectKey;
            LaunchMode = launchMode;
        }
        
        /// <summary>
        /// Set current application <see cref="Environment"/>. Used to distinguish sandbox and production users.
        /// </summary>
        /// <param name="environment">current environment.</param>
        /// <returns>builder instance for chain calls.</returns>
        public QonversionConfigBuilder SetEnvironment(Environment environment)
        {
            Environment = environment;
            return this;
        }

        /// <summary>
        /// Entitlements cache is used when there are problems with the Qonversion API
        /// or internet connection. If so, Qonversion will return the last successfully loaded
        /// entitlements. The current method allows you to configure how long that cache may be used.
        /// The default value is <see cref="EntitlementsCacheLifetime.Month"/>.
        /// </summary>
        /// <param name="lifetime">desired entitlements cache lifetime duration.</param>
        /// <returns>builder instance for chain calls.</returns>
        public QonversionConfigBuilder SetEntitlementsCacheLifetime(EntitlementsCacheLifetime lifetime)
        {
            EntitlementsCacheLifetime = lifetime;
            return this;
        }

        /// <summary>
        /// Generate <see cref="QonversionConfig"/> instance with all the provided configurations.
        /// </summary>
        /// <returns>the complete <see cref="QonversionConfig"/> instance.</returns>
        public QonversionConfig Build()
        {
            return new QonversionConfig(
                ProjectKey,
                LaunchMode,
                Environment,
                EntitlementsCacheLifetime
            );
        }
    }
}