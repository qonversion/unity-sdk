using System;
using System.Collections.Generic;
using UnityEngine.Scripting;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class Transaction
    {
        /// Original transaction identifier.
        public readonly string OriginalTransactionId;

        /// Transaction identifier.
        public readonly string TransactionId;

        /// Offer code.
        [CanBeNull] public readonly string OfferCode;

        /// Transaction date.
        public readonly DateTime TransactionDate;

        /// Expiration date for subscriptions.
        public readonly DateTime? ExpirationDate;

        /// The date when transaction was revoked. This field represents the time and date the App Store refunded a transaction or revoked it from family sharing.
        public readonly DateTime? TransactionRevocationDate;

        /// Environment of the transaction.
        public readonly QTransactionEnvironment Environment;

        /// Type of ownership for the transaction.  Owner/Family sharing.
        public readonly QTransactionOwnershipType OwnershipType;

        /// Type of the transaction.
        public readonly QTransactionType Type;

        [Preserve]
        public Transaction(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("originalTransactionId", out object value)) OriginalTransactionId = value as string;
            if (dict.TryGetValue("transactionId", out value)) TransactionId = value as string;
            if (dict.TryGetValue("offerCode", out value)) OfferCode = value as string;
            if (dict.TryGetValue("transactionTimestamp", out value)) TransactionDate = FormatDate(value);
            if (dict.TryGetValue("expirationTimestamp", out value) && value != null) ExpirationDate = FormatDate(value);
            if (dict.TryGetValue("transactionRevocationTimestamp", out value) && value != null) TransactionRevocationDate = FormatDate(value);
            Environment = dict.TryGetValue("environment", out value) ? FormatTransactionEnvironment(value) : QTransactionEnvironment.Production;
            OwnershipType = dict.TryGetValue("ownershipType", out value) ? FormatTransactionOwnershipType(value) : QTransactionOwnershipType.Owner;
            Type = dict.TryGetValue("type", out value) ? FormatTransactionType(value) : QTransactionType.SubscriptionRenewed;
        }

        public override string ToString()
        {
            return $"{nameof(OriginalTransactionId)}: {OriginalTransactionId}, " +
                   $"{nameof(TransactionId)}: {TransactionId}, " +
                   $"{nameof(OfferCode)}: {OfferCode}, " +
                   $"{nameof(TransactionDate)}: {TransactionDate}, " +
                   $"{nameof(ExpirationDate)}: {ExpirationDate}, " +
                   $"{nameof(TransactionRevocationDate)}: {TransactionRevocationDate}, " +
                   $"{nameof(Environment)}: {Environment}, " +
                   $"{nameof(OwnershipType)}: {OwnershipType}, " +
                   $"{nameof(Type)}: {Type}";
        }

        private DateTime FormatDate(object time)
        {
            if (time is double)
            {
                return Utils.FormatDate(Convert.ToInt64((double)time));
            }

            return Utils.FormatDate((long)time);
        }

        private QTransactionEnvironment FormatTransactionEnvironment(object environment)
        {
            string value = environment as string;
            QTransactionEnvironment result;
            switch (value)
            {
                case "Sandbox":
                    result = QTransactionEnvironment.Sandbox;
                    break;
                case "Production":
                    result = QTransactionEnvironment.Production;
                    break;
                default:
                    result = QTransactionEnvironment.Production;
                    break;
            }

            return result;
        }

        private QTransactionOwnershipType FormatTransactionOwnershipType(object ownershipType)
        {
            string value = ownershipType as string;
            QTransactionOwnershipType result;
            switch (value)
            {
                case "Owner":
                    result = QTransactionOwnershipType.Owner;
                    break;
                case "FamilySharing":
                    result = QTransactionOwnershipType.FamilySharing;
                    break;
                default:
                    result = QTransactionOwnershipType.Owner;
                    break;
            }

            return result;
        }

        private QTransactionType FormatTransactionType(object type)
        {
            string value = type as string;
            QTransactionType result;
            switch (value)
            {
                case "SubscriptionStarted":
                    result = QTransactionType.SubscriptionStarted;
                    break;
                case "SubscriptionRenewed":
                    result = QTransactionType.SubscriptionRenewed;
                    break;
                case "TrialStrated":
                    result = QTransactionType.TrialStrated;
                    break;
                case "IntroStarted":
                    result = QTransactionType.IntroStarted;
                    break;
                case "IntroRenewed":
                    result = QTransactionType.IntroRenewed;
                    break;
                case "NonConsumablePurchase":
                    result = QTransactionType.NonConsumablePurchase;
                    break;
                default:
                    result = QTransactionType.Unknown;
                    break;
            }

            return result;
        }
    }

    public enum QTransactionEnvironment
    {
        Sandbox,
        Production
    }

    public enum QTransactionOwnershipType
    {
        Owner,
        FamilySharing
    }

    public enum QTransactionType
    {
        Unknown,
        SubscriptionStarted,
        SubscriptionRenewed,
        TrialStrated,
        IntroStarted,
        IntroRenewed,
        NonConsumablePurchase
    }
}