using System.Runtime.Serialization;
using Commons.Extensions;
using Commons.Physics;

namespace Commons.Mathematics
{
    [DataContract]
    public class Vector3D : Vector
    {
        [IgnoreDataMember]
        public double X
        {
            get { return Data[0]; }
            set { Data[0] = value; }
        }
        [IgnoreDataMember]
        public double Y
        {
            get { return Data[1]; }
            set { Data[1] = value; }
        }
        [IgnoreDataMember]
        public double Z
        {
            get { return Data[2]; }
            set { Data[2] = value; }
        }

        public Vector3D() : base(3) { }
        public Vector3D(params double[] data) : base(3, data) { }
        public Vector3D(double x, double y, double z) : base(3, x, y, z) { }

        public static implicit operator Point3D(Vector3D v)
        {
            return new Point3D(v.X, v.Y, v.Z);
        }

        public static Vector3D operator -(Vector3D v)
        {
            return -1*v;
        }
        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Data.Sum(v2.Data));
        }
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.Data.Subtract(v2.Data));
        }
        public static Vector3D operator *(double scalar, Vector3D v)
        {
            return v.Multiply(scalar).ToVector3D();
        }
        public static Vector3D operator *(Vector3D v, double scalar)
        {
            return v.Multiply(scalar).ToVector3D();
        }
        public static UnitVector3D operator *(UnitValue scalar, Vector3D v)
        {
            var x = v.X * scalar;
            var y = v.Y * scalar;
            var z = v.Z * scalar;
            return new UnitVector3D(x, y, z);
        }
    }
}