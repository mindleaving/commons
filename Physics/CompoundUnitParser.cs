using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Commons.Extensions;

namespace Commons.Physics
{
    public static class CompoundUnitParser
    {
        public static CompoundUnit Parse(string unitString)
        {
            var unitMatch = Regex.Match(unitString, "([^/]+)(/([^/]+))?");
            if (!unitMatch.Success)
                throw new FormatException();
            var nominatorString = unitMatch.Groups[1].Value;
            var hasDenominator = unitMatch.Groups[2].Success;
            var denominatorString = hasDenominator ? unitMatch.Groups[3].Value : "";

            nominatorString = Regex.Replace(nominatorString, "\\s+", " ").Trim();
            denominatorString = Regex.Replace(denominatorString, "\\s+", " ").Trim();
            denominatorString = denominatorString.Replace("(", "").Replace(")", "");
            var splittedNominator = nominatorString.Split();
            var splittedDenominator = denominatorString.Split();

            var nomniator = splittedNominator.Length == 1 && splittedNominator[0] == "1" // For 1/UNITS
                ? new SIBaseUnit[0]
                : splittedNominator.Where(str => !string.IsNullOrEmpty(str)).SelectMany(ParseSIBaseUnit);
            var denomniator = splittedDenominator
                .Where(str => !string.IsNullOrEmpty(str))
                .SelectMany(ParseSIBaseUnit);
            var unit = new CompoundUnit(nomniator, denomniator);
            return unit;
        }

        private static IEnumerable<SIBaseUnit> ParseSIBaseUnit(string unitString)
        {
            var match = Regex.Match(unitString, "([a-zA-Z]+)(\\^([0-9]+))?");
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
    }
}
