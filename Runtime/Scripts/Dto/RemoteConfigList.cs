using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class RemoteConfigList
    {
        public readonly List<RemoteConfig> RemoteConfigs;

        public RemoteConfigList(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("remoteConfigs", out object value) && value is List<object> offerings)
            {
                RemoteConfigs = Mapper.ConvertObjectsList<RemoteConfig>(offerings);
            }
        }

        [CanBeNull]
        public RemoteConfig RemoteConfigForContextKey(String contextKey)
        {
            return FindRemoteConfig(contextKey);
        }

        [CanBeNull]
        public RemoteConfig RemoteConfigForEmptyContextKey()
        {
            return FindRemoteConfig(null);
        }

        [CanBeNull]
        private RemoteConfig FindRemoteConfig([CanBeNull] String contextKey)
        {
            return RemoteConfigs.Find(remoteConfig => remoteConfig.Source.ContextKey == contextKey);
        }

        public override string ToString()
        {
            return $"{nameof(RemoteConfigs)}: {Utils.PrintObjectList(RemoteConfigs)}";
        }
    }
}