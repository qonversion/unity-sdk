using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class ExperimentGroup
    {
        public readonly string Id;
        public readonly string Name;
        public readonly ExperimentGroupType Type;

        public ExperimentGroup(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("name", out value)) Name = value as string;
            if (dict.TryGetValue("type", out value)) Type = FormatGroupType(value);
        }

        public ExperimentGroupType FormatGroupType(object typeValue)
        {
            string type = typeValue as string;
            if (type == "control")
            {
                return ExperimentGroupType.Control;
            }
            else if (type == "treatment")
            {
                return ExperimentGroupType.Treatment;
            }

            return ExperimentGroupType.Unknown;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(Name)}: {Name}, " +
                   $"{nameof(Type)}: {Type}";
        }
    }

    public enum ExperimentGroupType
    {
        Unknown = -1,
        Control = 0,
        Treatment = 1
    }
}