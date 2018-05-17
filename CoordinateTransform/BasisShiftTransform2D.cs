using Commons.Extensions;
using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
    public class BasisShiftTransform2D : ICoordinateTransform<Point2D, Point2D>
    {
        public Vector2D XAxis { get; }
        public Vector2D YAxis { get; }

        public BasisShiftTransform2D(Vector2D xAxis, Vector2D yAxis)
        {
            XAxis = xAxis;
            YAxis = yAxis;
        }

        public Point2D Transform(Point2D point)
        {
            var vector2D = point.ToVector2D();
            return new Point2D(
                vector2D.DotProduct(XAxis) / XAxis.Magnitude().Square(),
                vector2D.DotProduct(YAxis) / YAxis.Magnitude().Square());
        }

        public Point2D InverseTransform(Point2D point)
        {
            throw new System.NotImplementedException();
        }
    }
}
