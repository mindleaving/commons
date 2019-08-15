using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class UnitConverterTest
    {
        private static object[] ConvertToUnitDoubleInputAsExpectedTestCases = {
            new object[] { 1, "ft", "m", 0.3048 },
            new object[]{ 30, "°C", "°F", 86 }
        };

        [Test]
        [TestCaseSource(nameof(ConvertToUnitDoubleInputAsExpectedTestCases))]
        public void ConvertToUnitDoubleInputAsExpected(double input, string fromUnitString, string toUnitString, double expected)
        {
            var actual = input.ConvertUnit(fromUnitString, toUnitString);
            Assert.That(actual, Is.EqualTo(expected).Within(1e-6));
        }

        private static object[] ConvertToUnitStringInputAsExpectedTestCases = {
            new object[] { "1 ft", "m", 0.3048 },
            new object[]{ "30 °C", "°F", 86 }
        };

        [Test]
        [TestCaseSource(nameof(ConvertToUnitStringInputAsExpectedTestCases))]
        public void ConvertToUnitStringInputAsExpected(string unitValueString, string toUnitString, double expected)
        {
            var actual = UnitConverter.ConvertUnit(unitValueString, toUnitString);
            Assert.That(actual, Is.EqualTo(expected).Within(1e-6));
        }

        [Test]
        [TestCase("1 m", "L")]
        public void CanConvertToFalse(string unitValueString, string toUnitString)
        {
            var unitValue = UnitValue.Parse(unitValueString);
            var actual = unitValue.CanConvertTo(toUnitString);
            Assert.That(actual, Is.False);
        }

        [Test]
        [TestCase("1 m", "ft")]
        public void CanConvertToTrue(string unitValueString, string toUnitString)
        {
            var unitValue = UnitValue.Parse(unitValueString);
            var actual = unitValue.CanConvertTo(toUnitString);
            Assert.That(actual, Is.True);
        }
    }
}
