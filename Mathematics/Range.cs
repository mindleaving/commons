using System;
using Commons.Extensions;

namespace Commons.Mathematics
{
    public class Range<T> where T: IComparable<T>
    {
        /// <summary>
        /// Inclusive start of interval
        /// </summary>
        public T From { get; }
        /// <summary>
        /// Exclusive end of interval
        /// </summary>
        public T To { get; }

        public Range(T from, T to)
        {
            if(from.IsGreaterThan(to))
                throw new ArgumentException($"'From' value must be smaller than the 'to' value, but got 'from': {from}, 'to': {to}");
            From = from;
            To = to;
        }

        public bool Contains(T item)
        {
            if (From.IsGreaterThan(item))
                return false;
            if (To.IsSmallerThanOrEqualTo(item))
                return false;
            return true;
        }

        public bool Overlaps(Range<T> otherRange)
        {
            if (To.IsSmallerThanOrEqualTo(otherRange.From))
                return false;
            if (otherRange.To.IsSmallerThanOrEqualTo(From))
                return false;
            return true;
        }
    }
}
