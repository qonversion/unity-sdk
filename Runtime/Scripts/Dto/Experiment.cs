using System.Collections.Generic;

namespace QonversionUnity
{
    public class Experiment
    {
        public readonly string Id;
        public readonly string Name;
        public readonly ExperimentGroup Group;


        public Experiment(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("id", out object value)) Id = value as string;
            if (dict.TryGetValue("name", out value)) Name = value as string;
            if (dict.TryGetValue("group", out value) && value is Dictionary<string, object> group)
            {
                Group = new ExperimentGroup(group);
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, " +
                   $"{nameof(Name)}: {Name}, " +
                   $"{nameof(Group)}: {Group}";
        }
    }
}