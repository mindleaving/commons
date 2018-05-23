using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class UnitValueExtensions
    {
        public static UnitValue Abs(this UnitValue unitValue)
        {
            return new UnitValue(unitValue.Unit, Math.Abs(unitValue.Value));
        }
        public static double In(this UnitValue unitValue, CompoundUnit newUnit)
        {
            if (!unitValue.Unit.Equals(newUnit))
                throw new InvalidOperationException($"Cannot convert {unitValue.Unit} to {newUnit}");
            return unitValue.Value;
        }

        public static double In(this UnitValue unitValue, Unit newUnit)
        {
            if (!newUnit.ToCompoundUnit().Equals(unitValue.Unit))
                throw new InvalidOperationException($"Cannot convert {unitValue.Unit} to {newUnit}");

            switch (newUnit)
            {
                case Unit.Compound:
                    throw new NotSupportedException("Conversion to compound unit is not supported. " +
                                                    "That enum value is intended to indicate non-named units");
                case Unit.Meter:
                case Unit.MetersPerSecond:
                case Unit.MetersPerSecondSquared:
                case Unit.Second:
                case Unit.Kelvin:
                case Unit.Pascal:
                case Unit.SquareMeter:
                case Unit.CubicMeters:
                case Unit.Kilogram:
                case Unit.Coulombs:
                case Unit.Joule:
                case Unit.Newton:
                case Unit.GramPerMole:
                case Unit.Radians:
                    return unitValue.Value;
                case Unit.Gram:
                    return 1000 * unitValue.Value;
                case Unit.Feet:
                    return 3.2808399*unitValue.Value;
                case Unit.StatuteMile:
                    return 0.000621371192 * unitValue.Value;
                case Unit.NauticalMile:
                    return 0.000539956803*unitValue.Value;
                case Unit.FeetPerMinute:
                    return 60*3.2808399*unitValue.Value;
                case Unit.Knots:
                    return 1.94384449244*unitValue.Value;
                case Unit.Mach:
                    return 0.002938669957977*unitValue.Value;
                case Unit.KnotsPerSeond:
                    return 1.94384449244 * unitValue.Value;
                case Unit.Celcius:
                    return unitValue.Value - 273.15;
                case Unit.Fahrenheit:
                    return unitValue.Value*(9.0/5.0) - 459.67;
                case Unit.Bar:
                    return 1e-5*unitValue.Value;
                case Unit.InchesOfMercury:
                    return 2.952998749e-4*unitValue.Value;
                case Unit.ElementaryCharge:
                    return unitValue.Value/(1.60217662*1e-19);
                case Unit.ElectronVolts:
                    return unitValue.Value / (1.60217662 * 1e-19);
                case Unit.Degree:
                    return unitValue.Value*180/Math.PI;
                case Unit.Liter:
                    return unitValue.Value * 1000;
                default:
                    throw new InvalidOperationException($"Conversion from {unitValue.Unit} to {newUnit} is not implemented");
            }
        }
        public static double In(this UnitValue unitValue, SIPrefix prefix, Unit unit)
        {
            var multiplier = GetMultiplier(prefix);
            return unitValue.In(unit) / multiplier;
        }

        public static UnitValue To(this double value, Unit unit)
        {
            return new UnitValue(unit, value);
        }

        public static UnitValue To(this float value, Unit unit)
        {
            return To((double)value, unit);
        }

        public static UnitValue To(this int value, Unit unit)
        {
            return To((double)value, unit);
        }

        public static UnitValue To(this double value, CompoundUnit unit)
        {
            return new UnitValue(unit, value);
        }

        public static UnitValue To(this double value, SIPrefix prefix, Unit unit)
        {
            var multiplier = GetMultiplier(prefix);
            return new UnitValue(unit, multiplier * value);
        }

        public static UnitValue To(this float value, SIPrefix prefix, Unit unit)
        {
            return To((double)value, prefix, unit);
        }

        public static UnitValue To(this int value, SIPrefix prefix, Unit unit)
        {
            return To((double)value, prefix, unit);
        }

        public static readonly Dictionary<Unit, string> UnitStringRepresentation = new Dictionary<Unit, string>
        {
            {Unit.Meter, "m"},
            {Unit.Feet, "ft"},
            {Unit.NauticalMile, "NM"},
            {Unit.StatuteMile, "mi"},
            {Unit.MetersPerSecond, "m/s"},
            {Unit.FeetPerMinute, "ft/min"},
            {Unit.Knots, "kn"},
            {Unit.Mach, "mach"},
            {Unit.MetersPerSecondSquared, "m/s^2"},
            {Unit.KnotsPerSeond, "kn/s"},
            {Unit.Second, "s"},
            {Unit.Kelvin, "°K"},
            {Unit.Celcius, "°C"},
            {Unit.Fahrenheit, "°F"},
            {Unit.Pascal, "Pa"},
            {Unit.Bar, "bar"},
            {Unit.InchesOfMercury, "inHg"},
            {Unit.SquareMeter, "m^2"},
            {Unit.CubicMeters, "m^3"},
            {Unit.Kilogram, "kg"},
            {Unit.Gram, "g"},
            {Unit.Liter, "L"},
            {Unit.GramPerMole, "g/mol"},
            {Unit.Coulombs, "C"},
            {Unit.ElementaryCharge, "e"},
            {Unit.Joule, "J"},
            {Unit.ElectronVolts, "eV"},
            {Unit.Newton, "N"},
            {Unit.Radians, "rad"}
        };
        public static readonly Dictionary<string, Unit> InverseUnitStringRepresentation =
            UnitStringRepresentation.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static string StringRepresentation(this Unit unit)
        {
            if(!UnitStringRepresentation.ContainsKey(unit))
                throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            return UnitStringRepresentation[unit];
        }

        public static string StringRepresentation(this SIBaseUnit siBaseUnit)
        {
            switch (siBaseUnit)
            {
                case SIBaseUnit.Meter: return "m";
                case SIBaseUnit.Kilogram: return "kg";
                case SIBaseUnit.Second: return "s";
                case SIBaseUnit.Ampere: return "A";
                case SIBaseUnit.Kelvin: return "K";
                case SIBaseUnit.Mole: return "mol";
                case SIBaseUnit.Candela: return "cd";
                case SIBaseUnit.Radians: return "rad";
                default:
                    throw new ArgumentOutOfRangeException(nameof(siBaseUnit), siBaseUnit, null);
            }
        }

        public static readonly Dictionary<SIPrefix, string> SIPrefixStringRepresentation = new Dictionary<SIPrefix, string>
        {
            {SIPrefix.None, string.Empty},
            {SIPrefix.Femto, "f"},
            {SIPrefix.Pico, "p"},
            {SIPrefix.Nano, "n"},
            {SIPrefix.Micro, "μ"},
            {SIPrefix.Milli, "m"},
            {SIPrefix.Centi, "c"},
            {SIPrefix.Deci, "d"},
            {SIPrefix.Deca, "D"},
            {SIPrefix.Hecto, "H"},
            {SIPrefix.Kilo, "k"},
            {SIPrefix.Mega, "M"},
            {SIPrefix.Giga, "G"},
            {SIPrefix.Tera, "T"},
            {SIPrefix.Peta, "P"},
            {SIPrefix.Exa, "E"}
        };
        public static readonly Dictionary<string, SIPrefix> InverseSIPrefixStringRepresentation = SIPrefixStringRepresentation
            .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static string StringRepresentation(this SIPrefix prefix)
        {
            if (!SIPrefixStringRepresentation.ContainsKey(prefix))
                throw new ArgumentOutOfRangeException(nameof(prefix), prefix, null);
            return SIPrefixStringRepresentation[prefix];
        }

        public static double GetMultiplier(this SIPrefix prefix)
        {
            switch (prefix)
            {
                case SIPrefix.Femto:
                    return 1e-15;
                case SIPrefix.Pico:
                    return 1e-12;
                case SIPrefix.Nano:
                    return 1e-9;
                case SIPrefix.Micro:
                    return 1e-6;
                case SIPrefix.Milli:
                    return 1e-3;
                case SIPrefix.Centi:
                    return 1e-2;
                case SIPrefix.Deci:
                    return 1e-1;
                case SIPrefix.None:
                    return 1;
                case SIPrefix.Deca:
                    return 1e1;
                case SIPrefix.Hecto:
                    return 1e2;
                case SIPrefix.Kilo:
                    return 1e3;
                case SIPrefix.Mega:
                    return 1e6;
                case SIPrefix.Giga:
                    return 1e9;
                case SIPrefix.Tera:
                    return 1e12;
                case SIPrefix.Peta:
                    return 1e15;
                case SIPrefix.Exa:
                    return 1e18;
                default:
                    throw new ArgumentOutOfRangeException(nameof(prefix), prefix, null);
            }
        }

        public static UnitValue RoundToNearest(this UnitValue value, UnitValue resolution)
        {
            return Math.Round(value.In(resolution.Unit) / resolution.Value) * resolution.Value.To(resolution.Unit);
        }
        public static UnitValue RoundDownToNearest(this UnitValue value, UnitValue resolution)
        {
            return Math.Floor(value.In(resolution.Unit) / resolution.Value) * resolution.Value.To(resolution.Unit);
        }
        public static UnitValue RoundUpToNearest(this UnitValue value, UnitValue resolution)
        {
            return Math.Ceiling(value.In(resolution.Unit) / resolution.Value) * resolution.Value.To(resolution.Unit);
        }

        public static UnitValue Sum<T>(this IEnumerable<T> items, Func<T, UnitValue> valueSelector, Unit unit)
        {
            return items.Select(valueSelector).Sum(unit);
        }
        public static UnitValue Sum(this IEnumerable<UnitValue> items, Unit unit)
        {
            return items.Select(item => item.In(unit)).Sum().To(unit);
        }

        public static UnitValue Average<T>(this IEnumerable<T> items, Func<T, UnitValue> valueSelector,
            SIPrefix siPrefix, Unit unit)
        {
            return items.Select(valueSelector).Average(siPrefix, unit);
        }
        public static UnitValue Average(this IEnumerable<UnitValue> items, SIPrefix siPrefix, Unit unit)
        {
            return items.Select(uv => uv.In(siPrefix, unit)).Average().To(siPrefix, unit);
        }
    }
}
