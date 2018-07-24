using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Commons.Mathematics
{
    public class GraphPath<TVertex, TEdge>
    {
        public uint StartVertexId { get; set; }
        public LinkedList<Edge<TEdge>> Path { get; private set; }
        public double PathLength { get; private set; }

        public GraphPath(uint startVertexId)
        {
            StartVertexId = startVertexId;
            PathLength = 0.0;
            Path = new LinkedList<Edge<TEdge>>();
        }

        public GraphPath(uint startVertexId, IList<Edge<TEdge>> edges)
            : this(startVertexId)
        {
            ValidateEdgeList(edges);
            Path = new LinkedList<Edge<TEdge>>(edges);
            RecalculatePathLength();
        }

        public GraphPath(uint startVertexId, GraphPath<TVertex, TEdge> path)
            : this(startVertexId)
        {
            // No validation necessary
            Path = new LinkedList<Edge<TEdge>>(path.Path);
            PathLength = path.PathLength;
        }

        public void Append(Edge<TEdge> edge)
        {
            if(Path.Any())
                ValidateEdgePair(Path.Last.Value, edge);
            Path.AddLast(edge);
            PathLength += edge.Weight;
        }

        public void RecalculatePathLength()
        {
            PathLength = Path.Sum(e => e.Weight);
        }

        private void ValidateEdgeList(IList<Edge<TEdge>> edges)
        {
            for (int edgeIdx = 0; edgeIdx < edges.Count - 1; edgeIdx++)
            {
                var currentEdge = edges[edgeIdx];
                var nextEdge = edges[edgeIdx + 1];
                ValidateEdgePair(currentEdge, nextEdge);
            }
        }

        private static void ValidateEdgePair(Edge<TEdge> currentEdge, Edge<TEdge> nextEdge)
        {
            if (currentEdge.IsDirected)
            {
                if (nextEdge.IsDirected)
                {
                    if (currentEdge.Vertex2Id != nextEdge.Vertex1Id)
                        throw new ArgumentException(nameof(GraphPath<TVertex, TEdge>) + ": At least one pair of edges in path are not connected");
                }
                else
                {
                    if(currentEdge.Vertex2Id != nextEdge.Vertex1Id && currentEdge.Vertex2Id != nextEdge.Vertex2Id)
                        throw new ArgumentException(nameof(GraphPath<TVertex, TEdge>) + ": At least one pair of edges in path are not connected");
                }
            }
            else
            {
                if (nextEdge.IsDirected)
                {
                    if(currentEdge.Vertex1Id != nextEdge.Vertex1Id && currentEdge.Vertex2Id != nextEdge.Vertex1Id)
                        throw new ArgumentException(nameof(GraphPath<TVertex, TEdge>) + ": At least one pair of edges in path are not connected");
                }
                else
                {
                    if(currentEdge.Vertex1Id != nextEdge.Vertex1Id && currentEdge.Vertex2Id != nextEdge.Vertex1Id
                        && currentEdge.Vertex1Id != nextEdge.Vertex2Id && currentEdge.Vertex2Id != nextEdge.Vertex2Id)
                        throw new ArgumentException(nameof(GraphPath<TVertex, TEdge>) + ": At least one pair of edges in path are not connected");
                }
            }
        }

        public override string ToString()
        {
            return PathLength.ToString(CultureInfo.InvariantCulture);
        }
    }
}