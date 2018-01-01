using Commons.Mathematics;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class UnitVector3DExtensions
    {
        public static UnitVector3D To(this Vector3D point, Unit unit)
        {
            return point.To(SIPrefix.None, unit);
        }
        public static UnitVector3D To(this Vector3D point, CompoundUnit unit)
        {
            return new UnitVector3D(unit, point.X, point.Y, point.Z);
        }
        public static UnitVector3D To(this Vector3D point, SIPrefix siPrefix, Unit unit)
        {
            return new UnitVector3D(siPrefix, unit, point.X, point.Y, point.Z);
        }

        public static Vector3D In(this UnitVector3D unitPoint, Unit targetUnit)
        {
            return unitPoint.In(SIPrefix.None, targetUnit);
        }
        public static Vector3D In(this UnitVector3D unitPoint, CompoundUnit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetUnit);
            return new Vector3D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y,
                multiplier*unitPoint.Z);
        }
        public static Vector3D In(this UnitVector3D unitPoint, SIPrefix targetSIPrefix, Unit targetUnit)
        {
            var multiplier = new UnitValue(unitPoint.Unit, 1).In(targetSIPrefix, targetUnit);
            return new Vector3D(
                multiplier*unitPoint.X,
                multiplier*unitPoint.Y,
                multiplier*unitPoint.Z);
        }
        public static UnitValue Magnitude(this UnitVector3D a)
        {
            return ((Vector3D)a).Magnitude().To(a.Unit);
        }
    }
}