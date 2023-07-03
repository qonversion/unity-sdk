using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class RemoteConfig
    {
        public readonly Dictionary<object, object> Payload;
        public readonly Experiment Experiment;

        public RemoteConfig(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("payload", out object value)) Id = value as Dictionary<object, object>;
            if (dict.TryGetValue("products", out value))
            {
                if (value is List<object> products) Products = Mapper.ConvertObjectsList<Product>(products);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Payload)}: {Payload}, " +
                   $"{nameof(Experiment)}: {Experiment}";
        }
    }
}