using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class ProductPricingPhase
    {
        public readonly int BillingCycleCount;
        public readonly ProductPeriod BillingPeriod;
        public readonly bool IsBasePlan;
        public readonly bool IsIntro;
        public readonly bool IsTrial;
        public readonly ProductPrice Price;
        public readonly PricingPhaseRecurrenceMode RecurrenceMode;
        public readonly PricingPhaseType Type;

        public ProductPricingPhase(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("billingCycleCount", out value)) BillingCycleCount = (int)value;

            if (dict.TryGetValue("billingPeriod", out value) && value is Dictionary<string, object> billingPeriod)
            {
                BillingPeriod = new ProductPeriod(billingPeriod);
            }

            if (dict.TryGetValue("isBasePlan", out value)) IsBasePlan = (bool)value;
            if (dict.TryGetValue("isIntro", out value)) IsIntro = (bool)value;
            if (dict.TryGetValue("isTrial", out value)) IsIntro = (bool)value;

            if (dict.TryGetValue("price", out value) && value is Dictionary<string, object> price)
            {
                Price = new ProductPrice(price);
            }

            if (dict.TryGetValue("recurrenceMode", out value)) PricingPhaseRecurrenceMode = FormatRecurrenceMode(value);
            if (dict.TryGetValue("type", out value)) PricingPhaseType = FormatType(value);
        }

        public PricingPhaseType FormatType(object typeValue)
        {
            string type = typeValue as string;
            if (type == "Regular")
            {
                return PricingPhaseType.Regular;
            }
            else if (type == "FreeTrial")
            {
                return PricingPhaseType.FreeTrial;
            }
            else if (type == "SinglePayment")
            {
                return PricingPhaseType.SinglePayment;
            }
            else if (type == "DiscountedRecurringPayment")
            {
                return PricingPhaseType.DiscountedRecurringPayment;
            }

            return PricingPhaseType.Unknown;
        }

        public PricingPhaseRecurrenceMode FormatRecurrenceMode(object recurrenceModeValue)
        {
            string recurrenceMode = recurrenceModeValue as string;
            if (recurrenceMode == "InfiniteRecurring")
            {
                return PricingPhaseRecurrenceMode.InfiniteRecurring;
            }
            else if (recurrenceMode == "FiniteRecurring")
            {
                return PricingPhaseRecurrenceMode.FiniteRecurring;
            }
            else if (recurrenceMode == "NonRecurring")
            {
                return PricingPhaseRecurrenceMode.NonRecurring;
            }

            return PricingPhaseRecurrenceMode.Unknown;
        }

        public override string ToString()
        {
            return $"{nameof(BillingCycleCount)}: {BillingCycleCount}, " +
                   $"{nameof(BillingPeriod)}: {BillingPeriod.ToString()}, " +
                   $"{nameof(IsBasePlan)}: {IsBasePlan}, " +
                   $"{nameof(IsIntro)}: {IsIntro}, " +
                   $"{nameof(IsTrial)}: {IsTrial}, " +
                   $"{nameof(Price)}: {Price.ToString()}, " +
                   $"{nameof(RecurrenceMode)}: {RecurrenceMode}, " +
                   $"{nameof(Type)}: {Type}";
        }
    }
}