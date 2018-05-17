using System;
using Commons.Mathematics;

namespace Commons.Extensions
{
    public static class Vector2DExtensions
    {
        public static double Determinant(this Vector2D a, Vector2D b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2D ToVector2D(this Vector vector)
        {
            if(vector.Dimension != 2)
                throw new InvalidOperationException($"Cannot convert Vector with dimension '{vector.Dimension}' to Vector2D");
            if (vector is Vector2D vector2D)
                return vector2D;
            return new Vector2D(vector.Data);
        }
    }
}