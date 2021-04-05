﻿using System;
using System.Collections.Generic;

namespace QonversionUnity
{
    public class Offering
    {
        public readonly string Id;
        public readonly QOfferingTag Tag;
        public readonly List<Product> Products;

        public Offering(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("tag", out value)) Tag = FormatOfferingTag(value);
            if (dict.TryGetValue("products", out value))
            {
                if (value is List<object> products) Products = Mapper.ConvertObjectsList<Product>(products);
            }
        }

        public Product ProductForID(string id)
        {
            return Products.Find(product => product.QonversionId == id);
        }

        public QOfferingTag FormatOfferingTag(object tag) =>
                (QOfferingTag)Convert.ToInt32(tag);

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(Tag)}: {Tag}, " +
                   $"{nameof(Products)}: {Utils.PrintObjectList(Products)}";
        }
    }

    public enum QOfferingTag
    {
        None,
        Main
    }
}