using Commons;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest.Physics
{
    [TestFixture]
    public class UnitValueTest
    {
        [Test]
        [TestCase(Unit.Meter)]
        [TestCase(Unit.Feet)]
        [TestCase(Unit.FeetPerMinute)]
        [TestCase(Unit.Kilogram)]
        [TestCase(Unit.InchesOfMercury)]
        [TestCase(Unit.Fahrenheit)]
        [TestCase(Unit.Mach)]
        [TestCase(Unit.Liter)]
        public void UnitConversionRoundtripReturnsInput(Unit unit)
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

            Assert.That(product.Unit.ToUnit() == Unit.Newton);
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

        [Test]
        [TestCase(Unit.CubicMeters, Unit.Liter)]
        [TestCase(Unit.Kilogram, Unit.Gram)]
        [TestCase(Unit.ElectronVolts, Unit.Joule)]
        [TestCase(Unit.Fahrenheit, Unit.Celcius)]
        [TestCase(Unit.Fahrenheit, Unit.Kelvin)]
        [TestCase(Unit.Meter, Unit.Feet)]
        [TestCase(Unit.Meter, Unit.Inches)]
        [TestCase(Unit.MetersPerSecond, Unit.Knots)]
        public void CanConvertToTrueForCompatibleUnits(Unit originalUnit, Unit targetUnit)
        {
            var unitValue = 1.To(originalUnit);
            Assert.That(unitValue.CanConvertTo(targetUnit), Is.True);
        }

        [Test]
        [TestCase(Unit.CubicMeters, Unit.Kilogram)]
        [TestCase(Unit.Bar, Unit.Celcius)]
        [TestCase(Unit.Knots, Unit.Feet)]
        public void CanConvertToFalseForIncompatibleUnits(Unit originalUnit, Unit targetUnit)
        {
            var unitValue = 1.To(originalUnit);
            Assert.That(unitValue.CanConvertTo(targetUnit), Is.False);
        }

        [Test]
        [TestCase(Unit.Meter, Unit.Inches, 39.3700787)]
        [TestCase(Unit.Inches, Unit.Meter, 0.0254)]
        [TestCase(Unit.Meter, Unit.NauticalMile, 0.000539956803)]
        [TestCase(Unit.NauticalMile, Unit.Meter, 1852)]
        [TestCase(Unit.Feet, Unit.Inches, 12)]
        public void UnitConversionAsExpected(Unit originalUnit, Unit targetUnit, double expectedValue)
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
    }
}
