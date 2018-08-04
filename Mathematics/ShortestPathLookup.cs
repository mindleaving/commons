using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Mathematics
{
    public class ShortestPathLookup
    {
        public IVertex Source { get; }
        private readonly IGraph graph;
        private readonly Dictionary<uint, uint> backtraceMap;
        private readonly Dictionary<uint, double> pathLengths;

        public ShortestPathLookup(IGraph graph, IVertex source, 
            Dictionary<uint, uint> backtraceMap,
            Dictionary<uint, double> pathLengths)
        {
            Source = source;
            this.graph = graph;
            this.backtraceMap = backtraceMap;
            this.pathLengths = pathLengths;
        }

        public GraphPath PathTo(IVertex target)
        {
            if(target.Id == Source.Id)
                return new GraphPath(Source.Id);
            if (!backtraceMap.ContainsKey(target.Id))
                throw new ArgumentException("Target is not in graph");

            var path = new List<IEdge>();
            var currentVertexId = target.Id;
            while (currentVertexId != Source.Id)
            {
                var nextVertexId = backtraceMap[currentVertexId];
                var shortestEdge = graph.GetVertexFromId(currentVertexId).EdgeIds
                    .Select(edgeId => graph.GetEdgeById(edgeId))
                    .Where(edge => edge.HasVertex(nextVertexId))
                    .OrderBy(edge => edge.Weight)
                    .First();
                path.Add(shortestEdge);
                currentVertexId = nextVertexId;
            }
            path.Reverse();

            return new GraphPath(Source.Id, path);
        }

        public double PathLengthTo(IVertex target)
        {
            return !pathLengths.ContainsKey(target.Id) ? double.PositiveInfinity : pathLengths[target.Id];
        }
    }
}