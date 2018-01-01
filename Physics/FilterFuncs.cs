using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Collections;
using Commons.Extensions;

namespace Commons.Physics
{
    public static class FilterFuncs
    {
        public static IEnumerable<double> MovingAverage(this IList<double> data, int windowSize)
        {
            if(windowSize.IsEven())
                throw new ArgumentException("Window size must be odd");
            if (data.Count == 0)
                yield break;

            var buffer = new CircularBuffer<double>(windowSize);
            var halfWindowSize = (windowSize - 1) / 2;
            var paddedData = Enumerable.Repeat(data.First(), halfWindowSize)
                .Concat(data)
                .Concat(Enumerable.Repeat(data.Last(), halfWindowSize))
                .ToList();
            paddedData.Take(windowSize-1).ForEach(x => buffer.Put(x));
            for (int dataIdx = 0; dataIdx < data.Count; dataIdx++)
            {
                buffer.Put(paddedData[windowSize - 1 + dataIdx]);
                yield return buffer.Average();
            }
        }
    }
}
