using Commons;
using Commons.Extensions;
using Commons.Mathematics;
using Commons.Physics;

namespace CommonsTest.UnitTests
{
    [TestFixture]
    public class GeometryTest
    {
        [Test]
        public void MoveAlongRadialReturnsExampleResult()
        {
            // An implementation from Aviation Formulary V1.46 is used.
            // The website provides an example calculation
            // Check that our implementation return the same results

            var laxCoordinate = new GeoCoordinate(33 + 57.0 / 60, -(118 + 24.0 / 60));
            var headingInDegrees = 66;
            var distance = new UnitValue(Unit.NauticalMile, 100);

            var actual = laxCoordinate.MoveAlongRadial(headingInDegrees, distance);

            Assert.That(actual.Latitude, Is.EqualTo(34 + 37.0 / 60).Within(0.02));
            Assert.That(actual.Longitude, Is.EqualTo(-(116 + 33.0 / 60)).Within(0.02));
        }

        [Test]
        public void HeadingToHandlesPoles()
        {
            // From Aviation Formulary V1.46
            var northPole = new GeoCoordinate(90, 0);
            var southhPole = new GeoCoordinate(-90, 0);
            var D = new GeoCoordinate(34.5, 116.5);
            var headingNorthPole = northPole.HeadingTo(D);
            var headingSouthPole = southhPole.HeadingTo(D);

            Assert.That(headingNorthPole, Is.EqualTo(180).Within(0.01));
            Assert.That(headingSouthPole, Is.EqualTo(0).Within(0.01));
        }

        [Test]
        public void HeadingToPointReturnsExampleResult()
        {
            // From Aviation Formulary V1.46
            var laxCoordinate = new GeoCoordinate(33 + 57.0 / 60, -(118 + 24.0 / 60));
            var D = new GeoCoordinate(34.5, -116.5);
            var heading = laxCoordinate.HeadingTo(D);

            Assert.That(heading, Is.EqualTo(70.17).Within(0.01));
        }

        [Test]
        [TestCase(55.592200, 12.603536, 55.612478, 12.634892, 39+3)]
        [TestCase(55.612478, 12.634892, 55.592200, 12.603536, 219+3)]
        public void HeadingAsExpected(double latitude1, double longitude1, double latitude2, double longitude2, double expectedHeading)
        {
            var coordinate1 = new GeoCoordinate(latitude1, longitude1);
            var coordinate2 = new GeoCoordinate(latitude2, longitude2);
            var heading = coordinate1.HeadingTo(coordinate2);

            Assert.That(heading, Is.EqualTo(expectedHeading).Within(1));
        }

        [Test]
        public void DistanceToLineReturnsDistanceToLine()
        {
            var position = new GeoCoordinate(0.1, 0.1);
            var linePoint1 = new GeoCoordinate(0, 0);
            var linePoint2 = new GeoCoordinate(0, 0.2);

            var actual = position.DistanceToLine(linePoint1, linePoint2);
            var expected = position.GetDistanceTo(new GeoCoordinate(0, 0.1));
            Assert.That(actual.In(Unit.Meter), Is.EqualTo(expected.In(Unit.Meter)).Within(0.01 * expected.In(Unit.Meter)));
        }

        [Test]
        public void DistanceToLineSegmentReturnsDistanceToLine()
        {
            var position = new GeoCoordinate(0.1,0.1);
            var linePoint1 = new GeoCoordinate(0,0);
            var linePoint2 = new GeoCoordinate(0,0.2);

            var actual = position.DistanceToLineSegment(linePoint1, linePoint2);
            var expected = position.GetDistanceTo(new GeoCoordinate(0, 0.1));
            Assert.That(actual.In(Unit.Meter), Is.EqualTo(expected.In(Unit.Meter)).Within(0.01*expected.In(Unit.Meter)));
        }

        [Test]
        public void DistanceToLineSegmentReturnsDistanceToFirstLinePoint()
        {
            var position = new GeoCoordinate(0.1, -0.1);
            var linePoint1 = new GeoCoordinate(0, 0);
            var linePoint2 = new GeoCoordinate(0, 0.2);

            var actual = position.DistanceToLineSegment(linePoint1, linePoint2);
            var expected = position.GetDistanceTo(linePoint1);
            Assert.That(actual.In(Unit.Meter), Is.EqualTo(expected.In(Unit.Meter)).Within(0.01 * expected.In(Unit.Meter)));
        }

        [Test]
        public void DistanceToLineSegmentReturnsDistanceToSecondLinePoint()
        {
            var position = new GeoCoordinate(0.1, 0.3);
            var linePoint1 = new GeoCoordinate(0, 0);
            var linePoint2 = new GeoCoordinate(0, 0.2);

            var actual = position.DistanceToLineSegment(linePoint1, linePoint2);
            var expected = position.GetDistanceTo(linePoint2);
            Assert.That(actual.In(Unit.Meter), Is.EqualTo(expected.In(Unit.Meter)).Within(0.01 * expected.In(Unit.Meter)));
        }
    }
}
