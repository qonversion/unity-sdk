using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.Scripting;

namespace QonversionUnity
{
    /// <summary>
    /// Represents the result of a purchase operation.
    /// Contains the status of the purchase, entitlements, and store transaction details.
    /// </summary>
    public class PurchaseResult
    {
        /// <summary>
        /// The status of the purchase operation.
        /// </summary>
        public readonly PurchaseResultStatus Status;

        /// <summary>
        /// The user's entitlements after the purchase.
        /// May be null if the purchase failed or is pending.
        /// </summary>
        [CanBeNull] public readonly Dictionary<string, Entitlement> Entitlements;

        /// <summary>
        /// The error that occurred during the purchase, if any.
        /// </summary>
        [CanBeNull] public readonly QonversionError Error;

        /// <summary>
        /// Indicates whether the entitlements were generated from a fallback source.
        /// </summary>
        public readonly bool IsFallbackGenerated;

        /// <summary>
        /// The source of the purchase result data.
        /// </summary>
        public readonly PurchaseResultSource Source;

        /// <summary>
        /// The store transaction details from the native platform.
        /// Contains raw transaction information from Apple App Store or Google Play Store.
        /// </summary>
        [CanBeNull] public readonly StoreTransaction StoreTransaction;

        [Preserve]
        public PurchaseResult(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("status", out object value))
            {
                Status = FormatStatus(value as string);
            }
            else
            {
                Status = PurchaseResultStatus.Unknown;
            }

            if (dict.TryGetValue("entitlements", out value) && value is Dictionary<string, object> entitlementsDict)
            {
                Entitlements = new Dictionary<string, Entitlement>();
                foreach (var pair in entitlementsDict)
                {
                    if (pair.Value is Dictionary<string, object> entitlementDict)
                    {
                        Entitlements[pair.Key] = new Entitlement(entitlementDict);
                    }
                }
            }

            if (dict.TryGetValue("error", out value) && value is Dictionary<string, object> errorDict)
            {
                Error = new QonversionError(errorDict);
            }

            if (dict.TryGetValue("isFallbackGenerated", out value) && value != null)
            {
                IsFallbackGenerated = (bool)value;
            }

            if (dict.TryGetValue("source", out value))
            {
                Source = FormatSource(value as string);
            }
            else
            {
                Source = PurchaseResultSource.Unknown;
            }

            if (dict.TryGetValue("storeTransaction", out value) && value is Dictionary<string, object> transactionDict)
            {
                StoreTransaction = new StoreTransaction(transactionDict);
            }
        }

        /// <summary>
        /// Returns true if the purchase was successful.
        /// </summary>
        public bool IsSuccess => Status == PurchaseResultStatus.Success;

        /// <summary>
        /// Returns true if the purchase was canceled by the user.
        /// </summary>
        public bool IsCanceled => Status == PurchaseResultStatus.UserCanceled;

        /// <summary>
        /// Returns true if the purchase is pending.
        /// </summary>
        public bool IsPending => Status == PurchaseResultStatus.Pending;

        /// <summary>
        /// Returns true if an error occurred during the purchase.
        /// </summary>
        public bool IsError => Status == PurchaseResultStatus.Error;

        public override string ToString()
        {
            return $"{nameof(Status)}: {Status}, " +
                   $"{nameof(Entitlements)}: {Entitlements?.Count ?? 0} items, " +
                   $"{nameof(Error)}: {Error}, " +
                   $"{nameof(IsFallbackGenerated)}: {IsFallbackGenerated}, " +
                   $"{nameof(Source)}: {Source}, " +
                   $"{nameof(StoreTransaction)}: {StoreTransaction}";
        }

        private static PurchaseResultStatus FormatStatus(string value)
        {
            switch (value)
            {
                case "Success":
                    return PurchaseResultStatus.Success;
                case "UserCanceled":
                    return PurchaseResultStatus.UserCanceled;
                case "Pending":
                    return PurchaseResultStatus.Pending;
                case "Error":
                    return PurchaseResultStatus.Error;
                default:
                    return PurchaseResultStatus.Unknown;
            }
        }

        private static PurchaseResultSource FormatSource(string value)
        {
            switch (value)
            {
                case "Api":
                    return PurchaseResultSource.Api;
                case "Local":
                    return PurchaseResultSource.Local;
                default:
                    return PurchaseResultSource.Unknown;
            }
        }
    }
}
