using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Commons.Physics
{
    [CollectionDataContract]
    public class TimeSeries<T> : List<TimePoint<T>>
    {
    }
}