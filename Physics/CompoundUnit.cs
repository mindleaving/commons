using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;

namespace Commons.Physics
{
    public class CompoundUnit : IEquatable<CompoundUnit>
    {
        public CompoundUnit()
        { }
        public CompoundUnit(IEnumerable<SIBaseUnit> nominatorUnits)
            : this(nominatorUnits, Enumerable.Empty<SIBaseUnit>())
        { }
        public CompoundUnit(IEnumerable<SIBaseUnit> nominatorUnits, IEnumerable<SIBaseUnit> denominatorUnits)
        {
            UnitExponents = new int[SIBaseUnits.Count];

            foreach (var nominatorUnit in nominatorUnits)
            {
                var unitIdx = SIBaseUnits[nominatorUnit];
                UnitExponents[unitIdx]++;
            }
            foreach (var denominatorUnit in denominatorUnits)
            {
                var unitIdx = SIBaseUnits[denominatorUnit];
                UnitExponents[unitIdx]--;
            }
        }
        public CompoundUnit(IEnumerable<int> unitExponents)
        {
            UnitExponents = unitExponents.ToArray();
        }

        public int[] UnitExponents { get; private set; }

        public static CompoundUnit operator *(CompoundUnit unit1, CompoundUnit unit2)
        {
            return new CompoundUnit(unit1.UnitExponents.PairwiseOperation(unit2.UnitExponents, (a,b) => a + b));
        }

        public static CompoundUnit operator /(CompoundUnit unit1, CompoundUnit unit2)
        {
            return new CompoundUnit(unit1.UnitExponents.PairwiseOperation(unit2.UnitExponents, (a, b) => a - b));
        }

        public static bool operator ==(CompoundUnit unit1, CompoundUnit unit2)
        {
            return unit1?.Equals(unit2) ?? false;
        }
        public static bool operator ==(CompoundUnit unit1, Unit unit2)
        {
            return unit1?.Equals(unit2.ToCompoundUnit()) ?? false;
        }
        public static bool operator !=(CompoundUnit unit1, CompoundUnit unit2)
        {
            return !(unit1 == unit2);
        }
        public static bool operator !=(CompoundUnit unit1, Unit unit2)
        {
            return !(unit1 == unit2);
        }

        public override bool Equals(object other)
        {
            if (other is Unit)
                return Equals(((Unit) other).ToCompoundUnit());
            return Equals(other as CompoundUnit);
        }

        private static readonly Dictionary<SIBaseUnit, int> SIBaseUnits = ((SIBaseUnit[]) Enum.GetValues(typeof(SIBaseUnit)))
            .ToDictionary(unit => unit, unit => (int)unit);

        public override int GetHashCode()
        {
            return UnitExponents?.GetHashCode() ?? 0;
        }

        public bool Equals(CompoundUnit other)
        {
            if ((object)other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return UnitExponents.SequenceEqual(other.UnitExponents);
        }

        public override string ToString()
        {
            var nominator = UnitExponents
                .Select((multiplicity, idx) => new
                {
                    SIBaseUnit = (SIBaseUnit)idx,
                    Multiplicity = multiplicity
                })
                .Where(kvp => kvp.Multiplicity > 0).ToList();
            var denominator = UnitExponents
                .Select((multiplicity, idx) => new
                {
                    SIBaseUnit = (SIBaseUnit)idx,
                    Multiplicity = multiplicity
                })
                .Where(kvp => kvp.Multiplicity < 0)
                .ToList();
            if (!nominator.Any() && !denominator.Any())
                return "";

            var nominatorStrings = new List<string>();
            foreach (var unitMultiplicity in nominator)
            {
                var nominatorUnit = unitMultiplicity.SIBaseUnit.StringRepresentation();
                if (unitMultiplicity.Multiplicity > 1)
                    nominatorUnit += "^" + unitMultiplicity.Multiplicity;
                nominatorStrings.Add(nominatorUnit);
            }
            var denominatorStrings = new List<string>();
            foreach (var unitMultiplicity in denominator)
            {
                var denominatorUnit = unitMultiplicity.SIBaseUnit.StringRepresentation();
                if (unitMultiplicity.Multiplicity.Abs() > 1)
                    denominatorUnit += "^" + unitMultiplicity.Multiplicity.Abs();
                denominatorStrings.Add(denominatorUnit);
            }
            var unitString = "";
            if (nominatorStrings.Any())
                unitString += nominatorStrings.Aggregate((a, b) => a + " " + b);
            else
                unitString += "1";
            if (!denominator.Any())
                return unitString;
            unitString += "/";
            unitString += denominatorStrings.Count > 1
                ? $"({denominatorStrings.Aggregate((a, b) => a + " " + b)})"
                : denominatorStrings.Single();
            return unitString;
        }

        public CompoundUnit Clone()
        {
            return new CompoundUnit(UnitExponents);
        }
    }
}