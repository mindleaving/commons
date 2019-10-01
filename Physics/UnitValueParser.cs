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
            var match = Regex.Match(preprocessedString, @"(-?[0-9]+([\.,][0-9]+)?([eE][-+0-9]+)?|NaN|-?∞|-?Inf |-?Infinity |-?inf |-?infinity )\s*(([fpnμumkMGTZE]|1/)?.*)");
            if(!match.Success)
                throw new FormatException($"Invalid unit value '{unitValueString}'");

            // Value
            var valueGroup = match.Groups[1];
            var value = ParseValue(valueGroup.Value);

            // Unit
            var unitGroup = match.Groups[4];
            var unitString = unitGroup.Value;

            return CompoundUnitParser.Parse(value, unitString);
        }

        private static double ParseValue(string valueGroupValue)
        {
            var normalizedValueGroup = valueGroupValue.ToLowerInvariant().Trim().Replace(',','.');
            double value;
            if (normalizedValueGroup.InSet("inf", "infinity", "∞"))
            {
                value = double.PositiveInfinity;
            }
            else if (normalizedValueGroup.InSet("-inf", "-infinity", "-∞"))
            {
                value = double.NegativeInfinity;
            }
            else if (normalizedValueGroup == "nan")
            {
                value = double.NaN;
            }
            else
            {
                value = double.Parse(normalizedValueGroup, NumberStyles.Any, CultureInfo.InvariantCulture);
            }

            return value;
        }
    }
}
