using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class SKProductDiscount
    {
        public readonly string Identifier;
        public readonly SKProductDiscountType Type;
        public readonly string Price;
        public readonly string LocaleIdentifier;
        public readonly SKProductDiscountPaymentMode PaymentMode;
        public readonly int NumberOfPeriods;
        public readonly SKProductSubscriptionPeriod SubscriptionPeriod;

        public SKProductDiscount(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("identifier", out object value)) Identifier = value as string;
            if (dict.TryGetValue("type", out value)) Type = FormatDiscountType(value);
            if (dict.TryGetValue("price", out value)) Price = value as string;
            if (dict.TryGetValue("localeIdentifier", out value)) LocaleIdentifier = value as string;
            if (dict.TryGetValue("paymentMode", out value)) PaymentMode = FormatPaymentMode(value);
            if (dict.TryGetValue("numberOfPeriods", out value)) NumberOfPeriods = Convert.ToInt32(value);

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
                   $"{nameof(SubscriptionPeriod)}: {SubscriptionPeriod}";
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