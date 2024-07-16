using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public interface IQonversion
    {
        /// <summary>
        /// This event will be fired when a user initiates a promotional in-app purchase from the App Store.
        /// Declare a delegate <see cref="Qonversion.OnPromoPurchasesReceived"/> for the event.
        /// If you are not using the PromoPurchasesReceived event promo purchases will proceed automatically.
        /// </summary>
        public event Qonversion.OnPromoPurchasesReceived PromoPurchasesReceived;

        /// <summary>
        /// This event will be fired for each asynchronous entitlements update,
        /// for example, when a deferred transaction happens.
        /// </summary>
        public event Qonversion.OnUpdatedEntitlementsReceived UpdatedEntitlementsReceived;

        /// <summary>
        /// Call this function to sync the subscriber data with the first launch when Qonversion is implemented.
        /// </summary>
        public void SyncHistoricalData();

        /// <summary>
        /// This method works for iOS only.
        /// Contact us before you start using this function.
        /// Call this function to sync purchases if you are using StoreKit2.
        /// </summary>
        public void SyncStoreKit2Purchases();

        /// <summary>
        /// Make a purchase and validate it through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="purchaseModel">Necessary information for purchase.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases">Making Purchases</see>
        public void Purchase(PurchaseModel purchaseModel, Qonversion.OnPurchaseResultReceived callback);

        /// <summary>
        /// Android only. Returns `null` if called on iOS.
        /// 
        /// Update (upgrade/downgrade) subscription on Google Play Store and validate it through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="purchaseUpdateModel">Necessary information for purchase update.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://developer.android.com/google/play/billing/subscriptions#replacement-modes">Update policy</see>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases">Making Purchases</see>
        public void UpdatePurchase(PurchaseUpdateModel purchaseUpdateModel, Qonversion.OnPurchaseResultReceived callback);

        /// <summary>
        /// Returns Qonversion products in association with Apple and Google Play Store Products.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://qonversion.io/docs/product-center">Product Center</see>
        public void Products(Qonversion.OnProductsReceived callback);

        /// <summary>
        /// Return Qonversion Offerings Object.
        ///
        /// An offering is a group of products that you can offer to a user on a given paywall based on your business logic.
        /// For example, you can offer one set of products on a paywall immediately after onboarding and another
        /// set of products with discounts later on if a user has not converted.
        /// Offerings allow changing the products offered remotely without releasing app updates.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://qonversion.io/docs/offerings">Offerings</see>
        /// <see href="https://qonversion.io/docs/product-center">Product Center</see>
        public void Offerings(Qonversion.OnOfferingsReceived callback);

        /// <summary>
        /// Returns default Qonversion remote config object
        /// Use this function to get the remote config with specific payload and experiment info.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/remote-config">Remote Configs</see>
        public void RemoteConfig(Qonversion.OnRemoteConfigReceived callback);

        /// <summary>
        /// Returns Qonversion remote config object by {@link contextKey}
        /// Use this function to get the remote config with specific payload and experiment info.
        /// </summary>
        /// <param name="contextKey">Context key to get remote config for.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/remote-config">Remote Configs</see>
        public void RemoteConfig(string contextKey, Qonversion.OnRemoteConfigReceived callback);

        /// <summary>
        /// Returns Qonversion remote config objects for all existing context key (including empty one).
        /// Use this function to get the remote configs with specific payload and experiment info.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/remote-config">Remote Configs</see>
        public void RemoteConfigList(Qonversion.OnRemoteConfigListReceived callback);
        
        /// <summary>
        /// Returns Qonversion remote config objects by a list of context keys.
        /// Use this function to get the remote configs with specific payload and experiment info.
        /// </summary>
        /// <param name="contextKeys">List of context keys to load remote configs for</param>
        /// <param name="includeEmptyContextKey">Set to true if you want to include remote config with empty context key to the result.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/remote-config">Remote Configs</see>
        public void RemoteConfigList(string[] contextKeys, bool includeEmptyContextKey, Qonversion.OnRemoteConfigListReceived callback);

        /// <summary>
        /// This function should be used for the test purposes only. Do not forget to delete the usage of this function before the release.
        /// Use this function to attach the user to the experiment.
        /// </summary>
        /// <param name="experimentId">Experiment identifier</param>
        /// <param name="groupId">Group identifier</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        public void AttachUserToExperiment(string experimentId, string groupId, Qonversion.OnAttachUserResponseReceived callback);

        /// <summary>
        /// This function should be used for the test purposes only. Do not forget to delete the usage of this function before the release.
        /// Use this function to detach the user from the experiment.
        /// </summary>
        /// <param name="experimentId">Experiment identifier</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        public void DetachUserFromExperiment(string experimentId, Qonversion.OnAttachUserResponseReceived callback);

        /// <summary>
        /// This function should be used for the test purposes only. Do not forget to delete the usage of this function before the release.
        /// Use this function to attach the user to the remote configuration.
        /// </summary>
        /// <param name="remoteConfigurationId">Remote configurationidentifier</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        public void AttachUserToRemoteConfiguration(string remoteConfigurationId, Qonversion.OnAttachUserResponseReceived callback);

        /// <summary>
        /// This function should be used for the test purposes only. Do not forget to delete the usage of this function before the release.
        /// Use this function to detach the user from the remote configuration.
        /// </summary>
        /// <param name="remoteConfigurationId">Remote configuration identifier</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        public void DetachUserFromRemoteConfiguration(string remoteConfigurationId, Qonversion.OnAttachUserResponseReceived callback);

        /// <summary>
        /// Call this function to check if the fallback file is accessible.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received.</param>
        public void IsFallbackFileAccessible(Qonversion.OnFallbackFileAccessibilityResponseReceived callback);

        /// <summary>
        /// You can check if a user is eligible for an introductory offer, including a free trial.
        /// You can show only a regular price for users who are not eligible for an introductory offer.
        /// </summary>
        /// <param name="productIds">Products identifiers that must be checked.</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        public void CheckTrialIntroEligibility(IList<string> productIds, Qonversion.OnEligibilitiesReceived callback);

        /// <summary>
        /// You need to call the CheckEntitlements method at the start of your app to check if a user has the required
        /// entitlement.
        ///
        /// This method will check the user receipt and will return the current entitlements.
        ///
        /// If Apple or Google servers are not responding at the time of the request, Qonversion provides the latest
        /// entitlements data from its database.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received</param>
        public void CheckEntitlements(Qonversion.OnEntitlementsReceived callback);

        /// <summary>
        /// Restoring purchases restores users purchases in your app, to maintain access to purchased content.
        /// Users sometimes need to restore purchased content, such as when they upgrade to a new phone.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received</param>
        public void Restore(Qonversion.OnEntitlementsReceived callback);

        /// <summary>
        /// This method will send all purchases to the Qonversion backend. Call this every time when purchase is handled
        /// by your own implementation.
        ///
        /// //////Warning!//////
        ///
        /// This method works for Android only.
        /// It should only be called if you're using Qonversion SDK in observer mode.
        /// </summary>
        /// <see href="https://documentation.qonversion.io/docs/observer-mode#android-sdk">Observer mode for Android SDK</see>
        public void SyncPurchases();

        /// <summary>
        /// Call this function to link a user to his unique ID in your system and share purchase data.
        /// </summary>
        /// <param name="userID">An unique user ID in your system.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-identifiers#3-user-identity"/>
        public void Identify(string userID);

        /// <summary>
        /// Call this function to link a user to his unique ID in your system and share purchase data.
        /// </summary>
        /// <param name="userID">An unique user ID in your system.</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        /// <see href="https://documentation.qonversion.io/docs/user-identifiers#3-user-identity"/>
        public void Identify(string userID, Qonversion.OnUserInfoReceived callback);

        /// <summary>
        /// Call this function to unlink a user from his unique ID in your system and his purchase data.
        /// </summary>
        /// <see href="https://documentation.qonversion.io/docs/user-identifiers#logging-out"/>
        public void Logout();

        /// <summary>
        /// This method returns information about the current Qonversion user.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received</param>
        public void UserInfo(Qonversion.OnUserInfoReceived callback);

        /// <summary>
        /// Sends your attribution data to the attribution source.
        /// </summary>
        /// <param name="conversionData">An object containing your attribution data.</param>
        /// <param name="attributionProvider">The attribution source to which the data will be sent.</param>
        public void Attribution(
            Dictionary<string, object> conversionData,
            AttributionProvider attributionProvider
        );

        /// <summary>
        /// Sends your attribution data to the attribution source.
        /// </summary>
        /// <param name="conversionData">A json string containing your attribution data.</param>
        /// <param name="attributionProvider">The attribution source to which the data will be sent.</param>
        public void Attribution(string conversionData, AttributionProvider attributionProvider);

        /// <summary>
        /// Sets Qonversion reserved user properties, like email or user id.
        ///
        /// User properties are attributes you can set on a user level.
        /// You can send user properties to third party platforms as well as use them in Qonversion for customer segmentation
        /// and analytics.
        ///
        /// Note that using <see cref="UserPropertyKey.Custom"/> here will do nothing.
        /// To set custom user property, use <see cref="SetCustomUserProperty"/> method instead.
        /// </summary>
        /// <param name="key">Defined enum key that will be transformed to string.</param>
        /// <param name="value">Property value.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-properties">User Properties</see>
        public void SetUserProperty(UserPropertyKey key, string value);

        /// <summary>
        /// Adds custom user property.
        ///
        /// User properties are attributes you can set on a user level.
        /// You can send user properties to third party platforms as well as use them in Qonversion for customer segmentation
        /// and analytics.
        /// </summary>
        /// <param name="key">Custom user property key.</param>
        /// <param name="value">Property value.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-properties">User Properties</see>
        public void SetCustomUserProperty(string key, string value);

        /// <summary>
        /// This method returns all the properties, set for the current Qonversion user.
        /// All set properties are sent to the server with delay, so if you call
        /// this function right after setting some property, it may not be included
        /// in the result.
        /// </summary>
        /// <param name="callback">Callback that will be called when response is received</param>
        public void UserProperties(Qonversion.OnUserPropertiesReceived callback);

        /// <summary>
        /// iOS only.
        /// On iOS 14.5+, after requesting the app tracking entitlement using ATT, you need to notify Qonversion if tracking
        /// is allowed and IDFA is available.
        /// </summary>
        public void CollectAdvertisingId();

        /// <summary>
        /// Collecting Apple Search Ads attribution data.
        /// </summary>
        /// <see href="https://documentation.qonversion.io/docs/apple-search-ads#enable-data-collection"/>
        public void CollectAppleSearchAdsAttribution();

        /// <summary>
        /// iOS only.
        /// On iOS 14.0+ shows up a sheet for users to redeem AppStore offer codes.
        /// </summary>
        public void PresentCodeRedemptionSheet();

        internal void InitializeInstance(QonversionConfig config);
    }
}
