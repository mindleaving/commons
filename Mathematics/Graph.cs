using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;
using Newtonsoft.Json;

namespace Commons.Mathematics
{
    public class Graph<TVertex, TEdge> : IDisposable, IGraph<TVertex, TEdge>
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
                nextEdgeId = edges.Keys.Max() + 1;
            if(Vertices.Any())
                nextVertexId = vertices.Keys.Max() + 1;
            idsAreInitialized = true;
        }

        
        [JsonProperty]
        private Dictionary<ulong, IEdge<TEdge>> edges { get; set; }
        [JsonProperty]
        private Dictionary<uint, IVertex<TVertex>> vertices { get; set; }

        [JsonIgnore]
        public IEnumerable<IVertex<TVertex>> Vertices => vertices.Values;
        [JsonIgnore]
        IEnumerable<IVertex> IGraph.Vertices => Vertices;

        [JsonIgnore]
        public IEnumerable<IEdge<TEdge>> Edges => edges.Values;
        [JsonIgnore]
        IEnumerable<IEdge> IGraph.Edges => Edges;

        [JsonConstructor]
        private Graph(Dictionary<ulong, IEdge<TEdge>> edges, Dictionary<uint, IVertex<TVertex>> vertices)
        {
            this.edges = edges;
            this.vertices = vertices;
        }
        public Graph()
        {
            edges = new Dictionary<ulong, IEdge<TEdge>>();
            vertices = new Dictionary<uint, IVertex<TVertex>>();
        }
        public Graph(IEnumerable<IVertex<TVertex>> vertices, IEnumerable<IEdge<TEdge>> edges)
            : this()
        {
            AddVertices(vertices);
            AddEdges(edges);
        }

        public IVertex<TVertex> GetVertexFromId(uint id)
        {
            return vertices[id];
        }

        IEdge IGraph.GetEdgeById(ulong id)
        {
            return GetEdgeById(id);
        }

        IVertex IGraph.GetVertexFromId(uint id)
        {
            return GetVertexFromId(id);
        }

        public IEdge<TEdge> GetEdgeById(ulong id)
        {
            return edges[id];
        }

        public bool HasVertex(uint id)
        {
            return vertices.ContainsKey(id);
        }

        public bool HasEdge(ulong id)
        {
            return edges.ContainsKey(id);
        }

        public void AddVertex(IVertex newVertex)
        {
            if (HasVertex(newVertex.Id))
                throw new ArgumentException($"Vertex with ID '{newVertex.Id}' already exists.");

            vertices.Add(newVertex.Id, (IVertex<TVertex>)newVertex);
            if (newVertex.Id >= nextVertexId)
                nextVertexId = newVertex.Id + 1;
        }

        public void AddVertices(IEnumerable<IVertex> newVertices)
        {
            newVertices.ForEach(AddVertex);
        }

        public bool RemoveVertex(IVertex vertex)
        {
            vertex.EdgeIds.ForEach(e => edges.Remove(e));
            return vertices.Remove(vertex.Id);
        }

        public bool RemoveVertex(uint vertexId)
        {
            if (HasVertex(vertexId))
                return false;
            var vertex = GetVertexFromId(vertexId);
            return RemoveVertex(vertex);
        }

        public void AddEdge(IEdge newEdge)
        {
            if (!HasVertex(newEdge.Vertex1Id) || !HasVertex(newEdge.Vertex2Id))
                throw new Exception("Cannot add edge because one or both of its vertices are not in the graph");

            // Add edge to vertices
            vertices[newEdge.Vertex1Id].EdgeIds.Add(newEdge.Id);
            vertices[newEdge.Vertex2Id].EdgeIds.Add(newEdge.Id);

            edges.Add(newEdge.Id, (IEdge<TEdge>)newEdge);
            if (newEdge.Id >= nextEdgeId)
                nextEdgeId = newEdge.Id + 1;
        }

        public void AddEdges(IEnumerable<IEdge> edges)
        {
            edges.ForEach(AddEdge);
        }

        public bool RemoveEdge(IEdge edge)
        {
            if (!HasEdge(edge.Id))
                return false;

            GetVertexFromId(edge.Vertex1Id).RemoveEdge(edge.Id);
            GetVertexFromId(edge.Vertex2Id).RemoveEdge(edge.Id);

            return edges.Remove(edge.Id);
        }

        public IEdge ConnectVertices(IVertex vertex1, IVertex vertex2)
        {
            if (!HasVertex(vertex1.Id) || !HasVertex(vertex2.Id))
                throw new Exception("Cannot add edge because one or both of its vertices are not in the graph");

            var edge = new Edge<TEdge>(GetUnusedEdgeId(), vertex1.Id, vertex2.Id);
            AddEdge(edge);
            return edge;
        }

        public IEdge ConnectVertices(uint vertex1Id, uint vertex2Id)
        {
            var vertex1 = GetVertexFromId(vertex1Id);
            var vertex2 = GetVertexFromId(vertex2Id);

            return ConnectVertices(vertex1, vertex2);
        }

        public GraphMergeInfo AddGraph(IGraph otherGraphGeneric)
        {
            if(!(otherGraphGeneric is IGraph<TVertex, TEdge> otherGraph))
                throw new InvalidOperationException("Can only merge graphs of same type");

            var vertexIdMap = new Dictionary<uint, uint>();
            foreach (var vertex in otherGraph.Vertices)
            {
                var newVertexId = GetUnusedVertexId();
                AddVertex(new Vertex<TVertex>(newVertexId, vertex.Weight)
                {
                    Object = vertex.Object
                });
                vertexIdMap.Add(vertex.Id, newVertexId);
            }
            var edgeIdMap = new Dictionary<ulong, ulong>();
            foreach (var edge in otherGraph.Edges)
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
            Vertices.ForEach(v => RemoveVertex(v));
            Vertices.OfType<IDisposable>().ForEach(v => v.Dispose());
            Edges.OfType<IDisposable>().ForEach(e => e.Dispose());
        }
    }
}