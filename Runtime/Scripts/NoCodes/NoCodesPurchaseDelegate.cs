using System;

namespace QonversionUnity
{
    /// <summary>
    /// Delegate interface for custom purchase and restore handling in No-Codes.
    /// When this delegate is provided, it replaces the default Qonversion SDK purchase flow,
    /// allowing you to use your own purchase system (e.g., RevenueCat, custom backend, etc.).
    /// 
    /// Implement this interface and pass it to <see cref="NoCodesConfigBuilder.SetPurchaseDelegate"/>
    /// to handle purchases initiated from No-Code screens with your own logic.
    /// 
    /// <para>
    /// <b>Android Warning:</b> This delegate does NOT work on Android. When a No-Codes screen is displayed,
    /// Unity's game loop is paused because the No-Codes screen runs as a separate Activity on top of Unity's Activity.
    /// As a result, the <see cref="Purchase"/> and <see cref="Restore"/> methods will not be called while
    /// the No-Codes screen is active. Use this delegate only on iOS, or use the default Qonversion SDK
    /// purchase flow on Android.
    /// </para>
    /// </summary>
    public interface NoCodesPurchaseDelegate
    {
        /// <summary>
        /// Handle purchase for the given product.
        /// 
        /// This method is called when a purchase is initiated from a No-Code screen.
        /// Implement your custom purchase logic here.
        /// 
        /// Call onSuccess when the purchase finishes successfully,
        /// or onError with an error message if the purchase fails.
        /// </summary>
        /// <param name="product">The product to purchase.</param>
        /// <param name="onSuccess">Callback to invoke when purchase succeeds.</param>
        /// <param name="onError">Callback to invoke when purchase fails, with error message.</param>
        void Purchase(Product product, Action onSuccess, Action<string> onError);

        /// <summary>
        /// Handle restore flow.
        /// 
        /// This method is called when a restore is initiated from a No-Code screen.
        /// Implement your custom restore logic here.
        /// 
        /// Call onSuccess when the restore finishes successfully,
        /// or onError with an error message if the restore fails.
        /// </summary>
        /// <param name="onSuccess">Callback to invoke when restore succeeds.</param>
        /// <param name="onError">Callback to invoke when restore fails, with error message.</param>
        void Restore(Action onSuccess, Action<string> onError);
    }
}
