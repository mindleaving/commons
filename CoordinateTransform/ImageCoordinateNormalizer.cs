using Commons.Extensions;
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
            var undistortedPoint = Undistort(normalizedPoint);
            return undistortedPoint;
        }

        public Point2D InverseTransform(Point2D point)
        {
            var r2 = CalculateRadiusSquared(point);
            var distortedPoint = CalculateDistortionScalar(r2) * point;
            var imagePoint = new Point2D(
                distortedPoint.X*focalLength.X + principalPoint.X,
                distortedPoint.Y*focalLength.Y + principalPoint.Y);
            return imagePoint;
        }

        private static double CalculateRadiusSquared(Point2D point)
        {
            return point.X.Square() + point.Y.Square();
        }

        private double CalculateDistortionScalar(double r2)
        {
            return 1 + distortionParameters.Radial2 * r2 + distortionParameters.Radial4 * r2 * r2;
        }

        /// <summary>
        /// Algorithm found here:
        /// 
        /// Drap P, Lefèvre J.
        /// An Exact Formula for Calculating Inverse Radial Lens Distortions.
        /// Vieira M, ed. Sensors (Basel, Switzerland). 2016;16(6):807. doi:10.3390/s16060807.
        /// </summary>
        private Point2D Undistort(Point2D normalizedPoint)
        {
            const double ConvergenceThreshold = 1e-5;
            const int MaxIterations = 10;

            var undistortedPoint = normalizedPoint;
            double absoluteChange;
            var iterations = 0;
            do
            {
                var distortedPoint = undistortedPoint;
                var r2 = CalculateRadiusSquared(undistortedPoint);
                var distortion = CalculateDistortionScalar(r2);
                undistortedPoint = (1/ distortion) * normalizedPoint;
                absoluteChange = distortedPoint.DistanceTo(undistortedPoint);
                iterations++;
            } while (absoluteChange > ConvergenceThreshold && iterations < MaxIterations);
            return undistortedPoint;
        }
    }
}