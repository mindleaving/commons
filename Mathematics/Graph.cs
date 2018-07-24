using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;
using Newtonsoft.Json;

namespace Commons.Mathematics
{
    public class Graph<TVertex, TEdge> : IDisposable
    {
        [JsonIgnore] 
        private bool idsAreInitialized;
        [JsonIgnore]
        private ulong nextEdgeId;
        public ulong GetUnusedEdgeId()
        {
            if (!idsAreInitialized)
                InitializeIds();
            return nextEdgeId++;
        }

        [JsonIgnore] 
        private uint nextVertexId;
        public uint GetUnusedVertexId()
        {
            if (!idsAreInitialized)
                InitializeIds();
            return nextVertexId++;
        }

        private void InitializeIds()
        {
            if(Edges.Any())
                nextEdgeId = Edges.Keys.Max() + 1;
            if(Vertices.Any())
                nextVertexId = Vertices.Keys.Max() + 1;
            idsAreInitialized = true;
        }

        public Dictionary<ulong, Edge<TEdge>> Edges { get; private set; }
        public Dictionary<uint, Vertex<TVertex>> Vertices { get; private set; }

        public Graph()
        {
            Edges = new Dictionary<ulong, Edge<TEdge>>();
            Vertices = new Dictionary<uint, Vertex<TVertex>>();
        }
        public Graph(IEnumerable<Vertex<TVertex>> vertices, IEnumerable<Edge<TEdge>> edges)
            : this()
        {
            AddVertices(vertices);
            AddEdges(edges);
        }

        public void AddVertex(Vertex<TVertex> newVertex)
        {
            if (Vertices.ContainsKey(newVertex.Id))
                throw new ArgumentException($"Vertex with ID '{newVertex.Id}' already exists.");

            Vertices.Add(newVertex.Id, newVertex);
            if (newVertex.Id >= nextVertexId)
                nextVertexId = newVertex.Id + 1;
        }

        public void AddVertices(IEnumerable<Vertex<TVertex>> newVertices)
        {
            newVertices.ForEach(AddVertex);
        }

        public bool RemoveVertex(Vertex<TVertex> vertex)
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

        public void AddEdge(Edge<TEdge> newEdge)
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

        public void AddEdges(IEnumerable<Edge<TEdge>> edges)
        {
            edges.ForEach(AddEdge);
        }

        public bool RemoveEdge(Edge<TEdge> edge)
        {
            if (!Edges.ContainsKey(edge.Id))
                return false;

            Vertices[edge.Vertex1Id].RemoveEdge(edge.Id);
            Vertices[edge.Vertex2Id].RemoveEdge(edge.Id);

            return Edges.Remove(edge.Id);
        }

        public Edge<TEdge> ConnectVertices(Vertex<TVertex> vertex1, Vertex<TVertex> vertex2)
        {
            if (!Vertices.ContainsKey(vertex1.Id) || !Vertices.ContainsKey(vertex2.Id))
                throw new Exception("Cannot add edge because one or both of its vertices are not in the graph");

            var edge = new Edge<TEdge>(GetUnusedEdgeId(), vertex1.Id, vertex2.Id);
            AddEdge(edge);
            return edge;
        }

        public Edge<TEdge> ConnectVertices(uint vertex1Id, uint vertex2Id)
        {
            var vertex1 = Vertices[vertex1Id];
            var vertex2 = Vertices[vertex2Id];

            return ConnectVertices(vertex1, vertex2);
        }

        public GraphMergeInfo AddGraph(Graph<TVertex, TEdge> otherGraph)
        {
            var vertexIdMap = new Dictionary<uint, uint>();
            foreach (var vertex in otherGraph.Vertices.Values)
            {
                var newVertexId = GetUnusedVertexId();
                AddVertex(new Vertex<TVertex>(newVertexId, vertex.Weight)
                {
                    Object = vertex.Object
                });
                vertexIdMap.Add(vertex.Id, newVertexId);
            }
            var edgeIdMap = new Dictionary<ulong, ulong>();
            foreach (var edge in otherGraph.Edges.Values)
            {
                var newEdgeId = GetUnusedEdgeId();
                AddEdge(new Edge<TEdge>(newEdgeId,
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

        public void Dispose()
        {
            var vertices = Vertices.Values.ToList();
            var edges = Edges.Values.ToList();
            vertices.ForEach(v => RemoveVertex(v));
            vertices.ForEach(v => v.Dispose());
            edges.ForEach(e => e.Dispose());
        }
    }
}