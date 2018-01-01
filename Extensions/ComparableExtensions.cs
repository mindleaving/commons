using System;

namespace Commons.Extensions
{
    public static class ComparableExtensions
    {
        public static bool IsEqualTo<T>(this T item1, T item2) where T : IComparable<T>
        {
            return item1.CompareTo(item2) == 0;
        }

        public static bool IsSmallerThan<T>(this T item1, T item2) where T : IComparable<T>
        {
            return item1.CompareTo(item2) < 0;
        }

        public static bool IsSmallerThanOrEqualTo<T>(this T item1, T item2) where T : IComparable<T>
        {
            return item1.CompareTo(item2) <= 0;
        }

        public static bool IsGreaterThan<T>(this T item1, T item2) where T : IComparable<T>
        {
            return item1.CompareTo(item2) > 0;
        }

        public static bool IsGreaterThanOrEqualTo<T>(this T item1, T item2) where T : IComparable<T>
        {
            return item1.CompareTo(item2) >= 0;
        }
    }
}