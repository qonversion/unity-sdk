using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class RemoteConfigurationSource
    {

        public readonly string Id;
        public readonly string Name;
        public readonly RemoteConfigurationSourceType Type;
        public readonly RemoteConfigurationAssignmentType AssignmentType;
        [CanBeNull] public readonly string ContextKey;


        public RemoteConfigurationSource(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("name", out value)) Name = value as string;
            if (dict.TryGetValue("assignmentType", out value)) AssignmentType = FormatAssignmentType(value);
            if (dict.TryGetValue("type", out value)) Type = FormatSourceType(value);
            if (dict.TryGetValue("contextKey", out value)) ContextKey = value as string;
        }

        public RemoteConfigurationAssignmentType FormatAssignmentType(object typeValue)
        {
            string type = typeValue as string;
            if (type == "auto")
            {
                return RemoteConfigurationAssignmentType.Auto;
            }
            else if (type == "manual")
            {
                return RemoteConfigurationAssignmentType.Manual;
            }

            return RemoteConfigurationAssignmentType.Unknown;
        }

        public RemoteConfigurationSourceType FormatSourceType(object typeValue)
        {
            string type = typeValue as string;
            if (type == "experiment_control_group")
            {
                return RemoteConfigurationSourceType.ExperimentControlGroup;
            }
            else if (type == "experiment_treatment_group")
            {
                return RemoteConfigurationSourceType.ExperimentTreatmentGroup;
            }
            else if (type == "remote_configuration")
            {
                return RemoteConfigurationSourceType.RemoteConfiguration;
            }

            return RemoteConfigurationSourceType.Unknown;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(Name)}: {Name}, " +
                   $"{nameof(AssignmentType)}: {AssignmentType}, " +
                   $"{nameof(Type)}: {Type}, " + 
                   $"{nameof(ContextKey)}: {ContextKey}";
        }
    }

    public enum RemoteConfigurationAssignmentType
    {
        Unknown,
        Auto,
        Manual
    }

    public enum RemoteConfigurationSourceType
    {
        Unknown,
        ExperimentControlGroup,
        ExperimentTreatmentGroup,
        RemoteConfiguration
    }
}
