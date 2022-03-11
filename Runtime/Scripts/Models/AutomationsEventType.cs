using System;
namespace QonversionUnity
{
    public enum AutomationsEventType
    {

    }

    private AutomationsEventType FormatAutomationsEventType(object type) {
            string value = type as string;
            AutomationsEventType result;
            switch (value)
            {
                case "trial_started":
                    result = AutomationsEventType.;
                    break;
                case "trial_converted":
                    result = EligibilityStatus.Eligible;
                    break;
                case "trial_canceled":
                    result = EligibilityStatus.Ineligible;
                    break;
                case "trial_billing_retry_entered":
                    result = EligibilityStatus.NonIntroProduct;
                    break;
                case "subscription_started":
                    result = EligibilityStatus.Eligible;
                    break;
                case "subscription_renewed":
                    result = EligibilityStatus.Ineligible;
                    break;
                case "subscription_refunded":
                    result = EligibilityStatus.NonIntroProduct;
                    break;
                case "subscription_canceled":
                    result = EligibilityStatus.Eligible;
                    break;
                case "subscription_billing_retry_entered":
                    result = EligibilityStatus.Ineligible;
                    break;
                case "in_app_purchase":
                    result = EligibilityStatus.NonIntroProduct;
                    break;
                case "subscription_upgraded":
                    result = EligibilityStatus.Eligible;
                    break;
                case "trial_still_active":
                    result = EligibilityStatus.Ineligible;
                    break;
                case "trial_expired":
                    result = EligibilityStatus.NonIntroProduct;
                    break;
                case "subscription_expired":
                    result = EligibilityStatus.Eligible;
                    break;
                case "subscription_downgraded":
                    result = EligibilityStatus.Ineligible;
                    break;
                case "subscription_product_changed":
                    result = EligibilityStatus.Ineligible;
                    break;
                default:
                    result = EligibilityStatus.Unknown;
                    break;
            }

            return result;
        }
}
