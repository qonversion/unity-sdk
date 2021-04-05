using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class SkuDetails
    {
        /// Textual description of the product.
        public readonly string Description;

        /// Trial period in ISO 8601 format.
        public readonly string FreeTrialPeriod;

        /// Icon of the product if present.
        public readonly string IconUrl;

        /// Introductory price, only applies to [SkuType.subs]. Formatted ("$0.99").
        public readonly string IntroductoryPrice;

        /// Introductory price in micro-units 
        public readonly long IntroductoryPriceAmountMicros;

        /// The number of billing perios that [introductoryPrice] is valid for ("2").
        public readonly int IntroductoryPriceCycles;

        /// The billing period of [introductoryPrice], in ISO 8601 format.
        public readonly string IntroductoryPricePeriod;

        /// String in JSON format that contains SKU details.
        public readonly string OriginalJson;

        /// The original price that the user purchased this product for.
        public readonly string OriginalPrice;

        /// [originalPrice] in micro-units (990000).
        public readonly long OriginalPriceAmountMicros;

        /// Formatted with currency symbol ("$0.99").
        public readonly string Price;

        /// Returns price in micro-units, where 1,000,000 micro-units equal one unit of the currency.
        public readonly long PriceAmountMicros;

        /// [price] ISO 4217 currency code.
        public readonly string PriceCurrencyCode;

        /// The product ID in Google Play Console.
        public readonly string Sku;

        /// Applies to [SkuType.subs], formatted in ISO 8601.
        public readonly string SubscriptionPeriod;

        /// The product's title.
        public readonly string Title;

        /// Inapp or subs.
        public readonly string Type;

        public SkuDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("description", out object value)) Description = value as string;
            if (dict.TryGetValue("freeTrialPeriod", out value)) FreeTrialPeriod = value as string;
            if (dict.TryGetValue("iconUrl", out value)) IconUrl = value as string;
            if (dict.TryGetValue("introductoryPrice", out value)) IntroductoryPrice = value as string;
            if (dict.TryGetValue("introductoryPriceAmountMicros", out value)) IntroductoryPriceAmountMicros = (long)value;
            if (dict.TryGetValue("introductoryPriceCycles", out value)) IntroductoryPriceCycles = Convert.ToInt32(value);
            if (dict.TryGetValue("introductoryPricePeriod", out value)) IntroductoryPricePeriod = value as string;
            if (dict.TryGetValue("originalJson", out value)) OriginalJson = value as string;
            if (dict.TryGetValue("originalPrice", out value)) OriginalPrice = value as string;
            if (dict.TryGetValue("originalPriceAmountMicros", out value)) OriginalPriceAmountMicros = (long)value;
            if (dict.TryGetValue("price", out value)) Price = value as string;
            if (dict.TryGetValue("priceAmountMicros", out value)) PriceAmountMicros = (long)value;
            if (dict.TryGetValue("priceCurrencyCode", out value)) PriceCurrencyCode = value as string;
            if (dict.TryGetValue("sku", out value)) Sku = value as string;
            if (dict.TryGetValue("subscriptionPeriod", out value)) SubscriptionPeriod = value as string;
            if (dict.TryGetValue("title", out value)) Title = value as string;
            if (dict.TryGetValue("type", out value)) Type = value as string;
        }

        public override string ToString()
        {
            return $"{nameof(Description)}: {Description}, \n" +
                   $"{nameof(FreeTrialPeriod)}: {FreeTrialPeriod}, " +
                   $"{nameof(IconUrl)}: {IconUrl}, " +
                   $"{nameof(IntroductoryPrice)}: {IntroductoryPrice}, " +
                   $"{nameof(IntroductoryPriceAmountMicros)}: {IntroductoryPriceAmountMicros}, " +
                   $"{nameof(IntroductoryPriceCycles)}: {IntroductoryPriceCycles} , " +
                   $"{nameof(IntroductoryPricePeriod)}: {IntroductoryPricePeriod}, " +
                   $"{nameof(OriginalJson)}: {OriginalJson}, " +
                   $"{nameof(OriginalPrice)}: {OriginalPrice}, " +
                   $"{nameof(OriginalPriceAmountMicros)}: {OriginalPriceAmountMicros}, " +
                   $"{nameof(Price)}: {Price}, " +
                   $"{nameof(PriceAmountMicros)}: {PriceAmountMicros}, " +
                   $"{nameof(PriceCurrencyCode)}: {PriceCurrencyCode}, " +
                   $"{nameof(Sku)}: {Sku}, " +
                   $"{nameof(SubscriptionPeriod)}: {SubscriptionPeriod}, " +
                   $"{nameof(Title)}: {Title}, " +
                   $"{nameof(Type)}: {Type}";
        }
    }
}