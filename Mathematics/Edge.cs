using System;
using Newtonsoft.Json;

namespace Commons.Mathematics
{
    public class Edge<T> : IDisposable, IEdge<T>
    {
        public ulong Id { get; private set; }
        /// <summary>
        /// Property for holding an object which is represented by this edge.
        /// Intended to make it possible to model problems as graphs.
        /// </summary>
        public T Object { get; set; }

        public uint Vertex1Id { get; private set; }
        public uint Vertex2Id { get; private set; }
        public bool IsDirected { get; set; }
        public double Weight { get; set; }

        /// <summary>
        /// Property used for efficient implementations of graph algorithms.
        /// E.g. could hold a flag if this vertex has already been visited
        /// </summary>
        [JsonIgnore]
        public object AlgorithmData { get; set; }

        public Edge(ulong id, uint vertex1Id, uint vertex2Id, double weight = 1.0, bool isDirected = false)
        {
            Id = id;
            Vertex1Id = vertex1Id;
            Vertex2Id = vertex2Id;
            Weight = weight;
            IsDirected = isDirected;
            Object = default(T);
        }

        public bool HasVertex(uint vertexId)
        {
            return Vertex1Id.Equals(vertexId) || Vertex2Id.Equals(vertexId);
        }

        public override string ToString()
        {
            var directednesSymbol = IsDirected ? "->" : "<->";
            return $"V{Vertex1Id} {directednesSymbol} V{Vertex2Id}, {Object}";
        }

        public void Dispose()
        {
            if(Object is IDisposable disposableObject)
                disposableObject.Dispose();
        }
    }
}