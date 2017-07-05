using System;

namespace Commons
{
    public static class UnitPoint2DExtensions
    {
        public static UnitPoint2D To(this Point2D point, Unit unit)
        {
            return point.To(SIPrefix.None, unit);
        }
        public static UnitPoint2D To(this Point2D point, CompoundUnit unit)
        {
            return new UnitPoint2D(unit, point.X, point.Y);
        }
        public static UnitPoint2D To(this Point2D point, SIPrefix siPrefix, Unit unit)
        {
            return new UnitPoint2D(siPrefix, unit, point.X, point.Y);
        }

        public static Point2D In(this UnitPoint2D unitPoint, Unit targetUnit)
        {
            return unitPoint.In(SIPrefix.None, targetUnit);
        }
        public static Point2D In(this UnitPoint2D unitPoint, CompoundUnit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetUnit);
            return new Point2D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y);
        }
        public static Point2D In(this UnitPoint2D unitPoint, SIPrefix targetSIPrefix, Unit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetSIPrefix, targetUnit);
            return new Point2D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y);
        }
        public static UnitValue DistanceTo(this UnitPoint2D unitPoint1, UnitPoint2D unitPoint2)
        {
#if CHECK_UNITS
            if (unitPoint1.Unit != unitPoint2.Unit)
                throw new InvalidOperationException($"Cannot calculate distance between points with different units, got {unitPoint1.Unit} and {unitPoint2.Unit}");
#endif
            var commonUnit = unitPoint1.Unit;
            var distance = unitPoint1.DistanceTo((Point2D) unitPoint2);

            return distance.To(commonUnit);
        }

        public static UnitVector2D VectorTo(this UnitPoint2D unitPoint1, UnitPoint2D unitPoint2)
        {
#if CHECK_UNITS
            if (unitPoint1.Unit != unitPoint2.Unit)
                throw new InvalidOperationException($"Cannot calculate vector between points with different units, got {unitPoint1.Unit} and {unitPoint2.Unit}");
#endif
            var commonUnit = unitPoint1.Unit;
            var vector = unitPoint1.VectorTo((Point2D) unitPoint2);

            return vector.To(commonUnit);
        }
    }
}