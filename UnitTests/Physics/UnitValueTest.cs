using System;
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
            Unit.Meter, Unit.Feet, Unit.FeetPerMinute, Unit.Celsius, 
            Unit.Kilogram, Unit.InchesOfMercury, Unit.Fahrenheit, Unit.Mach, 
            Unit.Liter
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
            var unit = Unit.Meter;
            var number = StaticRandom.Rng.NextDouble();

            var unitValue = number.To(prefix, unit);
            var roundtripNumber = unitValue.In(prefix, unit);

            Assert.That(roundtripNumber, Is.EqualTo(number).Within(1e-5));
        }

        [Test]
        public void ProductReturnsCorrectValueAndUnit()
        {
            var mass = 1.5.To(Unit.Kilogram);
            var acceleration = 3.To(Unit.MetersPerSecondSquared);

            var product = mass*acceleration;

            Assert.That(product.Unit.Equals(Unit.Newton));
            var expectedValue = mass.In(SIPrefix.Kilo, Unit.Gram)*acceleration.In(Unit.MetersPerSecondSquared);
            Assert.That(product.Value, Is.EqualTo(expectedValue).Within(1e-5));
        }

        [Test]
        public void GramPerMilliliterTest()
        {
            var weight = 4.To(SIPrefix.Kilo, Unit.Gram);
            var volume = 8.To(Unit.Liter);
            var density = weight / volume;
            Assert.That(density.Value, Is.EqualTo(500).Within(1e-5));
        }


        private static object[] CanConvertToTrueForCompatibleUnitsTestCases =
        {
            new object[] {Unit.CubicMeters, Unit.Liter},
            new object[] {Unit.Kilogram, Unit.Gram},
            new object[] {Unit.ElectronVolts, Unit.Joule},
            new object[] {Unit.Fahrenheit, Unit.Celsius},
            new object[] {Unit.Fahrenheit, Unit.Kelvin},
            new object[] {Unit.Meter, Unit.Feet},
            new object[] {Unit.Meter, Unit.Inches},
            new object[] {Unit.MetersPerSecond, Unit.Knots}
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
            new object[] {Unit.CubicMeters, Unit.Kilogram},
            new object[] {Unit.Bar, Unit.Celsius},
            new object[] {Unit.Knots, Unit.Feet}
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
            new object[] {Unit.Meter, Unit.Inches, 39.3700787},
            new object[] {Unit.Inches, Unit.Meter, 0.0254},
            new object[] {Unit.Meter, Unit.NauticalMile, 0.000539956803},
            new object[] {Unit.NauticalMile, Unit.Meter, 1852},
            new object[] {Unit.Feet, Unit.Inches, 12}
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
            var units = Unit.Effective.AllUnits.ToList();
            foreach (var prefix in prefixes)
            {
                foreach (var unit in units)
                {
                    UnitValue unitValue = null;
                    Assert.That(() => unitValue = 4.2.To(prefix, unit), Throws.Nothing);
                    Assert.That(unitValue, Is.Not.Null);
                    if(unit.InSet(Unit.Celsius, Unit.Fahrenheit)) // Because celsius/fahrenheit has a fixed offset,
                                     // very small values are lost because of insignificance compared to offset
                        continue;

                    Assert.That(unitValue.In(prefix, unit), Is.EqualTo(4.2).Within(1e-6));
                }
            }
        }
    }
}
