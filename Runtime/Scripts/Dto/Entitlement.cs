using System;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

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

        /// Renews count for the entitlement. Renews count starts from the second paid subscription.
        /// For example, we have 20 transactions.One is the trial, and one is the first paid transaction after the trial.
        /// Renews count is equal to 18.
        public readonly int RenewsCount;

        /// Trial start date.
        public readonly DateTime? TrialStartDate;

        /// First purchase date.
        public readonly DateTime? FirstPurchaseDate;

        /// Last purchase date.
        public readonly DateTime? LastPurchaseDate;

        /// Last activated offer code.
        [CanBeNull] public readonly string LastActivatedOfferCode;

        /// Grant type of the entitlement.
        public readonly QEntitlementGrantType GrantType;

        /// Auto-renew disable date.
        public readonly DateTime? AutoRenewDisableDate;

        /// Array of the transactions that unlocked current entitlement.
        public readonly List<Transaction> Transactions;

        public Entitlement(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("productId", out value)) ProductId = value as string;
            if (dict.TryGetValue("renewState", out value)) RenewState = FormatRenewState(value);
            Source = dict.TryGetValue("source", out value) ? FormatEntitlementSource(value) : QEntitlementSource.Unknown;
            if (dict.TryGetValue("active", out value)) IsActive = (bool)value;
            if (dict.TryGetValue("startedTimestamp", out value)) StartedDate = FormatDate(value);
            if (dict.TryGetValue("expirationTimestamp", out value) && value != null) ExpirationDate = FormatDate(value);
            if (dict.TryGetValue("trialStartTimestamp", out value) && value != null) TrialStartDate = FormatDate(value);
            if (dict.TryGetValue("firstPurchaseTimestamp", out value) && value != null) FirstPurchaseDate = FormatDate(value);
            if (dict.TryGetValue("lastPurchaseTimestamp", out value) && value != null) LastPurchaseDate = FormatDate(value);
            if (dict.TryGetValue("autoRenewDisableTimestamp", out value) && value != null) AutoRenewDisableDate = FormatDate(value);
            if (dict.TryGetValue("lastActivatedOfferCode", out value)) LastActivatedOfferCode = value as string;

            GrantType = dict.TryGetValue("grantType", out value) ? FormatGrantType(value) : QEntitlementGrantType.Purchase;

            if (dict.TryGetValue("transactions", out value) && value is List<object> transactions)
            {
                Transactions = Mapper.ConvertObjectsList<Transaction>(transactions);
            }

            RenewsCount = dict.TryGetValue("renewsCount", out value) ? (int)(long)value : 0;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(ProductId)}: {ProductId}, " +
                   $"{nameof(RenewState)}: {RenewState}, " +
                   $"{nameof(Source)}: {Source}, " +
                   $"{nameof(StartedDate)}: {StartedDate}, " +
                   $"{nameof(ExpirationDate)}: {ExpirationDate}, " +
                   $"{nameof(IsActive)}: {IsActive}, " +
                   $"{nameof(RenewsCount)}: {RenewsCount}, " +
                   $"{nameof(TrialStartDate)}: {TrialStartDate}, " +
                   $"{nameof(FirstPurchaseDate)}: {FirstPurchaseDate}, " +
                   $"{nameof(LastPurchaseDate)}: {LastPurchaseDate}, " +
                   $"{nameof(LastActivatedOfferCode)}: {LastActivatedOfferCode}, " +
                   $"{nameof(GrantType)}: {GrantType}, " +
                   $"{nameof(AutoRenewDisableDate)}: {AutoRenewDisableDate}, " +
                   $"{nameof(Transactions)}: {Utils.PrintObjectList(Transactions)}";
        }

        private DateTime FormatDate(object time)
        {
            if (time is double)
            {
                return Utils.FormatDate(Convert.ToInt64((double)time));
            }

            return Utils.FormatDate((long)time);
        }

        private QEntitlementRenewState FormatRenewState(object renewState)
        {
            string value = renewState as string;
            QEntitlementRenewState result;
            switch (value)
            {
                case "non_renewable":
                    result = QEntitlementRenewState.NonRenewable;
                    break;
                case "will_renew":
                    result = QEntitlementRenewState.WillRenew;
                    break;
                case "canceled":
                    result = QEntitlementRenewState.Canceled;
                    break;
                case "billing_issue":
                    result = QEntitlementRenewState.BillingIssue;
                    break;
                default:
                    result = QEntitlementRenewState.Unknown;
                    break;
            }

            return result;
        }

        private QEntitlementSource FormatEntitlementSource(object source)
        {
            return Enum.TryParse(source.ToString(), out QEntitlementSource parsedSource)
                ? parsedSource
                : QEntitlementSource.Unknown;
        }

        private QEntitlementGrantType FormatGrantType(object grantType)
        {
            string value = grantType as string;
            QEntitlementGrantType result;
            switch (value)
            {
                case "Purchase":
                    result = QEntitlementGrantType.Purchase;
                    break;
                case "FamilySharing":
                    result = QEntitlementGrantType.FamilySharing;
                    break;
                case "OfferCode":
                    result = QEntitlementGrantType.OfferCode;
                    break;
                case "Manual":
                    result = QEntitlementGrantType.Manual;
                    break;
                default:
                    result = QEntitlementGrantType.Purchase;
                    break;
            }

            return result;
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

    public enum QEntitlementGrantType
    {
        Purchase,
        FamilySharing,
        OfferCode,
        Manual
    }
}