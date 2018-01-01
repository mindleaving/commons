using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Commons
{
    // Added to Commons, because it is referenced as KnownType
    // for graph Edge in deserialization of graph
    [DataContract]
    public struct TaxiEdge
    {
        [DataMember]
        public string Identifier { get; set; }
        [DataMember]
        public bool CrossesRunway { get; set; }
        [DataMember]
        public List<string> RunwayNames { get; set; }
        [DataMember]
        public bool IsRunwayEntryZone { get; set; }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
