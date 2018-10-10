using Commons;
using Commons.Extensions;
using Commons.Physics;
using NUnit.Framework;

namespace CommonsTest
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
    }
}
