using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Extensions
{
    public static class EnumExtensions
    {
        public static bool InSet<T>(this T value, params T[] set)
        {
            return set.Contains(value);
        }

        public static IEnumerable<T> GetValues<T>()
        {
            if(!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enum");
            return (T[]) Enum.GetValues(typeof(T));
        }
    }
}
