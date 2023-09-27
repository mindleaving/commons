using Commons.Extensions;
using Commons.Mathematics;
using Commons.Serialization;
using Newtonsoft.Json;

namespace Commons.Physics
{
    [JsonConverter(typeof(UnitPointJsonConverter))]
    public class UnitPoint2D : Point2D
    {
        public CompoundUnit Unit { get; }

        public UnitPoint2D(UnitValue x, UnitValue y)
            : base(x.In(x.Unit), y.In(x.Unit))
        {
            Unit = x.Unit;
        }
        public UnitPoint2D(SIPrefix prefix, IUnitDefinition unit, double x, double y)
            : base(
                prefix.GetMultiplier() * unit.ConvertToUnitValue(x).Value, 
                prefix.GetMultiplier() * unit.ConvertToUnitValue(y).Value)
        {
            Unit = unit.CorrespondingCompoundUnit;
        }

        [JsonConstructor]
        public UnitPoint2D(CompoundUnit unit, double x, double y)
            : base(x, y)
        {
            Unit = unit;
        }
        public UnitPoint2D(IUnitDefinition unit, double x, double y)
            : this(SIPrefix.None, unit, x, y)
        {
        }

        public static UnitPoint2D operator +(UnitPoint2D point1, UnitPoint2D point2)
        {
#if CHECK_UNITS
            if (point1.Unit != point2.Unit)
                throw new InvalidOperationException($"Cannot add points with different units, got {point1.Unit} and {point2.Unit}");
#endif
            var x = point1.X + point2.X;
            var y = point1.Y + point2.Y;
            return new UnitPoint2D(point1.Unit, x, y);
        }
        public static UnitPoint2D operator -(UnitPoint2D point1, UnitPoint2D point2)
        {
#if CHECK_UNITS
            if (point1.Unit != point2.Unit)
                throw new InvalidOperationException($"Cannot subtract points with different units, got {point1.Unit} and {point2.Unit}");
#endif
            var x = point1.X - point2.X;
            var y = point1.Y - point2.Y;
            return new UnitPoint2D(point1.Unit, x, y);
        }
        public static UnitPoint2D operator +(UnitPoint2D point1, UnitVector2D v)
        {
#if CHECK_UNITS
            if (point1.Unit != v.Unit)
                throw new InvalidOperationException($"Cannot add point and vector with different units, got {point1.Unit} and {v.Unit}");
#endif
            var x = point1.X + v.X;
            var y = point1.Y + v.Y;
            return new UnitPoint2D(point1.Unit, x, y);
        }
        public static UnitPoint2D operator -(UnitPoint2D point1, UnitVector2D v)
        {
#if CHECK_UNITS
            if (point1.Unit != v.Unit)
                throw new InvalidOperationException($"Cannot subtract point and vector with different units, got {point1.Unit} and {v.Unit}");
#endif
            var x = point1.X - v.X;
            var y = point1.Y - v.Y;
            return new UnitPoint2D(point1.Unit, x, y);
        }
        public static UnitPoint2D operator *(double scalar, UnitPoint2D point)
        {
            var x = scalar * point.X;
            var y = scalar * point.Y;
            return new UnitPoint2D(point.Unit, x, y);
        }
        public static UnitPoint2D operator *(UnitValue scalar, UnitPoint2D point)
        {
            var x = scalar.Value * point.X;
            var y = scalar.Value * point.Y;
            return new UnitPoint2D(scalar.Unit * point.Unit, x, y);
        }

        public override string ToString()
        {
            return $"{X};{Y}";
        }

        public UnitPoint2D DeepClone()
        {
            return new UnitPoint2D(Unit.Clone(), X, Y);
        }
    }
}