using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// This class contains all the information about the Google subscription offer details.
    /// It might be either a plain base plan details or a base plan with the concrete offer details.
    /// </summary>
    public class ProductOfferDetails
    {
        /// The identifier of the current base plan.
        public readonly string BasePlanId;
        
        /// The identifier of the concrete offer, to which these details belong.
        /// Null, if these are plain base plan details.
        [CanBeNull] public readonly string OfferId;
        
        /// A token to purchase the current offer.
        public readonly string OfferToken;

        /// List of tags set for the current offer.
        public readonly string[] Tags;

        /// A time-ordered list of pricing phases for the current offer.
        public readonly ProductPricingPhase[] PricingPhases;

        /// A base plan phase details.
        [CanBeNull] public readonly ProductPricingPhase BasePlan;
        
        /// Additional details of an installment plan, if exists.
        [CanBeNull] public readonly ProductInstallmentPlanDetails InstallmentPlanDetails;
        
        /// A trial phase details, if exists.
        [CanBeNull] public readonly ProductPricingPhase IntroPhase;
        
        /// An intro phase details, if exists.
        /// The intro phase is one of single or recurrent discounted payments.
        [CanBeNull] public readonly ProductPricingPhase TrialPhase;

        /// True, if there is a trial phase in the current offer. False otherwise.
        public readonly bool HasTrial;
        
        /// True, if there is any intro phase in the current offer. False otherwise.
        /// The intro phase is one of single or recurrent discounted payments.
        public readonly bool HasIntro;
        
        /// True, if there is any trial or intro phase in the current offer. False otherwise.
        /// The intro phase is one of single or recurrent discounted payments.
        public readonly bool HasTrialOrIntro;

        public ProductOfferDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("basePlanId", out object value)) BasePlanId = value as string;
            if (dict.TryGetValue("offerId", out value)) OfferId = value as string;
            if (dict.TryGetValue("offerToken", out value)) OfferToken = value as string;
            if (dict.TryGetValue("tags", out value) && value is List<object> tags) {
                List<string> result = new List<string>();
                foreach (object tag in tags)
                {
                    result.Add(tag.ToString());
                }

                Tags = result.ToArray();
            }

            if (dict.TryGetValue("pricingPhases", out value) && value is List<object> pricingPhases)
            {
                List<ProductPricingPhase> result = new List<ProductPricingPhase>();
                foreach (object pricingPhase in pricingPhases)
                {
                    if (pricingPhase is Dictionary<string, object>)
                    {
                        ProductPricingPhase phase = new ProductPricingPhase(pricingPhase as Dictionary<string, object>);
                        result.Add(phase);
                    }
                }

                PricingPhases = result.ToArray();
            }
            
            if (dict.TryGetValue("basePlan", out value) && value is Dictionary<string, object> basePlan)
            {
                BasePlan = new ProductPricingPhase(basePlan);
            }
            
            if (dict.TryGetValue("installmentPlanDetails", out value) && value is Dictionary<string, object> installmentPlan)
            {
                InstallmentPlanDetails = new ProductInstallmentPlanDetails(installmentPlan);
            }

            if (dict.TryGetValue("introPhase", out value) && value is Dictionary<string, object> introPhase)
            {
                IntroPhase = new ProductPricingPhase(introPhase);
            }
            
            if (dict.TryGetValue("trialPhase", out value) && value is Dictionary<string, object> trialPhase)
            {
                TrialPhase = new ProductPricingPhase(trialPhase);
            }

            if (dict.TryGetValue("hasTrial", out value)) HasTrial = (bool)value;
            if (dict.TryGetValue("hasIntro", out value)) HasIntro = (bool)value;
            if (dict.TryGetValue("hasTrialOrIntro", out value)) HasTrialOrIntro = (bool)value;
        }

        public override string ToString()
        {
            string tags = "";
            if (Tags != null)
            {
                tags = string.Join(", ", Tags.Select(tag => tag));
            }

            string pricingPhases = "";
            if (PricingPhases != null)
            {
                pricingPhases = string.Join(", ", PricingPhases.Select(phase => phase.ToString()));
            }
            
            return $"{nameof(BasePlanId)}: {BasePlanId}, " +
                   $"{nameof(OfferId)}: {OfferId}, " +
                   $"{nameof(OfferToken)}: {OfferToken}, " +
                   $"{nameof(Tags)}: {tags}, " +
                   $"{nameof(PricingPhases)}: {pricingPhases}, " +
                   $"{nameof(BasePlan)}: {BasePlan}, " +
                   $"{nameof(InstallmentPlanDetails)}: {InstallmentPlanDetails}, " +	
                   $"{nameof(IntroPhase)}: {IntroPhase}, " +
                   $"{nameof(TrialPhase)}: {TrialPhase}, " +
                   $"{nameof(HasTrial)}: {HasTrial}, " +
                   $"{nameof(HasIntro)}: {HasIntro}, " +
                   $"{nameof(HasTrialOrIntro)}: {HasTrialOrIntro}";
        }
    }
}
