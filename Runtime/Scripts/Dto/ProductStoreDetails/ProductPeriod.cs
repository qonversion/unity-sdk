using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class ProductPeriod
    {
        public readonly int Count;
        public readonly string Iso;
        public readonly ProductPeriodUnit Unit;

        public ProductPeriod(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("iso", out object value)) Iso = value as string;
            if (dict.TryGetValue("count", out value)) Count = (int)value;
            if (dict.TryGetValue("unit", out value)) Unit = FormatProductPeriodUnit(value);
        }

        public ProductPeriodUnit FormatProductPeriodUnit(object unitValue)
        {
            string unit = unitValue as string;
            if (unit == "Day")
            {
                return ProductPeriodUnit.Day;
            }
            else if (unit == "Week")
            {
                return ProductPeriodUnit.Week;
            }
            else if (unit == "Month")
            {
                return ProductPeriodUnit.Month;
            }
            else if (unit == "Year")
            {
                return ProductPeriodUnit.Year;
            }

            return ProductPeriodUnit.Unknown;
        }

        public override string ToString()
        {
            return $"{nameof(Count)}: {Count}, " +
                   $"{nameof(Iso)}: {Iso}, " +
                   $"{nameof(Unit)}: {Unit}";
        }
    }
}