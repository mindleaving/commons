using System;
using Commons.Extensions;
using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
    /// <summary>
    /// Takes image coordinate and transforms to 
    /// plane defined by translation vector and rotation matrix
    /// </summary>
    public class PinholeToPlaneProjection : ICoordinateTransform<Point2D, Point2D>
    {
        private readonly ImageCoordinateNormalizer imageCoordinateNormalizer;
        private readonly Embed2DIn3DTransform imagePlaneEmbeddingTransform;
        private readonly Embed2DIn3DTransform planeEmbeddingTransform;
        private readonly PlaneProjection planeProjection;
        private readonly PlaneProjection imagePlaneProjection;

        public PinholeToPlaneProjection(Point2D focalLength,
            Point2D principalPoint,
            DistortionParameters distortionParameters,
            Vector3D translationVector, 
            Matrix3X3 rotationMatrix)
        {
            imageCoordinateNormalizer = new ImageCoordinateNormalizer(focalLength, principalPoint, distortionParameters);
            var imagePlane3D = new Plane(
                new Point3D(0,0,0),
                new Vector3D(1, 0, 0), 
                new Vector3D(0, 1, 0));
            imagePlaneEmbeddingTransform = new Embed2DIn3DTransform(imagePlane3D);

            var planeVector1 = new Vector3D(rotationMatrix.Data
                .Multiply(new double[] {1, 0, 0}.ConvertToMatrix())
                .Vectorize());
            var planeVector2 = new Vector3D(rotationMatrix.Data
                .Multiply(new double[] { 0, 1, 0 }.ConvertToMatrix())
                .Vectorize());
            var plane = new Plane(translationVector.ToPoint3D(),
                planeVector1,
                planeVector2);
            planeEmbeddingTransform = new Embed2DIn3DTransform(plane);
            var pinHoleProjectionPoint = new Point3D(0, 0, -1);
            planeProjection = new PlaneProjection(pinHoleProjectionPoint, plane);
            imagePlaneProjection = new PlaneProjection(pinHoleProjectionPoint, imagePlane3D);
        }

        public Point2D Transform(Point2D point)
        {
            var normalizedImageCoordinate = imageCoordinateNormalizer.Transform(point);
            var embeddedNormalizedImageCoordinate = imagePlaneEmbeddingTransform.Transform(normalizedImageCoordinate);
            return planeProjection.Transform(embeddedNormalizedImageCoordinate);
        }

        public Point2D InverseTransform(Point2D point)
        {
            var planePoint3D = planeEmbeddingTransform.Transform(point);
            var imagePoint = imagePlaneProjection.Transform(planePoint3D);
            return imagePoint;
        }
    }

    public class ImageCoordinateNormalizer : ICoordinateTransform<Point2D, Point2D>
    {
        private readonly Point2D focalLength;
        private readonly Point2D principalPoint;
        private readonly DistortionParameters distortionParameters;

        public ImageCoordinateNormalizer(Point2D focalLength, Point2D principalPoint, DistortionParameters distortionParameters)
        {
            this.focalLength = focalLength;
            this.principalPoint = principalPoint;
            this.distortionParameters = distortionParameters;
        }

        public Point2D Transform(Point2D point)
        {
            var offsetPoint = point - principalPoint;
            var normalizedPoint = new Point2D(
                offsetPoint.X / focalLength.X,
                offsetPoint.Y / focalLength.Y);
            // TODO: Correct distortion
            return normalizedPoint;
        }

        public Point2D InverseTransform(Point2D point)
        {
            var distortedPoint = point; // TODO: Apply distortion
            var imagePoint = new Point2D(
                distortedPoint.X*focalLength.X + principalPoint.X,
                distortedPoint.Y*focalLength.Y + principalPoint.Y);
            return imagePoint;
        }
    }

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
            var vectorToPlaneOrigin = (point - plane.Origin).ToVector3D();
            var distanceFromPlane = vectorToPlaneOrigin
                .ProjectOnto(plane.NormalVector)
                .Magnitude();
            var lineVector = (point - projectionPoint).ToVector3D();
            var scalar = distanceFromPlane/lineVector.ProjectOnto(plane.NormalVector).Magnitude();
            var polarity = lineVector.DotProduct(plane.NormalVector) > 0 ? -1 : 1;
            var intersectionPoint = point + polarity*scalar*lineVector;

            return embeddingTransform.InverseTransform(intersectionPoint);
        }

        public Point3D InverseTransform(Point2D point)
        {
            throw new InvalidOperationException("Projections cannot be inverted.");
        }
    }
}
