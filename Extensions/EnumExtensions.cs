using System.Linq;

namespace Commons.Extensions
{
    public static class EnumExtensions
    {
        public static bool InSet<T>(this T value, params T[] set)
        {
            return set.Contains(value);
        }
    }
}
