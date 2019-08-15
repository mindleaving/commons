using Commons.Extensions;

namespace Commons.Physics
{
    public static class UnitConverter
    {
        public static bool CanConvertTo(string unitValueString, string toUnitString)
        {
            var unitValue = UnitValue.Parse(unitValueString);
            return unitValue.CanConvertTo(toUnitString);
        }

        public static bool CanConvertTo(this UnitValue unitValue, string toUnitString)
        {
            var toUnit = Unit.Effective.InverseStringRepresentationLookup[toUnitString];
            return unitValue.CanConvertTo(toUnit);
        }

        public static bool CanConvertTo(this UnitValue unitValue, IUnitDefinition unit)
        {
            return unitValue.Unit == unit.CorrespondingCompoundUnit;
        }

        public static bool CanConvertTo(this UnitValue unitValue, CompoundUnit unit)
        {
            return unitValue.Unit == unit;
        }

        public static double ConvertUnit(string unitValueString, string toUnitString)
        {
            var unitValue = UnitValue.Parse(unitValueString);
            var toUnit = Unit.Effective.InverseStringRepresentationLookup[toUnitString];
            return unitValue.In(toUnit);
        }

        public static double ConvertUnit(this double value, IUnitDefinition fromUnit, IUnitDefinition toUnit)
        {
            return toUnit.ConvertBack(fromUnit.ConvertToUnitValue(value));
        }

        public static double ConvertUnit(this double value, string fromUnitString, string toUnitString)
        {
            var fromUnit = Unit.Effective.InverseStringRepresentationLookup[fromUnitString];
            var toUnit = Unit.Effective.InverseStringRepresentationLookup[toUnitString];
            return ConvertUnit(value, fromUnit, toUnit);
        }
    }
}
