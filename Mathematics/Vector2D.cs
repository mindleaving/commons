using System;

namespace Commons.Mathematics
{
    public class Vector2D : Vector
    {
        public double X
        {
            get { return Data[0]; }
            set { Data[0] = value; }
        }
        public double Y
        {
            get { return Data[1]; }
            set { Data[1] = value; }
        }

        public Vector2D() : base(2) { }
        public Vector2D(params double[] data) : base(data)
        {
            if (data.Length != 2)
                throw new ArgumentException($"Wrong dimension of data. Expected 2 values, got {data.Length}");
        }
        public Vector2D(double x, double y) : base(x, y) { }

        public static implicit operator Point2D(Vector2D v)
        {
            return new Point2D(v.X, v.Y);
        }
    }
}