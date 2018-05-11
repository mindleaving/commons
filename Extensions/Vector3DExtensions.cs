using System;
using Commons.Mathematics;

namespace Commons.Extensions
{
    public static class Vector3DExtensions
    {
        public static Vector3D CrossProduct(this Vector3D a, Vector3D b)
        {
            return new Vector3D(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static Vector3D ToVector3D(this Vector vector)
        {
            if(vector.Dimension != 3)
                throw new InvalidOperationException($"Cannot convert Vector with dimension '{vector.Dimension}' to Vector3D");
            if (vector is Vector3D vector3D)
                return vector3D;
            return new Vector3D(vector.Data);
        }

        public static Point3D ToPoint3D(this Vector3D v)
        {
            return new Point3D(v.X, v.Y, v.Z);
        }
    }
}