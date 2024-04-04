using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public static class Qonversion
    {
        [CanBeNull] private static volatile IQonversion _backingInstance;
        private static object _syncRoot = new Object();

        /// <summary>
        /// Use this variable to get a current initialized instance of the Qonversion SDK.
        /// Please, use the property only after calling <see cref="Qonversion.Initialize"/>.
        /// Otherwise, trying to access the variable will cause an exception.
        /// </summary>
        /// <returns>Current initialized instance of the Qonversion SDK.</returns>
        /// <exception cref="Exception">throws exception if the instance has not been initialized</exception>
        public static IQonversion GetSharedInstance()
        {
            if (_backingInstance == null)
            {
                throw new Exception(
                    "Qonversion has not been initialized. You should call " +
                    "the initialize method before accessing the shared instance of Qonversion."
                );
            }

            return _backingInstance;
        }

        /// <summary>
        /// An entry point to use Qonversion SDK. Call to initialize Qonversion SDK with required and extra configs.
        /// The function is the best way to set additional configs you need to use Qonversion SDK.
        /// You still have an option to set a part of additional configs later via calling separate setters.
        /// </summary>
        /// <param name="config">a config that contains key SDK settings.
        /// Call <see cref="QonversionConfigBuilder.Build"/> to configure and create a QonversionConfig instance.</param>
        /// <returns>Initialized instance of the Qonversion SDK.</returns>
        public static IQonversion Initialize(QonversionConfig config)
        {
            if (_backingInstance == null)
            {
                lock (_syncRoot)
                {
                    if (_backingInstance == null)
                    {
                        IQonversion instance = QonversionInternal.CreateInstance();
                        instance.InitializeInstance(config);

                        _backingInstance = instance;
                    }
                }
            }

            return _backingInstance;
        }

        public delegate void OnPurchaseResultReceived(Dictionary<string, Entitlement> entitlements, QonversionError error, bool isCancelled);
        public delegate void OnEntitlementsReceived(Dictionary<string, Entitlement> entitlements, QonversionError error);
        public delegate void OnProductsReceived(Dictionary<string, Product> products, QonversionError error);
        public delegate void OnOfferingsReceived(Offerings offerings, QonversionError error);
        public delegate void OnRemoteConfigReceived(RemoteConfig remoteConfig, QonversionError error);
        public delegate void OnRemoteConfigListReceived(RemoteConfigList remoteConfigList, QonversionError error);
        public delegate void OnUserPropertiesReceived(UserProperties userProperties, QonversionError error);
        public delegate void OnAttachUserResponseReceived(bool success, QonversionError error);
        public delegate void OnEligibilitiesReceived(Dictionary<string, Eligibility> eligibilities, QonversionError error);
        public delegate void OnUserInfoReceived(User userInfo, QonversionError error);

        /// <summary>
        /// Delegate fires each time a promo purchase from the App Store happens.
        /// Be sure you define a delegate for the event <see cref="IQonversion.PromoPurchasesReceived"/>. 
        /// Call StartPromoPurchase in case of your app is ready to start promo purchase. 
        /// Or cache that delegate and call later when you need.
        /// </summary>
        /// <param name="productId">StoreKit product identifier</param>
        /// <param name="purchaseDelegate">A delegate that will start a promo purchase flow.
        /// <see cref="StartPromoPurchase"/>
        /// </param>
        public delegate void OnPromoPurchasesReceived(string productId, StartPromoPurchase purchaseDelegate);

        /// <summary>
        /// Call the function if your app can handle a promo purchase at the current time.
        /// Or you can cache the delegate, and call it when the app is ready to make the purchase.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received. Returns entitlements or potentially a QonversionError.
        /// <see cref="Qonversion.OnEntitlementsReceived"/>
        /// </param>
        public delegate void StartPromoPurchase(OnEntitlementsReceived callback);
        
        /// <summary>
        /// Delegate fires each time a user entitlements change asynchronously,
        /// for example, when pending purchases like SCA, Ask to buy, etc., happen.
        /// </summary>
        public delegate void OnUpdatedEntitlementsReceived(Dictionary<string, Entitlement> entitlements);
    }
}
