using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Mathematics;

namespace Commons.Extensions
{
    public static class Point2DExtensions
    {
        public static double DistanceTo(this Point2D point1, Point2D point2)
        {
            var diffX = point1.X - point2.X;
            var diffY = point1.Y - point2.Y;
            var distance = Math.Sqrt(diffX * diffX + diffY * diffY);
            return distance;
        }

        public static Vector2D VectorTo(this Point2D point1, Point2D point2)
        {
            var diffX = point2.X - point1.X;
            var diffY = point2.Y - point1.Y;
            return new Vector2D(diffX, diffY);
        }

        public static Point2D Average(this IEnumerable<Point2D> points)
        {
            var pointList = points.ToList();
            return new Point2D(
                pointList.Average(p => p.X),
                pointList.Average(p => p.Y));
        }

        public static Vector2D ToVector2D(this Point2D point2D)
        {
            return new Vector2D(point2D.X, point2D.Y);
        }

        public static PolarPoint ToPolarPoint(this Point2D point2D)
        {
            var angle = Math.Atan2(point2D.Y, point2D.X);
            var r = point2D.DistanceTo(new Point2D(0, 0));
            return new PolarPoint(angle, r);
        }

        public static Point2D ToPoint2D(this PolarPoint polarPoint)
        {
            var x = polarPoint.DistanceFromCenter * Math.Cos(polarPoint.Angle);
            var y = polarPoint.DistanceFromCenter * Math.Sin(polarPoint.Angle);
            return new Point2D(x, y);
        }
    }
}