using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class AutomationsEvent
    {
        public readonly AutomationsEventType Type;
        public readonly DateTime Date;

        public AutomationsEvent(Dictionary<string, object> dict)
        {

            if (dict.TryGetValue("timestamp", out object time)) Date = Utils.FormatDate((long)time);
            if (dict.TryGetValue("type", out object eventType)) Type = FormatAutomationsEventType(eventType);

        }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Date)}: {Date}";
        }

        private AutomationsEventType FormatAutomationsEventType(object type) {
            string value = type as string;
            AutomationsEventType result;
            switch (value)
            {
                case "trial_started":
                    result = AutomationsEventType.TrialStarted;
                    break;
                case "trial_converted":
                    result = AutomationsEventType.TrialConverted;
                    break;
                case "trial_canceled":
                    result = AutomationsEventType.TrialCanceled;
                    break;
                case "trial_billing_retry_entered":
                    result = AutomationsEventType.TrialBillingRetry;
                    break;
                case "subscription_started":
                    result = AutomationsEventType.SubscriptionStarted;
                    break;
                case "subscription_renewed":
                    result = AutomationsEventType.SubscriptionRenewed;
                    break;
                case "subscription_refunded":
                    result = AutomationsEventType.SubscriptionRefunded;
                    break;
                case "subscription_canceled":
                    result = AutomationsEventType.SubscriptionCanceled;
                    break;
                case "subscription_billing_retry_entered":
                    result = AutomationsEventType.SubscriptionBillingRetry;
                    break;
                case "in_app_purchase":
                    result = AutomationsEventType.InAppPurchase;
                    break;
                case "subscription_upgraded":
                    result = AutomationsEventType.SubscriptionUpgraded;
                    break;
                case "trial_still_active":
                    result = AutomationsEventType.TrialStillActive;
                    break;
                case "trial_expired":
                    result = AutomationsEventType.TrialExpired;
                    break;
                case "subscription_expired":
                    result = AutomationsEventType.SubscriptionExpired;
                    break;
                case "subscription_downgraded":
                    result = AutomationsEventType.SubscriptionDowngraded;
                    break;
                case "subscription_product_changed":
                    result = AutomationsEventType.SubscriptionProductChanged;
                    break;
                default:
                    result = AutomationsEventType.Unknown;
                    break;
            }

            return result;
        }
    }
}
