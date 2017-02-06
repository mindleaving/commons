using System;

namespace Commons
{
    public static class Vector2DExtensions
    {
        public static Vector2D Divide(this Vector2D vector, double scalar)
        {
            return new Vector2D(vector.X / scalar, vector.Y / scalar);
        }

        public static double Magnitude(this Vector2D a)
        {
            return Math.Sqrt(a.X * a.X + a.Y * a.Y);
        }

        public static double Determinant(this Vector2D a, Vector2D b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}