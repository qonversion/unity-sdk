namespace QonversionUnity
{
    public enum ProrationMode
    {
        UnknownSubscriptionUpgradeDowngradePolicy,
        /// Replacement takes effect immediately, and the remaining time will be prorated and credited to the user.
        ImmediateWithTimeProration,
        /// Replacement takes effect immediately, and the billing cycle remains the same.
        ImmediateAndChargeProratedPrice,
        /// Replacement takes effect immediately, and the new price will be charged on next recurrence time.
        ImmediateWithoutProration,
        /// Replacement takes effect when the old plan expires, and the new price will be charged at the same time.
        Deferred
    }
}