using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class PurchaseOptions
    {
        [CanBeNull] public readonly string OfferId;

        public readonly bool ApplyOffer;

        [CanBeNull] public readonly Product OldProduct;

        [CanBeNull] public readonly PurchaseUpdatePolicy? UpdatePolicy;

        [CanBeNull] public readonly List<string> ContextKeys;

        public readonly int Quantity;

        public PurchaseOptions(
            [CanBeNull] string offerId,
            bool applyOffer,
            [CanBeNull] Product oldProduct,
            [CanBeNull] PurchaseUpdatePolicy? updatePolicy,
            [CanBeNull] List<string> contextKeys,
            int quantity)
        {
            OfferId = offerId;
            ApplyOffer = applyOffer;
            OldProduct = oldProduct;
            UpdatePolicy = updatePolicy;
            ContextKeys = contextKeys;
            Quantity = quantity;
        }

        public override string ToString() {
            return $"{nameof(OfferId)}: {OfferId}, " +
                   $"{nameof(ApplyOffer)}: {ApplyOffer}, " +
                   $"{nameof(OldProduct)}: {OldProduct}, " +
                   $"{nameof(UpdatePolicy)}: {UpdatePolicy}, " +
                   $"{nameof(ContextKeys)}: {ContextKeys}";
        }
    }
}