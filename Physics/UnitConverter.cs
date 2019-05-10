using System;
using Commons.Extensions;

namespace Commons.Physics
{
    public static class UnitConverter
    {
        public static bool CanConvertTo(this UnitValue unitValue, Unit unit)
        {
            return unitValue.Unit == unit.ToSIUnit().ToCompoundUnit();
        }

        public static bool CanConvertTo(this UnitValue unitValue, CompoundUnit unit)
        {
            return unitValue.Unit == unit;
        }

        public static CompoundUnit ToCompoundUnit(this Unit unit)
        {
            if(!unit.IsSIUnit())
                throw new Exception("Can only convert SI unit");
            switch (unit.ToSIUnit())
            {
                case Unit.None:
                    return new CompoundUnit();
                case Unit.Meter:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Meter });
                case Unit.MetersPerSecond:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Meter },
                        new[] { SIBaseUnit.Second });
                case Unit.MetersPerSecondSquared:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Meter },
                        new[] { SIBaseUnit.Second, SIBaseUnit.Second });
                case Unit.Second:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Second });
                case Unit.Kelvin:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Kelvin });
                case Unit.Pascal:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Kilogram },
                        new[] { SIBaseUnit.Meter, SIBaseUnit.Second, SIBaseUnit.Second });
                case Unit.SquareMeter:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Meter, SIBaseUnit.Meter });
                case Unit.CubicMeters:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Meter, SIBaseUnit.Meter, SIBaseUnit.Meter });
                case Unit.Kilogram:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Kilogram });
                case Unit.KilogramPerMole:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Kilogram },
                        new[] { SIBaseUnit.Mole });
                case Unit.Coulombs:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Ampere, SIBaseUnit.Second });
                case Unit.Joule:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Kilogram, SIBaseUnit.Meter, SIBaseUnit.Meter },
                        new[] { SIBaseUnit.Second, SIBaseUnit.Second });
                case Unit.Newton:
                    return new CompoundUnit(
                        new[] { SIBaseUnit.Kilogram, SIBaseUnit.Meter },
                        new[] { SIBaseUnit.Second, SIBaseUnit.Second });
                case Unit.Radians:
                    return new CompoundUnit(
                        new []{SIBaseUnit.Radians });
                case Unit.Mole:
                    return new CompoundUnit(
                        new []{SIBaseUnit.Mole });
                case Unit.Molar:
                    return new CompoundUnit(new []{SIBaseUnit.Mole}, new []{SIBaseUnit.Meter, SIBaseUnit.Meter, SIBaseUnit.Meter});
                default:
                    throw new NotImplementedException();
            }
        }

        public static bool IsSIUnit(this Unit unit)
        {
            // Units that are directly representable by compound unit without value conversion
            return unit.InSet(
                Unit.None,
                Unit.Meter,
                Unit.MetersPerSecond,
                Unit.MetersPerSecondSquared,
                Unit.Second,
                Unit.Kelvin,
                Unit.Pascal,
                Unit.SquareMeter,
                Unit.CubicMeters,
                Unit.Kilogram,
                Unit.KilogramPerMole,
                Unit.Mole,
                Unit.Molar,
                Unit.Coulombs,
                Unit.Joule,
                Unit.Newton,
                Unit.Radians);
        }

        public static Unit ToSIUnit(this Unit unit)
        {
            if (unit.IsSIUnit())
                return unit;

            switch (unit)
            {
                case Unit.Feet:
                case Unit.StatuteMile:
                case Unit.NauticalMile:
                case Unit.Inches:
                    return Unit.Meter;
                case Unit.Minute:
                    return Unit.Second;
                case Unit.FeetPerMinute:
                case Unit.Knots:
                case Unit.Mach:
                    return Unit.MetersPerSecond;
                case Unit.KnotsPerSeond:
                    return Unit.MetersPerSecondSquared;
                case Unit.Celsius:
                case Unit.Fahrenheit:
                    return Unit.Kelvin;
                case Unit.Bar:
                case Unit.InchesOfMercury:
                case Unit.MillimeterOfMercury:
                case Unit.Torr:
                    return Unit.Pascal;
                case Unit.ElementaryCharge:
                    return Unit.Coulombs;
                case Unit.ElectronVolts:
                    return Unit.Joule;
                case Unit.Degree:
                    return Unit.Radians;
                case Unit.Liter:
                    return Unit.CubicMeters;
                case Unit.Gram:
                    return Unit.Kilogram;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            }
        }

        public static UnitConversionResult ConvertToSI(this double value, Unit unit)
        {
            switch (unit)
            {
                case Unit.Compound:
                    throw new NotSupportedException("Cannot convert compound unit");
                case Unit.Feet:
                    return new UnitConversionResult(Unit.Meter.ToCompoundUnit(), value * 0.3048);
                case Unit.NauticalMile:
                    return new UnitConversionResult(Unit.Meter.ToCompoundUnit(), value * 1852);
                case Unit.Inches:
                    return new UnitConversionResult(Unit.Meter.ToCompoundUnit(), value * 0.0254);
                case Unit.StatuteMile:
                    return new UnitConversionResult(Unit.Meter.ToCompoundUnit(), value * 1609.344);
                case Unit.Minute:
                    return new UnitConversionResult(Unit.Second.ToCompoundUnit(), value * 60);
                case Unit.FeetPerMinute:
                    return new UnitConversionResult(Unit.MetersPerSecond.ToCompoundUnit(), value * 0.00508);
                case Unit.Knots:
                    return new UnitConversionResult(Unit.MetersPerSecond.ToCompoundUnit(), value * 0.514444444);
                case Unit.Mach:
                    return new UnitConversionResult(Unit.MetersPerSecond.ToCompoundUnit(), value * 340.29);
                case Unit.KnotsPerSeond:
                    return new UnitConversionResult(Unit.MetersPerSecondSquared.ToCompoundUnit(), value * 0.514444444);
                case Unit.Celsius:
                    return new UnitConversionResult(Unit.Kelvin.ToCompoundUnit(), value + 273.15);
                case Unit.Fahrenheit:
                    return new UnitConversionResult(Unit.Kelvin.ToCompoundUnit(), (value + 459.67) * (5.0 / 9.0));
                case Unit.Bar:
                    return new UnitConversionResult(Unit.Pascal.ToCompoundUnit(), value * 1e5);
                case Unit.InchesOfMercury:
                    return new UnitConversionResult(Unit.Pascal.ToCompoundUnit(), value * 3386.38816);
                case Unit.MillimeterOfMercury:
                    return new UnitConversionResult(Unit.Pascal.ToCompoundUnit(), value * 133.322387415);
                case Unit.Torr:
                    return new UnitConversionResult(Unit.Pascal.ToCompoundUnit(), value * (101325.0/760));
                case Unit.ElementaryCharge:
                    return new UnitConversionResult(Unit.Coulombs.ToCompoundUnit(), value * 1.60217662 * 1e-19);
                case Unit.ElectronVolts:
                    return new UnitConversionResult(Unit.Joule.ToCompoundUnit(), value * 1.60217662 * 1e-19);
                case Unit.Degree:
                    return new UnitConversionResult(Unit.Radians.ToCompoundUnit(), value * Math.PI / 180);
                case Unit.Liter:
                    return new UnitConversionResult(Unit.CubicMeters.ToCompoundUnit(), value * 0.001);
                case Unit.Gram:
                    return new UnitConversionResult(Unit.Kilogram.ToCompoundUnit(), value * 0.001);
                case Unit.Molar:
                    return new UnitConversionResult(Unit.Molar.ToCompoundUnit(), value * 0.001);

                // SI-units. Needs to match .IsSIUnit()-list
                case Unit.None:
                case Unit.Meter:
                case Unit.MetersPerSecond:
                case Unit.MetersPerSecondSquared:
                case Unit.Second:
                case Unit.Kelvin:
                case Unit.Pascal:
                case Unit.SquareMeter:
                case Unit.CubicMeters:
                case Unit.Kilogram:
                case Unit.KilogramPerMole:
                case Unit.Mole:
                case Unit.Coulombs:
                case Unit.Joule:
                case Unit.Newton:
                case Unit.Radians:
                    return new UnitConversionResult(unit.ToCompoundUnit(), value);
                default:
                    throw new NotSupportedException($"Conversion of {unit} to standard is not implemented");
            }
        }
    }

    public class UnitConversionResult
    {
        public UnitConversionResult(CompoundUnit unit, double value)
        {
            Unit = unit;
            Value = value;
        }

        public CompoundUnit Unit { get; }
        public double Value { get; }
    }
}
