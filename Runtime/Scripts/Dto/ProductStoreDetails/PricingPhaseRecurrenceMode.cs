namespace QonversionUnity
{
    /// <summary>
    /// Recurrence mode of the pricing phase.
    /// </summary>
    public enum PricingPhaseRecurrenceMode
    {
        /// The billing plan payment recurs for infinite billing periods unless canceled.
        InfiniteRecurring,
        
        /// The billing plan payment recurs for a fixed number of billing periods
        /// set in <see cref="ProductPricingPhase.BillingCycleCount"/>.
        FiniteRecurring,
        
        /// The billing plan payment is a one-time charge that does not repeat.
        NonRecurring,
        
        /// Unknown recurrence mode.
        Unknown,
    }
}