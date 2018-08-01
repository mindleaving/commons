using System.Collections.Generic;

namespace Commons.Mathematics
{
    public interface IGraph<TVertex, TEdge>
    {
        Dictionary<ulong, IEdge<TEdge>> Edges { get; }
        Dictionary<uint, IVertex<TVertex>> Vertices { get; }

        ulong GetUnusedEdgeId();
        uint GetUnusedVertexId();
        void AddVertex(IVertex<TVertex> newVertex);
        void AddVertices(IEnumerable<IVertex<TVertex>> newVertices);
        bool RemoveVertex(IVertex<TVertex> vertex);
        bool RemoveVertex(uint vertexId);
        void AddEdge(IEdge<TEdge> newEdge);
        void AddEdges(IEnumerable<IEdge<TEdge>> edges);
        bool RemoveEdge(IEdge<TEdge> edge);
        Edge<TEdge> ConnectVertices(IVertex<TVertex> vertex1, IVertex<TVertex> vertex2);
        Edge<TEdge> ConnectVertices(uint vertex1Id, uint vertex2Id);
        GraphMergeInfo AddGraph(IGraph<TVertex, TEdge> otherGraph);
    }
}