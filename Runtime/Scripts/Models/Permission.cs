using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public class Permission
    {
        /// Qonversion Permission ID, like premium.
        [Tooltip("Create Permission: https://qonversion.io/docs/create-permission")]
        public readonly string PermissionID;

        /// Product ID created in Qonversion Dashboard.
        [Tooltip("Create Products: https://qonversion.io/docs/create-products")]
        public readonly string ProductID;

        /// A renew state for an associate product that unlocked permission
        public readonly QProductRenewState RenewState;

        /// Purchase date
        public readonly DateTime StartedDate;

        /// Expiration date for subscription
        public readonly DateTime? ExpirationDate;

        /// Use for checking permission for current user.
        /// Pay attention, isActive == true does not mean that subscription is renewable.
        /// Subscription could be canceled, but the user could still have a permission
        public readonly bool IsActive;

        public Permission(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) PermissionID = value as string;
            if (dict.TryGetValue("associatedProduct", out value)) ProductID = value as string;
            if (dict.TryGetValue("renewState", out value)) RenewState = FormatRenewState(value);
            if (dict.TryGetValue("active", out value)) IsActive = (bool)value;
            if (dict.TryGetValue("startedTimestamp", out value)) StartedDate = FormatDate(value);
            if (dict.TryGetValue("expirationTimestamp", out value) && value != null) ExpirationDate = FormatDate(value);
        }

        public override string ToString()
        {
            return $"{nameof(PermissionID)}: {PermissionID}, " +
                   $"{nameof(ProductID)}: {ProductID}, " +
                   $"{nameof(RenewState)}: {RenewState}, " +
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

        private QProductRenewState FormatRenewState(object renewState) =>
            (QProductRenewState)Convert.ToInt32(renewState);
    }

    public enum QProductRenewState
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
}