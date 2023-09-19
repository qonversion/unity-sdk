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
        public readonly RemoteConfigurationSource Source;

        public RemoteConfig(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("payload", out object value) && value is Dictionary<string, object>) Payload = value as Dictionary<string, object>;
            if (dict.TryGetValue("experiment", out value) && value is Dictionary<string, object> experimentInfo)
            {
                Experiment = new Experiment(experimentInfo);
            }
            if (dict.TryGetValue("source", out value) && value is Dictionary<string, object> sourceInfo)
            {
                Source = new RemoteConfigurationSource(sourceInfo);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Payload)}: {Payload}, " +
                   $"{nameof(Experiment)}: {Experiment}, " +
                   $"{nameof(Source)}: {Source}";
        }
    }
}