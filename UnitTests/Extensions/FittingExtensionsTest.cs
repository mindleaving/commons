using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;
using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class FittingExtensionsTest
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

        [Test]
        public void PolynomialFitFindsBestSolution()
        {
            var points = new List<Point2D>
            {
                new Point2D(67.14, 6.63),
                new Point2D(67.69, 6.58),
                new Point2D(73.33, 6.56),
                new Point2D(73.85, 6.51),
                new Point2D(76.22, 6.46),
                new Point2D(77.65, 6.38),
                new Point2D(82.94, 6.33),
                new Point2D(84.37, 6.27),
                new Point2D(90.97, 6.22),
                new Point2D(97.78, 6.20),
                new Point2D(110.40, 6.13),
                new Point2D(120.00, 6.05),
                new Point2D(138.62, 6.02),
                new Point2D(139.29, 5.95),
                new Point2D(150.00, 5.86),
                new Point2D(162.86, 5.84),
                new Point2D(170.27, 5.70),
                new Point2D(171.89, 5.51),
                new Point2D(173.51, 5.37),
                new Point2D(173.68, 5.20),
                new Point2D(175.50, 5.01),
                new Point2D(177.00, 4.90),
                new Point2D(178.54, 4.58),
                new Point2D(180.00, 4.44),
                new Point2D(183.91, 4.29),
                new Point2D(186.25, 4.20),
                new Point2D(185.00, 4.10),
                new Point2D(184.71, 4.02),
                new Point2D(186.79, 4.00),
                new Point2D(189.82, 3.98),
                new Point2D(189.64, 3.78),
                new Point2D(190.53, 3.80)
            };
            var benchmark = new PolynomialFittingResult2D(new[]
            {
                -12.62,
                9.1118e-1,
                -1.6117e-2,
                1.3185e-4,
                -5.0090e-7,
                7.0052e-10
            }, double.NaN);

            var benchmarkMse = points.Average(p => (benchmark.Apply(p.X) - p.Y).Square());
            var actual = points.FitPolynomial(5);
            var actualValues = points.Select(p => actual.Apply(p.X)).ToList();
            var fullRange = Enumerable.Range(0, 360).Select(x => actual.Apply(x)).ToList();
            Assert.That(actual.MeanSquareError, Is.LessThanOrEqualTo(benchmarkMse));
        }

        [Test]
        public void PolynomialFitThroughAllPoints()
        {
            var points = new List<Point2D>
            {
                new Point2D(0, 1),
                new Point2D(1, 3),
                new Point2D(2, 3),
                new Point2D(4, 1)
            };
            var actual = points.FitPolynomial(3);
            foreach (var point in points)
            {
                var expectedY = point.Y;
                var actualY = actual.Apply(point.X);
                Assert.That(actualY, Is.EqualTo(expectedY).Within(Tolerance));
            }
        }
    }
}
