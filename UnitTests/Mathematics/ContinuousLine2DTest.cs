using System.Collections.Generic;
using Commons.Mathematics;
using NUnit.Framework;

namespace Commons.UnitTests.Mathematics
{
    [TestFixture]
    public class ContinuousLine2DTest
    {
        [Test]
        public void EmptyPointCollectionReturnsNaN()
        {
            var sut = new ContinuousLine2D(new List<Point2D>());
            Assert.That(sut.ValueAtX(0), Is.NaN);
            Assert.That(sut.ValueAtX(1), Is.NaN);
            Assert.That(sut.ValueAtX(double.NegativeInfinity), Is.NaN);
            Assert.That(sut.ValueAtX(double.PositiveInfinity), Is.NaN);
        }

        [Test]
        public void ValueAtNaNReturnsNaN()
        {
            var sut = new ContinuousLine2D(new List<Point2D> { new Point2D(0, 0), new Point2D(1, 1)});
            Assert.That(sut.ValueAtX(double.NaN), Is.NaN);
        }

        [Test]
        public void NegativeInfinityEndpointYIsUsedBeforeFirstPoint()
        {
            var sut = new ContinuousLine2D(new List<Point2D>
            {
                new Point2D(double.NegativeInfinity, 1),
                new Point2D(0, 0),
                new Point2D(1, 0),
            });
            Assert.That(sut.ValueAtX(-1), Is.EqualTo(1));
        }

        [Test]
        public void PositiveInfinityEndpointYIsUsedAfterLastPoint()
        {
            var sut = new ContinuousLine2D(new List<Point2D>
            {
                new Point2D(0, 0),
                new Point2D(1, 0),
                new Point2D(double.PositiveInfinity, 1)
            });
            Assert.That(sut.ValueAtX(2), Is.EqualTo(1));
        }

        [Test]
        public void PointsAreSortedAndInterpolatedAsExpected()
        {
            var sut = new ContinuousLine2D(new List<Point2D>
            {
                new Point2D(3, 4),
                new Point2D(1, 1),
                new Point2D(2, 2),
            });
            Assert.That(sut.ValueAtX(1), Is.EqualTo(1).Within(Tolerance));
            Assert.That(sut.ValueAtX(1.5), Is.EqualTo(1.5).Within(Tolerance));
            Assert.That(sut.ValueAtX(2), Is.EqualTo(2).Within(Tolerance));
            Assert.That(sut.ValueAtX(2.5), Is.EqualTo(3).Within(Tolerance));
            Assert.That(sut.ValueAtX(3), Is.EqualTo(4).Within(Tolerance));
        }

        private const double Tolerance = 1e-12;
    }
}
