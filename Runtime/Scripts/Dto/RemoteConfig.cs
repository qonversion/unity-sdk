using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace QonversionUnity
{
    public class RemoteConfig
    {
        public readonly Dictionary<string, object> Payload;
        [CanBeNull] public readonly Experiment Experiment;

        public RemoteConfig(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("payload", out object value) && value is Dictionary<string, object>) Payload = value as Dictionary<string, object>;
            if (dict.TryGetValue("experiment", out value) && value is Dictionary<string, object> experimentInfo)
            {
                Experiment = new Experiment(experimentInfo);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Payload)}: {Payload}, " +
                   $"{nameof(Experiment)}: {Experiment}";
        }
    }
}