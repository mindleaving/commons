using System.Collections.Generic;

namespace Commons.Mathematics
{
    public interface IVertex
    {
        uint Id { get; }
        List<ulong> EdgeIds { get; }
        double Weight { get; set; }

        /// <summary>
        /// Property used for efficient implementations of graph algorithms.
        /// E.g. could hold a flag if this vertex has already been visited
        /// </summary>
        object AlgorithmData { get; set; }

        void AddEdge(ulong edgeId);
        bool RemoveEdge(ulong edgeId);
    }

    public interface IVertex<TVertex> : IVertex
    {
        TVertex Object { get; set; }
    }
}