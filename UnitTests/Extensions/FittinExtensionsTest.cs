using System.Linq;
using Commons.Extensions;
using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class FittinExtensionsTest
    {
        private const double Tolerance = 1e-12;

        [Test]
        public void LineFitForTwoPointsEqualsLineThroughPoints()
        {
            var points = new[]
            {
                new Point2D(0, 2),
                new Point2D(3, 5)
            };
            var expectedSlope = (points[1].Y - points[0].Y) / (points[1].X - points[0].X);
            var expectedOffset = points[0].Y - expectedSlope * points[0].X;
            var actual = points.FitLine();
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Slope, Is.EqualTo(expectedSlope).Within(Tolerance));
            Assert.That(actual.Offset, Is.EqualTo(expectedOffset).Within(Tolerance));
            Assert.That(actual.MeanSquareError, Is.EqualTo(0).Within(Tolerance));
        }

        [Test]
        public void ConstantPointsAreFittedWithoutSlope()
        {
            var expectedSlope = 0;
            var expectedOffset = 32.2;
            var points = SequenceGeneration.FixedStep(-5, 5, 1)
                .Select(x => new Point2D(x, expectedOffset))
                .ToList();
            var actual = points.FitLine();
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Slope, Is.EqualTo(expectedSlope).Within(Tolerance));
            Assert.That(actual.Offset, Is.EqualTo(expectedOffset).Within(Tolerance));
            Assert.That(actual.MeanSquareError, Is.EqualTo(0).Within(Tolerance));
        }

        [Test]
        public void SecondDegreePolynomialAsExpected()
        {
            var expectedBeta = new[] { 1, 2, -0.5 };
            var points = new[] { -1, 0, 1 }
                .Select(x => new Point2D(x, expectedBeta[0] + expectedBeta[1]*x + expectedBeta[2]*x*x))
                .ToList();
            var actual = points.FitPolynomial(2);
            Assert.That(actual, Is.Not.Null);
            for (int order = 0; order < expectedBeta.Length; order++)
            {
                Assert.That(actual.Beta[order], Is.EqualTo(expectedBeta[order]).Within(Tolerance));
            }
        }

        [Test]
        public void PolynomialIsAdjustedToNumberOfPoints()
        {
            var pointCount = 3; // Max order can hence be 2 (parabola)
            var expectedBeta = new[] { 1, 2, -0.5, 0, 0, 0 };
            var points = SequenceGeneration.Linspace(-1, 1, pointCount)
                .Select(x => new Point2D(x, expectedBeta[0] + expectedBeta[1]*x + expectedBeta[2]*x*x))
                .ToList();
            var actual = points.FitPolynomial(5);
            Assert.That(actual, Is.Not.Null);
            for (int order = 0; order < expectedBeta.Length; order++)
            {
                Assert.That(actual.Beta[order], Is.EqualTo(expectedBeta[order]).Within(Tolerance));
            }
        }
    }
}
