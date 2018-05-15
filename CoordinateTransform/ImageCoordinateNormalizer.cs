using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
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
}