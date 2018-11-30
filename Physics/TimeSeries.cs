using System;
using System.Collections.Generic;

namespace Commons.Physics
{
    public class TimeSeries<T> : List<TimePoint<T>>
    {
        public void Add(DateTime time, T value)
        {
            Add(new TimePoint<T>(time, value));
        }
    }
}