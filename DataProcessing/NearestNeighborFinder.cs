using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;

namespace Commons.DataProcessing
{
    public static class NearestNeighborFinder
    {
        public static T FindNearestNeighbor<T>(IEnumerable<T> points, Func<T, GeoCoordinate> coordinateSelector, GeoCoordinate position)
        {
            return points.OrderBy(x => coordinateSelector(x).GetDistanceTo(position)).FirstOrDefault();
        }
    }
}
