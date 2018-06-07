using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Collections;
using Commons.Mathematics;

namespace Commons.DataProcessing
{
    /// <summary>
    /// Finds clusters by dragging a window along 1D-data
    /// and constructs N clusters covering as many data points as possible
    /// </summary>
    public class WindowClusterer
    {
        public List<Cluster<T>> Cluster<T>(IEnumerable<T> items, Func<T, double> valueSelector, double windowSize, int clusterCount)
        {
            var orderedItems = items.OrderBy(valueSelector).ToList();
            var slidingWindow = new SlidingWindow<T>(orderedItems, valueSelector, windowSize, WindowPositioningType.CenteredAtPosition);
            var clusterSizeTrace = new List<Point2D>();
            foreach (var item in orderedItems)
            {
                var windowPosition = valueSelector(item);
                slidingWindow.SetPosition(windowPosition);
                var clusterSize = slidingWindow.Count();
                clusterSizeTrace.Add(new Point2D(windowPosition, clusterSize));
            }
            var clusters = new List<Cluster<T>>(clusterCount);
            var orderedClusterSizes = clusterSizeTrace.OrderByDescending(p => p.Y);
            foreach (var clusterSize in orderedClusterSizes)
            {
                var windowPosition = clusterSize.X;
                var clusterCandidateRange = new Range<double>(windowPosition - windowSize / 2, windowPosition + windowSize / 2);
                var itemsInRange = orderedItems
                    .SkipWhile(x => valueSelector(x) < clusterCandidateRange.From)
                    .TakeWhile(x => valueSelector(x) <= clusterCandidateRange.To)
                    .ToList();
                // Check that cluster candidate items do not overlap with existing clusters
                if(clusters.Any(cluster => cluster.Items.Intersect(itemsInRange).Any()))
                    continue;
                clusters.Add(new Cluster<T>(itemsInRange));
                if(clusters.Count == clusterCount)
                    break;
            }
            return clusters;
        }
    }
}
