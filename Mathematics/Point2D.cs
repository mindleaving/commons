using System.Globalization;
using System.Runtime.Serialization;

namespace Commons.Mathematics
{
    [DataContract]
    public class Point2D
    {
        public Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

        public static Point2D operator +(Point2D point, Vector2D vector)
        {
            return new Point2D(point.X + vector.X, point.Y + vector.Y);
        }

        public static Point2D operator +(Point2D point, Point2D vector)
        {
            return new Point2D(point.X + vector.X, point.Y + vector.Y);
        }

        public static Point2D operator -(Point2D point, Vector2D vector)
        {
            return new Point2D(point.X - vector.X, point.Y - vector.Y);
        }

        public static Point2D operator -(Point2D point, Point2D vector)
        {
            return new Point2D(point.X - vector.X, point.Y - vector.Y);
        }

        public static Point2D operator *(double scalar, Point2D point)
        {
            return new Point2D(scalar * point.X, scalar * point.Y);
        }

        public override string ToString()
        {
            return $"{X.ToString("F6", CultureInfo.InvariantCulture)};" +
                   $"{Y.ToString("F6", CultureInfo.InvariantCulture)}";
        }
    }
}