using System;
using System.Linq;
using Commons.Mathematics;
using NUnit.Framework;

namespace Commons.UnitTests
{
    [TestFixture]
    public class GraphClassesTest
    {
        [Test]
        public void AddingVertexTwiceToGraphThrowsException()
        {
            var sut = new Graph<object, object>();
            var vertex = new Vertex<object>(0);

            Assume.That(() => sut.AddVertex(vertex), Throws.Nothing);
            Assert.Throws<ArgumentException>(() => sut.AddVertex(vertex));
        }

        [Test]
        public void AddingSeveralVerticesThrowsExceptionIfOneIsAlreadyInGraph()
        {
            var sut = new Graph<object,object>();
            var vertex0 = new Vertex<object>(0);
            Assume.That(() => sut.AddVertex(vertex0), Throws.Nothing);

            var moreVertices = new[] { new Vertex<object>(1), new Vertex<object>(3), new Vertex<object>(0) };
            Assert.Throws<ArgumentException>(() => sut.AddVertices(moreVertices));
        }

        [Test]
        public void VerticesCanBeRemoved()
        {
            var sut = new Graph<object,object>();
            var vertex0 = new Vertex<object>(0);
            sut.AddVertex(vertex0);

            Assume.That(sut.Vertices.ContainsKey(vertex0.Id));

            sut.RemoveVertex(vertex0);
            Assert.That(sut.Vertices.ContainsKey(vertex0.Id), Is.False);
        }

        [Test]
        public void CreatingEdgeMustNotAddThisEdgeToVertices()
        {
            // It's the responsibility of the 'Graph' class to add and remove
            // edge references to vertices. Creating a new edge must therefore
            // not add a reference to the corresponding vertices

            var vertex1 = new Vertex<object>(1);
            var vertex2 = new Vertex<object>(2);
            Assume.That(vertex1.EdgeIds, Is.Empty);
            Assume.That(vertex2.EdgeIds, Is.Empty);

            var edge = new Edge<object>(0, vertex1.Id, vertex2.Id);

            Assert.That(vertex1.EdgeIds, Is.Empty);
            Assert.That(vertex2.EdgeIds, Is.Empty);
        }

        [Test]
        public void RemovingVertexAlsoRemovesAdjacentEdges()
        {
            var vertices = new[] { new Vertex<object>(0), new Vertex<object>(1), new Vertex<object>(2) };
            var edges = new[]
            {
                new Edge<object>(0, vertices[0].Id, vertices[1].Id),
                new Edge<object>(1, vertices[1].Id, vertices[2].Id),
                new Edge<object>(2, vertices[2].Id, vertices[0].Id)
            };
            var sut = new Graph<object,object>(vertices, edges);

            sut.RemoveVertex(vertices[0]);

            CollectionAssert.AreEquivalent(vertices.Skip(1), sut.Vertices.Values);
            CollectionAssert.AreEquivalent(new[] { edges[1] }, sut.Edges.Values);
        }

        [Test]
        public void CallingGraphConstructorWithEdgesAndVerticesBuildsGraph()
        {
            var vertices = new[]
            {
                new Vertex<object>(0),
                new Vertex<object>(1)
            };
            var edges = new[]
            {
                new Edge<object>(0, vertices[0].Id, vertices[1].Id)
            };
            var sut = new Graph<object,object>(vertices, edges);

            // Check that the data was stored
            Assert.That(sut.Vertices.Values, Is.EquivalentTo(vertices));
            Assert.That(sut.Edges.Values, Is.EquivalentTo(edges));

            // Check that the references were stored
            Assert.That(sut.Vertices[0].EdgeIds.Contains(edges[0].Id), "Expected that vertex 0 would receive references to adjacent edges");
            Assert.That(sut.Vertices[1].EdgeIds.Contains(edges[0].Id), "Expected that vertex 1 would receive references to adjacent edges");
        }

        [Test]
        public void GraphConstructorThrowsExceptionIfEdgesContainUnknownVertices()
        {
            // While the methods 'AddEdge' throws an exception when trying to add
            // an edge with vertices not in the graph, the graph constructor should
            // accept unknown vertices in edges and add these to the graph as well

            var vertices = new[]
            {
                new Vertex<object>(0),
                new Vertex<object>(1)
            };
            var edges = new[]
            {
                new Edge<object>(0, vertices[0].Id, vertices[1].Id),
                new Edge<object>(1, vertices[1].Id, 2) // Unknown vertex
            };

            Graph<object,object> sut = null;
            Assert.That(() => sut = new Graph<object,object>(vertices, edges), Throws.Exception);
        }
    }
}