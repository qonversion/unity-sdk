using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Used to provide all the necessary purchase data to the <see cref="IQonversion.UpdatePurchase"/> method.
    /// Can be created manually or using the <see cref="Product.ToPurchaseUpdateModel"/> method.
    ///
    /// Requires Qonversion product identifiers - <see cref="ProductId"/> for the purchasing one and
    /// <see cref="OldProductId"/> for the purchased one.
    ///
    /// If <see cref="OfferId"/> is not specified for Android, then the default offer will be applied.
    /// To know how we choose the default offer, see <see cref="ProductStoreDetails.DefaultSubscriptionOfferDetails"/>.
    ///
    /// If you want to remove any intro/trial offer from the purchase on Android (use only bare base plan),
    /// call the <see cref="RemoveOffer"/> method.
    /// </summary>
    public class PurchaseUpdateModel
    {
        public readonly string ProductId;
        public readonly string OldProductId;
        [CanBeNull] public PurchaseUpdatePolicy? UpdatePolicy;
        [CanBeNull] public string OfferId;

        internal bool ApplyOffer = true;

        public PurchaseUpdateModel(string productId, string oldProductId, [CanBeNull] PurchaseUpdatePolicy? updatePolicy = null, [CanBeNull] string offerId = null)
        {
            ProductId = productId;
            OldProductId = oldProductId;
            UpdatePolicy = updatePolicy;
            OfferId = offerId;
        }

        public PurchaseUpdateModel RemoveOffer()
        {
            ApplyOffer = false;
            return this;
        }
    }
}