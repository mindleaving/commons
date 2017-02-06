﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons
{
    public static class NearestNeighborFinder
    {
        public static T FindNearestNeighbor<T>(IEnumerable<T> points, Func<T, GeoCoordinate> coordinateSelector, GeoCoordinate position)
        {
            return points.OrderBy<T, UnitValue>(x => coordinateSelector(x).GetDistanceTo(position)).FirstOrDefault();
        }
    }
}
