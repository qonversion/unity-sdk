using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace QonversionUnity
{
    public class UserProperties
    {
        /// <summary>
        /// List of all user properties.
        /// </summary>
        public readonly List<UserProperty> Properties;
        
        /// <summary>
        /// List of user properties, set for the Qonversion defined keys.
        /// This is a subset of all <see cref="Properties"/> list.
        /// <see cref="IQonversion.SetUserProperty"/>.
        /// </summary>
        public readonly List<UserProperty> DefinedProperties;
        
        /// <summary>
        /// List of user properties, set for custom keys.
        /// This is a subset of all <see cref="Properties"/> list.
        /// <see cref="IQonversion.SetCustomUserProperty"/>.
        /// </summary>
        public readonly List<UserProperty> CustomProperties;

        /// <summary>
        /// Map of all user properties.
        /// This is a flattened version of the <see cref="Properties"/> list as a key-value map.
        /// </summary>
        public readonly Dictionary<string, string> FlatPropertiesMap;

        /// <summary>
        /// Map of user properties, set for the Qonversion defined keys.
        /// This is a flattened version of the <see cref="DefinedProperties"/> list as a key-value map.
        /// <see cref="IQonversion.SetUserProperty"/>.
        /// </summary>
        public readonly Dictionary<UserPropertyKey, string> FlatDefinedPropertiesMap;

        /// <summary>
        /// Map of user properties, set for custom keys.
        /// This is a flattened version of the <see cref="CustomProperties"/> list as a key-value map.
        /// <see cref="IQonversion.SetCustomUserProperty"/>.
        /// </summary>
        public readonly Dictionary<string, string> FlatCustomPropertiesMap;

        public UserProperties(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("properties", out var value) && value is List<object> properties)
            {
                Properties = Mapper.ConvertObjectsList<UserProperty>(properties);
            }
            else
            {
                Debug.LogError("Failed to parse user properties array, assigning empty list.");
                Properties = new List<UserProperty>();
            }
            DefinedProperties = CalculateDefinedProperties();
            CustomProperties = CalculateCustomProperties();
            FlatPropertiesMap = CalculateFlatPropertiesMap();
            FlatDefinedPropertiesMap = CalculateFlatDefinedPropertiesMap();
            FlatCustomPropertiesMap = CalculateFlatCustomPropertiesMap();
        }

        /// <summary>
        /// Searches for a property with the given property <see cref="key"/> in all properties list.
        /// </summary>
        [CanBeNull]
        public UserProperty GetProperty(string key)
        {
            return Properties.Find(property => property.Key == key);
        }

        /// <summary>
        /// Searches for a property with the given Qonversion defined property <see cref="key"/>
        /// in defined properties list.
        /// </summary>
        [CanBeNull]
        public UserProperty GetDefinedProperty(UserPropertyKey key)
        {
            return DefinedProperties.Find(property => property.DefinedKey == key);
        }

        private List<UserProperty> CalculateDefinedProperties()
        {
            return Properties.FindAll(
                userProperty => userProperty.DefinedKey != UserPropertyKey.Custom
            );
        }

        private List<UserProperty> CalculateCustomProperties()
        {
            return Properties.FindAll(
                userProperty => userProperty.DefinedKey == UserPropertyKey.Custom
            );
        }

        private Dictionary<string, string> CalculateFlatPropertiesMap()
        {
            Dictionary<string, string> flatPropertiesMap = new Dictionary<string, string>();
            foreach (var property in Properties)
            {
                flatPropertiesMap[property.Key] = property.Value;
            }
            return flatPropertiesMap;
        }

        private Dictionary<UserPropertyKey, string> CalculateFlatDefinedPropertiesMap()
        {
            Dictionary<UserPropertyKey, string> flatDefinedPropertiesMap = new Dictionary<UserPropertyKey, string>();
            foreach (var property in Properties)
            {
                if (property.DefinedKey != UserPropertyKey.Custom)
                {
                    flatDefinedPropertiesMap[property.DefinedKey] = property.Value;
                }
            }
            return flatDefinedPropertiesMap;
        }

        private Dictionary<string, string> CalculateFlatCustomPropertiesMap()
        {
            Dictionary<string, string> flatCustomPropertiesMap = new Dictionary<string, string>();
            foreach (var property in Properties)
            {
                if (property.DefinedKey == UserPropertyKey.Custom)
                {
                    flatCustomPropertiesMap[property.Key] = property.Value;
                }
            }
            return flatCustomPropertiesMap;
        }
    }
}
