using System;
using System.Collections.Generic;
using UnityEngine;

namespace QonversionUnity
{
    public class Product
    {
        /// Product ID created in Qonversion Dashboard.
        [Tooltip("Create Products: https://qonversion.io/docs/create-products")]
        public readonly string QonversionId;

        /// App Store ID or Google Play ID
        [Tooltip("Create Products: (https://qonversion.io/docs/create-products")]
        public readonly string StoreId;

        /// Product type.
        [Tooltip("Products types: https://qonversion.io/docs/product-types")]
        public readonly QProductType Type;

        /// Product duration.

        [Tooltip("Products durations: https://qonversion.io/docs/product-durations")]
        public readonly QProductDuration Duration;

        /// Localized price, e.g. 4.99 USD
        public readonly string PrettyPrice;

        /// Trial duration of the subscription
        public readonly QTrialDuration TrialDuration;

        /// Associated SKProduct.
        /// Available for iOS only.
        public readonly SKProduct SkProduct;

        /// Associated SkuDetails.
        /// Available for Android only.
        public readonly SkuDetails SkuDetails;

        public Product(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) QonversionId = value as string;
            if (dict.TryGetValue("store_id", out value)) StoreId = value as string;
            if (dict.TryGetValue("type", out value)) Type = FormatType(value);
            if (dict.TryGetValue("duration", out value)) Duration = FormatDuration(value);
            if (dict.TryGetValue("trialDuration", out value)) TrialDuration = FormatTrialDuration(value);
            if (dict.TryGetValue("prettyPrice", out value)) PrettyPrice = value as string;

            if (dict.TryGetValue("storeProduct", out value))
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    if (value is Dictionary<string, object> skuDetails) SkuDetails = new SkuDetails(skuDetails);
                }
                else
                {
                    if (value is Dictionary<string, object> skProduct) SkProduct = new SKProduct(skProduct);
                }
            }
        }

        public override string ToString()
        {
            return $"{nameof(QonversionId)}: {QonversionId}, " +
                   $"{nameof(StoreId)}: {StoreId}, " +
                   $"{nameof(Type)}: {Type}, " +
                   $"{nameof(Duration)}: {Duration}, " +
                   $"{nameof(TrialDuration)}: {TrialDuration}, " +
                   $"{nameof(PrettyPrice)}: {PrettyPrice}, " +
                   $"{nameof(SkProduct)}: {SkProduct}, " +
                   $"{nameof(SkuDetails)}: {SkuDetails}";
        }

        private QProductType FormatType(object productType) =>
            (QProductType)Convert.ToInt32(productType);

        private QProductDuration FormatDuration(object duration)
        {
            Int32 value = Convert.ToInt32(duration);
            var result = QProductDuration.Unknown;

            switch (value)
            {
                case 0:
                    result = QProductDuration.Weekly;
                    break;
                case 1:
                    result = QProductDuration.Monthly;
                    break;
                case 2:
                    result = QProductDuration.ThreeMonths;
                    break;
                case 3:
                    result = QProductDuration.SixMonths;
                    break;
                case 4:
                    result = QProductDuration.Annual;
                    break;
                case 5:
                    result = QProductDuration.Lifetime;
                    break;
                default:
                    result = QProductDuration.Unknown;
                    break;
            }

            return result;
        }

        private QTrialDuration FormatTrialDuration(object trialDuration) =>
            (QTrialDuration)Convert.ToInt32(trialDuration);
    }

    public enum QProductDuration
    {
        Unknown,
        Weekly,
        Monthly,
        ThreeMonths,
        SixMonths,
        Annual,
        Lifetime
    }

    public enum QProductType
    {
        /// Provides access to content on a recurring basis with a free introductory offer
        Trial,
        /// Provides access to content on a recurring basis
        Subscription,
        /// Content that users can purchase with a single, non-recurring charge
        InApp
    }

    public enum QTrialDuration
    {
        NotAvailable = -1,
        Unknown = 0,
        ThreeDays = 1,
        Week = 2,
        TwoWeeks = 3,
        Month = 4,
        TwoMonths = 5,
        ThreeMonths = 6,
        SixMonths = 7,
        Year = 8,
        Other = 9,
    }
}