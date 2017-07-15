﻿using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Commons
{
    [DataContract]
    [KnownType(typeof(GeoCoordinate))]
    public class Vertex<T>
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

        public override string ToString()
        {
            return $"V{Id}, #Edges: {EdgeIds.Count}";
        }
    }
}