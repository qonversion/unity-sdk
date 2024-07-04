using System.Collections.Generic;

namespace QonversionUnity
{
    public class Eligibility
    {
        public readonly EligibilityStatus Status;

        public Eligibility(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("status", out object value)) Status = FormatEligibilityStatus(value);
        }

        public override string ToString()
        {
            return $"{nameof(Status)}: {Status}";
        }

        private EligibilityStatus FormatEligibilityStatus(object status)
        {
            string value = status as string;
            EligibilityStatus result;
            switch (value)
            {
                case "non_intro_or_trial_product":
                    result = EligibilityStatus.NonIntroOrTrialProduct;
                    break;
                case "intro_or_trial_ineligible":
                    result = EligibilityStatus.Ineligible;
                    break;
                case "intro_or_trial_eligible":
                    result = EligibilityStatus.Eligible;
                    break;
                default:
                    result = EligibilityStatus.Unknown;
                    break;
            }

            return result;
        }
    }
}