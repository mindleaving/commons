using System.Collections.Generic;

namespace Commons.Mathematics
{
    public interface IGraph<out TVertex, out TEdge> : IGraph
    {
        new IEnumerable<IVertex<TVertex>> Vertices { get; }
        new IEnumerable<IEdge<TEdge>> Edges { get; }
        new IVertex<TVertex> GetVertexFromId(uint id);
        new IEdge<TEdge> GetEdgeById(ulong id);
    }

    public interface IGraph
    {
        IEnumerable<IVertex> Vertices { get; }
        IEnumerable<IEdge> Edges { get; }
        IVertex GetVertexFromId(uint id);
        IEdge GetEdgeById(ulong id);

        bool HasVertex(uint id);
        bool HasEdge(ulong id);

        ulong GetUnusedEdgeId();
        uint GetUnusedVertexId();
        void AddVertex(IVertex newVertex);
        void AddVertices(IEnumerable<IVertex> newVertices);
        bool RemoveVertex(IVertex vertex);
        bool RemoveVertex(uint vertexId);
        void AddEdge(IEdge newEdge);
        void AddEdges(IEnumerable<IEdge> edges);
        bool RemoveEdge(IEdge edge);
        IEdge ConnectVertices(IVertex vertex1, IVertex vertex2);
        IEdge ConnectVertices(uint vertex1Id, uint vertex2Id);
        GraphMergeInfo AddGraph(IGraph otherGraph);
    }
}