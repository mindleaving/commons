namespace Commons.Physics
{
    public static class UnitConverter
    {
        public static bool CanConvertTo(this UnitValue unitValue, IUnitDefinition unit)
        {
            return unitValue.Unit == unit.CorrespondingCompoundUnit;
        }

        public static bool CanConvertTo(this UnitValue unitValue, CompoundUnit unit)
        {
            return unitValue.Unit == unit;
        }

        public static double ConvertUnit(this double value, IUnitDefinition fromUnit, IUnitDefinition toUnit)
        {
            return toUnit.ConvertBack(fromUnit.ConvertToUnitValue(value));
        }
    }
}
