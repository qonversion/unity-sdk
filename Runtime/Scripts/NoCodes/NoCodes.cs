using System;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Main entry point for No-Codes SDK.
    /// Use this class to initialize and access No-Codes functionality.
    /// </summary>
    public static class NoCodes
    {
        [CanBeNull] private static INoCodes _backingInstance;

        /// <summary>
        /// Use this variable to get a current initialized instance of the No-Codes SDK.
        /// Please, use the property only after calling <see cref="Initialize"/>.
        /// Otherwise, trying to access the variable will cause an error.
        /// </summary>
        /// <returns>Current initialized instance of the No-Codes SDK.</returns>
        /// <exception cref="Exception">Throws exception if No-Codes has not been initialized.</exception>
        public static INoCodes GetSharedInstance()
        {
            if (_backingInstance == null)
            {
                throw new Exception("NoCodes has not been initialized. " +
                                    "You should call the Initialize method before accessing the shared instance of NoCodes.");
            }

            return _backingInstance;
        }

        /// <summary>
        /// An entry point to use No-Codes SDK. Call to initialize No-Codes SDK with required configs.
        /// </summary>
        /// <param name="config">A config that contains key SDK settings.
        /// Call <see cref="NoCodesConfigBuilder.Build"/> to configure and create a <see cref="NoCodesConfig"/> instance.</param>
        /// <returns>Initialized instance of the No-Codes SDK.</returns>
        public static INoCodes Initialize(NoCodesConfig config)
        {
            _backingInstance = NoCodesInternal.CreateInstance(config);
            return _backingInstance;
        }
    }
}
