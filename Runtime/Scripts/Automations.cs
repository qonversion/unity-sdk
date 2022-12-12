using System;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public static class Automations
    {
        [CanBeNull] private static IAutomations _backingInstance;

        /// <summary>
        /// Use this variable to get a current initialized instance of the Qonversion Automations.
        /// Please, use Automations only after calling <see cref="Qonversion.Initialize"/>.
        /// Otherwise, trying to access the variable will cause an error.
        /// </summary>
        /// <returns>Current initialized instance of the Qonversion Automations.</returns>
        /// <exception cref="Exception">throws exception if Qonversion has not been initialized.</exception>
        public static IAutomations GetSharedInstance()
        {
            if (_backingInstance == null)
            {
                try
                {
                    Qonversion.GetSharedInstance();
                }
                catch (Exception e)
                {
                    throw new Exception("Qonversion has not been initialized. " + 
                                        "Automations should be used after Qonversion is initialized.");
                }

                _backingInstance = AutomationsInternal.CreateInstance();
            }

            return _backingInstance;
        }

        public delegate void OnShowScreenResponseReceived(QonversionError error);
    }
}
