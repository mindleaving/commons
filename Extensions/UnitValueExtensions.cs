using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class UnitValueExtensions
    {
        static UnitValueExtensions()
        {
            InverseSIPrefixStringRepresentation = SIPrefixStringRepresentation
                .ToDictionary(kvp => kvp.Value, kvp => kvp.Key);
            InverseSIPrefixStringRepresentation.Add("u", SIPrefix.Micro);
            InverseSIPrefixStringRepresentation.Add("\u03bc", SIPrefix.Micro);
        }

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

        public static double In(this UnitValue unitValue, IUnitDefinition newUnit)
        {
            return newUnit.ConvertBack(unitValue);
        }
        public static double In(this UnitValue unitValue, SIPrefix prefix, IUnitDefinition unit)
        {
            var multiplier = GetMultiplier(prefix);
            return unitValue.In(unit) / multiplier;
        }

        public static UnitValue To(this double value, IUnitDefinition unit)
        {
            return unit.ConvertToUnitValue(value);
        }

        public static UnitValue To(this float value, IUnitDefinition unit)
        {
            return To((double)value, unit);
        }

        public static UnitValue To(this int value, IUnitDefinition unit)
        {
            return To((double)value, unit);
        }

        public static UnitValue To(this double value, CompoundUnit unit)
        {
            return new UnitValue(unit, value);
        }

        public static UnitValue To(this double value, SIPrefix prefix, IUnitDefinition unit)
        {
            var multiplier = GetMultiplier(prefix);
            return unit.ConvertToUnitValue(multiplier * value);
        }

        public static UnitValue To(this double value, SIPrefix prefix, CompoundUnit unit)
        {
            var multiplier = GetMultiplier(prefix);
            return new UnitValue(unit, multiplier * value);
        }

        public static UnitValue To(this float value, SIPrefix prefix, IUnitDefinition unit)
        {
            return To((double)value, prefix, unit);
        }

        public static UnitValue To(this int value, SIPrefix prefix, IUnitDefinition unit)
        {
            return To((double)value, prefix, unit);
        }

        public static readonly Dictionary<SIBaseUnit, string> SIBaseUnitStringRepresentation = new Dictionary<SIBaseUnit, string>
        {
            {SIBaseUnit.Meter, "m"},
            {SIBaseUnit.Kilogram, "kg"},
            {SIBaseUnit.Second, "s"},
            {SIBaseUnit.Ampere, "A"},
            {SIBaseUnit.Kelvin, "°K"},
            {SIBaseUnit.Mole, "mol"},
            {SIBaseUnit.Candela, "cd"},
            {SIBaseUnit.Radians, "rad"}
        };

        public static readonly Dictionary<string, SIBaseUnit> InverseSIBaseUnitStringRepresentation =
            SIBaseUnitStringRepresentation.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

        public static string StringRepresentation(this SIBaseUnit unit)
        {
            if(!SIBaseUnitStringRepresentation.ContainsKey(unit))
                throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
            return SIBaseUnitStringRepresentation[unit];
        }

        public static readonly Dictionary<SIPrefix, string> SIPrefixStringRepresentation = new Dictionary<SIPrefix, string>
        {
            {SIPrefix.None, string.Empty},
            {SIPrefix.Femto, "f"},
            {SIPrefix.Pico, "p"},
            {SIPrefix.Nano, "n"},
            {SIPrefix.Micro, "µ"},
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
        public static Dictionary<string, SIPrefix> InverseSIPrefixStringRepresentation { get; }

        public static string StringRepresentation(this SIPrefix prefix)
        {
            if (!SIPrefixStringRepresentation.ContainsKey(prefix))
                throw new ArgumentOutOfRangeException(nameof(prefix), prefix, null);
            return SIPrefixStringRepresentation[prefix];
        }

        public static SIPrefix SelectSIPrefix(this double value)
        {
            if (value.IsNaN() || value == 0 || value.IsInfinity())
                return SIPrefix.None;
            var absValue = Math.Abs(value);
            var allPrefixes = ((SIPrefix[]) Enum.GetValues(typeof(SIPrefix)))
                .Except(new []{SIPrefix.Deca, SIPrefix.Deci, SIPrefix.Hecto, SIPrefix.Centi })
                .ToDictionary(x => x, GetMultiplier);
            var multipliersSmallerThanOrEqualToValue = allPrefixes.Where(kvp => kvp.Value <= absValue).ToList();
            if (!multipliersSmallerThanOrEqualToValue.Any())
                return allPrefixes.MinimumItem(kvp => kvp.Value).Key;
            return multipliersSmallerThanOrEqualToValue.MaximumItem(kvp => kvp.Value).Key;
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

        public static UnitValue Sum<T>(this IEnumerable<T> items, Func<T, UnitValue> valueSelector, IUnitDefinition unit)
        {
            return items.Select(valueSelector).Sum(unit);
        }
        public static UnitValue Sum(this IEnumerable<UnitValue> items, IUnitDefinition unit)
        {
            return items.Select(item => item.In(unit)).Sum().To(unit);
        }

        public static UnitValue Average<T>(this IEnumerable<T> items, Func<T, UnitValue> valueSelector,
            SIPrefix siPrefix, IUnitDefinition unit)
        {
            return items.Select(valueSelector).Average(siPrefix, unit);
        }
        public static UnitValue Average(this IEnumerable<UnitValue> items, IUnitDefinition unit)
        {
            return items.Average(SIPrefix.None, unit);
        }
        public static UnitValue Average(this IEnumerable<UnitValue> items, SIPrefix siPrefix, IUnitDefinition unit)
        {
            return items.Select(uv => uv.In(siPrefix, unit)).Average().To(siPrefix, unit);
        }
    }
}
