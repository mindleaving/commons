using System.Linq;
using Commons.Physics;

namespace Commons.Extensions
{
    public static class CompoundUnitExtensions
    {
        public static CompoundUnit Pow(this CompoundUnit unit, int pow)
        {
            return new CompoundUnit(unit.UnitExponents.Select(x => pow*x));
        }
    }
}