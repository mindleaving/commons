using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Commons
{
    [DataContract]
    public class Graph
    {
        [IgnoreDataMember]
        private ulong nextEdgeId;
        public ulong GetUnusedEdgeId()
        {
            return nextEdgeId++;
        }

        public uint GetUnusedVertexId()
        {
            if (!Vertices.Any())
                return 0;
            return Vertices.Max(v => v.Key) + 1;
        }

        [DataMember]
        public Dictionary<ulong, Edge> Edges { get; private set; }
        [DataMember]
        public Dictionary<uint, Vertex> Vertices { get; private set; }

        public Graph()
        {
            Edges = new Dictionary<ulong, Edge>();
            Vertices = new Dictionary<uint, Vertex>();
        }
        public Graph(IEnumerable<Vertex> vertices, IEnumerable<Edge> edges)
            : this()
        {
            AddVertices(vertices);
            AddEdges(edges);
        }

        public void AddVertex(Vertex newVertex)
        {
            if (Vertices.ContainsKey(newVertex.Id))
                throw new ArgumentException($"Vertex with ID '{newVertex.Id}' already exists.");

            Vertices.Add(newVertex.Id, newVertex);
        }

        public void AddVertices(IEnumerable<Vertex> newVertices)
        {
            newVertices.ForEach(AddVertex);
        }

        public bool RemoveVertex(Vertex vertex)
        {
            vertex.EdgeIds.ForEach(e => Edges.Remove(e));
            return Vertices.Remove(vertex.Id);
        }

        public bool RemoveVertex(uint vertexId)
        {
            if (Vertices.ContainsKey(vertexId))
                return false;
            var vertex = Vertices[vertexId];
            return RemoveVertex(vertex);
        }

        public void AddEdge(Edge newEdge)
        {
            if (!Vertices.ContainsKey(newEdge.Vertex1Id) || !Vertices.ContainsKey(newEdge.Vertex2Id))
                throw new Exception("Cannot add edge because one or both of its vertices are not in the graph");

            // Add edge to vertices
            Vertices[newEdge.Vertex1Id].EdgeIds.Add(newEdge.Id);
            Vertices[newEdge.Vertex2Id].EdgeIds.Add(newEdge.Id);

            Edges.Add(newEdge.Id, newEdge);
            if (newEdge.Id >= nextEdgeId)
                nextEdgeId = newEdge.Id + 1;
        }

        public void AddEdges(IEnumerable<Edge> edges)
        {
            edges.ForEach(AddEdge);
        }

        public bool RemoveEdge(Edge edge)
        {
            if (!Edges.ContainsKey(edge.Id))
                return false;

            Vertices[edge.Vertex1Id].RemoveEdge(edge);
            Vertices[edge.Vertex2Id].RemoveEdge(edge);

            return Edges.Remove(edge.Id);
        }

        public Edge ConnectVertices(Vertex vertex1, Vertex vertex2)
        {
            if (!Vertices.ContainsKey(vertex1.Id) || !Vertices.ContainsKey(vertex2.Id))
                throw new Exception("Cannot add edge because one or both of its vertices are not in the graph");

            var edge = new Edge(GetUnusedEdgeId(), vertex1.Id, vertex2.Id);
            AddEdge(edge);
            return edge;
        }

        public Edge ConnectVertices(uint vertex1Id, uint vertex2Id)
        {
            var vertex1 = Vertices[vertex1Id];
            var vertex2 = Vertices[vertex2Id];

            return ConnectVertices(vertex1, vertex2);
        }

        public GraphMergeInfo AddGraph(Graph otherGraph)
        {
            var vertexIdMap = new Dictionary<uint, uint>();
            foreach (var vertex in otherGraph.Vertices.Values)
            {
                var newVertexId = GetUnusedVertexId();
                AddVertex(new Vertex(newVertexId, vertex.Weight)
                {
                    Object = vertex.Object
                });
                vertexIdMap.Add(vertex.Id, newVertexId);
            }
            var edgeIdMap = new Dictionary<ulong, ulong>();
            foreach (var edge in otherGraph.Edges.Values)
            {
                var newEdgeId = GetUnusedEdgeId();
                AddEdge(new Edge(newEdgeId,
                    vertexIdMap[edge.Vertex1Id],
                    vertexIdMap[edge.Vertex2Id],
                    edge.Weight,
                    edge.IsDirected)
                {
                    Object = edge.Object
                });
                edgeIdMap.Add(edge.Id, newEdgeId);
            }
            return new GraphMergeInfo(vertexIdMap, edgeIdMap);
        }
    }
}