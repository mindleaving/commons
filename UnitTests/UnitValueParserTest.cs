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
        public void CanParseUnitValue(string s, double expectedValue, Unit expectedUnit)
        {
            UnitValue unitValue = null;
            Assert.That(() => unitValue = UnitValue.Parse(s), Throws.Nothing);
            Assert.That(unitValue.Value, Is.EqualTo(expectedValue).Within(1e-15));
            Assert.That(unitValue.Unit.ToUnit(), Is.EqualTo(expectedUnit));
        }
    }
}
