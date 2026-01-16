namespace QonversionUnity
{
    /// <summary>
    /// Source of the purchase result data.
    /// </summary>
    public enum PurchaseResultSource
    {
        /// <summary>
        /// Unknown source.
        /// </summary>
        Unknown,

        /// <summary>
        /// The result was obtained from the Qonversion API.
        /// </summary>
        Api,

        /// <summary>
        /// The result was obtained from the local store.
        /// </summary>
        Local
    }
}
