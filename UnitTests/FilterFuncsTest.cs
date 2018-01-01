using System.Collections.Generic;
using System.Linq;
using Commons.Physics;
using NUnit.Framework;

namespace Commons.UnitTests
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
    }
}
