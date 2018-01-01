namespace Commons
{
    public static class UnitPoint3DExtensions
    {
        public static UnitPoint3D To(this Point3D point, Unit unit)
        {
            return point.To(SIPrefix.None, unit);
        }
        public static UnitPoint3D To(this Point3D point, CompoundUnit unit)
        {
            return new UnitPoint3D(unit, point.X, point.Y, point.Z);
        }
        public static UnitPoint3D To(this Point3D point, SIPrefix siPrefix, Unit unit)
        {
            return new UnitPoint3D(siPrefix, unit, point.X, point.Y, point.Z);
        }

        public static Point3D In(this UnitPoint3D unitPoint, Unit targetUnit)
        {
            return unitPoint.In(SIPrefix.None, targetUnit);
        }
        public static Point3D In(this UnitPoint3D unitPoint, CompoundUnit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetUnit);
            return new Point3D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y,
                multiplier*unitPoint.Z);
        }
        public static Point3D In(this UnitPoint3D unitPoint, SIPrefix targetSIPrefix, Unit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetSIPrefix, targetUnit);
            return new Point3D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y,
                multiplier*unitPoint.Z);
        }
        public static UnitValue DistanceTo(this UnitPoint3D unitPoint1, UnitPoint3D unitPoint2)
        {
#if CHECK_UNITS
            if (unitPoint1.Unit != unitPoint2.Unit)
                throw new InvalidOperationException($"Cannot calculate distance between points with different units, got {unitPoint1.Unit} and {unitPoint2.Unit}");
#endif
            var commonUnit = unitPoint1.Unit;
            var distance = unitPoint1.DistanceTo((Point3D) unitPoint2);

            return distance.To(commonUnit);
        }

        public static UnitVector3D VectorTo(this UnitPoint3D unitPoint1, UnitPoint3D unitPoint2)
        {
#if CHECK_UNITS
            if (unitPoint1.Unit != unitPoint2.Unit)
                throw new InvalidOperationException($"Cannot calculate vector between points with different units, got {unitPoint1.Unit} and {unitPoint2.Unit}");
#endif
            var commonUnit = unitPoint1.Unit;
            var vector = unitPoint1.VectorTo((Point3D) unitPoint2);

            return vector.To(commonUnit);
        }
    }
}