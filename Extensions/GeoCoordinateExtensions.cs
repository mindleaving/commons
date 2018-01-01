using System;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class GeoCoordinateExtensions
    {
        public static UnitValue GetDistanceTo(this GeoCoordinate coordinate1, GeoCoordinate coordinate2)
        {
            if(coordinate1 == null) throw new ArgumentNullException(nameof(coordinate1));
            if (coordinate2 == null) throw new ArgumentNullException(nameof(coordinate2));

            var dotNetGeoCoordinate1 = new System.Device.Location.GeoCoordinate(coordinate1.Latitude, coordinate1.Longitude);
            var dotNetGeoCoordinate2 = new System.Device.Location.GeoCoordinate(coordinate2.Latitude, coordinate2.Longitude);
            var distance = dotNetGeoCoordinate1.GetDistanceTo(dotNetGeoCoordinate2).To(Unit.Meter);
            return distance;
        }
    }
}
