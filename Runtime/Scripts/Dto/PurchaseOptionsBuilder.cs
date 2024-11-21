using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class PurchaseOptionsBuilder {
        [CanBeNull] private string _offerId;
        private bool _applyOffer = true;
        [CanBeNull] private Product _oldProduct;
        [CanBeNull] private PurchaseUpdatePolicy? _updatePolicy;
        [CanBeNull] private List<string> _contextKeys;
        private int _quantity = 1;
        [CanBeNull] private PromotionalOffer _promotionalOffer;

        /// <summary>
        /// iOS only.
        /// Set quantity of product purchasing. Use for consumable in-app products.
        /// </summary>
        /// <param name="quantity">Quantity of product purchasing.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetQuantity(int quantity)
        {
            this._quantity = quantity;
            return this;
        }

        /// <summary>
        /// Android only.
        /// Set offer for the purchase.
        /// </summary>
        /// <param name="offer">Concrete offer which you'd like to purchase.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetOffer(ProductOfferDetails offer)
        {
            _offerId = offer.OfferId;
            return this;
        }

        /// <summary>
        /// Android only.
        /// Set the offer ID to the purchase.
        /// </summary>
        /// <param name="offerId">Concrete offer ID which you'd like to purchase.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetOfferId(string offerId)
        {
            _offerId = offerId;
            return this;
        }

        /// <summary>
        /// Android only.
        /// Call this function to remove any intro/trial offer from the purchase (use only a bare base plan).
        /// </summary>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder RemoveOffer()
        {
            _applyOffer = false;
            return this;
        }

        /// <summary>
        /// Android only.
        /// Set Qonversion product from which the upgrade/downgrade will be initialized.
        /// </summary>
        /// <param name="oldProduct">Qonversion product from which the upgrade/downgrade will be initialized.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetOldProduct(Product oldProduct)
        {
            _oldProduct = oldProduct;
            return this;
        }


        /// <summary>
        /// Android only.
        /// Set the update policy for the purchase.
        /// </summary>
        /// <param name="updatePolicy">Update policy for the purchase.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetUpdatePolicy(PurchaseUpdatePolicy updatePolicy)
        {
            _updatePolicy = updatePolicy;
            return this;
        }

        /// <summary>
        /// Set the context keys associated with a purchase.
        /// </summary>
        /// <param name="contextKeys">Context keys for the purchase.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetContextKeys(List<string> contextKeys)
        {
            _contextKeys = contextKeys;
            return this;
        }

        /// <summary>
        /// Set the promotional offer details.
        /// </summary>
        /// <param name="promoOffer">Promotional offer details.</param>
        /// <returns>Builder instance for chain calls.</returns>
        public PurchaseOptionsBuilder SetPromotionalOffer(PromotionalOffer promoOffer)
        {
            _promotionalOffer = promoOffer;
            return this;
        }

        /// <summary>
        /// Generate a <see cref="PurchaseOptions"/> instance with all the provided options.
        /// </summary>
        /// <returns>The complete <see cref="PurchaseOptions"/> instance.</returns>
        public PurchaseOptions Build()
        {
            return new PurchaseOptions(_offerId, _applyOffer, _oldProduct, _updatePolicy, _contextKeys, _quantity, _promotionalOffer);
        }
    }
}