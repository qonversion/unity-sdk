namespace QonversionUnity
{
    /// <summary>
    /// Type of the pricing phase.
    /// </summary>
    public enum PricingPhaseType
    {
        /// Regular subscription without any discounts like trial or intro offers.
        Regular,
        
        /// A free phase.
        FreeTrial,
        
        /// A phase with a discounted payment for a single period.
        DiscountedSinglePayment,
        
        /// A phase with a discounted payment for several periods, described in <see cref="ProductPricingPhase.BillingCycleCount"/>.
        DiscountedRecurringPayment,
        
        /// Unknown pricing phase type.
        Unknown,
    }
}