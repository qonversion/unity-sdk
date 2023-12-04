namespace QonversionUnity
{
    public enum QProductType
    {
        /// Provides access to content on a recurring basis with a free offer
        Trial,
        /// Provides access to content on a recurring basis with an introductory offer
        Intro,
        /// Provides access to content on a recurring basis
        Subscription,
        /// Content that users can purchase with a single, non-recurring charge
        InApp,
        /// Unknown product type
        Unknown,
    }
}