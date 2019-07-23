using System;
using System.Globalization;
using System.Linq;
using Commons.Extensions;
using Commons.Serialization;
using Newtonsoft.Json;

namespace Commons.Physics
{
    [JsonConverter(typeof(UnitValueJsonConverter))]
    public class UnitValue : IComparable
    {
        [JsonConstructor]
        private UnitValue(string stringValue)
        {
            var parsedValue = Parse(stringValue);
            Value = parsedValue.Value;
            Unit = parsedValue.Unit;
        }
        public UnitValue(IUnitDefinition unit, double value)
        {
            var unitValue = unit.ConvertToUnitValue(value);
            Unit = unitValue.Unit;
            Value = unitValue.Value;
        }
        public UnitValue(CompoundUnit unit, double value)
        {
            Unit = unit;
            Value = value;
        }

        public double Value { get; }
        public CompoundUnit Unit { get; }

        public static bool operator <(UnitValue value1, UnitValue value2)
        {
            if (!value1.Unit.Equals(value2.Unit))
                throw new InvalidOperationException($"Cannot compare unit values with incompatible units {value1.Unit} and {value2.Unit}");

            return value1.Value < value2.Value;
        }
        public static bool operator >(UnitValue value1, UnitValue value2)
        {
            return value2 < value1;
        }
        public static bool operator <=(UnitValue value1, UnitValue value2)
        {
            if (!value1.Unit.Equals(value2.Unit))
                throw new InvalidOperationException($"Cannot compare unit values with incompatible units {value1.Unit} and {value2.Unit}");

            return value1.Value <= value2.Value;
        }
        public static bool operator >=(UnitValue value1, UnitValue value2)
        {
            return value2 <= value1;
        }
        public static bool operator ==(UnitValue value1, UnitValue value2)
        {
            if (ReferenceEquals(value1, null) && ReferenceEquals(value2, null))
                return true;
            if (ReferenceEquals(value1, null))
                return false;
            return value1.Equals(value2);
        }
        public static bool operator !=(UnitValue value1, UnitValue value2)
        {
            return !(value1 == value2);
        }
        public static UnitValue operator -(UnitValue value1)
        {
            return new UnitValue(value1.Unit, -value1.Value);
        }
        public static UnitValue operator +(UnitValue value1, UnitValue value2)
        {
            if (!value1.Unit.Equals(value2.Unit))
                throw new InvalidOperationException($"Cannot sum unit values with incompatible units {value1.Unit} and {value2.Unit}");

            return (value1.Value + value2.Value).To(value1.Unit);
        }
        public static UnitValue operator -(UnitValue value1, UnitValue value2)
        {
            if (!value1.Unit.Equals(value2.Unit))
                throw new InvalidOperationException($"Cannot subtract unit values with incompatible units {value1.Unit} and {value2.Unit}");

            return (value1.Value - value2.Value).To(value1.Unit);
        }
        public static UnitValue operator *(double scalar, UnitValue unitValue)
        {
            return new UnitValue(unitValue.Unit, scalar * unitValue.Value);
        }
        public static UnitValue operator *(int scalar, UnitValue unitValue)
        {
            return new UnitValue(unitValue.Unit, scalar * unitValue.Value);
        }
        public static UnitValue operator *(UnitValue unitValue, double scalar)
        {
            return scalar * unitValue;
        }
        public static UnitValue operator *(UnitValue unitValue, int scalar)
        {
            return scalar * unitValue;
        }
        public static UnitValue operator /(UnitValue unitValue, double scalar)
        {
            return new UnitValue(unitValue.Unit, unitValue.Value / scalar);
        }
        public static UnitValue operator /(UnitValue unitValue, int scalar)
        {
            return new UnitValue(unitValue.Unit, unitValue.Value / scalar);
        }
        public static UnitValue operator *(UnitValue value1, UnitValue value2)
        {
            return (value1.Value*value2.Value).To(value1.Unit*value2.Unit);
        }
        public static UnitValue operator /(UnitValue value1, UnitValue value2)
        {
            return (value1.Value / value2.Value).To(value1.Unit / value2.Unit);
        }

        public override bool Equals(object other)
        {
            if (ReferenceEquals(other, this))
                return true;
            if (ReferenceEquals(other, null))
                return false;
            var otherUnitValue = other as UnitValue;
            if (otherUnitValue == null)
                return false;
            if (!Unit.Equals(otherUnitValue.Unit))
                return false;
            if (Value.IsNaN() && otherUnitValue.Value.IsNaN())
                return true;
            if (Value == otherUnitValue.Value)
                return true;
            return (Value - otherUnitValue.Value).Abs() < 1e-15;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var otherUnitValue = obj as UnitValue;
            if (otherUnitValue != null)
            {
                return Value.CompareTo(otherUnitValue.Value);
            }
            return 0;
        }

        public UnitValue DeepClone()
        {
            return new UnitValue(Unit.Clone(), Value);
        }

        public override string ToString()
        {
            return ToString("G6", CultureInfo.InvariantCulture);
        }
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.InvariantCulture);
        }

        public string ToString(string format, CultureInfo cultureInfo)
        {
            if(Unit.UnitExponents.All(x => x == 0))
                return Value.ToString(format, cultureInfo);
            var simpleUnit = Physics.Unit.Effective.AllUnits.FirstOrDefault(x => x.CorrespondingCompoundUnit == Unit);
            if (Unit.UnitExponents.Any(exp => exp > 1 || exp < 0) || simpleUnit == null)
            {
                return $"{Value.ToString(format, cultureInfo)} {Unit}";
            }
            if (simpleUnit == Physics.Unit.Kilogram)
            {
                var gramValue = Value * 1000;
                var appropriateSIPrefix = gramValue.SelectSIPrefix();
                var multiplier = appropriateSIPrefix.GetMultiplier();
                var valueString = (gramValue/multiplier).ToString(format, cultureInfo) 
                                  + " "
                                  + appropriateSIPrefix.StringRepresentation();
                return valueString + Physics.Unit.Gram.StringRepresentation;
            }
            else
            {
                var appropriateSIPrefix = Value.SelectSIPrefix();
                var multiplier = appropriateSIPrefix.GetMultiplier();
                var valueString = (Value/multiplier).ToString(format, cultureInfo) 
                                  + " "
                                  + appropriateSIPrefix.StringRepresentation();
                return valueString + simpleUnit.StringRepresentation;
            }
        }

        public static UnitValue Parse(string value)
        {
            return UnitValueParser.Parse(value);
        }
    }
}
