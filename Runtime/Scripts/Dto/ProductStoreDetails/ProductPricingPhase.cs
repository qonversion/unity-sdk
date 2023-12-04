using System.Collections.Generic;

namespace QonversionUnity
{
    /// <summary>
    /// This class represents a pricing phase, describing how a user pays at a point in time.
    /// </summary>
    public class ProductPricingPhase
    {
        /// Price for the current phase.
        public readonly ProductPrice Price;
       
        /// Billing period for which the given price applies.
        public readonly SubscriptionPeriod BillingPeriod;
        
        /// Number of cycles for which the billing period is applied.
        public readonly int BillingCycleCount;
        
        /// Recurrence mode for the pricing phase.
        public readonly PricingPhaseRecurrenceMode RecurrenceMode;
                
        /// Type of the pricing phase.
        public readonly PricingPhaseType Type;

        /// True if the current phase is a trial period. False otherwise.
        public readonly bool IsTrial;

        /// True if the current phase is an intro period. False otherwise.
        /// Intro phase is one of single or recurrent discounted payments.
        public readonly bool IsIntro;
        
        /// True if the current phase represents the base plan. False otherwise.
        public readonly bool IsBasePlan;

        public ProductPricingPhase(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("price", out object value) && value is Dictionary<string, object> price)
            {
                Price = new ProductPrice(price);
            }

            if (dict.TryGetValue("billingPeriod", out value) && value is Dictionary<string, object> billingPeriod)
            {
                BillingPeriod = new SubscriptionPeriod(billingPeriod);
            }

            if (dict.TryGetValue("billingCycleCount", out value)) BillingCycleCount = (int)(long)value;
            if (dict.TryGetValue("recurrenceMode", out value)) RecurrenceMode = FormatRecurrenceMode(value);
            if (dict.TryGetValue("type", out value)) Type = FormatType(value);
            if (dict.TryGetValue("isTrial", out value)) IsTrial = (bool)value;
            if (dict.TryGetValue("isIntro", out value)) IsIntro = (bool)value;
            if (dict.TryGetValue("isBasePlan", out value)) IsBasePlan = (bool)value;
        }

        private PricingPhaseType FormatType(object typeValue)
        {
            string type = typeValue as string;
            switch (type) {
                case "Regular": return PricingPhaseType.Regular;
                case "FreeTrial": return PricingPhaseType.FreeTrial; 
                case "SinglePayment": return PricingPhaseType.SinglePayment;
                case "DiscountedRecurringPayment": return PricingPhaseType.DiscountedRecurringPayment;
                default: return PricingPhaseType.Unknown;
            }
        }

        private PricingPhaseRecurrenceMode FormatRecurrenceMode(object recurrenceModeValue)
        {
            string recurrenceMode = recurrenceModeValue as string;
            switch (recurrenceMode) {
                case "InfiniteRecurring": return PricingPhaseRecurrenceMode.InfiniteRecurring;
                case "FiniteRecurring": return PricingPhaseRecurrenceMode.FiniteRecurring;
                case "NonRecurring": return PricingPhaseRecurrenceMode.NonRecurring;
                default: return PricingPhaseRecurrenceMode.Unknown;
            }
        }

        public override string ToString() {
            return $"{nameof(Price)}: {Price}, " +
                   $"{nameof(BillingPeriod)}: {BillingPeriod}, " +
                   $"{nameof(BillingCycleCount)}: {BillingCycleCount}, " +
                   $"{nameof(RecurrenceMode)}: {RecurrenceMode}, " +
                   $"{nameof(Type)}: {Type}, " +
                   $"{nameof(IsTrial)}: {IsTrial}, " +
                   $"{nameof(IsIntro)}: {IsIntro}, " +
                   $"{nameof(IsBasePlan)}: {IsBasePlan}";
        }
    }
}