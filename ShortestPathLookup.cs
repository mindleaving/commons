using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons
{
    public class ShortestPathLookup
    {
        public Vertex Source { get; }
        private readonly Graph graph;
        private readonly Dictionary<uint, uint> backtraceMap;
        private readonly Dictionary<uint, double> pathLengths;

        public ShortestPathLookup(Graph graph, Vertex source, 
            Dictionary<uint, uint> backtraceMap,
            Dictionary<uint, double> pathLengths)
        {
            Source = source;
            this.graph = graph;
            this.backtraceMap = backtraceMap;
            this.pathLengths = pathLengths;
        }

        public GraphPath PathTo(Vertex target)
        {
            if (!backtraceMap.ContainsKey(target.Id))
                throw new ArgumentException("Target is not in graph");

            var path = new List<Edge>();
            var currentVertexId = target.Id;
            while (currentVertexId != Source.Id)
            {
                var nextVertexId = backtraceMap[currentVertexId];
                var shortestEdge = graph.Vertices[currentVertexId].EdgeIds
                    .Select(edgeId => graph.Edges[edgeId])
                    .Where(edge => edge.HasVertex(nextVertexId))
                    .OrderBy(edge => edge.Weight)
                    .First();
                path.Add(shortestEdge);
                currentVertexId = nextVertexId;
            }
            path.Reverse();

            return new GraphPath(Source.Id, path);
        }

        public double PathLengthTo(Vertex target)
        {
            return !pathLengths.ContainsKey(target.Id) ? double.PositiveInfinity : pathLengths[target.Id];
        }
    }
}