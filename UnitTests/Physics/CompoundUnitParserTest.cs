using System;
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
            var actual = CompoundUnitParser.Parse(1d, null);
            Assert.That(actual.Unit, Is.EqualTo(new CompoundUnit()));
            Assert.That(actual.Value, Is.EqualTo(1));
        }

        [Test]
        public void EmptyStringResultsInUnitlessValue()
        {
            var actual = CompoundUnitParser.Parse(1d, string.Empty);
            Assert.That(actual.Unit, Is.EqualTo(new CompoundUnit()));
            Assert.That(actual.Value, Is.EqualTo(1));
        }

        [Test]
        public void CanParseSimpleUnits()
        {
            var units = Unit.Effective.AllUnits;
            foreach (var unit in units)
            {
                var unitString = unit.StringRepresentation;
                var actual = CompoundUnitParser.Parse(1d, unitString);
                Assert.That(actual.Unit, Is.EqualTo(unit.CorrespondingCompoundUnit));
            }
        }

        [Test]
        public void CanParseComplexUnit1()
        {
            var unitString = "mm^3/us";
            var actual = CompoundUnitParser.Parse(1d, unitString);
            var expected = 1e-9.To(Unit.CubicMeters) / 1e-6.To(Unit.Second);
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
            Assert.That(actual.Value, Is.EqualTo(expected.Value).Within(1e-3*expected.Value));
        }

        [Test]
        public void CanParseComplexUnit2()
        {
            var unitString = "mg mM/(L ms^2)";
            var actual = CompoundUnitParser.Parse(1d, unitString);
            var expected = 1e-3.To(Unit.Gram) * 1e-3.To(Unit.Molar) / (1.To(Unit.Liter) * 1e-6.To(Unit.Second)*1.To(Unit.Second));
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
            Assert.That(actual.Value, Is.EqualTo(expected.Value).Within(1e-3*expected.Value));
        }

        [Test]
        public void UnitWithNumbersIsRejected()
        {
            var unitString = "µL/200mL";
            Assert.That(() => CompoundUnitParser.Parse(1d, unitString), Throws.TypeOf<FormatException>());
        }

        [Test]
        [TestCase("uL", 1e-9)]
        [TestCase("μL", 1e-9)]
        [TestCase("nm", 1e-9)]
        [TestCase("kN", 1e3)]
        public void PrefixResultsInExpectedMultiplier(string str, double expectedMultiplier)
        {
            var actual = CompoundUnitParser.Parse(1d, str);
            Assert.That(actual.Value, Is.EqualTo(expectedMultiplier).Within(1e-3*expectedMultiplier));
        }
    }
}
