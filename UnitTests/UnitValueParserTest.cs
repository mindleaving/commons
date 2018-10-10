using System;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest
{
    [TestFixture]
    public class UnitValueParserTest
    {
        [Test]
        [TestCase("100 m", 100, Unit.Meter)]
        [TestCase("100m", 100, Unit.Meter)]
        [TestCase("11.3 g", 0.0113, Unit.Kilogram)]
        [TestCase("3.4 s", 3.4, Unit.Second)]
        [TestCase("5.1 ft", 1.55448, Unit.Meter)]
        [TestCase("-3.4 s", -3.4, Unit.Second)]
        [TestCase("-5.3 km", -5300, Unit.Meter)]
        [TestCase("∞ kg", double.PositiveInfinity, Unit.Kilogram)]
        [TestCase("Inf m", double.PositiveInfinity, Unit.Meter)]
        [TestCase("inf m", double.PositiveInfinity, Unit.Meter)]
        [TestCase("Infinity m", double.PositiveInfinity, Unit.Meter)]
        [TestCase("infinity m", double.PositiveInfinity, Unit.Meter)]
        [TestCase("-∞ kg", double.NegativeInfinity, Unit.Kilogram)]
        [TestCase("-Inf m", double.NegativeInfinity, Unit.Meter)]
        [TestCase("-inf m", double.NegativeInfinity, Unit.Meter)]
        [TestCase("-Infinity m", double.NegativeInfinity, Unit.Meter)]
        [TestCase("-infinity m", double.NegativeInfinity, Unit.Meter)]
        [TestCase("NaN m", double.NaN, Unit.Meter)]
        public void CanParseUnitValue(string s, double expectedValue, Unit expectedUnit)
        {
            UnitValue unitValue = null;
            Assert.That(() => unitValue = UnitValue.Parse(s), Throws.Nothing);
            Assert.That(unitValue.Value, Is.EqualTo(expectedValue).Within(1e-15));
            Assert.That(unitValue.Unit.ToUnit(), Is.EqualTo(expectedUnit));
        }

        [Test]
        [TestCase("5.3 not a value")]
        [TestCase("not a value")]
        [TestCase("m")]
        public void UnparsableUnitValueThrowsFormatException(string str)
        {
            Assert.That(() => UnitValue.Parse(str), Throws.TypeOf<FormatException>());
        }
    }
}
