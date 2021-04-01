using System.Collections.Generic;

namespace QonversionUnity
{
    public class SKProduct
    {
        public readonly string ProductIdentifier;
        public readonly string LocalizedDescription;
        public readonly string LocalizedTitle;
        public readonly bool IsFamilyShareable;
        public readonly string Price;
        public readonly SKProductDiscount IntroductoryPrice;
        public readonly List<SKProductDiscount> Discounts;
        public readonly string SubscriptionGroupIdentifier;
        public readonly SKProductSubscriptionPeriod SubscriptionPeriod;
        public readonly bool IsDownloadable;
        public readonly List<int> DownloadContentLengths;
        public readonly string DownloadContentVersion;
        public readonly string CurrencyCode;

        public SKProduct(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("productIdentifier", out object value)) ProductIdentifier = value as string;
            if (dict.TryGetValue("localizedDescription", out value)) LocalizedDescription = value as string;
            if (dict.TryGetValue("localizedTitle", out value)) LocalizedTitle = value as string;

            if (dict.TryGetValue("isFamilyShareable", out value))
            {
                if(value is bool boolValue) IsFamilyShareable = boolValue;
            }

            if (dict.TryGetValue("price", out value)) Price = value as string;

            if (dict.TryGetValue("introductoryPrice", out value) && value is Dictionary<string, object> introPrice)
            {
                IntroductoryPrice = new SKProductDiscount(introPrice);
            }

            if (dict.TryGetValue("discounts", out value))
            {
                if (value is List<object> discounts) Discounts = Mapper.ConvertObjectsList<SKProductDiscount>(discounts);
            }

            if (dict.TryGetValue("subscriptionGroupIdentifier", out value)) SubscriptionGroupIdentifier = value as string;

            if (dict.TryGetValue("subscriptionPeriod", out value))
            {
                if (value is Dictionary<string, object> subsPeriod) SubscriptionPeriod = new SKProductSubscriptionPeriod(subsPeriod);
            }

            if (dict.TryGetValue("isDownloadable", out value))
            {
                if (value is bool boolValue) IsDownloadable = boolValue;
            }

            if (dict.TryGetValue("downloadContentLengths", out value)) DownloadContentLengths = value as List<int>;
            if (dict.TryGetValue("downloadContentVersion", out value)) DownloadContentVersion = value as string;
            if (dict.TryGetValue("currencyCode", out value)) CurrencyCode = value as string;
        }

        public override string ToString()
        {
            return $"{nameof(ProductIdentifier)}: {ProductIdentifier}, " +
                   $"{nameof(LocalizedDescription)}: {LocalizedDescription}, " +
                   $"{nameof(LocalizedTitle)}: {LocalizedTitle}, " +
                   $"{nameof(IsFamilyShareable)}: {IsFamilyShareable}, " +
                   $"{nameof(Price)}: {Price} , " +
                   $"{nameof(IntroductoryPrice)}: {IntroductoryPrice}, " +
                   $"{nameof(Discounts)}: {Discounts}, " +
                   $"{nameof(SubscriptionGroupIdentifier)}: {SubscriptionGroupIdentifier}, " +
                   $"{nameof(SubscriptionPeriod)}: {SubscriptionPeriod}, " +
                   $"{nameof(IsDownloadable)}: {IsDownloadable} , " +
                   $"{nameof(DownloadContentLengths)}: {DownloadContentLengths}, " +
                   $"{nameof(DownloadContentVersion)}: {DownloadContentVersion}, " +
                   $"{nameof(CurrencyCode)}: {CurrencyCode}";
        }
    }
}