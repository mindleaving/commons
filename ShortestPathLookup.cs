using System;
using System.Collections.Generic;

namespace Commons
{
    public class ShortestPathLookup
    {
        public readonly Vertex Source;
        public Dictionary<Vertex, GraphPath> Paths { get; }

        public ShortestPathLookup(Vertex source, Dictionary<Vertex, GraphPath> paths)
        {
            Source = source;
            Paths = paths;
        }

        public GraphPath PathTo(Vertex target)
        {
            if (!Paths.ContainsKey(target))
                throw new ArgumentException("Target is not in graph");

            return Paths[target];
        }

        public double PathLengthTo(Vertex target)
        {
            return !Paths.ContainsKey(target) ? Double.PositiveInfinity : Paths[target].PathLength;
        }

        public void RecalculateAllPathLengths()
        {
            Paths.Values.ForEach(path => path.RecalculatePathLength());
        }
    }
}