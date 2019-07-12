using System;
using System.Collections.Generic;

namespace Commons.Physics
{
    public interface IUnitDefinition
    {
        string StringRepresentation { get; }
        bool IsSIUnit { get; }
        CompoundUnit CorrespondingCompoundUnit { get; }
        IList<string> AlternativeStringRepresentations { get; }

        UnitValue ConvertToUnitValue(double value);
        double ConvertBack(UnitValue unitValue);
    }

    public class UnitDefinition : IUnitDefinition
    {
        private readonly Func<double, double> toUnitValue;
        private readonly Func<double, double> convertBack;

        public UnitDefinition(
            string stringRepresentation, 
            bool isSIUnit, 
            CompoundUnit correspondingCompoundUnit,
            Func<double, double> toUnitValue,
            Func<double, double> convertBack,
            IList<string> alternativeStringRepresentations = null)
        {
            this.toUnitValue = toUnitValue;
            this.convertBack = convertBack;
            StringRepresentation = stringRepresentation;
            IsSIUnit = isSIUnit;
            CorrespondingCompoundUnit = correspondingCompoundUnit;
            AlternativeStringRepresentations = alternativeStringRepresentations ?? new List<string>();
        }


        public string StringRepresentation { get; }
        public bool IsSIUnit { get; }
        public CompoundUnit CorrespondingCompoundUnit { get; }
        public IList<string> AlternativeStringRepresentations { get; }

        public UnitValue ConvertToUnitValue(double value)
        {
            return new UnitValue(CorrespondingCompoundUnit, toUnitValue(value));
        }

        public double ConvertBack(UnitValue unitValue)
        {
            if(unitValue.Unit != CorrespondingCompoundUnit)
                throw new InvalidOperationException($"Cannot convert unit '{unitValue.Unit}' to '{CorrespondingCompoundUnit}'");
            return convertBack(unitValue.Value);
        }
    }
}
