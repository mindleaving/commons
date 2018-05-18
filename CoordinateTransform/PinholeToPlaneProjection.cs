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

        public PinholeToPlaneProjection(Calibration calibration)
            : this(calibration.FocalLength,
                calibration.PrincipalPoint,
                calibration.Distortion,
                calibration.TranslationVector,
                calibration.RotationMatrix)
        { }
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
            return imageCoordinateNormalizer.InverseTransform(imagePoint);
        }
    }
}
