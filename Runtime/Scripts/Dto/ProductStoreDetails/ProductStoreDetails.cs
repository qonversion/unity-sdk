using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class ProductStoreDetails
    {
        public readonly string BasePlanId;
        [CanBeNull] public readonly ProductOfferDetails DefaultOfferDetails;
        public readonly string Description;
        public readonly bool HasIntroOffer;
        public readonly bool HasTrialOffer;
        public readonly bool HasTrialOrIntroOffer;
        [CanBeNull] public readonly ProductInAppDetails InAppOfferDetails;
        public readonly string Name;
        public readonly string ProductId;
        public readonly QProductType ProductType;
        [CanBeNull] public readonly ProductOfferDetails SubscriptionOfferDetails;
        public readonly string Title;

        public ProductStoreDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("basePlanId", out object value)) BasePlanId = value as string;

            if (dict.TryGetValue("defaultOfferDetails", out value) && value is Dictionary<string, object> defaultOfferDetails)
            {
                DefaultOfferDetails = new ProductOfferDetails(defaultOfferDetails);
            }

            if (dict.TryGetValue("description", out value)) Description = value as string;
            if (dict.TryGetValue("hasIntroOffer", out value)) HasIntroOffer = (bool)value;
            if (dict.TryGetValue("hasTrialOffer", out value)) HasTrialOffer = (bool)value;
            if (dict.TryGetValue("hasTrialOrIntroOffer", out value)) HasTrialOrIntroOffer = (bool)value;

            if (dict.TryGetValue("inAppOfferDetails", out value) && value is Dictionary<string, object> inAppOfferDetails)
            {
                DefaultOfferDetails = new ProductInAppDetails(inAppOfferDetails);
            }

            if (dict.TryGetValue("name", out value)) Name = value as string;
            if (dict.TryGetValue("productId", out value)) ProductId = value as string;
            if (dict.TryGetValue("productType", out value)) ProductType = FormatType(value);

            if (dict.TryGetValue("subscriptionOfferDetails", out value) && value is Dictionary<string, object> subscriptionOfferDetails)
            {
                DefaultOfferDetails = new ProductOfferDetails(subscriptionOfferDetails);
            }

            if (dict.TryGetValue("title", out value)) Title = value as string;
        }

        private QProductType FormatType(object productType) =>
            (QProductType)Convert.ToInt32(productType);

        public override string ToString()
        {
            return $"{nameof(BasePlanId)}: {BasePlanId}, " +
                   $"{nameof(DefaultOfferDetails)}: {DefaultOfferDetails.ToString()}, " +
                   $"{nameof(Description)}: {Description}, " +
                   $"{nameof(HasIntroOffer)}: {HasIntroOffer}, " +
                   $"{nameof(HasTrialOffer)}: {HasTrialOffer}, " +
                   $"{nameof(HasTrialOrIntroOffer)}: {HasTrialOrIntroOffer}, " +
                   $"{nameof(InAppOfferDetails)}: {InAppOfferDetails.ToString()}, " +
                   $"{nameof(Name)}: {Name}, " +
                   $"{nameof(ProductId)}: {ProductId}, " +
                   $"{nameof(ProductType)}: {ProductType}, " +
                   $"{nameof(SubscriptionOfferDetails)}: {SubscriptionOfferDetails.ToString()}, " +
                   $"{nameof(Title)}: {Title}";
        }
    }
}