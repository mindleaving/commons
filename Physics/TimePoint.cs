using System;
using System.Runtime.Serialization;

namespace Commons.Physics
{
    [DataContract]
    public class TimePoint<T>
    {
        public TimePoint() { } // For serialization
        public TimePoint(DateTime time, T value)
        {
            Time = time;
            Value = value;
        }

        [DataMember]
        public DateTime Time { get; set; }
        [DataMember]
        public T Value { get; set; }
    }
}