namespace Commons.Mathematics
{
    public interface IEdge
    {
        ulong Id { get; }
        uint Vertex1Id { get; }
        uint Vertex2Id { get; }
        bool IsDirected { get; }
        double Weight { get; set; }

        /// <summary>
        /// Property used for efficient implementations of graph algorithms.
        /// E.g. could hold a flag if this vertex has already been visited
        /// </summary>
        object AlgorithmData { get; set; }

        bool HasVertex(uint vertexId);
    }

    public interface IEdge<out TEdge> : IEdge
    {
        TEdge Object { get; }
    }
}