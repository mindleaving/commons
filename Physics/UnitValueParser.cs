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
            if(unitValueString == null)
                throw new ArgumentNullException();
            var preprocessedString = unitValueString.Trim();
            var match = Regex.Match(preprocessedString, @"(-?[0-9]+(\.[0-9]+)?)\s*([^\s]+)");
            if(!match.Success)
                throw new FormatException();
            var valueGroup = match.Groups[1];
            var unitGroup = match.Groups[3];
            var value = double.Parse(valueGroup.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            ParseUnit(unitGroup.Value, out var unit, out var siPrefix);
            var multiplier = siPrefix.GetMultiplier();
            return new UnitValue(unit, multiplier * value);
        }

        public static void ParseUnit(string unitString, out Unit unit, out SIPrefix siPrefix)
        {
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
