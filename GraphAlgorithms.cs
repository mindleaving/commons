using System;
using System.Collections.Generic;
using System.Linq;

namespace Commons
{
    public static class GraphAlgorithms
    {
        public static double[,] ComputeAdjacencyMatrix<TVertex, TEdge>(Graph<TVertex, TEdge> graph)
        {
            var adjacencyMatrix = new double[graph.Vertices.Count, graph.Vertices.Count];

            var sortedVertexIds = graph.Vertices.Values.Select(v => v.Id).OrderBy(x => x).ToList();
            foreach (var edge in graph.Edges.Values)
            {
                var vertex1Index = sortedVertexIds.IndexOf(edge.Vertex1Id);
                var vertex2Index = sortedVertexIds.IndexOf(edge.Vertex2Id);
                adjacencyMatrix[vertex1Index, vertex2Index]++;
                adjacencyMatrix[vertex2Index, vertex1Index]++;
            }

            return adjacencyMatrix;
        }

        public static ShortestPathLookup<TVertex, TEdge> ShortestPaths<TVertex, TEdge>(Graph<TVertex,TEdge> graph, uint sourceVertexId)
        {
            return ShortestPaths(graph, graph.Vertices[sourceVertexId]);
        }
        public static ShortestPathLookup<TVertex, TEdge> ShortestPaths<TVertex, TEdge>(Graph<TVertex, TEdge> graph, Vertex<TVertex> source)
        {
            if (!graph.Vertices.ContainsKey(source.Id))
                throw new ArgumentException("GraphAlgorithms.ShortestPath: Source vertex not in graph");
            if (graph.Edges.Values.Any(e => e.Weight < 0))
                throw new NotImplementedException("GraphAlgorithms.ShortestPath: No shortest path algorithm is implemented for graphs with negative edge-weights");

            // Dijkstra's algorithm
            var unVisitedVertexDictionary = graph.Vertices.ToDictionary(v => v.Key, v => true);
            var unvisitedVertexCount = unVisitedVertexDictionary.Count;
            var shortestPathLengths = new Dictionary<uint, double> { { source.Id, 0} };
            var backtraceMap = new Dictionary<uint, uint>();

            while (unvisitedVertexCount > 0)
            {
                var vertexWithShortestDistance = shortestPathLengths
                    .Where(kvp => unVisitedVertexDictionary[kvp.Key])
                    .OrderBy(kvp => kvp.Value)
                    .First().Key;
                var currentVertex = graph.Vertices[vertexWithShortestDistance];

                unVisitedVertexDictionary[currentVertex.Id] = false;
                unvisitedVertexCount--;

                // If all un-visisted vertices have +inf path lengths, the graph is not connected
                // Either stop here and return vertices with +inf path length
                // or throw an exception.
                if (!shortestPathLengths.ContainsKey(currentVertex.Id))
                    break;//throw new Exception("GraphAlgorithms.ShortestPath: Graph appears to be not connected.");

                // Update adjacent vertices
                var adjacentEdgeVertexDictionary = currentVertex.EdgeIds
                    .Select(e => graph.Edges[e])
                    .Where(e => !e.IsDirected || e.Vertex1Id == currentVertex.Id)
                    .Select(e => new { VertexId = e.Vertex1Id != currentVertex.Id ? e.Vertex1Id : e.Vertex2Id, Edge = e });
                var currentVertexPathLength = shortestPathLengths[currentVertex.Id];
                foreach (var vertexEdgePair in adjacentEdgeVertexDictionary)
                {
                    var adjacentVertex = graph.Vertices[vertexEdgePair.VertexId];
                    var adjacentEdge = vertexEdgePair.Edge;
                    // Skip already visited vertices
                    if (!unVisitedVertexDictionary[adjacentVertex.Id])
                        continue;
                    var currentShortestPathLength = shortestPathLengths.ContainsKey(adjacentVertex.Id)
                        ? shortestPathLengths[adjacentVertex.Id]
                        : double.PositiveInfinity;
                    if (currentShortestPathLength < currentVertexPathLength + adjacentEdge.Weight)
                        continue;
                    backtraceMap[adjacentVertex.Id] = currentVertex.Id;
                    shortestPathLengths[adjacentVertex.Id] = currentVertexPathLength + adjacentEdge.Weight;
                }
            }
            return new ShortestPathLookup<TVertex, TEdge>(graph,
                source,
                backtraceMap,
                shortestPathLengths);
        }

