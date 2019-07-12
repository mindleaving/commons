using System.Linq;
using Commons;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class UnitValueTest
    {
        private static object[] UnitConversionRoundtripReturnsInputTestCases =
        {
            Units.Meter, Units.Feet, Units.FeetPerMinute, Units.Celsius, 
            Units.Kilogram, Units.InchesOfMercury, Units.Fahrenheit, Units.Mach, 
            Units.Liter
        };

        [Test]
        [TestCaseSource(nameof(UnitConversionRoundtripReturnsInputTestCases))]
        public void UnitConversionRoundtripReturnsInput(IUnitDefinition unit)
        {
            var number = StaticRandom.Rng.NextDouble();

            var unitValue = number.To(unit);
            var roundtripNumber = unitValue.In(unit);

            Assert.That(roundtripNumber, Is.EqualTo(number).Within(1e-5));
        }

        [Test]
        public void CompoundUnitConversionRoundtripReturnsInput()
        {
            var unit = new CompoundUnit(
                new []{SIBaseUnit.Kilogram, SIBaseUnit.Second, SIBaseUnit.Meter },
                new []{SIBaseUnit.Ampere, SIBaseUnit.Ampere });
            var number = StaticRandom.Rng.NextDouble();

            var unitValue = number.To(unit);
            var roundtripNumber = unitValue.In(unit);

            Assert.That(roundtripNumber, Is.EqualTo(number).Within(1e-5));
        }

        [Test]
        public void PrefixedUnitConversionRoundtripReturnsInput()
        {
            var prefix = SIPrefix.Micro;
            var unit = Units.Meter;
            var number = StaticRandom.Rng.NextDouble();

            var unitValue = number.To(prefix, unit);
            var roundtripNumber = unitValue.In(prefix, unit);

            Assert.That(roundtripNumber, Is.EqualTo(number).Within(1e-5));
        }

        [Test]
        public void ProductReturnsCorrectValueAndUnit()
        {
            var mass = 1.5.To(Units.Kilogram);
            var acceleration = 3.To(Units.MetersPerSecondSquared);

            var product = mass*acceleration;

            Assert.That(product.Unit.Equals(Units.Newton));
            var expectedValue = mass.In(SIPrefix.Kilo, Units.Gram)*acceleration.In(Units.MetersPerSecondSquared);
            Assert.That(product.Value, Is.EqualTo(expectedValue).Within(1e-5));
        }

        [Test]
        public void GramPerMilliliterTest()
        {
            var weight = 4.To(SIPrefix.Kilo, Units.Gram);
            var volume = 8.To(Units.Liter);
            var density = weight / volume;
            Assert.That(density.Value, Is.EqualTo(500).Within(1e-5));
        }


        private static object[] CanConvertToTrueForCompatibleUnitsTestCases =
        {
            new object[] {Units.CubicMeters, Units.Liter},
            new object[] {Units.Kilogram, Units.Gram},
            new object[] {Units.ElectronVolts, Units.Joule},
            new object[] {Units.Fahrenheit, Units.Celsius},
            new object[] {Units.Fahrenheit, Units.Kelvin},
            new object[] {Units.Meter, Units.Feet},
            new object[] {Units.Meter, Units.Inches},
            new object[] {Units.MetersPerSecond, Units.Knots}
        };
        [Test]
        [TestCaseSource(nameof(CanConvertToTrueForCompatibleUnitsTestCases))]
        public void CanConvertToTrueForCompatibleUnits(IUnitDefinition originalUnit, IUnitDefinition targetUnit)
        {
            var unitValue = 1.To(originalUnit);
            Assert.That(unitValue.CanConvertTo(targetUnit), Is.True);
        }

        private static object[] CanConvertToFalseForIncompatibleUnitsTestCases =
        {
            new object[] {Units.CubicMeters, Units.Kilogram},
            new object[] {Units.Bar, Units.Celsius},
            new object[] {Units.Knots, Units.Feet}
        };
        [Test]
        [TestCaseSource(nameof(CanConvertToFalseForIncompatibleUnitsTestCases))]
        public void CanConvertToFalseForIncompatibleUnits(IUnitDefinition originalUnit, IUnitDefinition targetUnit)
        {
            var unitValue = 1.To(originalUnit);
            Assert.That(unitValue.CanConvertTo(targetUnit), Is.False);
        }

        private static object[] UnitConversionAsExpectedTestCases =
        {
            new object[] {Units.Meter, Units.Inches, 39.3700787},
            new object[] {Units.Inches, Units.Meter, 0.0254},
            new object[] {Units.Meter, Units.NauticalMile, 0.000539956803},
            new object[] {Units.NauticalMile, Units.Meter, 1852},
            new object[] {Units.Feet, Units.Inches, 12}
        };
        [Test]
        [TestCaseSource(nameof(UnitConversionAsExpectedTestCases))]
        public void UnitConversionAsExpected(IUnitDefinition originalUnit, IUnitDefinition targetUnit, double expectedValue)
        {
            Assert.That(1.To(originalUnit).In(targetUnit), Is.EqualTo(expectedValue).Within(1e-3*expectedValue));
        }

        [Test]
        [TestCase(1, SIPrefix.None)]
        [TestCase(3, SIPrefix.None)]
        [TestCase(-4, SIPrefix.None)]
        [TestCase(1e-1, SIPrefix.Milli)] // Deci-prefix is excluded
        [TestCase(1e-2, SIPrefix.Milli)] // Centi-prefix is excluded
        [TestCase(1e-3, SIPrefix.Milli)]
        [TestCase(-1e-3, SIPrefix.Milli)]
        [TestCase(-5e-3, SIPrefix.Milli)]
        [TestCase(-5e-2, SIPrefix.Milli)]
        [TestCase(1e-6, SIPrefix.Micro)]
        public void SelectSIPrefixAsExpected(double value, SIPrefix expected)
        {
            var actual = value.SelectSIPrefix();
            Assert.That(actual, Is.EqualTo(expected));
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
                    UnitValue unitValue = null;
                    Assert.That(() => unitValue = 4.2.To(prefix, unit), Throws.Nothing);
                    Assert.That(unitValue, Is.Not.Null);
                    if(unit.InSet(Units.Celsius, Units.Fahrenheit)) // Because celsius/fahrenheit has a fixed offset,
                                     // very small values are lost because of insignificance compared to offset
                        continue;

                    Assert.That(unitValue.In(prefix, unit), Is.EqualTo(4.2).Within(1e-6));
                }
            }
        }
    }
}
