using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class SKProductSubscriptionPeriod
    {
        public readonly int NumberOfUnits;
        public readonly SKPeriodUnit Unit;

        public SKProductSubscriptionPeriod(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("numberOfUnits", out object value)) NumberOfUnits = Convert.ToInt32(value);
            if (dict.TryGetValue("unit", out value)) Unit = FormatPeriodUnit(value);
        }

        public override string ToString()
        {
            return $"{nameof(NumberOfUnits)}: {NumberOfUnits}, " +
                   $"{nameof(Unit)}: {Unit}";
        }

        private SKPeriodUnit FormatPeriodUnit(object periodUnit) =>
            (SKPeriodUnit)Convert.ToInt32(periodUnit);
    }

    public enum SKPeriodUnit
    {
        Day,
        Week,
        Month,
        Year,
    }
}