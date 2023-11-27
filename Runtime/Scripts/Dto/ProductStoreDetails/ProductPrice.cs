using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class ProductPrice
    {
        [CanBeNull] public readonly string CurrencySymbol;
        public readonly string FormattedPrice;
        public readonly bool IsFree;
        public readonly int PriceAmountMicros;
        public readonly string PriceCurrencyCode;

        public ProductPrice(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("currencySymbol", out object value)) CurrencySymbol = value as string;
            if (dict.TryGetValue("formattedPrice", out value)) FormattedPrice = value as string;
            if (dict.TryGetValue("isFree", out value)) IsFree = (bool)value;
            if (dict.TryGetValue("priceAmountMicros", out value)) PriceAmountMicros = (int)value;
            if (dict.TryGetValue("priceCurrencyCode", out value)) PriceCurrencyCode = value as string;
        }

        public override string ToString()
        {
            return $"{nameof(CurrencySymbol)}: {CurrencySymbol}, " +
                   $"{nameof(FormattedPrice)}: {FormattedPrice}, " +
                   $"{nameof(IsFree)}: {IsFree}, " +
                   $"{nameof(PriceAmountMicros)}: {PriceAmountMicros}, " +
                   $"{nameof(PriceCurrencyCode)}: {PriceCurrencyCode}";
        }
    }
}