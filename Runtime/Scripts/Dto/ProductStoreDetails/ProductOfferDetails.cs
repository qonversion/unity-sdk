//offerId: string | null;
//offerToken: string;
//tags: string[];
//trialPhase: ProductPricingPhase | null;

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class ProductOfferDetails
    {
        public readonly string BasePlanId;
        public readonly bool HasIntro;
        public readonly bool HasTrial;
        public readonly bool HasTrialOrIntro;
        [CanBeNull] public readonly ProductPricingPhase IntroPhase;
        public readonly ProductPricingPhase[] PricingPhases;
        public readonly string OfferId;
        public readonly string OfferToken;
        public readonly string[] Tags;
        [CanBeNull] public readonly ProductPricingPhase TrialPhase;

        public ProductOfferDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("basePlanId", out object value)) BasePlanId = value as string;

            if (dict.TryGetValue("hasIntro", out value)) HasIntro = (bool)value;
            if (dict.TryGetValue("hasTrial", out value)) HasTrial = (bool)value;
            if (dict.TryGetValue("hasTrialOrIntro", out value)) HasTrialOrIntro = (bool)value;

            if (dict.TryGetValue("introPhase", out value) && value is Dictionary<string, object> introPhase)
            {
                IntroPhase = new ProductPricingPhase(introPhase);
            }

            if (dict.TryGetValue("pricingPhases", out value) && value is object[] pricingPhases)
            {
                var result = new List<ProductPricingPhase>();
                foreach (Dictionary<string, object> pricingPhase in pricingPhases)
                {
                    if (pricingPhase is Dictionary<string, object>)
                    {
                        ProductPricingPhase phase = new ProductPricingPhase(pricingPhase);
                        result.Add(phase);
                    }
                }

                PricingPhases = result.ToArray();
            }

            if (dict.TryGetValue("offerId", out value)) OfferId = value as string;
            if (dict.TryGetValue("offerToken", out value)) OfferToken = value as string;
            if (dict.TryGetValue("tags", out value)) Tags = value as string[];


            if (dict.TryGetValue("trialPhase", out value) && value is Dictionary<string, object> trialPhase)
            {
                TrialPhase = new ProductPricingPhase(trialPhase);
            }
        }

        private QProductType FormatType(object productType) =>
            (QProductType)Convert.ToInt32(productType);

        public override string ToString()
        {
            return $"{nameof(BasePlanId)}: {BasePlanId}, " +
                   $"{nameof(HasIntro)}: {HasIntro}, " +
                   $"{nameof(HasTrial)}: {HasTrial}, " +
                   $"{nameof(HasTrialOrIntro)}: {HasTrialOrIntro}, " +
                   $"{nameof(IntroPhase)}: {IntroPhase.ToString()}, " +
                   $"{nameof(PricingPhases)}: {PricingPhases}, " +
                   $"{nameof(OfferId)}: {OfferId}, " +
                   $"{nameof(OfferToken)}: {OfferToken}, " +
                   $"{nameof(Tags)}: {Tags}, " +
                   $"{nameof(TrialPhase)}: {TrialPhase.ToString()}";
        }
    }
}