        public static bool IsGraphConnected<TVertex, TEdge>(Graph<TVertex, TEdge> graph)
        {
            if (!graph.Vertices.Any())
                return true;
            var startVertex = graph.Vertices.Values.First();
            var connectedVertices = GetConnectedSubgraph(graph, startVertex).Vertices;
            return graph.Vertices.Count == connectedVertices.Count;
        }

        public static void ApplyMethodToAllConnectedVertices<TVertex, TEdge>(Graph<TVertex, TEdge> graph, Vertex<TVertex> startVertex, Action<Vertex<TVertex>> action)
        {
            foreach (var connectedVertex in GetConnectedSubgraph(graph, startVertex).Vertices.Values)
            {
                action(connectedVertex);
            }
        }

        public static Graph<TVertex, TEdge> GetConnectedSubgraph<TVertex, TEdge>(Graph<TVertex, TEdge> graph, Vertex<TVertex> startVertex)
        {
            // Use depth first search for traversing connected component
            // Initialize graph algorithm data
            graph.Vertices.ForEach(v => v.Value.AlgorithmData = false);
            graph.Edges.Values.ForEach(e => e.AlgorithmData = false);

            var connectedVertices = GetConnectedVertices(graph, startVertex).Select(v => v.Id).ToList();
            return GetSubgraph(graph, connectedVertices);
        }

        private static IEnumerable<Vertex<TVertex>> GetConnectedVertices<TVertex, TEdge>(Graph<TVertex, TEdge> graph, Vertex<TVertex> currentVertex)
        {
            // Mark vertex as visited
            currentVertex.AlgorithmData = true;

            var unvisitedAdjacentVertices = currentVertex.EdgeIds
                .Select(edgeId => graph.Edges[edgeId])
                .Select(edge => edge.Vertex1Id == currentVertex.Id ? edge.Vertex2Id : edge.Vertex1Id);

            foreach (var adjacentVertexId in unvisitedAdjacentVertices)
            {
                var adjacentVertex = graph.Vertices[adjacentVertexId];
                if (adjacentVertex.AlgorithmData.Equals(true))
                    continue;

                foreach (var vertex in GetConnectedVertices(graph, adjacentVertex))
                {
                    yield return vertex;
                }
            }
            yield return currentVertex;
        }

        public static IEnumerable<Vertex<TVertex>> GetAdjacentVertices<TVertex, TEdge>(Graph<TVertex, TEdge> graph, Vertex<TVertex> vertex)
        {
            return vertex.EdgeIds
                .Select(edgeId => graph.Edges[edgeId])
                .Select(edge => edge.Vertex1Id == vertex.Id ? edge.Vertex2Id : edge.Vertex1Id)
                .Distinct()
                .Select(vId => graph.Vertices[vId]);
        }

        public static Graph<TVertex, TEdge> GetSubgraph<TVertex, TEdge>(Graph<TVertex, TEdge> graph, IList<uint> vertices)
        {
            var subgraphVertices = vertices.Select(vertexId => graph.Vertices[vertexId]);
            var vertexIdHashSet = new HashSet<uint>(vertices);
            var subgraphEdges = graph.Edges.Values
                .Where(e => vertexIdHashSet.Contains(e.Vertex1Id) && vertexIdHashSet.Contains(e.Vertex2Id));
            return new Graph<TVertex, TEdge>(subgraphVertices, subgraphEdges);
        }
    }
}