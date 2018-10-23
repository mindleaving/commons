using Commons.Extensions;
using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.Extensions
{
    [TestFixture]
    public class VectorExtensionsTest
    {
        [Test]
        public void ProjectionOntoXAxisReturnsFirstCoordinate()
        {
            var xAxis = new Vector2D(1, 0);
            var v = new Vector2D(42, 46);
            var projection = v.ProjectOnto(xAxis).ToVector2D();
            Assert.That(projection.X, Is.EqualTo(v.X));
        }

        [Test]
        public void ProjectionOntoYAxisReturnsFirstCoordinate()
        {
            var yAxis = new Vector2D(0, 1);
            var v = new Vector2D(42, 46);
            var projection = v.ProjectOnto(yAxis).ToVector2D();
            Assert.That(projection.Y, Is.EqualTo(v.Y));
        }

        [Test]
        public void ProjectionNotLongerThanOriginalVector()
        {
            var v1 = new Vector2D(163, 131);
            var v2 = new Vector2D(386, 4532);
            var projection = v2.ProjectOnto(v1).ToVector2D();
            Assert.That(projection.Magnitude(), Is.LessThanOrEqualTo(v2.Magnitude()));
        }
    }
}
