using Commons.Extensions;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class MathExtensionsTest
    {
        private const double Tolerance = 1e-12;

        [Test]
        [TestCase(1, 0, 1)]
        [TestCase(1, 1, 1)]
        [TestCase(1, 2, 1)]
        [TestCase(0, 0, 1)]
        [TestCase(0, 1, 0)]
        [TestCase(5, 0, 1)]
        [TestCase(5, 1, 5)]
        [TestCase(5, 2, 25)]
        [TestCase(5, 3, 125)]
        [TestCase(5, -1, 0.2)]
        [TestCase(5, -2, 0.04)]
        [TestCase(-5, 0, 1)]
        [TestCase(-5, 1, -5)]
        [TestCase(-5, 2, 25)]
        [TestCase(-5, 3, -125)]
        [TestCase(-5, -1, -0.2)]
        [TestCase(-5, -2, 0.04)]
        public void IntegerPowerAsExpected(double value, int power, double expected)
        {
            var actual = value.IntegerPower(power);
            Assert.That(actual, Is.EqualTo(expected).Within(Tolerance));
        }
    }
}
