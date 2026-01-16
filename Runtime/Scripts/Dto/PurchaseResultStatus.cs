namespace QonversionUnity
{
    /// <summary>
    /// Status of the purchase result.
    /// </summary>
    public enum PurchaseResultStatus
    {
        /// <summary>
        /// Unknown status.
        /// </summary>
        Unknown,

        /// <summary>
        /// The purchase was successful.
        /// </summary>
        Success,

        /// <summary>
        /// The purchase was canceled by the user.
        /// </summary>
        UserCanceled,

        /// <summary>
        /// The purchase is pending (e.g., waiting for parental approval).
        /// </summary>
        Pending,

        /// <summary>
        /// An error occurred during the purchase.
        /// </summary>
        Error
    }
}
