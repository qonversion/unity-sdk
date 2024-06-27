using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace QonversionUnity
{
    /// <summary>
    /// This class contains all the information about the concrete Google product,
    /// either subscription or in-app. In case of a subscription also determines concrete base plan.
    /// </summary>
    public class ProductStoreDetails
    {
        /// Identifier of the base plan to which these details relate.
        /// Null for in-app products.
        public readonly string BasePlanId;

        /// Identifier of the subscription or the in-app product.
        public readonly string ProductId;
        
        /// Name of the subscription or the in-app product.
        public readonly string Name;
        
        /// Title of the subscription or the in-app product.
        /// The title includes the name of the app.
        public readonly string Title;
        
        /// Description of the subscription or the in-app product.        
        public readonly string Description;
        
        /// Offer details for the subscription.
        /// Offer details contain all the available variations of purchase offers,
        /// including both base plan and eligible base plan + offer combinations
        /// from Google Play Console for current <see cref="BasePlanId"/>.
        /// Null for in-app products.
        [CanBeNull] public readonly List<ProductOfferDetails> SubscriptionOfferDetails;
        
        /// The most profitable subscription offer for the client in our opinion from all the available offers.
        /// We calculate the cheapest price for the client by comparing all the trial or intro phases
        /// and the base plan.
        [CanBeNull] public readonly ProductOfferDetails DefaultSubscriptionOfferDetails;
        
        /// Subscription offer details containing only the base plan without any offer.
        [CanBeNull] public readonly ProductOfferDetails BasePlanSubscriptionOfferDetails;
        
        /// Offer details for the in-app product.
        /// Null for subscriptions.
        [CanBeNull] public readonly ProductInAppDetails InAppOfferDetails;
        
        /// True, if there is any eligible offer with a trial
        /// for this subscription and base plan combination.
        /// False otherwise or for an in-app product.
        public readonly bool HasTrialOffer;
        
        /// True, if there is any eligible offer with an intro price
        /// for this subscription and base plan combination.
        /// False otherwise or for an in-app product.
        public readonly bool HasIntroOffer;
        
        /// True, if there is any eligible offer with a trial or an intro price
        /// for this subscription and base plan combination.
        /// False otherwise or for an in-app product.
        public readonly bool HasTrialOrIntroOffer;
        
        /// The calculated type of the current product.
        public readonly QProductType ProductType;
        
        /// True, if the product type is InApp.
        public readonly bool IsInApp;
        
        /// True, if the product type is Subscription.
        public readonly bool IsSubscription;

        /// True, if the subscription product is prepaid, which means that users pay in advance -
        /// they will need to make a new payment to extend their plan.
        public readonly bool IsPrepaid;

        /// True, if the subscription product is installment, which means that users commit
        /// to pay for a specified amount of periods every month.
        public readonly bool IsInstallment;

        public ProductStoreDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("productId", out object value)) ProductId = value as string;
            if (dict.TryGetValue("basePlanId", out value)) BasePlanId = value as string;
            if (dict.TryGetValue("name", out value)) Name = value as string;
            if (dict.TryGetValue("title", out value)) Title = value as string;
            if (dict.TryGetValue("description", out value)) Description = value as string;
            
            if (dict.TryGetValue("subscriptionOfferDetails", out value) && value is object[] offerDetailsList)
            {
                List<ProductOfferDetails> result = new List<ProductOfferDetails>();
                foreach (object offerDetails in offerDetailsList)
                {
                    if (offerDetails is Dictionary<string, object>)
                    {
                        ProductOfferDetails details = new ProductOfferDetails(offerDetails as Dictionary<string, object>);
                        result.Add(details);
                    }
                }

                SubscriptionOfferDetails = result;
            }

            if (dict.TryGetValue("defaultSubscriptionOfferDetails", out value) && value is Dictionary<string, object> defaultSubscriptionOfferDetails)
            {
                DefaultSubscriptionOfferDetails = new ProductOfferDetails(defaultSubscriptionOfferDetails);
            }

            if (dict.TryGetValue("basePlanSubscriptionOfferDetails", out value) && value is Dictionary<string, object> basePlanSubscriptionOfferDetails)
            {
                BasePlanSubscriptionOfferDetails = new ProductOfferDetails(basePlanSubscriptionOfferDetails);
            }

            if (dict.TryGetValue("inAppOfferDetails", out value) && value is Dictionary<string, object> inAppOfferDetails)
            {
                InAppOfferDetails = new ProductInAppDetails(inAppOfferDetails);
            }

            if (dict.TryGetValue("hasTrialOffer", out value)) HasTrialOffer = (bool)value;
            if (dict.TryGetValue("hasIntroOffer", out value)) HasIntroOffer = (bool)value;
            if (dict.TryGetValue("hasTrialOrIntroOffer", out value)) HasTrialOrIntroOffer = (bool)value;
            if (dict.TryGetValue("productType", out value)) ProductType = Mapper.FormatType(value);
            if (dict.TryGetValue("isInApp", out value)) IsInApp = (bool)value;
            if (dict.TryGetValue("isSubscription", out value)) IsSubscription = (bool)value;
            if (dict.TryGetValue("isPrepaid", out value)) IsPrepaid = (bool)value;
            if (dict.TryGetValue("isInstallment", out value)) IsInstallment = (bool)value;
        }

        public override string ToString()
        {
            string subscriptionOfferDetails = "";
            if (SubscriptionOfferDetails != null)
            {
                subscriptionOfferDetails = string.Join(", ", SubscriptionOfferDetails.Select(obj => obj.ToString()));
            }

            return $"{nameof(BasePlanId)}: {BasePlanId}, " +
                   $"{nameof(ProductId)}: {ProductId}, " +
                   $"{nameof(Name)}: {Name}, " +
                   $"{nameof(Title)}: {Title}, " +
                   $"{nameof(Description)}: {Description}, " +
                   $"{nameof(SubscriptionOfferDetails)}: {subscriptionOfferDetails}, " +
                   $"{nameof(DefaultSubscriptionOfferDetails)}: {DefaultSubscriptionOfferDetails}, " +
                   $"{nameof(BasePlanSubscriptionOfferDetails)}: {BasePlanSubscriptionOfferDetails}, " +
                   $"{nameof(InAppOfferDetails)}: {InAppOfferDetails}, " +
                   $"{nameof(HasTrialOffer)}: {HasTrialOffer}, " +
                   $"{nameof(HasIntroOffer)}: {HasIntroOffer}, " +
                   $"{nameof(HasTrialOrIntroOffer)}: {HasTrialOrIntroOffer}, " +
                   $"{nameof(ProductType)}: {ProductType}, " +
                   $"{nameof(IsInApp)}: {IsInApp}, " +
                   $"{nameof(IsSubscription)}: {IsSubscription}, " +
                   $"{nameof(IsPrepaid)}: {IsPrepaid}" +
                   $"{nameof(IsInstallment)}: {IsInstallment}";
        }
    }
}