using System;

namespace Commons.Physics
{
    public class TimePoint<T>
    {
        public TimePoint(DateTime time, T value)
        {
            Time = time;
            Value = value;
        }

        public DateTime Time { get; }
        public T Value { get; }
    }
}