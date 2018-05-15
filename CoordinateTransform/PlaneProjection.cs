using System;
using Commons.Extensions;
using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
    public class PlaneProjection : ICoordinateTransform<Point3D, Point2D>
    {
        private readonly Point3D projectionPoint;
        private readonly Plane plane;
        private readonly Embed2DIn3DTransform embeddingTransform;

        public PlaneProjection(Point3D projectionPoint, Plane plane)
        {
            this.projectionPoint = projectionPoint;
            this.plane = plane;
            embeddingTransform = new Embed2DIn3DTransform(plane);
        }

        public Point2D Transform(Point3D point)
        {
            // Find intersection of line defined by point and projection point
            // and plane
            var lineVector = (point - projectionPoint).ToVector3D();
            var denominator = lineVector.DotProduct(plane.NormalVector);
            if(denominator.Abs() < 1e-12)
                throw new Exception("Intersection of plane and line could not be calculated as they appear to be parallel");
            var vectorToPlaneOrigin = (point - plane.Origin).ToVector3D();
            var nominator = vectorToPlaneOrigin.DotProduct(plane.NormalVector);
            var scalar = nominator / denominator;
            var intersectionPoint = point - scalar * lineVector;

            return embeddingTransform.InverseTransform(intersectionPoint);
        }

        public Point3D InverseTransform(Point2D point)
        {
            throw new InvalidOperationException("Projections cannot be inverted.");
        }
    }
}