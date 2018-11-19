using Commons.Physics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class UnitPoint2DTest
    {
        [Test]
        public void SerializationRoundTrip()
        {
            var unitPoint = new UnitPoint2D(SIPrefix.Milli, Unit.Meter, 1.5, 6.3);
            var json = JsonConvert.SerializeObject(unitPoint);
            UnitPoint2D reconstructedUnitPoint = null;
            Assert.That(() => reconstructedUnitPoint = JsonConvert.DeserializeObject<UnitPoint2D>(json), Throws.Nothing);
            Assert.That(reconstructedUnitPoint, Is.Not.Null);
            Assert.That(reconstructedUnitPoint.Unit, Is.EqualTo(unitPoint.Unit));
            Assert.That(reconstructedUnitPoint.X, Is.EqualTo(unitPoint.X));
            Assert.That(reconstructedUnitPoint.Y, Is.EqualTo(unitPoint.Y));
        }
    }
}
