using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;

namespace Commons.Mathematics
{
    /// <summary>
    /// Uses linear interpolation for calculating interpolated value between points
    /// </summary>
    public class ContinuousLine2D
    {
        private bool arePointsOrdered;
        private readonly List<Point2D> points;
        private readonly Point2DXYComparer pointComparer = new Point2DXYComparer();

        public ContinuousLine2D(IEnumerable<Point2D> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            this.points = points.ToList();
        }
        public ContinuousLine2D(IEnumerable<PolarPoint> points)
        {
            if (points == null) throw new ArgumentNullException(nameof(points));
            this.points = points.Select(p => new Point2D(p.Angle, p.DistanceFromCenter)).ToList();
        }

        private void SortPoints()
        {
            if(arePointsOrdered)
                return;
            points.Sort(pointComparer);
            arePointsOrdered = true;
        }

        /// <summary>
        /// Find interpolated value at X.
        /// </summary>
        /// <returns>Interpolated value at X if X is between the minimum and maximum X of all points. NaN otherwise</returns>
        public double ValueAtX(double x)
        {
            SortPoints();
            if (!points.Any())
                return double.NaN;
            if(x < points.First().X)
                return double.NaN;
            if (x > points.Last().X)
                return double.NaN;
            var searchPoint = new Point2D(x, double.NegativeInfinity);
            var resultIndex = points.BinarySearch(searchPoint, pointComparer);
            if (resultIndex > 0)
                return points[resultIndex].Y;
            resultIndex = ~resultIndex;
            if (resultIndex == 0)
            {
                if (points[resultIndex].X == x)
                    return points[resultIndex].Y;
                return double.NaN;
            }
            if (resultIndex >= points.Count) // Should already be covered by above checks, but to be sure check again here
                return double.NaN;
            var previousPoint = points[resultIndex - 1];
            if (previousPoint.X.IsNegativeInfinity())
                return previousPoint.Y;
            var nextPoint = points[resultIndex];
            if (nextPoint.X.IsPositiveInfinity())
                return nextPoint.Y;
            if (previousPoint.X == nextPoint.X)
                return (previousPoint.Y + nextPoint.Y) / 2;
            var ratio = (x - previousPoint.X) / (nextPoint.X - previousPoint.X);
            return (1 - ratio) * previousPoint.Y + ratio * nextPoint.Y;
        }
    }
}
