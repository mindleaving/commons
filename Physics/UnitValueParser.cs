using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Commons.Extensions;

namespace Commons.Physics
{
    public static class UnitValueParser
    {
        public static UnitValue Parse(string unitValueString)
        {
            if (string.IsNullOrEmpty(unitValueString))
                return null;
            var preprocessedString = unitValueString.Trim();
            var match = Regex.Match(preprocessedString, @"(-?[0-9]+(\.[0-9]+)?([eE][-+0-9]+)?|NaN|-?∞|-?Inf |-?Infinity |-?inf |-?infinity )\s*(([fpnμumkMGTZE]|1/)?.*)");
            if(!match.Success)
                throw new FormatException($"Invalid unit value '{unitValueString}'");
            var valueGroup = match.Groups[1];
            var unitGroup = match.Groups[4];
            double value;
            var valueGroupValue = valueGroup.Value.ToLowerInvariant().Trim();
            if (valueGroupValue.InSet("inf", "infinity", "∞"))
            {
                value = double.PositiveInfinity;
            }
            else if (valueGroupValue.InSet("-inf", "-infinity", "-∞"))
            {
                value = double.NegativeInfinity;
            }
            else
            {
                value = double.Parse(valueGroup.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            var unitString = unitGroup.Value;
            var unitConversionResult = CompoundUnitParser.Parse(unitString);
            return (value * unitConversionResult.Value).To(unitConversionResult.Unit);
        }

        internal static void ParseSimpleUnit(string unitString, out Unit unit, out SIPrefix siPrefix)
        {
            if(string.IsNullOrEmpty(unitString))
                throw new FormatException($"Invalid unit '{unitString}'");
            if (UnitValueExtensions.InverseUnitStringRepresentation.ContainsKey(unitString))
            {
                siPrefix = SIPrefix.None;
                unit = UnitValueExtensions.InverseUnitStringRepresentation[unitString];
                return;
            }
            // Try with SI multiplier prefix
            var prefixString = unitString.Substring(0, 1);
            var newUnitString = unitString.Substring(1);
            if(string.IsNullOrWhiteSpace(newUnitString) || !UnitValueExtensions.InverseUnitStringRepresentation.ContainsKey(newUnitString))
                throw new FormatException($"Invalid unit '{unitString}'");
            unit = UnitValueExtensions.InverseUnitStringRepresentation[newUnitString];
            if (UnitValueExtensions.InverseSIPrefixStringRepresentation.ContainsKey(prefixString))
            {
                siPrefix = UnitValueExtensions.InverseSIPrefixStringRepresentation[prefixString];
                return;
            }
            if (prefixString.ToLowerInvariant() == "k")
            {
                siPrefix = SIPrefix.Kilo;
                return;
            }
            if (prefixString.InSet("u", "\u03bc", "\u00b5"))
            {
                siPrefix = SIPrefix.Micro;
                return;
            }
            throw new FormatException($"Invalid unit '{unitString}'");
        }
    }
}
