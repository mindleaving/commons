using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.Mathematics
{
    [TestFixture]
    public class GraphAlgorithmsTest
    {
        [Test]
        public void ComputeAdjacencyMatrixIsSymmetricForUndirectedGraph()
        {
            var graph = CreateTestGraph();
            Assume.That(!graph.Edges.Any(e => e.IsDirected));

            var sut = GraphAlgorithms.ComputeAdjacencyMatrix(graph);

            // Check the size
            var vertexCount = graph.Vertices.Count();
            Assert.That(sut.GetLength(0), Is.EqualTo(vertexCount));
            Assert.That(sut.GetLength(1), Is.EqualTo(vertexCount));

            // Check for symmetry
            for (int i = 0; i < vertexCount; i++)
            {
                for (int j = i + 1; j < vertexCount; j++)
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
            graph.Edges.First().Weight = -1;

            Assert.Throws<NotImplementedException>(() => GraphAlgorithms.ShortestPaths(graph, graph.GetVertexFromId(0)));
        }

        [Test]
        public void ShortestPathFinishesWithinTimespan()
        {
            // This test is testing for possible deadlocks or inefficiencies
            // in the shortest path algorithm.

            var graph = CreateTestGraph();
            var timeout = TimeSpan.FromMilliseconds(3000);

            ShortestPathLookup distanceDictionary = null;
            var thread = new Thread(() =>
            {
                distanceDictionary = GraphAlgorithms.ShortestPaths(graph, graph.GetVertexFromId(0));
            });
            thread.Start();
            thread.Join(timeout);

            Assert.That(distanceDictionary, Is.Not.Null);
        }

        [Test]
        public void ShortestPathDoesntThrowIfGraphNotConnected()
        {
            var graph = CreateTestGraph();
            var v1v2Edge = graph.Edges.Single(e => e.Vertex1Id == 0 && e.Vertex2Id == 1);
            graph.RemoveEdge(v1v2Edge);

            Assume.That(graph.GetVertexFromId(0).EdgeIds, Is.Empty);

            ShortestPathLookup shortestPathLookup = null;
            try
            {
                shortestPathLookup = GraphAlgorithms.ShortestPaths(graph, graph.GetVertexFromId(0));
            }
            catch (Exception)
            {
                Assert.Fail("GraphAlgorithm.ShortestPath should not throw exception for not connected test graph");
            }
            Assert.That(shortestPathLookup.PathLengthTo(graph.GetVertexFromId(1)), Is.EqualTo(double.PositiveInfinity));
        }

        [Test]
        public void ShortestPathFindsExpectedPathLengts()
        {
            var graph = CreateTestGraph();
            graph.Edges.Single(e => e.Vertex1Id == 1 && e.Vertex2Id == 3).Weight = 0.5;

            var shortestPathLookup = GraphAlgorithms.ShortestPaths(graph, graph.GetVertexFromId(0));

            Assert.That(shortestPathLookup.PathLengthTo(graph.GetVertexFromId(0)), Is.EqualTo(0));
            Assert.That(shortestPathLookup.PathLengthTo(graph.GetVertexFromId(1)), Is.EqualTo(1));
            Assert.That(shortestPathLookup.PathLengthTo(graph.GetVertexFromId(2)), Is.EqualTo(2));
            Assert.That(shortestPathLookup.PathLengthTo(graph.GetVertexFromId(3)), Is.EqualTo(1.5));
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
            var connectedVertices = GraphAlgorithms.GetConnectedSubgraph(graph, graph.Vertices.First()).Vertices;

            Assert.That(connectedVertices, Is.EquivalentTo(graph.Vertices));
        }

        [Test]
        public void GetConnectedVerticesReturnsOnlyConnectedVertices()
        {
            var graph = CreateTestGraph();
            var nonConnectedVertex = new Vertex<object>(99);
            graph.AddVertex(nonConnectedVertex);

            var connectedVertices = GraphAlgorithms.GetConnectedSubgraph(graph, graph.Vertices.First()).Vertices;

            Assert.That(connectedVertices, Is.EquivalentTo(graph.Vertices.Except(new[] { nonConnectedVertex })));
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

            Assert.That(subGraph.Vertices.Select(v => v.Id), Is.EquivalentTo(subgraphVertices));
            Assert.That(subGraph.Edges.Select(v => v.Id), Is.EquivalentTo(new ulong[] {3, 4, 5}));
            Assert.That(subGraph.GetVertexFromId(3).EdgeIds, Is.EquivalentTo(new ulong[] {3, 4}));
            Assert.That(subGraph.GetVertexFromId(4).EdgeIds, Is.EquivalentTo(new ulong[] {3, 5}));
            Assert.That(subGraph.GetVertexFromId(5).EdgeIds, Is.EquivalentTo(new ulong[] {4, 5}));
        }

        [Test]
        public void FindStrongConnectedComponentsFindsExpectedComponents()
        {
            // See https://upload.wikimedia.org/wikipedia/commons/6/60/Tarjan%27s_Algorithm_Animation.gif
            var graph = new Graph<object, object>(
                Enumerable.Range(1, 8).Select(vertexId => new Vertex<object>((uint)vertexId)),
                new []
                {
                    new Edge<object>(0, 1, 2, isDirected: true),
                    new Edge<object>(1, 2, 3, isDirected: true),
                    new Edge<object>(2, 3, 1, isDirected: true),
                    new Edge<object>(3, 4, 2, isDirected: true),
                    new Edge<object>(4, 4, 3, isDirected: true),
                    new Edge<object>(5, 4, 5, isDirected: true),
                    new Edge<object>(6, 5, 4, isDirected: true),
                    new Edge<object>(7, 5, 6, isDirected: true),
                    new Edge<object>(8, 6, 3, isDirected: true),
                    new Edge<object>(9, 6, 7, isDirected: true),
                    new Edge<object>(10, 7, 6, isDirected: true),
                    new Edge<object>(11, 8, 6, isDirected: true),
                    new Edge<object>(12, 8, 7, isDirected: true),
                    new Edge<object>(13, 8, 8, isDirected: true)
                });
            var expected = new List<IList<uint>>
            {
                new uint[]{ 1, 2, 3},
                new uint[]{ 4, 5 },
                new uint[]{ 6, 7 },
                new uint[]{ 8 }
            };
            var actual = GraphAlgorithms.FindStronglyConnectedComponents(graph);
            Assert.That(actual.Count, Is.EqualTo(expected.Count));
            foreach (var vertexIds in expected)
            {
                var referenceVertex = graph.GetVertexFromId(vertexIds[0]);
                var matchingActual = actual.Single(c => c.Contains(referenceVertex));
                CollectionAssert.AreEquivalent(vertexIds, matchingActual.Select(c => c.Id));
            }
        }

        [Test]
        public void HasCyclesReturnsTrueForTestGraph()
        {
            var graph = CreateTestGraph();
            Assert.That(GraphAlgorithms.HasCycles(graph), Is.True);
        }

        [Test]
        public void HasCyclesReturnsTrueForCircleGraph()
        {
            var graph = new Graph<object, object>(
                Enumerable.Range(1, 4).Select(vertexId => new Vertex<object>((uint) vertexId)),
                new []
                {
                    new Edge<object>(0, 1, 2, isDirected: true), 
                    new Edge<object>(1, 2, 3, isDirected: true), 
                    new Edge<object>(2, 3, 4, isDirected: true), 
                    new Edge<object>(3, 4, 1, isDirected: true), 
                });
            Assert.That(GraphAlgorithms.HasCycles(graph), Is.True);
        }

        [Test]
        public void HasCyclesReturnsFalseForDiamondGraph()
        {
            var graph = new Graph<object, object>(
                Enumerable.Range(1, 4).Select(vertexId => new Vertex<object>((uint) vertexId)),
                new []
                {
                    new Edge<object>(0, 1, 2, isDirected: true), 
                    new Edge<object>(1, 1, 3, isDirected: true), 
                    new Edge<object>(2, 2, 4, isDirected: true), 
                    new Edge<object>(3, 3, 4, isDirected: true), 
                });
            Assert.That(GraphAlgorithms.HasCycles(graph), Is.False);
        }

        [Test]
        public void HasCyclesThrowsExceptionForPartiallyUndirectedGraph()
        {
            var graph = new Graph<object, object>(
                Enumerable.Range(1, 4).Select(vertexId => new Vertex<object>((uint) vertexId)),
                new []
                {
                    new Edge<object>(0, 1, 2), 
                    new Edge<object>(1, 1, 3), 
                    new Edge<object>(2, 2, 4, isDirected: true), 
                    new Edge<object>(3, 3, 4, isDirected: true), 
                });
            Assert.That(() => GraphAlgorithms.HasCycles(graph), Throws.Exception);
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