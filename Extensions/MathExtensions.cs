using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Extensions
{
    public static class MathExtensions
    {
        public static double Square(this double x)
        {
            return x * x;
        }
        public static double Sqrt(this double x)
        {
            return Math.Sqrt(x);
        }
        public static int Modulus(this int n, int modulus)
        {
            var residual = n % modulus;
            return residual < 0 ? residual + modulus : residual;
        }
        public static double Modulus(this double n, double modulus)
        {
            var residual = n % modulus;
            return residual < 0 ? residual + modulus : residual;
        }

        public static double Abs(this int value)
        {
            return Math.Abs(value);
        }
        public static double Abs(this double value)
        {
            return Math.Abs(value);
        }

        public static double IntegerPower(this double x, int n)
        {
            if (n == 0)
                return 1;
            if (n == 1)
                return x;
            if (n < 0)
                return 1.0 / x.IntegerPower(-n);
            return x * x.IntegerPower(n - 1);
        }

        public static double Pow(this double x, double exponent)
        {
            return Math.Pow(x, exponent);
        }

        public static bool IsEven(this int i)
        {
            return i%2 == 0;
        }
        public static bool IsOdd(this int i)
        {
            return !i.IsEven();
        }

        public static bool IsBetween(this int x, int a, int b)
        {
            var min = Math.Min(a, b);
            var max = Math.Max(a, b);

            return x >= min && x <= max;
        }
        public static bool IsBetween(this double x, double a, double b)
        {
            var min = Math.Min(a, b);
            var max = Math.Max(a, b);

            return x >= min && x <= max;
        }
        public static bool IsEven(this uint i)
        {
            return i % 2 == 0;
        }
        public static bool IsOdd(this uint i)
        {
            return !i.IsEven();
        }

        public static bool IsNaN(this double value)
        {
            return double.IsNaN(value);
        }

        public static bool IsPositiveInfinity(this double value)
        {
            return double.IsPositiveInfinity(value);
        }

        public static bool IsNegativeInfinity(this double value)
        {
            return double.IsNegativeInfinity(value);
        }

        public static bool IsInfinity(this double value)
        {
            return double.IsInfinity(value);
        }

        /// <summary>
        /// Calculates the small angle between two headings
        /// </summary>
        public static double CircularDifference(this double angle1, double angle2)
        {
            var modulusAngle1 = angle1.Modulus(360);
            var modulusAngle2 = angle2.Modulus(360);
            var greaterAngle = Math.Max(modulusAngle1, modulusAngle2);
            var smallerAngle = Math.Min(modulusAngle1, modulusAngle2);
            return greaterAngle - smallerAngle > 180
                ? 360 - (greaterAngle - smallerAngle)
                : greaterAngle - smallerAngle;
        }

        public static double CircularDifference(this int angle1, double angle2)
        {
            return CircularDifference((double) angle1, angle2);
        }

        public static double RoundToNearest(this double value, double resolution)
        {
            return Math.Round(value / resolution) * resolution;
        }
        public static double RoundDownToNearest(this double value, double resolution)
        {
            return Math.Floor(value / resolution) * resolution;
        }
        public static double RoundUpToNearest(this double value, double resolution)
        {
            return Math.Ceiling(value / resolution) * resolution;
        }

        public static int RoundToNearest(this int value, int resolution)
        {
            return (int) ((double) value).RoundToNearest(resolution);
        }
        public static int RoundDownToNearest(this int value, int resolution)
        {
            return (int)((double)value).RoundDownToNearest(resolution);
        }
        public static int RoundUpToNearest(this int value, int resolution)
        {
            return (int)((double)value).RoundUpToNearest(resolution);
        }

        public static double Median(this IEnumerable<double> items)
        {
            var orderedItemList = items.OrderBy(x => x).ToList();
            var halfIndex = orderedItemList.Count / 2;
            if (orderedItemList.Count.IsEven())
            {
                return (orderedItemList[halfIndex - 1] + orderedItemList[halfIndex])/2.0;
            }
            return orderedItemList[halfIndex];
        }
        public static double Median<T>(this IEnumerable<T> items, Func<T,double> valueSelector)
        {
            return items.Select(valueSelector).Median();
        }
    }
}
