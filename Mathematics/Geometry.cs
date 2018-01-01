using Commons.Extensions;
using Commons.Physics;

namespace Commons.Mathematics
{
    public static class Geometry
    {
        public static UnitValue DistanceToLine(this GeoCoordinate point, GeoCoordinate linePoint1, GeoCoordinate linePoint2)
        {
            // Assume short distances, such that euclidean coordinate system is a good approximation
            var distanceToLinePoint1 = point.GetDistanceTo(linePoint1).In(Unit.Meter);
            var distanceToLinePoint2 = point.GetDistanceTo(linePoint2).In(Unit.Meter);
            if (distanceToLinePoint1 == 0 || distanceToLinePoint2 == 0)
                return 0.To(Unit.Meter);
            var lineLength = linePoint1.GetDistanceTo(linePoint2).In(Unit.Meter);
            if (lineLength == 0)
                return distanceToLinePoint1.To(Unit.Meter);

            var cosAngle = (lineLength*lineLength + distanceToLinePoint1*distanceToLinePoint1 - distanceToLinePoint2*distanceToLinePoint2)
                / (2*lineLength*distanceToLinePoint1);
            var angle = System.Math.Acos(cosAngle);
            var distanceToLine = System.Math.Sin(angle) * distanceToLinePoint1.To(Unit.Meter);
            return distanceToLine;
        }

        public static double DistanceToLine(this Point2D point, Point2D linePoint1, Point2D linePoint2)
        {
            var distanceToLinePoint1 = point.DistanceTo(linePoint1);
            var distanceToLinePoint2 = point.DistanceTo(linePoint2);
            if (distanceToLinePoint1 == 0 || distanceToLinePoint2 == 0)
                return 0;
            var lineLength = linePoint1.DistanceTo(linePoint2);
            if (lineLength == 0)
                return distanceToLinePoint1;

            var cosAngle = (lineLength * lineLength + distanceToLinePoint1 * distanceToLinePoint1 - distanceToLinePoint2 * distanceToLinePoint2)
                / (2 * lineLength * distanceToLinePoint1);
            var angle = System.Math.Acos(cosAngle);
            var distanceToLine = System.Math.Sin(angle) * distanceToLinePoint1;
            return distanceToLine;
        }

        public static double DistanceToLine(this Point3D point, Point3D linePoint1, Point3D linePoint2)
        {
            var distanceToLinePoint1 = point.DistanceTo(linePoint1);
            var distanceToLinePoint2 = point.DistanceTo(linePoint2);
            if (distanceToLinePoint1 == 0 || distanceToLinePoint2 == 0)
                return 0;
            var lineLength = linePoint1.DistanceTo(linePoint2);
            if (lineLength == 0)
                return distanceToLinePoint1;

            var cosAngle = (lineLength * lineLength + distanceToLinePoint1 * distanceToLinePoint1 - distanceToLinePoint2 * distanceToLinePoint2)
                / (2 * lineLength * distanceToLinePoint1);
            var angle = System.Math.Acos(cosAngle);
            var distanceToLine = System.Math.Sin(angle) * distanceToLinePoint1;
            return distanceToLine;
        }

        public static UnitValue DistanceToLineSegment(this GeoCoordinate point, GeoCoordinate linePoint1, GeoCoordinate linePoint2)
        {
            // Assume short distances, such that euclidean coordinate system is a good approximation
            var distanceToLinePoint1 = point.GetDistanceTo(linePoint1).In(Unit.Meter);
            var distanceToLinePoint2 = point.GetDistanceTo(linePoint2).In(Unit.Meter);
            if (distanceToLinePoint1 == 0 || distanceToLinePoint2 == 0)
                return 0.To(Unit.Meter);
            var lineLength = linePoint1.GetDistanceTo(linePoint2).In(Unit.Meter);
            if (lineLength == 0)
                return distanceToLinePoint1.To(Unit.Meter);

            var cosAngle1 = (lineLength * lineLength + distanceToLinePoint1 * distanceToLinePoint1 - distanceToLinePoint2 * distanceToLinePoint2)
                / (2 * lineLength * distanceToLinePoint1);
            if (cosAngle1 < 0)
                return distanceToLinePoint1.To(Unit.Meter);

            var cosAngle2 = (lineLength * lineLength + distanceToLinePoint2 * distanceToLinePoint2 - distanceToLinePoint1 * distanceToLinePoint1)
                / (2 * lineLength * distanceToLinePoint2);
            if (cosAngle2 < 0)
                return distanceToLinePoint2.To(Unit.Meter);

            var angle = System.Math.Acos(cosAngle1);
            var distanceToLine = System.Math.Sin(angle) * distanceToLinePoint1.To(Unit.Meter);
            return distanceToLine;
        }
        public static double DistanceToLineSegment(this Point2D point, Point2D linePoint1, Point2D linePoint2)
        {
            var distanceToLinePoint1 = point.DistanceTo(linePoint1);
            var distanceToLinePoint2 = point.DistanceTo(linePoint2);
            if (distanceToLinePoint1 == 0 || distanceToLinePoint2 == 0)
                return 0;
            var lineLength = linePoint1.DistanceTo(linePoint2);
            if (lineLength == 0)
                return distanceToLinePoint1;

            var cosAngle1 = (lineLength * lineLength + distanceToLinePoint1 * distanceToLinePoint1 - distanceToLinePoint2 * distanceToLinePoint2)
                / (2 * lineLength * distanceToLinePoint1);
            if (cosAngle1 < 0)
                return distanceToLinePoint1;

            var cosAngle2 = (lineLength * lineLength + distanceToLinePoint2 * distanceToLinePoint2 - distanceToLinePoint1 * distanceToLinePoint1)
                / (2 * lineLength * distanceToLinePoint2);
            if (cosAngle2 < 0)
                return distanceToLinePoint2;

            var angle = System.Math.Acos(cosAngle1);
            var distanceToLine = System.Math.Sin(angle) * distanceToLinePoint1;
            return distanceToLine;
        }

