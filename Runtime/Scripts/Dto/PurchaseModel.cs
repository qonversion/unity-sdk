using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Used to provide all the necessary purchase data to the <see cref="IQonversion.Purchase"/> method.
    /// Can be created manually or using the <see cref="Product.ToPurchaseModel"/> method.
    /// 
    /// If <see cref="OfferId"/> is not specified for Android, then the default offer will be applied.
    /// To know how we choose the default offer, see <see cref="ProductStoreDetails.DefaultSubscriptionOfferDetails"/>.
    ///
    /// If you want to remove any intro/trial offer from the purchase on Android (use only a bare base plan),
    /// call the <see cref="RemoveOffer"/> method.
    /// </summary>
    public class PurchaseModel
    {
        public readonly string ProductId;
        [CanBeNull] public string OfferId;

        internal bool ApplyOffer = true;

        public PurchaseModel(string productId, [CanBeNull] string offerId = null)
        {
            ProductId = productId;
            OfferId = offerId;
        }

        public PurchaseModel RemoveOffer()
        {
            ApplyOffer = false;
            return this;
        }
    }
}