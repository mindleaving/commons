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
            var sut = Units.Pascal.CorrespondingCompoundUnit;
            var json = JsonConvert.SerializeObject(sut);
            CompoundUnit reconstructuedUnit = null;
            Assert.That(() =>reconstructuedUnit = JsonConvert.DeserializeObject<CompoundUnit>(json), Throws.Nothing);
            Assert.That(reconstructuedUnit, Is.Not.Null);
            for (var idx = 0; idx < sut.UnitExponents.Length; idx++)
            {
                Assert.That(reconstructuedUnit.UnitExponents[idx], Is.EqualTo(sut.UnitExponents[idx]));
            }
        }

        private static object[] CompoundUnitIsCorrectlyParsedTestCases = {
            new object[] { "m", Units.Meter},
            new object[] { "kg m/s^2", Units.Newton},
            new object[] { "°K", Units.Kelvin}
        };

        [Test]
        [TestCaseSource(nameof(CompoundUnitIsCorrectlyParsedTestCases))]
        public void CompoundUnitIsCorrectlyParsed(string unitString, IUnitDefinition expectedUnit)
        {
            var expected = expectedUnit.CorrespondingCompoundUnit;
            var actual = CompoundUnitParser.Parse(1d, unitString);
            Assert.That(actual.Unit, Is.EqualTo(expected));
        }
    }
}
