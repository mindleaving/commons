using System;
using System.Linq;
using Commons.Mathematics;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class VectorExtensions
    {
        /// <summary>
        /// Divide the vector by its length to produce a unit vector
        /// </summary>
        /// <returns>The resulting unit vector</returns>
        public static Vector Normalize(this Vector v)
        {
            var magnitude = v.Magnitude();
            return v.Divide(magnitude);
        }

        public static Vector ProjectOnto(this Vector v1, Vector v2)
        {
            if(v1.Dimension != v2.Dimension)
                throw new InvalidOperationException("Cannot project vector onto vector with other dimension");
            return (v1.DotProduct(v2)/(v2.DotProduct(v2))) * v2;
        }

        public static Vector Divide(this Vector vector, double scalar)
        {
            return new Vector(vector.Data.Select(x => x/scalar).ToArray());
        }

        public static Vector Multiply(this Vector vector, double scalar)
        {
            return new Vector(vector.Data.Select(x => scalar * x).ToArray());
        }

        public static double DotProduct(this Vector v1, Vector v2)
        {
            if(v1.Dimension != v2.Dimension)
                throw new InvalidOperationException("Cannot calculate dot product of vectors with different lengths");
            return v1.Data.PairwiseOperation(v2.Data, (a, b) => a * b).Sum();
        }

        public static double Magnitude(this Vector v)
        {
            return Math.Sqrt(v.Data.Select(x => x*x).Sum());
        }

        /// <summary>
        /// Measures angle in radians between two vectors
        /// </summary>
        public static UnitValue AngleWith(this Vector v1, Vector v2)
        {
            var normalizedV1 = v1.Normalize();
            var normalizedV2 = v2.Normalize();
            var dotProduct = normalizedV1.DotProduct(normalizedV2);
            if (dotProduct > 1)
                dotProduct = 1;
            var angle = Math.Acos(dotProduct).To(Unit.Radians);
            return angle.Value >= 0 ? angle : angle + (Math.PI/2).To(Unit.Radians);
        }
    }
}
