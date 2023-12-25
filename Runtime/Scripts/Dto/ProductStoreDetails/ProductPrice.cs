using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// Information about the Google product's price.
    /// </summary>
    public class ProductPrice
    {
        /// Total amount of money in micro-units,
        /// where 1,000,000 micro-units equal one unit of the currency.
        public readonly long PriceAmountMicros;

        /// ISO 4217 currency code for price.
        public readonly string PriceCurrencyCode;

        /// Formatted price for the payment, including its currency sign.
        public readonly string FormattedPrice;
        
        /// True, if the price is zero. False otherwise.
        public readonly bool IsFree;
        
        /// Price currency symbol. Null if failed to parse.
        [CanBeNull] public readonly string CurrencySymbol;

        public ProductPrice(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("priceAmountMicros", out object value)) PriceAmountMicros = (long)value;
            if (dict.TryGetValue("priceCurrencyCode", out value)) PriceCurrencyCode = value as string;
            if (dict.TryGetValue("formattedPrice", out value)) FormattedPrice = value as string;
            if (dict.TryGetValue("isFree", out value)) IsFree = (bool)value;
            if (dict.TryGetValue("currencySymbol", out value)) CurrencySymbol = value as string;
        }

        public override string ToString() {
            return $"{nameof(PriceAmountMicros)}: {PriceAmountMicros}, " +
                   $"{nameof(PriceCurrencyCode)}: {PriceCurrencyCode}, " +
                   $"{nameof(FormattedPrice)}: {FormattedPrice}, " +
                   $"{nameof(IsFree)}: {IsFree}, " +
                   $"{nameof(CurrencySymbol)}: {CurrencySymbol}";
        }
    }
}