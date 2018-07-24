using System.Collections.Generic;
using System.Linq;
using Commons.Mathematics;
using Commons.Physics;

namespace CommonsTest.UnitTests
{
    [TestFixture]
    public class FilterFuncsTest
    {
        [Test]
        public void AverageFilterThrowsExceptionForEvenWindowSize()
        {
            var data = new List<double>();
            Assert.That(() => data.MovingAverage(2).ToList(), Throws.ArgumentException);
        }

        [Test]
        public void AverageFilterEmptyArrayReturnsEmpty()
        {
            var data = new List<double>();
            var actual = data.MovingAverage(3).ToList();
            Assert.That(actual, Is.Empty);
        }

        [Test]
        public void AverageFilterSingleValueArrayReturnsThisValue()
        {
            var data = new List<double> { 42 };
            var actual = data.MovingAverage(3).ToList();
            Assert.That(actual.Count, Is.EqualTo(1));
            Assert.That(actual[0], Is.EqualTo(42));
        }

        [Test]
        public void AverageFilterReturnsInputForWindowSizeOne()
        {
            var data = new List<double> { 1, 2, 3, 13, 15 };
            var actual = data.MovingAverage(1).ToList();
            Assert.That(actual.Count, Is.EqualTo(data.Count));
            Assert.That(actual, Is.EqualTo(data));
        }

        [Test]
        public void AverageFilterReturnsExceptedMovingAverage()
        {
            var data = new List<double> { 1, 2, 3, 13, 15 };
            var actual = data.MovingAverage(3).ToList();
            var expected = new List<double> { 4.0/3, 2, 6, 31.0/3, 43.0/3 };
            Assert.That(actual.Count, Is.EqualTo(data.Count));
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void MedianFilterAsExpected()
        {
            var data = new List<Point2D>
            {
                new Point2D(0, 1),
                new Point2D(1, 1.5),
                new Point2D(2, 1.8),
                new Point2D(3, 1.1),
                new Point2D(4, 1.2),
            };
            var filteredSignal = data.MedianFilter(2.5).ToList();
            Assert.That(filteredSignal.Count, Is.EqualTo(data.Count));

            var expectedY = new[] {1.25, 1.5, 1.5, 1.2, 1.15};
            for (var pointIdx = 0; pointIdx < filteredSignal.Count; pointIdx++)
            {
                var filteredPoint = filteredSignal[pointIdx];
                Assert.That(filteredPoint.X, Is.EqualTo(data[pointIdx].X).Within(Tolerance));
                Assert.That(filteredPoint.Y, Is.EqualTo(expectedY[pointIdx]).Within(Tolerance));
            }
        }

        private const double Tolerance = 1e-12;
    }
}
