using System.Collections.Generic;
using JetBrains.Annotations;

namespace QonversionUnity
{
    public class Offerings
    {
        [CanBeNull] public readonly Offering Main;
        public readonly List<Offering> AvailableOfferings;

        public Offerings(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("main", out object value) && value is Dictionary<string, object> offering)
            {
                Main = new Offering(offering);
            }
            if (dict.TryGetValue("availableOfferings", out value) && value is List<object> offerings)
            {
                AvailableOfferings = Mapper.ConvertObjectsList<Offering>(offerings);
            }
        }

        public Offering OfferingForID(string id)
        {
            return AvailableOfferings.Find(offering => offering.Id == id);
        }

        public override string ToString()
        {
            return $"{nameof(Main)}: {Main}, " +
            $"{nameof(AvailableOfferings)}: {Utils.PrintObjectList(AvailableOfferings)}";
        }
    }   
}