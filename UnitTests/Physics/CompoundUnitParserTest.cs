using System.Linq;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class CompoundUnitParserTest
    {
        [Test]
        public void NullStringResultsInUnitlessValue()
        {
            var actual = CompoundUnitParser.Parse(null);
            Assert.That(actual.Unit, Is.EqualTo(new CompoundUnit()));
            Assert.That(actual.Value, Is.EqualTo(1));
        }

        [Test]
        public void EmptyStringResultsInUnitlessValue()
        {
            var actual = CompoundUnitParser.Parse(string.Empty);
            Assert.That(actual.Unit, Is.EqualTo(new CompoundUnit()));
            Assert.That(actual.Value, Is.EqualTo(1));
        }

        [Test]
        public void CanParseSimpleUnits()
        {
            var units = EnumExtensions.GetValues<Unit>().Except(new[] {Unit.Compound});
            foreach (var unit in units)
            {
                var unitString = UnitValueExtensions.UnitStringRepresentation[unit];
                var actual = CompoundUnitParser.Parse(unitString);
                var conversionResult = 1d.ConvertToSI(unit);
                Assert.That(actual.Unit, Is.EqualTo(conversionResult.Unit));
                Assert.That(actual.Value, Is.EqualTo(conversionResult.Value).Within(1e-3*conversionResult.Value));
            }
        }

        [Test]
        public void CanParseComplexUnit1()
        {
            var unitString = "mm^3/us";
            var actual = CompoundUnitParser.Parse(unitString);
            var expected = 1e-9.To(Unit.CubicMeters) / 1e-6.To(Unit.Second);
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
            Assert.That(actual.Value, Is.EqualTo(expected.Value).Within(1e-3*expected.Value));
        }

        [Test]
        public void CanParseComplexUnit2()
        {
            var unitString = "mg mM/(L ms^2)";
            var actual = CompoundUnitParser.Parse(unitString);
            var expected = 1e-3.To(Unit.Gram) * 1e-3.To(Unit.Molar) / (1.To(Unit.Liter) * 1e-6.To(Unit.Second)*1.To(Unit.Second));
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
            Assert.That(actual.Value, Is.EqualTo(expected.Value).Within(1e-3*expected.Value));
        }
    }
}
