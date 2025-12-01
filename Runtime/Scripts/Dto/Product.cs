using System;
using System.Collections.Generic;
using System.Globalization;
using JetBrains.Annotations;
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
        [CanBeNull] public readonly string StoreId;

        /// Identifier of the base plan for Google product.
        [CanBeNull] public readonly string BasePlanId; 

        /// Google Play Store details of this product.
        /// Android only. Null for iOS, or if the product was not found.
        [CanBeNull] public readonly ProductStoreDetails StoreDetails;

        /// Associated SKProduct.
        /// Available for iOS only.
        [CanBeNull] public readonly SKProduct SkProduct;

        /// Associated Offering Id
        [CanBeNull] public readonly string OfferingId;

        /// For Android - the subscription base plan duration.
        /// For iOS - the duration of the <see cref="SkProduct"/>.
        /// Null, if it's not a subscription product or the product was not found in the store.
        [CanBeNull] public readonly SubscriptionPeriod SubscriptionPeriod;

        /// The subscription trial duration of the default offer for Android or of the product for iOS.
        /// See <see cref="ProductStoreDetails.DefaultSubscriptionOfferDetails"/> for the information on how we
        /// choose the default offer for Android.
        /// Null, if it's not a subscription product or the product was not found the store.
        [CanBeNull] public readonly SubscriptionPeriod TrialPeriod;

        /// The calculated type of this product based on the store information.
        /// On Android uses <see cref="StoreDetails"/>.
        /// On iOS uses <see cref="SkProduct"/> information.
        public readonly QProductType Type;

        /// Formatted price of for this product, including the currency sign.
        [CanBeNull] public readonly string PrettyPrice;

        /// Price of the product
        public readonly double Price;

        /// Store Product currency code, such as USD
        [CanBeNull] public readonly string CurrencyCode;

        /// Store product title
        [CanBeNull] public readonly string StoreTitle;

        /// Store product description
        [CanBeNull] public readonly string StoreDescription;

        /// Formatted introductory price of a subscription, including its currency sign, such as €2.99
        [CanBeNull] public readonly string PrettyIntroductoryPrice;

        public Product(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) QonversionId = value as string;
            if (dict.TryGetValue("storeId", out value)) StoreId = value as string;
            if (dict.TryGetValue("basePlanId", out value)) BasePlanId = value as string;
            if (dict.TryGetValue("offeringId", out value)) OfferingId = value as string;

            if (dict.TryGetValue("subscriptionPeriod", out value) && value is Dictionary<string, object> subscriptionPeriod)
            {
                SubscriptionPeriod = new SubscriptionPeriod(subscriptionPeriod);
            }
            
            if (dict.TryGetValue("trialPeriod", out value) && value is Dictionary<string, object> trialPeriod)
            {
                TrialPeriod = new SubscriptionPeriod(trialPeriod);
            }

            if (dict.TryGetValue("prettyPrice", out value)) PrettyPrice = value as string;
            if (dict.TryGetValue("type", out value)) Type = Mapper.FormatType(value);

            if (Application.platform == RuntimePlatform.Android)
            {
                long priceMicros = 0;
                if (dict.TryGetValue("storeDetails", out value) && value is Dictionary<string, object> productStoreDetails)
                {
                    StoreDetails = new ProductStoreDetails(productStoreDetails);
                    
                    StoreTitle = StoreDetails.Title;
                    StoreDescription = StoreDetails.Description;

                    ProductOfferDetails defaultOffer = StoreDetails.DefaultSubscriptionOfferDetails;
                    ProductInAppDetails inAppOffer = StoreDetails.InAppOfferDetails;
                    if (defaultOffer != null)
                    {
                        priceMicros = defaultOffer.BasePlan?.Price?.PriceAmountMicros ?? 0;
                        CurrencyCode = defaultOffer.BasePlan?.Price?.PriceCurrencyCode;
                        PrettyIntroductoryPrice = defaultOffer.IntroPhase?.Price?.FormattedPrice;
                    } else if (inAppOffer != null)
                    {
                        priceMicros = inAppOffer.Price.PriceAmountMicros;
                        CurrencyCode = inAppOffer.Price.PriceCurrencyCode;
                        PrettyIntroductoryPrice = null;
                    }
                }

                Price = (double) priceMicros / Constants.PriceMicrosRatio;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer && dict.TryGetValue("skProduct", out value))
            {
                if (value is Dictionary<string, object> skProduct)
                {
                    SkProduct = new SKProduct(skProduct);

                    double parsedPrice;
                    if (double.TryParse(SkProduct.Price, NumberStyles.Any, CultureInfo.InvariantCulture, out parsedPrice))
                    {
                        Price = parsedPrice;
                    }
                    else
                    {
                        Debug.LogError("Failed to parse SKProduct price: " + SkProduct.Price);
                    }

                    CurrencyCode = SkProduct.CurrencyCode;
                    StoreTitle = SkProduct.LocalizedTitle;
                    StoreDescription = SkProduct.LocalizedDescription;
                    if (SkProduct.IntroductoryPrice != null)
                    {
                        PrettyIntroductoryPrice = SkProduct.IntroductoryPrice.CurrencySymbol +
                                                  SkProduct.IntroductoryPrice.Price;
                    }
                }
            }
        }

        /// <summary>
        /// Converts this product to purchase model to pass to {@link Qonversion.purchase}.
        /// <param name="offerId">Concrete Android offer identifier if necessary.
        ///    If the products' base plan id is specified, but offer id is not provided for
        ///    purchase, then default offer will be used.
        ///    Ignored if base plan id is not specified.
        ///    Ignored for iOS.
        /// To know how we choose the default offer, see {@link ProductStoreDetails.defaultSubscriptionOfferDetails}.
        /// </param>
        /// <returns>Purchase model to pass to the purchase method.</returns>
        /// </summary>
        public PurchaseModel ToPurchaseModel([CanBeNull] string offerId = null) {
            return new PurchaseModel(QonversionId, offerId);
        }

        /// <summary>
        /// Converts this product to purchase model to pass to {@link Qonversion.purchase}.
        /// <param name="offer">Concrete Android offer which you'd like to purchase.</param>
        /// <returns>Purchase model to pass to the purchase method.</returns>
        /// </summary>
        public PurchaseModel ToPurchaseModel(ProductOfferDetails offer) {
            PurchaseModel model = ToPurchaseModel(offer.OfferId);
            // Remove offer for the case when provided offer details are for bare base plan.
            if (offer.OfferId == null) {
                model.RemoveOffer();
            }

            return model;
        }


        /// <summary>
        /// Android only.
        ///
        /// Converts this product to purchase update (upgrade/downgrade) model
        /// to pass to <see cref="IQonversion.UpdatePurchase"/>.
        /// <param name="oldProductId">Qonversion product identifier from which
        ///     the upgrade/downgrade will be initialized.
        /// </param>
        /// <param name="updatePolicy">Purchase update policy.</param>
        /// <returns>Purchase model to pass to the update purchase method.</returns>
        /// </summary>
        public PurchaseUpdateModel ToPurchaseUpdateModel(
            string oldProductId,
            PurchaseUpdatePolicy? updatePolicy = null
        ) {
            return new PurchaseUpdateModel(QonversionId, oldProductId, updatePolicy);
        }
        
        public override string ToString()
        {
            return $"{nameof(QonversionId)}: {QonversionId}, " +
                   $"{nameof(StoreId)}: {StoreId}, " +
                   $"{nameof(BasePlanId)}: {BasePlanId}, " +
                   $"{nameof(Type)}: {Type}, " +
                   $"{nameof(SubscriptionPeriod)}: {SubscriptionPeriod}, " +
                   $"{nameof(TrialPeriod)}: {TrialPeriod}, " +
                   $"{nameof(PrettyPrice)}: {PrettyPrice}, " +
                   $"{nameof(SkProduct)}: {SkProduct}, " +
                   $"{nameof(StoreDetails)}: {StoreDetails}, " +
                   $"{nameof(OfferingId)}: {OfferingId}, " +
                   $"{nameof(StoreTitle)}: {StoreTitle}, " +
                   $"{nameof(StoreDescription)}: {StoreDescription}, " +
                   $"{nameof(Price)}: {Price}, " +
                   $"{nameof(CurrencyCode)}: {CurrencyCode}, " +
                   $"{nameof(PrettyIntroductoryPrice)}: {PrettyIntroductoryPrice}";
        }
    }
}