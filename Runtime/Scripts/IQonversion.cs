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
        /// Make a purchase and validate it through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="productId">Qonversion product identifier for purchase.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#1-make-a-purchase"/>
        public void Purchase(string productId, Qonversion.OnPurchaseResultReceived callback);

        /// <summary>
        /// Make a purchase and validate it through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="product">Qonversion product for purchase.</param>
        /// <param name="callback">Callback that will be called when response is received.</param>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#1-make-a-purchase"/>
        public void PurchaseProduct([NotNull] Product product, Qonversion.OnPurchaseResultReceived callback);

        /// <summary>
        /// Update (upgrade/downgrade) subscription and validate it through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="productId">Qonversion product identifier for purchase</param>
        /// <param name="oldProductId">Qonversion product identifier from which the upgrade/downgrade will be initialized</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        /// <param name="prorationMode">Proration Mode</param>
        /// <see href="https://developer.android.com/google/play/billing/subscriptions#proration">Proration Mode</see>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#3-update-purchases-android-only">Update Purchase</see>
        public void UpdatePurchase(
            string productId,
            string oldProductId,
            Qonversion.OnPurchaseResultReceived callback,
            ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy
        );

        /// <summary>
        /// Update (upgrade/downgrade) subscription and validate it through server-to-server using Qonversion's Backend.
        /// </summary>
        /// <param name="product">Qonversion product for purchase</param>
        /// <param name="oldProductId">Qonversion product identifier from which the upgrade/downgrade will be initialized</param>
        /// <param name="callback">Callback that will be called when response is received</param>
        /// <param name="prorationMode">Proration Mode</param>
        /// <see href="https://developer.android.com/google/play/billing/subscriptions#proration">Proration Mode</see>
        /// <see href="https://documentation.qonversion.io/docs/making-purchases#3-update-purchases-android-only">Update Purchase</see>
        public void UpdatePurchaseWithProduct(
            [NotNull] Product product,
            string oldProductId,
            Qonversion.OnPurchaseResultReceived callback,
            ProrationMode prorationMode = ProrationMode.UnknownSubscriptionUpgradeDowngradePolicy
        );

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
        /// <see href="https://qonversion.io/docs/offerings">Offerings</see>
        /// <see href="https://qonversion.io/docs/product-center">Product Center</see>
        public void Offerings(Qonversion.OnOfferingsReceived callback);

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
        /// Sets user property for pre-defined case property.
        ///
        /// User properties are attributes you can set on a user level.
        /// You can send user properties to third party platforms as well as use them in Qonversion for customer segmentation
        /// and analytics.
        /// </summary>
        /// <param name="key">Defined enum key that will be transformed to string.</param>
        /// <param name="value">Property value.</param>
        /// <see href="https://documentation.qonversion.io/docs/user-properties">User Properties</see>
        public void SetProperty(UserProperty key, string value);

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
        public void SetUserProperty(string key, string value);

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
