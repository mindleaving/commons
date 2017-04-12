using System;

namespace Commons
{
    public class UnitVector3D : Vector3D
    {
        public CompoundUnit Unit { get; }

        public UnitVector3D(UnitValue x, UnitValue y, UnitValue z)
            : base(x.In(x.Unit), y.In(x.Unit), z.In(x.Unit))
        {
            Unit = x.Unit;
        }
        public UnitVector3D(SIPrefix prefix, Unit unit, double x, double y, double z)
            : base(prefix.GetMultiplier() * x, prefix.GetMultiplier() * y, prefix.GetMultiplier() * z)
        {
            Unit = unit.ToCompoundUnit();
        }

        public UnitVector3D(CompoundUnit unit, double x, double y, double z)
            : base(x, y, z)
        {
            Unit = unit;
        }

        public UnitVector3D(Unit unit, double x, double y, double z)
            : this(SIPrefix.None, unit, x, y, z)
        {
        }

        public static UnitVector3D operator +(UnitVector3D v1, UnitVector3D v2)
        {
#if CHECK_UNITS
            if (v1.Unit != v2.Unit)
                throw new InvalidOperationException($"Cannot add vectors with different units, got {v1.Unit} and {v2.Unit}");
#endif
            var x = v1.X + v2.X;
            var y = v1.Y + v2.Y;
            var z = v1.Z + v2.Z;
            return new UnitVector3D(v1.Unit, x, y, z);
        }
        public static UnitVector3D operator -(UnitVector3D v1, UnitVector3D v2)
        {
#if CHECK_UNITS
            if (v1.Unit != v2.Unit)
                throw new InvalidOperationException($"Cannot subtract vectors with different units, got {v1.Unit} and {v2.Unit}");
#endif
            var x = v1.X - v2.X;
            var y = v1.Y - v2.Y;
            var z = v1.Z - v2.Z;
            return new UnitVector3D(v1.Unit, x, y, z);
        }
        public static UnitVector3D operator -(UnitVector3D v)
        {
            return -1.0 * v;
        }
        public static UnitVector3D operator *(UnitValue scalar, UnitVector3D v)
        {
            var x = scalar.Value * v.X;
            var y = scalar.Value * v.Y;
            var z = scalar.Value * v.Z;
            return new UnitVector3D(v.Unit * scalar.Unit, x, y, z);
        }
        public static UnitVector3D operator *(int scalar, UnitVector3D v)
        {
            return (double)scalar * v;
        }
        public static UnitVector3D operator *(UnitVector3D v, double scalar)
        {
            return scalar * v;
        }
        public static UnitVector3D operator *(UnitVector3D v, UnitValue scalar)
        {
            return scalar * v;
        }
        public static UnitVector3D operator *(UnitVector3D v, int scalar)
        {
            return scalar * v;
        }
        public static UnitVector3D operator *(double scalar, UnitVector3D v)
        {
            var x = scalar * v.X;
            var y = scalar * v.Y;
            var z = scalar * v.Z;
            return new UnitVector3D(v.Unit, x, y, z);
        }
        public static UnitVector3D operator /(UnitVector3D v, UnitValue scalar)
        {
            var x = v.X / scalar.Value;
            var y = v.Y / scalar.Value;
            var z = v.Z / scalar.Value;
            return new UnitVector3D(v.Unit / scalar.Unit, x, y, z);
        }

        public override string ToString()
        {
            return $"{X};{Y};{Z}";
        }
    }
}