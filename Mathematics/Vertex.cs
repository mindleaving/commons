using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Commons.Mathematics
{
    [DataContract]
    [KnownType(typeof(GeoCoordinate))]
    public class Vertex<T> : IDisposable
    {
        /// <summary>
        /// Property for holding an object which is represented by this vertex.
        /// Intended to make it possible to model problems as graphs.
        /// </summary>
        [DataMember]
        public T Object { get; set; }

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

        internal void AddEdge(ulong edgeId)
        {
            EdgeIds.Add(edgeId);
        }

        internal bool RemoveEdge(ulong edgeId)
        {
            return EdgeIds.Remove(edgeId);
        }

        public static bool operator ==(Vertex<T> vertex1, Vertex<T> vertex2)
        {
            if (ReferenceEquals(vertex1, vertex2))
                return true;
            if (ReferenceEquals(vertex1, null) || ReferenceEquals(vertex2, null))
                return false;
            return vertex1.Id == vertex2.Id;
        }

        public static bool operator !=(Vertex<T> vertex1, Vertex<T> vertex2)
        {
            return !(vertex1 == vertex2);
        }

        private bool Equals(Vertex<T> other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vertex<T>)obj);
        }

        public override int GetHashCode()
        {
            return (int)Id;
        }

        public override string ToString()
        {
            return $"V{Id}, #Edges: {EdgeIds.Count}";
        }

        public Vertex<T> Clone()
        {
            return new Vertex<T>(Id, Weight)
            {
                EdgeIds = EdgeIds.ToList(),
                Object = Object
            };
        }

        public void Dispose()
        {
            if(Object is IDisposable disposableObject)
                disposableObject.Dispose();
        }
    }
}