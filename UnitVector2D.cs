using System;

namespace Commons
{
    public class UnitVector2D : Vector2D
    {
        public CompoundUnit Unit { get; }

        public UnitVector2D(UnitValue x, UnitValue y)
            : base(x.In(x.Unit), y.In(x.Unit))
        {
            Unit = x.Unit;
        }
        public UnitVector2D(SIPrefix prefix, Unit unit, double x, double y)
            : base(prefix.GetMultiplier() * x, prefix.GetMultiplier() * y)
        {
            Unit = unit.ToCompoundUnit();
        }

        public UnitVector2D(CompoundUnit unit, double x, double y)
            : base(x, y)
        {
            Unit = unit;
        }

        public UnitVector2D(Unit unit, double x, double y)
            : this(SIPrefix.None, unit, x, y)
        {
        }

        public static UnitVector2D operator +(UnitVector2D v1, UnitVector2D v2)
        {
#if CHECK_UNITS
            if (v1.Unit != v2.Unit)
                throw new InvalidOperationException($"Cannot add vectors with different units, got {v1.Unit} and {v2.Unit}");
#endif
            var x = v1.X + v2.X;
            var y = v1.Y + v2.Y;
            return new UnitVector2D(v1.Unit, x, y);
        }
        public static UnitVector2D operator -(UnitVector2D v1, UnitVector2D v2)
        {
#if CHECK_UNITS
            if (v1.Unit != v2.Unit)
                throw new InvalidOperationException($"Cannot subtract vectors with different units, got {v1.Unit} and {v2.Unit}");
#endif
            var x = v1.X - v2.X;
            var y = v1.Y - v2.Y;
            return new UnitVector2D(v1.Unit, x, y);
        }
        public static UnitVector2D operator -(UnitVector2D v)
        {
            return -1.0 * v;
        }
        public static UnitVector2D operator *(UnitValue scalar, UnitVector2D v)
        {
            var x = scalar.Value * v.X;
            var y = scalar.Value * v.Y;
            return new UnitVector2D(v.Unit * scalar.Unit, x, y);
        }
        public static UnitVector2D operator *(int scalar, UnitVector2D v)
        {
            return (double)scalar * v;
        }
        public static UnitVector2D operator *(UnitVector2D v, double scalar)
        {
            return scalar * v;
        }
        public static UnitVector2D operator *(UnitVector2D v, UnitValue scalar)
        {
            return scalar * v;
        }
        public static UnitVector2D operator *(UnitVector2D v, int scalar)
        {
            return scalar * v;
        }
        public static UnitVector2D operator *(double scalar, UnitVector2D v)
        {
            var x = scalar * v.X;
            var y = scalar * v.Y;
            return new UnitVector2D(v.Unit, x, y);
        }
        public static UnitVector2D operator /(UnitVector2D v, UnitValue scalar)
        {
            var x = v.X / scalar.Value;
            var y = v.Y / scalar.Value;
            return new UnitVector2D(v.Unit / scalar.Unit, x, y);
        }

        public override string ToString()
        {
            return $"{X};{Y}";
        }

        public UnitVector2D DeepClone()
        {
            return new UnitVector2D(Unit.Clone(), X, Y);
        }
    }
}