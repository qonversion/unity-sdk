using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    /// <summary>
    /// This class contains all the information about the Google in-app product details.
    /// </summary>
    public class ProductInAppDetails
    {
        /// The price of an in-app product.
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
            return $"{nameof(Price)}: {Price}";
        }
    }
}