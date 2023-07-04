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
            if (dict.TryGetValue("payload", out object value)) Payload = value as Dictionary<object, object>;
            if (dict.TryGetValue("experiment", out value) && value is Dictionary<string, object> experimentInfo)
            {
                Experiment= new Experiment(experimentInfo);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Payload)}: {Payload}, " +
                   $"{nameof(Experiment)}: {Experiment}";
        }
    }
}