using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Commons.Extensions;

namespace Commons.Physics
{
    public static class CompoundUnitParser
    {
        public static UnitConversionResult Parse(string unitString)
        {
            if (string.IsNullOrEmpty(unitString))
            {
                return new UnitConversionResult(new CompoundUnit(), 1);
            }
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

            var nomniatorUnitConversions = splittedNominator.Length == 1 && splittedNominator[0] == "1" // For 1/UNITS
                ? new List<UnitConversionResult>()
                : splittedNominator.Where(str => !string.IsNullOrEmpty(str)).Select(ParseUnitComponent).ToList();
            var denomniatorUnitConversions = splittedDenominator
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(ParseUnitComponent).ToList();
            return CombineUnitConversions(nomniatorUnitConversions, denomniatorUnitConversions);
        }

        private static UnitConversionResult CombineUnitConversions(
            List<UnitConversionResult> nomniatorUnitConversions, 
            List<UnitConversionResult> denomniatorUnitConversions)
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
            return new UnitConversionResult(combinedUnit, combinedMultiplier);
        }

        private static UnitConversionResult ParseUnitComponent(string str)
        {
            var match = Regex.Match(str, "([μa-zA-Z°]+)(\\^([0-9]+))?");
            if(!match.Success)
                throw new FormatException($"Could not parse unit '{str}'");
            var unitName = match.Groups[1].Value;
            UnitValueParser.ParseSimpleUnit(unitName, out var simpleUnit, out var siPrefix);
            var baseConversion = 1d.ConvertToSI(simpleUnit);

            var hasExponent = match.Groups[2].Success;
            var exponent = hasExponent ? int.Parse(match.Groups[3].Value) : 1;
            var compoundUnit = hasExponent ? baseConversion.Unit.Pow(exponent) : baseConversion.Unit;
            var value = (baseConversion.Value*siPrefix.GetMultiplier()).IntegerPower(exponent);
            return new UnitConversionResult(compoundUnit, value);
        }
    }
}
