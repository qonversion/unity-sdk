using System.Collections.Generic;

namespace QonversionUnity
{
    public class QOfferings
    {
        public readonly QOffering Main;
        public readonly List<QOffering> AvailableOfferings;

        public QOfferings(Dictionary<string, object> dict)
        {
            if (dict.TryGetValue("main", out object value) && value is Dictionary<string, object> offering)
            {
                Main = new QOffering(offering);
            }
            if (dict.TryGetValue("availableOfferings", out value) && value is List<object> offerings)
            {
                AvailableOfferings = Mapper.ConvertObjectsList<QOffering>(offerings);
            }
        }

        public QOffering OfferingForID(string id)
        {
            return AvailableOfferings.Find(offering => offering.Id == id);
        }

        public override string ToString()
        {
            return $"{nameof(Main)}: {Main}, " +
                   $"{nameof(AvailableOfferings)}: {AvailableOfferings}";
        }
    }   
}