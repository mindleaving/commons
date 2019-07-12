using System.Linq;
using Commons.Extensions;
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
            var unitPoint = new UnitPoint2D(SIPrefix.Milli, Units.Meter, 1.5, 6.3);
            var json = JsonConvert.SerializeObject(unitPoint);
            UnitPoint2D reconstructedUnitPoint = null;
            Assert.That(() => reconstructedUnitPoint = JsonConvert.DeserializeObject<UnitPoint2D>(json), Throws.Nothing);
            Assert.That(reconstructedUnitPoint, Is.Not.Null);
            Assert.That(reconstructedUnitPoint.Unit, Is.EqualTo(unitPoint.Unit));
            Assert.That(reconstructedUnitPoint.X, Is.EqualTo(unitPoint.X));
            Assert.That(reconstructedUnitPoint.Y, Is.EqualTo(unitPoint.Y));
        }

        [Test]
        [TestCase("v1.1.25", "{\"Unit\":{\"UnitString\":\"m\"},\"X\":0.0015,\"Y\":0.0063}")]
        [TestCase("v2.0.1", "{\"Unit\":\"m\",\"X\":0.0015,\"Y\":0.0063}")]
        public void DeserializationIsBackwardCompatible(string version, string json)
        {
            UnitPoint2D unitPoint = null;
            Assert.That(() => unitPoint = JsonConvert.DeserializeObject<UnitPoint2D>(json), Throws.Nothing);
            Assert.That(unitPoint, Is.Not.Null);
            Assert.That(unitPoint.In(SIPrefix.Milli, Units.Meter).X, Is.EqualTo(1.5).Within(1e-6));
            Assert.That(unitPoint.In(SIPrefix.Milli, Units.Meter).Y, Is.EqualTo(6.3).Within(1e-6));
        }

        [Test]
        public void CanCreateAllPrefixUnitCombinations()
        {
            var prefixes = EnumExtensions.GetValues<SIPrefix>();
            var units = Units.Effective.AllUnits.ToList();
            foreach (var prefix in prefixes)
            {
                foreach (var unit in units)
                {
                    UnitPoint2D unitPoint = null;
                    Assert.That(() => unitPoint = new UnitPoint2D(prefix, unit, 4.2, -1.1), Throws.Nothing);
                    Assert.That(unitPoint, Is.Not.Null);
                    if(unit.InSet(Units.Celsius, Units.Fahrenheit)) // Because celsius/fahrenheit has a fixed offset,
                        // very small values are lost because of insignificance compared to offset
                        continue;

                    Assert.That(unitPoint.In(prefix, unit).X, Is.EqualTo(4.2).Within(1e-6));
                    Assert.That(unitPoint.In(prefix, unit).Y, Is.EqualTo(-1.1).Within(1e-6));
                }
            }
        }
    }
}
