using System.Collections.Generic;

namespace QonversionUnity
{
    public class PromotionalOffer
    {
        public readonly SKProductDiscount ProductDiscount;
        public readonly SKPaymentDiscount PaymentDiscount;

        public PromotionalOffer(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("productDiscount", out object value) && value is Dictionary<string, object> productDiscount)
            {
                ProductDiscount = new SKProductDiscount(productDiscount);
            }
            if (dict.TryGetValue("paymentDiscount", out value) && value is Dictionary<string, object> paymentDiscount)
            {
                PaymentDiscount = new SKPaymentDiscount(paymentDiscount);
            }
        }

        public override string ToString()
        {
            return $"{nameof(ProductDiscount)}: {ProductDiscount}, " +
                   $"{nameof(PaymentDiscount)}: {PaymentDiscount}";
        }
    }
}