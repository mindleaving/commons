using Commons.Physics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class CompoundUnitTest
    {
        [Test]
        public void SerializationRoundTrip()
        {
            var sut = Unit.Pascal.ToCompoundUnit();
            var json = JsonConvert.SerializeObject(sut);
            CompoundUnit reconstructuedUnit = null;
            Assert.That(() =>reconstructuedUnit = JsonConvert.DeserializeObject<CompoundUnit>(json), Throws.Nothing);
            Assert.That(reconstructuedUnit, Is.Not.Null);
            for (var idx = 0; idx < sut.UnitExponents.Length; idx++)
            {
                Assert.That(reconstructuedUnit.UnitExponents[idx], Is.EqualTo(sut.UnitExponents[idx]));
            }
        }

        [Test]
        [TestCase("m", Unit.Meter)]
        [TestCase("kg m/s^2", Unit.Newton)]
        [TestCase("K", Unit.Kelvin)]
        public void CompoundUnitIsCorrectlyParsed(string unitString, Unit expectedUnit)
        {
            var expected = expectedUnit.ToSIUnit().ToCompoundUnit();
            var actual = CompoundUnitParser.Parse(unitString);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
