using Commons.Mathematics;
using NUnit.Framework;

namespace Commons.UnitTests.Mathematics
{
    [TestFixture]
    public class LineSegment2DTest
    {
        private const double Tolerance = 1e-12;

        [Test]
        public void IntersectionPointFoundForIntersectingLineSegments()
        {
            var segment1 = new LineSegment2D(new Point2D(0, 0), new Point2D(1, 1));
            var segment2 = new LineSegment2D(new Point2D(1, 0), new Point2D(0, 1));

            var areIntersecting = segment1.Intersects(segment2, out var intersectionPoint);
            Assert.That(areIntersecting, Is.True);
            Assert.That(intersectionPoint.X, Is.EqualTo(0.5).Within(Tolerance));
            Assert.That(intersectionPoint.Y, Is.EqualTo(0.5).Within(Tolerance));
        }

        [Test]
        public void IntersectsReturnsFalseForNonIntersectingSegments()
        {
            var segment1 = new LineSegment2D(new Point2D(0, 0), new Point2D(1, 1));
            var segment2 = new LineSegment2D(new Point2D(1, 0), new Point2D(2, 0));

            var areIntersecting = segment1.Intersects(segment2, out _);
            Assert.That(areIntersecting, Is.False);
        }
    }
}
