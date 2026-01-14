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
