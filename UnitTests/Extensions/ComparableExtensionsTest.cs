using Commons.Extensions;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class ComparableExtensionsTest
    {
        [Test]
        [TestCase(0, 1)]
        public void IsSmallerThanTrue(double item1, double item2)
        {
            Assert.That(item1.IsSmallerThan(item2));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        public void IsSmallerThanFalse(double item1, double item2)
        {
            Assert.That(item1.IsSmallerThan(item2), Is.False);
        }

        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 1)]
        public void IsSmallerThanOrEqualToTrue(double item1, double item2)
        {
            Assert.That(item1.IsSmallerThanOrEqualTo(item2));
        }

        [Test]
        [TestCase(2, 1)]
        public void IsSmallerThanOrEqualToFalse(double item1, double item2)
        {
            Assert.That(item1.IsSmallerThanOrEqualTo(item2), Is.False);
        }


        [Test]
        [TestCase(1, 0)]
        public void IsGreaterThanTrue(double item1, double item2)
        {
            Assert.That(item1.IsGreaterThan(item2));
        }

        [Test]
        [TestCase(1, 1)]
        [TestCase(1, 2)]
        public void IsGreaterThanFalse(double item1, double item2)
        {
            Assert.That(item1.IsGreaterThan(item2), Is.False);
        }

        [Test]
        [TestCase(1, 0)]
        [TestCase(1, 1)]
        public void IsGreaterThanOrEqualToTrue(double item1, double item2)
        {
            Assert.That(item1.IsGreaterThanOrEqualTo(item2));
        }

        [Test]
        [TestCase(1, 2)]
        public void IsGreaterThanOrEqualToFalse(double item1, double item2)
        {
            Assert.That(item1.IsGreaterThanOrEqualTo(item2), Is.False);
        }
    }
}
