using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public class Entitlement
    {
        /// Qonversion Entitlement ID, like premium.
        [Tooltip("Create Entitlement: https://qonversion.io/docs/create-permission")]
        public readonly string Id;

        /// Product ID created in Qonversion Dashboard.
        [Tooltip("Create Products: https://qonversion.io/docs/create-products")]
        public readonly string ProductId;

        /// A renew state for an associate product that unlocked entitlement
        public readonly QEntitlementRenewState RenewState;

        /// A source determining where this entitlement is originally from - App Store, Play Store, Stripe, etc.
        public readonly QEntitlementSource Source;
        
        /// Purchase date
        public readonly DateTime StartedDate;

        /// Expiration date for subscription
        public readonly DateTime? ExpirationDate;

        /// Use for checking entitlement for current user.
        /// Pay attention, isActive == true does not mean that subscription is renewable.
        /// Subscription could be canceled, but the user could still have a entitlement
        public readonly bool IsActive;

        public Entitlement(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("productId", out value)) ProductId = value as string;
            if (dict.TryGetValue("renewState", out value)) RenewState = FormatRenewState(value);
            Source = dict.TryGetValue("source", out value) ? FormatEntitlementSource(value) : QEntitlementSource.Unknown;
            if (dict.TryGetValue("active", out value)) IsActive = (bool)value;
            if (dict.TryGetValue("startedTimestamp", out value)) StartedDate = FormatDate(value);
            if (dict.TryGetValue("expirationTimestamp", out value) && value != null) ExpirationDate = FormatDate(value);
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(ProductId)}: {ProductId}, " +
                   $"{nameof(RenewState)}: {RenewState}, " +
                   $"{nameof(Source)}: {Source}, " +
                   $"{nameof(StartedDate)}: {StartedDate}, " +
                   $"{nameof(ExpirationDate)}: {ExpirationDate}, " +
                   $"{nameof(IsActive)}: {IsActive}";
        }

        private DateTime FormatDate(object time) {
            if (time is double) {
                return Utils.FormatDate(Convert.ToInt64((double)time));
            } 

            return Utils.FormatDate((long) time);
        }

        private QEntitlementRenewState FormatRenewState(object renewState) =>
            (QEntitlementRenewState)Convert.ToInt32(renewState);

        private QEntitlementSource FormatEntitlementSource(object source) {
            return Enum.TryParse(source.ToString(), out QEntitlementSource parsedSource)
                ? parsedSource
                : QEntitlementSource.Unknown;
        }
    }

    public enum QEntitlementRenewState
    {
        /// For in-app purchases.
        NonRenewable = -1,
        /// Unknown.
        Unknown = 0,
        /// Subscription is active and will renew
        WillRenew = 1,
        /// The user canceled the subscription, but the subscription may be active.
        /// Check isActive to be sure that the subscription has not expired yet.
        Canceled = 2,
        /// There was some billing issue.
        /// Prompt the user to update the payment method.
        BillingIssue = 3
    }

    public enum QEntitlementSource
    {
        Unknown,
        AppStore,
        PlayStore,
        Stripe,
        Manual
    }
}