using System;
using Commons.Extensions;
using Commons.Physics;

namespace Commons.Mathematics
{
    public static class Geometry
    {
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
            var angle = Math.Acos(cosAngle);
            var distanceToLine = Math.Sin(angle) * distanceToLinePoint1;
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
            var angle = Math.Acos(cosAngle);
            var distanceToLine = Math.Sin(angle) * distanceToLinePoint1;
            return distanceToLine;
        }

        public static double DistanceToLineSegment(this Point2D point, LineSegment2D lineSegment)
        {
            return DistanceToLineSegment(point, lineSegment.Point, lineSegment.Point + lineSegment.Vector);
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

            var angle = Math.Acos(cosAngle1);
            var distanceToLine = Math.Sin(angle) * distanceToLinePoint1;
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

            if (Math.Abs(rref[1, 1]) <= double.Epsilon) // Lines are parallel
                return false;
            return rref[0, 2] >= 0 && rref[0, 2] <= 1 && rref[1, 2] >= 0 && rref[1, 2] <= 1;
        }

        public static GeoCoordinate MoveAlongRadial(this GeoCoordinate point, UnitValue heading, UnitValue distance)
        {
            var distanceInNM = distance.In(Unit.NauticalMile);
            var distanceInRadians = distanceInNM * Math.PI / (180 * 60);
            var headingInRadians = heading.In(Unit.Radians);
            var latitudeInRadians = point.Latitude * Math.PI / 180;
            var longitudeInRadians = -point.Longitude * Math.PI / 180;
            var newLatitudeInRadians = Math.Asin(Math.Sin(latitudeInRadians) * Math.Cos(distanceInRadians)
                + Math.Cos(latitudeInRadians)*Math.Sin(distanceInRadians)*Math.Cos(headingInRadians));
            
            var longitudeDiff = Math.Atan2(
                Math.Sin(headingInRadians)*Math.Sin(distanceInRadians)*Math.Cos(latitudeInRadians),
                Math.Cos(distanceInRadians) - Math.Sin(latitudeInRadians)*Math.Sin(newLatitudeInRadians));
            var newLongitudeInRadians = (longitudeInRadians - longitudeDiff + Math.PI).Modulus(2 * Math.PI) - Math.PI;

            var newLatitude = newLatitudeInRadians * 180 / Math.PI;
            var newLongitude = -newLongitudeInRadians * 180 / Math.PI;

            return new GeoCoordinate(newLatitude, newLongitude);
        }

        public static UnitValue HeadingTo(this GeoCoordinate point1, GeoCoordinate point2)
        {
            var latitude1InRadians = point1.Latitude * Math.PI / 180;
            var longitude1InRadians = point1.Longitude * Math.PI / 180;
            var latitude2InRadians = point2.Latitude * Math.PI / 180;
            var longitude2InRadians = point2.Longitude * Math.PI / 180;

            // Handle poles
            if(Math.Cos(latitude1InRadians) < 1e-6)
            {
                return latitude1InRadians > 0 ? 180.To(Unit.Degree) : 0.To(Unit.Degree);
            }

            var headingInRadians = Math.Atan2(
                    Math.Sin(longitude2InRadians - longitude1InRadians) * Math.Cos(latitude2InRadians),
                    Math.Cos(latitude1InRadians)*Math.Sin(latitude2InRadians) - Math.Sin(latitude1InRadians)*Math.Cos(latitude2InRadians)*Math.Cos(longitude2InRadians-longitude1InRadians) )
                .Modulus(2 * Math.PI);
            return headingInRadians.To(Unit.Radians);
        }

        public static GeoCoordinate ExtendCenterLineBy(GeoCoordinate startCoordinate, GeoCoordinate endCoordinate, UnitValue extendDistance)
        {
            var runwayHeading = startCoordinate.HeadingTo(endCoordinate);
            return extendDistance.Value < 0
                ? startCoordinate.MoveAlongRadial((runwayHeading.In(Unit.Degree) + 180).Modulus(360).To(Unit.Degree), -extendDistance)
                : endCoordinate.MoveAlongRadial(runwayHeading, extendDistance);
        }
    }
}
