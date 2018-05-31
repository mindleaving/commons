using System.Linq;
using Commons.Extensions;

namespace Commons.Mathematics
{
    public class LineSegment2D
    {
        public LineSegment2D(Point2D point1, Point2D point2)
        {
            Point = point1;
            Vector = point1.VectorTo(point2);
        }

        public Point2D Point { get; }
        public Vector2D Vector { get; }

        public bool Intersects(LineSegment2D otherLine, out Point2D intersectionPoint)
        {
            intersectionPoint = null;
            var linSolveResult = MatrixOperations.LinSolve(new[,]
            {
                {Vector.X, -otherLine.Vector.X},
                {Vector.Y, -otherLine.Vector.Y}
            }, new[]
            {
                otherLine.Point.X - Point.X,
                otherLine.Point.Y - Point.Y
            }, out var isValid);

            if (!isValid)
                return false;
            if (!linSolveResult.All(x => x.IsBetween(0, 1)))
                return false;
            intersectionPoint = Point + (linSolveResult[0] * Vector).ToVector2D();
            return true;
        }
    }
}
