using Commons.Extensions;

namespace Commons.Mathematics
{
    public class Plane
    {
        public Point3D Origin { get; set; }
        public Vector3D SpanningVector1 { get; }
        public Vector3D SpanningVector2 { get; }
        public Vector3D NormalVector { get; }

        public Plane(Point3D origin,
            Vector3D vector1,
            Vector3D vector2)
        {
            Origin = origin;
            SpanningVector1 = vector1;
            SpanningVector2 = vector2;
            NormalVector = vector1.CrossProduct(vector2);
        }
    }
}
