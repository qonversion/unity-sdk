using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class SKProductDiscount
    {
        [CanBeNull] public readonly string Identifier;
        public readonly SKProductDiscountType Type;
        [CanBeNull] public readonly string Price;
        [CanBeNull] public readonly string LocaleIdentifier;
        public readonly SKProductDiscountPaymentMode PaymentMode;
        public readonly int NumberOfPeriods;
        [CanBeNull] public readonly SKProductSubscriptionPeriod SubscriptionPeriod;
        public readonly string CurrencySymbol;

        public SKProductDiscount(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("identifier", out object value)) Identifier = value as string;
            if (dict.TryGetValue("type", out value)) Type = FormatDiscountType(value);
            if (dict.TryGetValue("price", out value)) Price = value as string;
            if (dict.TryGetValue("paymentMode", out value)) PaymentMode = FormatPaymentMode(value);
            if (dict.TryGetValue("numberOfPeriods", out value)) NumberOfPeriods = Convert.ToInt32(value);

            if (dict.TryGetValue("priceLocale", out priceLocale))
            {
                if (priceLocale is Dictionary<string, object> priceLocaleDict)
                {
                    if (priceLocaleDict.TryGetValue("localeIdentifier", out value)) LocaleIdentifier = value as string;
                    if (priceLocaleDict.TryGetValue("currencySymbol", out value)) CurrencySymbol = value as string;
                }
            }

            if (dict.TryGetValue("subscriptionPeriod", out value))
            {
                if (value is Dictionary<string, object> subsPeriod) SubscriptionPeriod = new SKProductSubscriptionPeriod(subsPeriod);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Identifier)}: {Identifier}, " +
                   $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Price)}: {Price}, " +
                   $"{nameof(LocaleIdentifier)}: {LocaleIdentifier}, " +
                   $"{nameof(Price)}: {Price} , " +
                   $"{nameof(PaymentMode)}: {PaymentMode}, " +
                   $"{nameof(NumberOfPeriods)}: {NumberOfPeriods}, " +
                   $"{nameof(SubscriptionPeriod)}: {SubscriptionPeriod}" +
                   $"{nameof(CurrencySymbol)}: {CurrencySymbol}";
        }

        private SKProductDiscountType FormatDiscountType(object discountType) =>
            (SKProductDiscountType)Convert.ToInt32(discountType);

        private SKProductDiscountPaymentMode FormatPaymentMode(object paymentMode) =>
            (SKProductDiscountPaymentMode)Convert.ToInt32(paymentMode);
    }

    public enum SKProductDiscountType
    {
        Introductory,
        Subscription
    }

    public enum SKProductDiscountPaymentMode
    {
        PayAsYouGo,
        PayUpFront,
        FreeTrial
    }
}