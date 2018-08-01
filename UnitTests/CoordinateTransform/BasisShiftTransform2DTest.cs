using Commons.CoordinateTransform;
using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.CoordinateTransform
{
    [TestFixture]
    public class BasisShiftTransform2DTest
    {
        private const double Tolerance = 1e-12;

        [Test]
        public void BasisVectorsAreReturnedAsUnitVectors()
        {
            var basis1 = new Vector2D(1, 2);
            var basis2 = new Vector2D(-2, 1); // ortogonal
            var sut = new BasisShiftTransform2D(basis1, basis2);

            var basis1Transformed = sut.Transform(basis1);
            var basis2Transformed = sut.Transform(basis2);

            Assert.That(basis1Transformed.X, Is.EqualTo(1).Within(Tolerance));
            Assert.That(basis1Transformed.Y, Is.EqualTo(0).Within(Tolerance));
            Assert.That(basis2Transformed.X, Is.EqualTo(0).Within(Tolerance));
            Assert.That(basis2Transformed.Y, Is.EqualTo(1).Within(Tolerance));
        }
    }
}
