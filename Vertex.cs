using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Commons
{
    [DataContract]
    [KnownType(typeof(GeoCoordinate))]
    public class Vertex
    {
        /// <summary>
        /// Property for holding an object which is represented by this vertex.
        /// Intended to make it possible to model problems as graphs.
        /// </summary>
        [DataMember]
        public object Object { get; set; }

        [DataMember]
        public uint Id { get; private set; }
        [DataMember]
        public List<ulong> EdgeIds { get; private set; } = new List<ulong>();
        [DataMember]
        public double Weight { get; set; }

        /// <summary>
        /// Property used for efficient implementations of graph algorithms.
        /// E.g. could hold a flag if this vertex has already been visited
        /// </summary>
        [IgnoreDataMember]
        public object AlgorithmData { get; set; }

        public Vertex(uint id, double weight = 1.0)
        {
            Id = id;
            Weight = weight;
        }

        internal void AddEdge(Edge edge)
        {
            if (edge.Vertex1Id != Id || edge.Vertex2Id != Id)
                throw new ArgumentException("Cannot add edge to vertex if vertex is not an endpoint");

            EdgeIds.Add(edge.Id);
        }

        internal bool RemoveEdge(Edge edge)
        {
            return EdgeIds.Remove(edge.Id);
        }

        public override string ToString()
        {
            return $"V{Id}, #Edges: {EdgeIds.Count}";
        }
    }
}