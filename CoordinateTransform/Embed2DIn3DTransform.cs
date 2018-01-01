using System;
using Commons.Extensions;
using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
    public class Embed2DIn3DTransform : ICoordinateTransform<Point2D, Point3D>
    {
        private readonly Plane plane;

        public Embed2DIn3DTransform(Plane plane)
        {
            this.plane = plane;
        }

        public Point3D Transform(Point2D point)
        {
            return plane.Origin
                   + point.X*plane.SpanningVector1
                   + point.Y*plane.SpanningVector2;
        }

        public Point2D InverseTransform(Point3D point)
        {
            var vectorFromOrigin = (point - plane.Origin).ToVector3D();
            var distanceFromOrigin = vectorFromOrigin.Magnitude();
            var distanceFromPlane = vectorFromOrigin
                .ProjectOnto(plane.NormalVector)
                .Magnitude();
            if (distanceFromPlane > 1e-6 * distanceFromOrigin)
                throw new ArgumentException("Point to be transformed is not on plane");
            var x = vectorFromOrigin.DotProduct(plane.SpanningVector1);
            var y = vectorFromOrigin.DotProduct(plane.SpanningVector2);
            return new Point2D(x, y);
        }
    }
}
