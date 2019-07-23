using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Commons.Extensions;

namespace Commons.Physics
{
    public static class CompoundUnitParser
    {
        public static UnitValue Parse(double value, string unitString)
        {
            if (string.IsNullOrEmpty(unitString))
            {
                return new UnitValue(new CompoundUnit(), value);
            }
            var unitMatch = Regex.Match(unitString, "^([^/]+)(/([^/]+))?$");
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

            var nomniatorUnitConversions = splittedNominator.Length == 1 && splittedNominator[0] == "1" // For 1/UNITS
                ? new List<UnitValue>()
                : splittedNominator.Where(str => !string.IsNullOrEmpty(str)).Select(ParseUnitComponent).ToList();
            var denomniatorUnitConversions = splittedDenominator
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(ParseUnitComponent).ToList();
            return value * CombineUnitConversions(nomniatorUnitConversions, denomniatorUnitConversions);
        }

        private static UnitValue CombineUnitConversions(
            List<UnitValue> nomniatorUnitConversions, 
            List<UnitValue> denomniatorUnitConversions)
        {
            var combinedMultiplier10Base = 0d;
            var combinedUnit = new CompoundUnit();
            foreach (var nomniatorUnitConversion in nomniatorUnitConversions)
            {
                combinedUnit *= nomniatorUnitConversion.Unit;
                combinedMultiplier10Base += Math.Log10(nomniatorUnitConversion.Value);
            }

            foreach (var denomniatorUnitConversion in denomniatorUnitConversions)
            {
                combinedUnit /= denomniatorUnitConversion.Unit;
                combinedMultiplier10Base -= Math.Log10(denomniatorUnitConversion.Value);
            }
            var combinedMultiplier = Math.Pow(10, combinedMultiplier10Base);
            return new UnitValue(combinedUnit, combinedMultiplier);
        }

        private static UnitValue ParseUnitComponent(string str)
        {
            var match = Regex.Match(str, "^([\\u03bc\\u00B5a-zA-Z°]+)(\\^([0-9]+))?$");
            if(!match.Success)
                throw new FormatException($"Could not parse unit '{str}'");
            var unitName = match.Groups[1].Value;
            ParseSimpleUnit(unitName, out var simpleUnit, out var siPrefix);
            var baseConversion = simpleUnit.ConvertToUnitValue(1d);

            var hasExponent = match.Groups[2].Success;
            var exponent = hasExponent ? int.Parse(match.Groups[3].Value) : 1;
            var compoundUnit = hasExponent ? baseConversion.Unit.Pow(exponent) : baseConversion.Unit;
            var value = (baseConversion.Value*siPrefix.GetMultiplier()).IntegerPower(exponent);
            return new UnitValue(compoundUnit, value);
        }

        private static void ParseSimpleUnit(string unitString, out IUnitDefinition unit, out SIPrefix siPrefix)
        {
            if(string.IsNullOrEmpty(unitString))
                throw new FormatException($"Invalid unit '{unitString}'");

            var normalizedUnitString = unitString.Trim();
            if (Unit.Effective.InverseStringRepresentationLookup.ContainsKey(unitString))
            {
                siPrefix = SIPrefix.None;
                unit = Unit.Effective.InverseStringRepresentationLookup[unitString];
                return;
            }
            if(normalizedUnitString.Length == 1)
                throw new FormatException($"Invalid unit '{unitString}'");

            // Try with SI multiplier prefix
            var prefixString = unitString.Substring(0, 1);
            var newUnitString = unitString.Substring(1);
            if(!Unit.Effective.InverseStringRepresentationLookup.ContainsKey(newUnitString))
                throw new FormatException($"Invalid unit '{unitString}'");
            unit = Unit.Effective.InverseStringRepresentationLookup[newUnitString];
            if (UnitValueExtensions.InverseSIPrefixStringRepresentation.ContainsKey(prefixString))
            {
                siPrefix = UnitValueExtensions.InverseSIPrefixStringRepresentation[prefixString];
                return;
            }
            throw new FormatException($"Invalid unit '{unitString}'");
        }
    }
}
