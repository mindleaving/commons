namespace Commons
{
    public static class UnitVector2DExtensions
    {
        public static UnitVector2D To(this Vector2D point, Unit unit)
        {
            return point.To(SIPrefix.None, unit);
        }
        public static UnitVector2D To(this Vector2D point, CompoundUnit unit)
        {
            return new UnitVector2D(unit, point.X, point.Y);
        }
        public static UnitVector2D To(this Vector2D point, SIPrefix siPrefix, Unit unit)
        {
            return new UnitVector2D(siPrefix, unit, point.X, point.Y);
        }

        public static Vector2D In(this UnitVector2D unitPoint, Unit targetUnit)
        {
            return unitPoint.In(SIPrefix.None, targetUnit);
        }
        public static Vector2D In(this UnitVector2D unitPoint, CompoundUnit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetUnit);
            return new Vector2D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y);
        }
        public static Vector2D In(this UnitVector2D unitPoint, SIPrefix targetSIPrefix, Unit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetSIPrefix, targetUnit);
            return new Vector2D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y);
        }
        public static UnitValue Magnitude(this UnitVector2D a)
        {
            return ((Vector2D)a).Magnitude().To(a.Unit);
        }
    }
}