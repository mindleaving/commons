using System.Runtime.Serialization;

namespace Commons.CoordinateTransform
{
    [DataContract]
    public class DistortionParameters
    {
        [DataMember]
        public double Radial2 { get; set; }
        [DataMember]
        public double Radial4 { get; set; }
    }
}