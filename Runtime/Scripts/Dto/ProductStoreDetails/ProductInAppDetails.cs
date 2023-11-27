using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class ProductInAppDetails
    {
        public readonly ProductPrice Price;

        public ProductInAppDetails(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("price", out object value) && value is Dictionary<string, object> price)
            {
                Price = new ProductPrice(price);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Price)}: {Price.ToString()}";
        }
    }
}