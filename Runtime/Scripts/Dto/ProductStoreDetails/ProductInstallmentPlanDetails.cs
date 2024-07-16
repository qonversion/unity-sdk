using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    /// <summary>
    /// This class represents the details about the installment plan for a subscription product.
    /// </summary>
    public class ProductInstallmentPlanDetails
    {
        /// Committed payments count after a user signs up for this subscription plan.
        public readonly int CommitmentPaymentsCount;

        /// Subsequent committed payments count after this subscription plan renews.
        /// 
        /// Returns 0 if the installment plan doesn't have any subsequent commitment,
        /// which means this subscription plan will fall back to a normal
        /// non-installment monthly plan when the plan renews.
        public readonly int SubsequentCommitmentPaymentsCount;

        public ProductInstallmentPlanDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("commitmentPaymentsCount", out object value)) CommitmentPaymentsCount = (int)(long)value;
            if (dict.TryGetValue("subsequentCommitmentPaymentsCount", out value)) SubsequentCommitmentPaymentsCount = (int)(long)value;
        }

        public override string ToString()
        {
            return $"{nameof(CommitmentPaymentsCount)}: {CommitmentPaymentsCount}, " + 
                   $"{nameof(SubsequentCommitmentPaymentsCount)}: {SubsequentCommitmentPaymentsCount}";
        }
    }
}