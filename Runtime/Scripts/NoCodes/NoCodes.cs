using System;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Main entry point for NoCodes SDK functionality.
    /// </summary>
    public static class NoCodes
    {
        [CanBeNull] private static volatile INoCodes _backingInstance;
        private static object _syncRoot = new object();

        /// <summary>
        /// Use this method to get the current initialized instance of the NoCodes SDK.
        /// Please, use this method only after calling <see cref="NoCodes.Initialize"/>.
        /// Otherwise, trying to access the instance will cause an exception.
        /// </summary>
        /// <returns>Current initialized instance of the NoCodes SDK.</returns>
        /// <exception cref="Exception">Throws exception if the instance has not been initialized.</exception>
        public static INoCodes GetSharedInstance()
        {
            if (_backingInstance == null)
            {
                throw new Exception(
                    "NoCodes has not been initialized. You should call " +
                    "the Initialize method before accessing the shared instance of NoCodes."
                );
            }

            return _backingInstance;
        }

        /// <summary>
        /// An entry point to use NoCodes SDK. Call to initialize NoCodes SDK with configuration.
        /// </summary>
        /// <param name="config">Configuration that contains key SDK settings.
        /// Call <see cref="NoCodesConfigBuilder.Build"/> to configure and create a NoCodesConfig instance.</param>
        /// <returns>Initialized instance of the NoCodes SDK.</returns>
        public static INoCodes Initialize(NoCodesConfig config)
        {
            if (_backingInstance == null)
            {
                lock (_syncRoot)
                {
                    if (_backingInstance == null)
                    {
                        INoCodes instance = NoCodesInternal.CreateInstance();
                        instance.InitializeInstance(config);

                        _backingInstance = instance;
                    }
                }
            }

            return _backingInstance;
        }

        /// <summary>
        /// Simplified initialization method that only requires a project key.
        /// </summary>
        /// <param name="projectKey">Qonversion project key.</param>
        /// <returns>Initialized instance of the NoCodes SDK.</returns>
        public static INoCodes InitializeWithProjectKey(string projectKey)
        {
            var config = new NoCodesConfig(projectKey);
            return Initialize(config);
        }

        /// <summary>
        /// Event fired when any NoCodes event occurs.
        /// </summary>
        public static event Action<NoCodesEvent> EventReceived
        {
            add
            {
                if (_backingInstance != null)
                {
                    _backingInstance.EventReceived += value;
                }
            }
            remove
            {
                if (_backingInstance != null)
                {
                    _backingInstance.EventReceived -= value;
                }
            }
        }

        /// <summary>
        /// Sets the delegate for handling NoCodes events.
        /// </summary>
        /// <param name="delegate">Delegate instance for handling NoCodes events.</param>
        public static void SetDelegate(NoCodesDelegate @delegate)
        {
            if (_backingInstance != null)
            {
                _backingInstance.SetDelegate(@delegate);
            }
        }
    }
}

