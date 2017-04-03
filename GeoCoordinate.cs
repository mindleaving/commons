using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Commons
{
    [DataContract]
    public class GeoCoordinate : IEquatable<GeoCoordinate>
    {
        public GeoCoordinate(double latitude, double longitude)
        {
            if(latitude > 90 || latitude < -90)
                throw new ArgumentOutOfRangeException(nameof(latitude));
            if(longitude < -180 || longitude > 180)
                throw new ArgumentOutOfRangeException(nameof(longitude));
            Latitude = latitude;
            Longitude = longitude;
        }

        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }

        public bool Equals(GeoCoordinate other)
        {
            if (ReferenceEquals(other, null))
                return false;
            return Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override string ToString()
        {
            return $"{Latitude.ToString("F5", CultureInfo.InvariantCulture)};{Longitude.ToString("F5", CultureInfo.InvariantCulture)}";
        }
    }
}
