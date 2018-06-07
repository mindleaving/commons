using System.Collections.Generic;
using System.Linq;
using Commons.DataProcessing;
using Commons.Mathematics;
using NUnit.Framework;

namespace Commons.UnitTests.DataProcessing
{
    [TestFixture]
    public class SlidingWindowTest
    {
        [Test]
        public void SetPositionThrowsExceptionWhenWindowMovedBackwards()
        {
            var points = CreateTestPoints();
            var windowSize = 1;
            var windowPositioningType = WindowPositioningType.CenteredAtPosition;
            var sut = new SlidingWindow<Point2D>(points, p => p.X, windowSize, windowPositioningType);
            Assert.That(() => sut.SetPosition(2), Throws.Nothing);
            Assert.That(() => sut.SetPosition(1), Throws.InvalidOperationException);
        }

        [Test]
        public void ThrowsExceptionIfWindowNotSet()
        {
            var points = CreateTestPoints();
            var windowSize = 1;
            var windowPositioningType = WindowPositioningType.CenteredAtPosition;
            var sut = new SlidingWindow<Point2D>(points, p => p.X, windowSize, windowPositioningType);
            // No call to .SetPosition!
            Assert.That(() => sut.Count(), Throws.InvalidOperationException);
        }

        [Test]
        public void SlidingWindowReturnsPointsInWindow()
        {
            var points = CreateTestPoints();
            var windowSize = 1;
            var windowPositioningType = WindowPositioningType.CenteredAtPosition;
            var sut = new SlidingWindow<Point2D>(points, p => p.X, windowSize, windowPositioningType);
            sut.SetPosition(2.2);
            Assert.That(sut.Count(), Is.EqualTo(2));
            Assert.That(sut.Select(p => p.X), Is.EquivalentTo(new[] {2, 2.5}));
        }

        private static List<Point2D> CreateTestPoints()
        {
            var points = new List<Point2D>
            {
                new Point2D(0, 1),
                new Point2D(1, 2),
                new Point2D(2, 3),
                new Point2D(2.5, 4),
                new Point2D(3, 5)
            };
            return points;
        }
    }
}
