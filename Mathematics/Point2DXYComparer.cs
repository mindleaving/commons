using System;
using System.Collections.Generic;

namespace Commons.Mathematics
{
    public class Point2DXYComparer : IComparer<Point2D>
    {
        public int Compare(Point2D p1, Point2D p2)
        {
            if(p1 == null)
                throw new ArgumentNullException(nameof(p1));
            if(p2 == null)
                throw new ArgumentNullException(nameof(p2));
            if (p1.X != p2.X)
                return p1.X.CompareTo(p2.X);
            return p1.Y.CompareTo(p2.Y);
        }
    }
}