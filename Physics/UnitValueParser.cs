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
                throw new FormatException();
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
            if (string.IsNullOrEmpty(unitString))
            {
                return new UnitValue(new CompoundUnit(new SIBaseUnit[0]), value);
            }
            try
            {
                ParseSimpleUnit(unitString, out var simpleUnit, out var siPrefix);
                var unitConversionResult = value.ConvertToSI(simpleUnit);
                return unitConversionResult.Value.To(siPrefix, unitConversionResult.Unit);
            }
            catch (FormatException)
            {
                var unit = CompoundUnitParser.Parse(unitString);
                return new UnitValue(unit, value);
            }
        }

        private static void ParseSimpleUnit(string unitString, out Unit unit, out SIPrefix siPrefix)
        {
            if(string.IsNullOrEmpty(unitString))
                throw new FormatException();
            if (UnitValueExtensions.InverseUnitStringRepresentation.ContainsKey(unitString))
            {
                siPrefix = SIPrefix.None;
                unit = UnitValueExtensions.InverseUnitStringRepresentation[unitString];
                return;
            }
            // Try with SI multiplier prefix
            var prefixString = unitString.Substring(0, 1);
            var newUnitString = unitString.Substring(1);
            if(!UnitValueExtensions.InverseUnitStringRepresentation.ContainsKey(newUnitString))
                throw new FormatException();
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
            if (prefixString == "u")
            {
                siPrefix = SIPrefix.Micro;
                return;
            }
            throw new FormatException();
        }
    }
}
