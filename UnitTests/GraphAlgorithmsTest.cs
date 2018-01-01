using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Commons.Mathematics;
using NUnit.Framework;

namespace Commons.UnitTests
{
    [TestFixture]
    public class GraphAlgorithmsTest
    {
        [Test]
        public void ComputeAdjacencyMatrixIsSymmetricForUndirectedGraph()
        {
            var graph = CreateTestGraph();
            Assume.That(!graph.Edges.Values.Any(e => e.IsDirected));

            var sut = GraphAlgorithms.ComputeAdjacencyMatrix(graph);

            // Check the size
            Assert.That(sut.GetLength(0), Is.EqualTo(graph.Vertices.Count));
            Assert.That(sut.GetLength(1), Is.EqualTo(graph.Vertices.Count));

            // Check for symmetry
            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                for (int j = i + 1; j < graph.Vertices.Count; j++)
                {
                    Assert.That(sut[i, j], Is.EqualTo(sut[j, i]));
                }
            }
        }

        [Test]
        public void ShortestPathThrowsExceptionIfAnyEdgeHasNegativeWeight()
        {
            // Remove this test if an algorithm with support for
            // negative weights has been implemented.

            var graph = CreateTestGraph();
            graph.Edges.Values.First().Weight = -1;

            Assert.Throws<NotImplementedException>(() => GraphAlgorithms.ShortestPaths(graph, graph.Vertices[0]));
        }

        [Test]
        public void ShortestPathFinishesWithinTimespan()
        {
            // This test is testing for possible deadlocks or inefficiencies
            // in the shortest path algorithm.

            var graph = CreateTestGraph();
            var timeout = TimeSpan.FromMilliseconds(3000);

            ShortestPathLookup<object, object> distanceDictionary = null;
            var thread = new Thread(() =>
            {
                distanceDictionary = GraphAlgorithms.ShortestPaths(graph, graph.Vertices[0]);
            });
            thread.Start();
            thread.Join(timeout);

            Assert.That(distanceDictionary, Is.Not.Null);
        }

        [Test]
        public void ShortestPathDoesntThrowIfGraphNotConnected()
        {
            var graph = CreateTestGraph();
            var v1v2Edge = graph.Edges.Values.Single(e => e.Vertex1Id == 0 && e.Vertex2Id == 1);
            graph.RemoveEdge(v1v2Edge);

            Assume.That(graph.Vertices[0].EdgeIds, Is.Empty);

            ShortestPathLookup<object,object> shortestPathLookup = null;
            try
            {
                shortestPathLookup = GraphAlgorithms.ShortestPaths(graph, graph.Vertices[0]);
            }
            catch (Exception)
            {
                Assert.Fail("GraphAlgorithm.ShortestPath should not throw exception for not connected test graph");
            }
            Assert.That(shortestPathLookup.PathLengthTo(graph.Vertices[1]), Is.EqualTo(double.PositiveInfinity));
        }

        [Test]
        public void ShortestPathFindsExpectedPathLengts()
        {
            var graph = CreateTestGraph();
            graph.Edges.Values.Single(e => e.Vertex1Id == 1 && e.Vertex2Id == 3).Weight = 0.5;

            var shortestPathLookup = GraphAlgorithms.ShortestPaths(graph, graph.Vertices[0]);

            Assert.That(shortestPathLookup.PathLengthTo(graph.Vertices[0]), Is.EqualTo(0));
            Assert.That(shortestPathLookup.PathLengthTo(graph.Vertices[1]), Is.EqualTo(1));
            Assert.That(shortestPathLookup.PathLengthTo(graph.Vertices[2]), Is.EqualTo(2));
            Assert.That(shortestPathLookup.PathLengthTo(graph.Vertices[3]), Is.EqualTo(1.5));
        }

