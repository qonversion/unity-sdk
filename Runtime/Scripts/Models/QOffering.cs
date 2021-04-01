using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class QOffering
    {
        public readonly string Id;
        public readonly QOfferingTag Tag;
        public readonly List<QProduct> Products;

        public QOffering(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("tag", out value)) Tag = FormatOfferingTag(value);
            if (dict.TryGetValue("products", out value))
            {
                if (value is List<object> products) Products = Mapper.ConvertObjectsList<QProduct>(products);
            }
        }

        public QProduct ProductForID(string id)
        {
            return Products.Find(product => product.QonversionId == id);
        }

        public QOfferingTag FormatOfferingTag(object tag) =>
                (QOfferingTag)Convert.ToInt32(tag);

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(Tag)}: {Tag}, " +
                   $"{nameof(Products)}: {Products}";
        }
    }

    public enum QOfferingTag
    {
        None,
        Main
    }
}