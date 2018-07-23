using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            var match = Regex.Match(preprocessedString, @"(-?[0-9]+(\.[0-9]+)?)\s*(([a-zA-Z]|1/).*)");
            if(!match.Success)
                throw new FormatException();
            var valueGroup = match.Groups[1];
            var unitGroup = match.Groups[3];
            var value = double.Parse(valueGroup.Value, NumberStyles.Any, CultureInfo.InvariantCulture);
            ParseUnit(unitGroup.Value, out var unit, out var siPrefix);
            var multiplier = siPrefix.GetMultiplier();
            return new UnitValue(unit, multiplier * value);
        }

        public static void ParseUnit(string unitString, out CompoundUnit unit, out SIPrefix siPrefix)
        {
            try
            {
                ParseSimpleUnit(unitString, out var simpleUnit, out siPrefix);
                unit = simpleUnit.ToCompoundUnit();
            }
            catch (FormatException)
            {
                var match = Regex.Match(unitString, "([^/]+)(/([^/]+))?");
                if(!match.Success)
                    throw new FormatException();
                var nominatorString = match.Groups[1].Value;
                var hasDenominator = match.Groups[2].Success;
                var denominatorString = hasDenominator ? match.Groups[3].Value : "";

                nominatorString = Regex.Replace(nominatorString, "\\s+", " ").Trim();
                denominatorString = Regex.Replace(denominatorString, "\\s+", " ").Trim();
                denominatorString = denominatorString.Replace("(", "").Replace(")","");
                var splittedNominator = nominatorString.Split();
                var splittedDenominator = denominatorString.Split();

                var nomniator = splittedNominator.Length == 1 && splittedNominator[0] == "1" // For 1/UNITS
                                ? new SIBaseUnit[0]
                                : splittedNominator.Where(str => !string.IsNullOrEmpty(str)).SelectMany(ParseSIBaseUnit);
                var denomniator = splittedDenominator.Where(str => !string.IsNullOrEmpty(str)).SelectMany(ParseSIBaseUnit);
                unit = new CompoundUnit(nomniator, denomniator);
                siPrefix = SIPrefix.None;
            }
        }

        private static IEnumerable<SIBaseUnit> ParseSIBaseUnit(string unitString)
        {
            var match = Regex.Match(unitString, "([a-zA-Z]+)(^([0-9]+))?");
            if(!match.Success)
                throw new FormatException($"Could not parse SI base unit '{unitString}'");
            var unitName = match.Groups[1].Value;
            if(!UnitValueExtensions.InverseSIBaseUnitStringRepresentation.ContainsKey(unitName))
                throw new FormatException($"Unknown SI base unit '{unitName}'");
            var siBaseUnit = UnitValueExtensions.InverseSIBaseUnitStringRepresentation[unitName];
            var hasExponent = match.Groups[2].Success;
            var exponent = hasExponent ? int.Parse(match.Groups[3].Value) : 1;
            return Enumerable.Repeat(siBaseUnit, exponent);
        }

        private static void ParseSimpleUnit(string unitString, out Unit unit, out SIPrefix siPrefix)
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
