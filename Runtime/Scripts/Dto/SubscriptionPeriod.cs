using System.Collections.Generic;

namespace QonversionUnity 
{
    /// <summary>
    /// A class describing a subscription period
    /// </summary>
    public class SubscriptionPeriod
    {
        /// A count of subsequent intervals.
        public readonly int UnitCount;

        /// Interval unit.
        public readonly SubscriptionPeriodUnit Unit;

        /// ISO 8601 representation of the period, e.g. "P7D", meaning 7 days period.
        public readonly string Iso;
        
        public SubscriptionPeriod(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("iso", out object value)) Iso = value as string;
            if (dict.TryGetValue("unitCount", out value)) UnitCount = (int)(long)value;
            if (dict.TryGetValue("unit", out value)) Unit = FormatSubscriptionPeriodUnit(value);
        }

        public SubscriptionPeriodUnit FormatSubscriptionPeriodUnit(object unitValue)
        {
            string unit = unitValue as string;
            if (unit == "Day")
            {
                return SubscriptionPeriodUnit.Day;
            }

            if (unit == "Week")
            {
                return SubscriptionPeriodUnit.Week;
            }

            if (unit == "Month")
            {
                return SubscriptionPeriodUnit.Month;
            }

            if (unit == "Year")
            {
                return SubscriptionPeriodUnit.Year;
            }

            return SubscriptionPeriodUnit.Unknown;
        }

        public override string ToString()
        {
            return $"{nameof(UnitCount)}: {UnitCount}, " +
                   $"{nameof(Iso)}: {Iso}, " +
                   $"{nameof(Unit)}: {Unit}";
        }
    }
}
