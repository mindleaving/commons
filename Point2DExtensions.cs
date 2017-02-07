using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons
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

        public static Vector2D ToVector3D(this Point2D point3D)
        {
            return new Vector2D(point3D.X, point3D.Y);
        }

        public static Point2D Average(this IEnumerable<Point2D> points)
        {
            var pointList = points.ToList();
            return new Point2D(
                pointList.Average(p => p.X),
                pointList.Average(p => p.Y));
        }
    }
}