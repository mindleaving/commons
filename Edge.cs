using System.Runtime.Serialization;

namespace Commons
{
    [DataContract]
    [KnownType(typeof(TaxiEdge))]
    public class Edge
    {
        [DataMember]
        public ulong Id { get; private set; }
        /// <summary>
        /// Property for holding an object which is represented by this edge.
        /// Intended to make it possible to model problems as graphs.
        /// </summary>
        [DataMember]
        public object Object { get; set; }

        [DataMember]
        public uint Vertex1Id { get; private set; }
        [DataMember]
        public uint Vertex2Id { get; private set; }
        [DataMember]
        public bool IsDirected { get; set; }
        [DataMember]
        public double Weight { get; set; }

        /// <summary>
        /// Property used for efficient implementations of graph algorithms.
        /// E.g. could hold a flag if this vertex has already been visited
        /// </summary>
        [IgnoreDataMember]
        public object AlgorithmData { get; set; }

        public Edge(ulong id, uint vertex1Id, uint vertex2Id, double weight = 1.0, bool isDirected = false)
        {
            Id = id;
            Vertex1Id = vertex1Id;
            Vertex2Id = vertex2Id;
            Weight = weight;
            IsDirected = isDirected;
            Object = null;
        }

        public bool HasVertex(Vertex vertex)
        {
            return HasVertex(vertex.Id);
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
    }
}