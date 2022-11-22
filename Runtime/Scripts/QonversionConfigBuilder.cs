namespace QonversionUnity
{
    public class QonversionConfigBuilder
    {
        private readonly string _projectKey;
        private readonly LaunchMode _launchMode;
        private Environment _environment;
        private EntitlementsCacheLifetime _entitlementsCacheLifetime;

        public QonversionConfigBuilder(string projectKey, LaunchMode launchMode)
        {
            _projectKey = projectKey;
            _launchMode = launchMode;
        }
        
        /// <summary>
        /// Set current application <see cref="_environment"/>. Used to distinguish sandbox and production users.
        /// </summary>
        /// <param name="environment">current environment.</param>
        /// <returns>builder instance for chain calls.</returns>
        public QonversionConfigBuilder SetEnvironment(Environment environment)
        {
            _environment = environment;
            return this;
        }

        /// <summary>
        /// Entitlements cache is used when there are problems with the Qonversion API
        /// or internet connection. If so, Qonversion will return the last successfully loaded
        /// entitlements. The current method allows you to configure how long that cache may be used.
        /// The default value is <see cref="_entitlementsCacheLifetime.Month"/>.
        /// </summary>
        /// <param name="lifetime">desired entitlements cache lifetime duration.</param>
        /// <returns>builder instance for chain calls.</returns>
        public QonversionConfigBuilder SetEntitlementsCacheLifetime(EntitlementsCacheLifetime lifetime)
        {
            _entitlementsCacheLifetime = lifetime;
            return this;
        }

        /// <summary>
        /// Generate <see cref="QonversionConfig"/> instance with all the provided configurations.
        /// </summary>
        /// <returns>the complete <see cref="QonversionConfig"/> instance.</returns>
        public QonversionConfig Build()
        {
            return new QonversionConfig(
                _projectKey,
                _launchMode,
                _environment,
                _entitlementsCacheLifetime
            );
        }
    }
}