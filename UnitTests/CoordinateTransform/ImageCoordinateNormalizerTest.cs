using Commons.CoordinateTransform;
using Commons.Extensions;
using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.CoordinateTransform
{
    [TestFixture]
    public class ImageCoordinateNormalizerTest
    {
        [Test]
        public void RoundtripReturnsSamePoint()
        {
            var point = new Point2D(1353, 253);
            var focalLength = new Point2D(2100, 2100);
            var principalPoint = new Point2D(900, 400);
            var distortion = new DistortionParameters
            {
                Radial2 = -0.13,
                Radial4 = 0.93
            };
            var sut = new ImageCoordinateNormalizer(focalLength, principalPoint, distortion);
            var normalizedPoint = sut.Transform(point);
            var reconstructedPoint = sut.InverseTransform(normalizedPoint);
            Assert.That(reconstructedPoint.DistanceTo(point), Is.LessThan(0.1));
        }
    }
}
