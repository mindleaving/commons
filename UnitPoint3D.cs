using System;

namespace Commons
{
    public class UnitPoint3D : Point3D
    {
        public CompoundUnit Unit { get; }

        public UnitPoint3D(UnitValue x, UnitValue y, UnitValue z)
            : base(x.In(x.Unit), y.In(x.Unit), z.In(x.Unit))
        {
            Unit = x.Unit;
        }
        public UnitPoint3D(SIPrefix prefix, Unit unit, double x, double y, double z)
            : base(prefix.GetMultiplier() *  x, prefix.GetMultiplier() * y, prefix.GetMultiplier() * z)
        {
            Unit = unit.ToCompoundUnit();
        }

        public UnitPoint3D(CompoundUnit unit, double x, double y, double z)
            : base(x, y, z)
        {
            Unit = unit;
        }
        public UnitPoint3D(Unit unit, double x, double y, double z)
            : this(SIPrefix.None, unit, x, y, z)
        {
        }

        public static UnitPoint3D operator +(UnitPoint3D point1, UnitPoint3D point2)
        {
#if CHECK_UNITS
            if (point1.Unit != point2.Unit)
                throw new InvalidOperationException($"Cannot add points with different units, got {point1.Unit} and {point2.Unit}");
#endif
            var x = point1.X + point2.X;
            var y = point1.Y + point2.Y;
            var z = point1.Z + point2.Z;
            return new UnitPoint3D(point1.Unit, x, y, z);
        }
        public static UnitPoint3D operator -(UnitPoint3D point1, UnitPoint3D point2)
        {
#if CHECK_UNITS
            if (point1.Unit != point2.Unit)
                throw new InvalidOperationException($"Cannot subtract points with different units, got {point1.Unit} and {point2.Unit}");
#endif
            var x = point1.X - point2.X;
            var y = point1.Y - point2.Y;
            var z = point1.Z - point2.Z;
            return new UnitPoint3D(point1.Unit, x, y, z);
        }
        public static UnitPoint3D operator +(UnitPoint3D point1, UnitVector3D v)
        {
#if CHECK_UNITS
            if (point1.Unit != v.Unit)
                throw new InvalidOperationException($"Cannot add point and vector with different units, got {point1.Unit} and {v.Unit}");
#endif
            var x = point1.X + v.X;
            var y = point1.Y + v.Y;
            var z = point1.Z + v.Z;
            return new UnitPoint3D(point1.Unit, x, y, z);
        }
        public static UnitPoint3D operator -(UnitPoint3D point1, UnitVector3D v)
        {
#if CHECK_UNITS
            if (point1.Unit != v.Unit)
                throw new InvalidOperationException($"Cannot subtract point and vector with different units, got {point1.Unit} and {v.Unit}");
#endif
            var x = point1.X - v.X;
            var y = point1.Y - v.Y;
            var z = point1.Z - v.Z;
            return new UnitPoint3D(point1.Unit, x, y, z);
        }
        public static UnitPoint3D operator *(double scalar, UnitPoint3D point)
        {
            var x = scalar * point.X;
            var y = scalar * point.Y;
            var z = scalar * point.Z;
            return new UnitPoint3D(point.Unit, x, y, z);
        }
        public static UnitPoint3D operator *(UnitValue scalar, UnitPoint3D point)
        {
            var x = scalar.Value * point.X;
            var y = scalar.Value * point.Y;
            var z = scalar.Value * point.Z;
            return new UnitPoint3D(scalar.Unit * point.Unit, x, y, z);
        }

        public override string ToString()
        {
            return $"{X};{Y};{Z}";
        }

        public UnitPoint3D DeepClone()
        {
            return new UnitPoint3D(Unit.Clone(), X, Y, Z);
        }
    }
}