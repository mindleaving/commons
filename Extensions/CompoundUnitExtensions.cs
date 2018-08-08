using System.Collections.Generic;
using System.Linq;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class CompoundUnitExtensions
    {
        private static readonly Dictionary<CompoundUnit, Unit> CompoundUnitToUnitMap =
            EnumExtensions.GetValues<Unit>().Where(unit => unit.IsSIUnit()).ToDictionary(x => x.ToCompoundUnit(), x => x);

        public static Unit ToUnit(this CompoundUnit unit)
        {
            foreach (var kvp in CompoundUnitToUnitMap)
            {
                if (kvp.Key.Equals(unit))
                    return kvp.Value;
            }
            return Unit.Compound;
        }
    }
}