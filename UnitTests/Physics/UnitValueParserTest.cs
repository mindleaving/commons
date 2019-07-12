using System;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class UnitValueParserTest
    {
        private static object[] CanParseUnitValueTestCases =
        {
            new object[] {"100 m", 100, Units.Meter},
            new object[] {"100m", 100, Units.Meter},
            new object[] {"11.3 g", 0.0113, Units.Kilogram},
            new object[] {"3.4 s", 3.4, Units.Second},
            new object[] {"5.1 ft", 1.55448, Units.Meter},
            new object[] {"-3.4 s", -3.4, Units.Second},
            new object[] {"-5.3 km", -5300, Units.Meter},
            new object[] {"∞ kg", double.PositiveInfinity, Units.Kilogram},
            new object[] {"Inf m", double.PositiveInfinity, Units.Meter},
            new object[] {"inf m", double.PositiveInfinity, Units.Meter},
            new object[] {"Infinity m", double.PositiveInfinity, Units.Meter},
            new object[] {"infinity m", double.PositiveInfinity, Units.Meter},
            new object[] {"-∞ kg", double.NegativeInfinity, Units.Kilogram},
            new object[] {"-Inf m", double.NegativeInfinity, Units.Meter},
            new object[] {"-inf m", double.NegativeInfinity, Units.Meter},
            new object[] {"-Infinity m", double.NegativeInfinity, Units.Meter},
            new object[] {"-infinity m", double.NegativeInfinity, Units.Meter},
            new object[] {"NaN m", double.NaN, Units.Meter},
            new object[] {"12 l", 1.2e-2, Units.CubicMeters},
            new object[] {"12 ml", 1.2e-5, Units.CubicMeters},
            new object[] {"1.2 µL", 1.2e-9, Units.CubicMeters}, // \u00b5
            new object[] {"1.2 μL", 1.2e-9, Units.CubicMeters} // \u03bc
        };
        [Test]
        [TestCaseSource(nameof(CanParseUnitValueTestCases))]
        public void CanParseUnitValue(string s, double expectedValue, IUnitDefinition expectedUnit)
        {
            UnitValue unitValue = null;
            Assert.That(() => unitValue = UnitValue.Parse(s), Throws.Nothing);
            Assert.That(unitValue.Value, Is.EqualTo(expectedValue).Within(1e-15));
            Assert.That(unitValue.Unit, Is.EqualTo(expectedUnit));
        }

        [Test]
        public void CanParseComplexUnit1()
        {
            var s = "1.3 mg/L";
            var expected = 1.3.To(SIPrefix.Milli, Units.Gram) / 1.To(Units.Liter);
            UnitValue actual = null;
            Assert.That(() => actual = UnitValue.Parse(s), Throws.Nothing);
            Assert.That(actual.Value, Is.EqualTo(expected.Value));
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
        }

        [Test]
        public void CanParseComplexUnit2()
        {
            var s = "1.3 mm^3/uL";
            var expected = 1.3*1e-9.To(Units.CubicMeters) / 1.To(SIPrefix.Micro, Units.Liter);
            UnitValue actual = null;
            Assert.That(() => actual = UnitValue.Parse(s), Throws.Nothing);
            Assert.That(actual.Value, Is.EqualTo(expected.Value));
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
        }

        [Test]
        public void CanParseComplexUnit3()
        {
            var s = "1.3 kn/°C";
            var expected = 1.3.To(Units.Knots) / 1.To(Units.Celsius);
            UnitValue actual = null;
            Assert.That(() => actual = UnitValue.Parse(s), Throws.Nothing);
            Assert.That(actual.Value, Is.EqualTo(expected.Value));
            Assert.That(actual.Unit, Is.EqualTo(expected.Unit));
        }

        [Test]
        [TestCase("5.3 not a value")]
        [TestCase("not a value")]
        [TestCase("m")]
        public void UnparsableUnitValueThrowsFormatException(string str)
        {
            Assert.That(() => UnitValue.Parse(str), Throws.TypeOf<FormatException>());
        }

        [Test]
        public void CanParseCustomUnit()
        {
            var unitString = "1.4 xx";
            Assume.That(() => UnitValue.Parse(unitString), Throws.Exception);

            Units.Effective = new MyUnits();
            UnitValue actual = null;
            Assert.That(() => actual = UnitValue.Parse(unitString), Throws.Nothing);
            Assert.That(actual.Unit, Is.EqualTo(MyUnits.HalfSecond.CorrespondingCompoundUnit));
            Assert.That(actual.In(MyUnits.HalfSecond), Is.EqualTo(1.4).Within(1e-3));
        }

        private class MyUnits : Units
        {
            public MyUnits()
            {
                AddUnit(HalfSecond);
            }

            public static readonly IUnitDefinition HalfSecond = new UnitDefinition("xx", false, CompoundUnits.Second, x => x / 2, x => x * 2);
        }
    }
}