        [Test]
        public void ShortestPathFoundForGraphWithMultipleEdgesBetweenTwoVertices()
        {
            var vertices = new List<Vertex<object>>
            {
                new Vertex<object>(0),
                new Vertex<object>(1),
                new Vertex<object>(2),
                new Vertex<object>(3),
                new Vertex<object>(4),
            };
            var edges = new List<Edge<object>>
            {
                new Edge<object>(0, 0, 1),
                new Edge<object>(1, 1, 2),
                new Edge<object>(2, 1, 2), // Double edge
                new Edge<object>(3, 2, 3),
                new Edge<object>(4, 1, 4),
                new Edge<object>(5, 2, 4),
            };
            var graph = new Graph<object, object>(vertices, edges);
            Assert.That(() => GraphAlgorithms.ShortestPaths(graph, 0), Throws.Nothing);
            var shortestPathLookup = GraphAlgorithms.ShortestPaths(graph, 0);
            var pathToV3 = shortestPathLookup.PathTo(vertices.Single(v => v.Id == 3));
            Assert.That(pathToV3.PathLength, Is.EqualTo(3));
        }

        [Test]
        public void GetConnectedVerticesReturnsAllVertices()
        {
            var graph = CreateTestGraph();
            var connectedVertices = GraphAlgorithms.GetConnectedSubgraph(graph, graph.Vertices.First().Value).Vertices.Values;

            Assert.That(connectedVertices, Is.EquivalentTo(graph.Vertices.Values));
        }

        [Test]
        public void GetConnectedVerticesReturnsOnlyConnectedVertices()
        {
            var graph = CreateTestGraph();
            var nonConnectedVertex = new Vertex<object>(99);
            graph.AddVertex(nonConnectedVertex);

            var connectedVertices = GraphAlgorithms.GetConnectedSubgraph(graph, graph.Vertices.First().Value).Vertices.Values;

            Assert.That(connectedVertices, Is.EquivalentTo(graph.Vertices.Values.Except(new[] { nonConnectedVertex })));
        }

        [Test]
        public void SubgraphConstructedCorrectly()
        {
            var vertices = new List<Vertex<object>>
            {
                new Vertex<object>(1),
                new Vertex<object>(2),
                new Vertex<object>(3),
                new Vertex<object>(4),
                new Vertex<object>(5),
                new Vertex<object>(6),
                new Vertex<object>(7)
            };
            var edges = new List<Edge<object>>
            {
                new Edge<object>(1, 1, 3),
                new Edge<object>(2, 2, 3),
                new Edge<object>(3, 3, 4),
                new Edge<object>(4, 3, 5),
                new Edge<object>(5, 4, 5),
                new Edge<object>(6, 5, 6),
                new Edge<object>(7, 5, 7)
            };
            var testGraph = new Graph<object, object>(vertices, edges);
            var subgraphVertices = new uint[] {3, 4, 5};
            var subGraph = GraphAlgorithms.GetSubgraph(testGraph, subgraphVertices);

            Assert.That(subGraph.Vertices.Keys, Is.EquivalentTo(subgraphVertices));
            Assert.That(subGraph.Edges.Keys, Is.EquivalentTo(new ulong[] {3, 4, 5}));
            Assert.That(subGraph.Vertices[3].EdgeIds, Is.EquivalentTo(new ulong[] {3, 4}));
            Assert.That(subGraph.Vertices[4].EdgeIds, Is.EquivalentTo(new ulong[] {3, 5}));
            Assert.That(subGraph.Vertices[5].EdgeIds, Is.EquivalentTo(new ulong[] {4, 5}));
        }

        private Graph<object,object> CreateTestGraph()
        {
            var graph = new Graph<object, object>();
            var vertices = new[]
            {
                new Vertex<object>(0),
                new Vertex<object>(1),
                new Vertex<object>(2),
                new Vertex<object>(3)
            };
            var edges = new[]
            {
                new Edge<object>(graph.GetUnusedEdgeId(), vertices[0].Id,vertices[1].Id),
                new Edge<object>(graph.GetUnusedEdgeId(), vertices[1].Id,vertices[2].Id),
                new Edge<object>(graph.GetUnusedEdgeId(), vertices[1].Id,vertices[3].Id),
                new Edge<object>(graph.GetUnusedEdgeId(), vertices[2].Id,vertices[3].Id)
            };
            graph.AddVertices(vertices);
            graph.AddEdges(edges);
            return graph;
        }
    }
}