using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Scripting;

namespace QonversionUnity
{
    /// <summary>
    /// Represents a raw store transaction from the native platform.
    /// This is the transaction information as received from Apple App Store or Google Play Store.
    /// </summary>
    public class StoreTransaction
    {
        /// <summary>
        /// The unique identifier for this transaction.
        /// </summary>
        [CanBeNull] public readonly string TransactionId;

        /// <summary>
        /// The original transaction identifier.
        /// For subscriptions, this identifies the first transaction in the subscription chain.
        /// </summary>
        [CanBeNull] public readonly string OriginalTransactionId;

        /// <summary>
        /// The date and time when the transaction occurred.
        /// </summary>
        [CanBeNull] public readonly DateTime? TransactionDate;

        /// <summary>
        /// The store product identifier associated with this transaction.
        /// </summary>
        [CanBeNull] public readonly string ProductId;

        /// <summary>
        /// The quantity of items purchased.
        /// </summary>
        public readonly int Quantity;

        /// <summary>
        /// iOS only. The identifier of the promotional offer applied to this purchase.
        /// </summary>
        [CanBeNull] public readonly string PromoOfferId;

        /// <summary>
        /// Android only. The purchase token from Google Play.
        /// </summary>
        [CanBeNull] public readonly string PurchaseToken;

        [Preserve]
        public StoreTransaction(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("transactionId", out object value)) TransactionId = value as string;
            if (dict.TryGetValue("originalTransactionId", out value)) OriginalTransactionId = value as string;
            if (dict.TryGetValue("transactionTimestamp", out value) && value != null)
            {
                TransactionDate = FormatDate(value);
            }
            if (dict.TryGetValue("productId", out value)) ProductId = value as string;
            if (dict.TryGetValue("quantity", out value) && value != null)
            {
                Quantity = Convert.ToInt32(value);
            }
            else
            {
                Quantity = 1;
            }
            if (dict.TryGetValue("promoOfferId", out value)) PromoOfferId = value as string;
            if (dict.TryGetValue("purchaseToken", out value)) PurchaseToken = value as string;
        }

        public override string ToString()
        {
            return $"{nameof(TransactionId)}: {TransactionId}, " +
                   $"{nameof(OriginalTransactionId)}: {OriginalTransactionId}, " +
                   $"{nameof(TransactionDate)}: {TransactionDate}, " +
                   $"{nameof(ProductId)}: {ProductId}, " +
                   $"{nameof(Quantity)}: {Quantity}, " +
                   $"{nameof(PromoOfferId)}: {PromoOfferId}, " +
                   $"{nameof(PurchaseToken)}: {PurchaseToken}";
        }

        private DateTime FormatDate(object time)
        {
            if (time is double doubleTime)
            {
                return Utils.FormatDate(Convert.ToInt64(doubleTime));
            }

            return Utils.FormatDate(Convert.ToInt64(time));
        }
    }
}
