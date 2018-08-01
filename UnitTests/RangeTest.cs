using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest
{
    [TestFixture]
    public class RangeTest
    {
        [Test]
        [TestCase(0.1, 0.9, 0.05, false)]
        [TestCase(0.1, 0.9, 0.1, true)]
        [TestCase(0.1, 0.9, 0.25, true)]
        [TestCase(0.1, 0.9, 0.9, false)]
        [TestCase(0.1, 0.9, 1.05, false)]
        public void ContainsReturnsExpected(double from, double to, double probe, bool expected)
        {
            var sut = new Range<double>(from, to);
            var actual = sut.Contains(probe);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0, 2, 0, 2)] // Equal
        [TestCase(0, 2, 1, 2)] // Subset 1
        [TestCase(0, 2, 0, 1)] // Subset 2
        [TestCase(0, 3, 1, 2)] // Subset 3
        [TestCase(0, 2, 1, 3)] // Partial overlap
        public void OverlapsTrue(
            double from1, double to1,
            double from2, double to2)
        {
            var range1 = new Range<double>(from1, to1);
            var range2 = new Range<double>(from2, to2);
            Assert.That(range1.Overlaps(range2), Is.True);
            Assert.That(range2.Overlaps(range1), Is.True);
        }

        [Test]
        [TestCase(0, 2, 3, 4)] // Widely seprated
        [TestCase(0, 2, 2, 4)] // Barely separated
        public void OverlapsFalse(
            double from1, double to1,
            double from2, double to2)
        {
            var range1 = new Range<double>(from1, to1);
            var range2 = new Range<double>(from2, to2);
            Assert.That(range1.Overlaps(range2), Is.False);
            Assert.That(range2.Overlaps(range1), Is.False);
        }
    }
}