        public static bool LineSegmentsIntersect(GeoCoordinate line1p1, GeoCoordinate line1p2, GeoCoordinate line2p1, GeoCoordinate line2p2)
        {
            var vectorLine1 = new Vector2D(line1p2.Latitude - line1p1.Latitude, line1p2.Longitude - line1p1.Longitude);
            var vectorLine2 = new Vector2D(line2p2.Latitude - line2p1.Latitude, line2p2.Longitude - line2p1.Longitude);

            var matrix = new Matrix(2, 3);
            matrix.Set(new[,]
            {
                { vectorLine1.X, -vectorLine2.X, line2p1.Latitude - line1p1.Latitude },
                { vectorLine1.Y, -vectorLine2.Y, line2p1.Longitude - line1p1.Longitude }
            });
            var rref = matrix.Data.ReducedRowEchelonForm();

            if (System.Math.Abs(rref[1, 1]) <= double.Epsilon) // Lines are parallel
                return false;
            return rref[0, 2] >= 0 && rref[0, 2] <= 1 && rref[1, 2] >= 0 && rref[1, 2] <= 1;
        }

        public static GeoCoordinate MoveAlongRadial(this GeoCoordinate point, double heading, UnitValue distance)
        {
            var distanceInNM = distance.In(Unit.NauticalMile);
            var distanceInRadians = distanceInNM * System.Math.PI / (180 * 60);
            var headingInRadians = heading * System.Math.PI / 180;
            var latitudeInRadians = point.Latitude * System.Math.PI / 180;
            var longitudeInRadians = -point.Longitude * System.Math.PI / 180;
            var newLatitudeInRadians = System.Math.Asin(System.Math.Sin(latitudeInRadians) * System.Math.Cos(distanceInRadians)
                + System.Math.Cos(latitudeInRadians)*System.Math.Sin(distanceInRadians)*System.Math.Cos(headingInRadians));
            
            var longitudeDiff = System.Math.Atan2(
                System.Math.Sin(headingInRadians)*System.Math.Sin(distanceInRadians)*System.Math.Cos(latitudeInRadians),
                System.Math.Cos(distanceInRadians) - System.Math.Sin(latitudeInRadians)*System.Math.Sin(newLatitudeInRadians));
            var newLongitudeInRadians = (longitudeInRadians - longitudeDiff + System.Math.PI).Modulus(2 * System.Math.PI) - System.Math.PI;

            var newLatitude = newLatitudeInRadians * 180 / System.Math.PI;
            var newLongitude = -newLongitudeInRadians * 180 / System.Math.PI;

            return new GeoCoordinate(newLatitude, newLongitude);
        }

        public static double HeadingTo(this GeoCoordinate point1, GeoCoordinate point2)
        {
            var latitude1InRadians = point1.Latitude * System.Math.PI / 180;
            var longitude1InRadians = point1.Longitude * System.Math.PI / 180;
            var latitude2InRadians = point2.Latitude * System.Math.PI / 180;
            var longitude2InRadians = point2.Longitude * System.Math.PI / 180;

            // Handle poles
            if(System.Math.Cos(latitude1InRadians) < 1e-6)
            {
                return latitude1InRadians > 0 ? 180 : 0;
            }

            var headingInRadians = System.Math.Atan2(
                    System.Math.Sin(longitude2InRadians - longitude1InRadians) * System.Math.Cos(latitude2InRadians),
                    System.Math.Cos(latitude1InRadians)*System.Math.Sin(latitude2InRadians) - System.Math.Sin(latitude1InRadians)*System.Math.Cos(latitude2InRadians)*System.Math.Cos(longitude2InRadians-longitude1InRadians) )
                .Modulus(2 * System.Math.PI);
            return headingInRadians * 180 / System.Math.PI;
        }

        public static GeoCoordinate ExtendCenterLineBy(GeoCoordinate startCoordinate, GeoCoordinate endCoordinate, UnitValue extendDistance)
        {
            var runwayHeading = startCoordinate.HeadingTo(endCoordinate);
            return extendDistance.Value < 0
                ? startCoordinate.MoveAlongRadial((runwayHeading + 180).Modulus(360), -extendDistance)
                : endCoordinate.MoveAlongRadial(runwayHeading, extendDistance);
        }

        public static UnitValue DistanceToRadial(this GeoCoordinate position, GeoCoordinate navaidCoordinate, double radial)
        {
            var distanceToNavaid = position.GetDistanceTo(navaidCoordinate);
            var headingToNavaid = position.HeadingTo(navaidCoordinate);
            var radialExtensionCoordinate = radial.CircularDifference(headingToNavaid) < 90
                ? navaidCoordinate.MoveAlongRadial(radial, -distanceToNavaid)
                : navaidCoordinate.MoveAlongRadial(radial, distanceToNavaid);
            var distanceFromRadial = position.DistanceToLine(navaidCoordinate, radialExtensionCoordinate);
            return distanceFromRadial;
        }
    }
}
