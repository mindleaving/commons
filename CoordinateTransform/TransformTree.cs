using System;
using System.Linq;

namespace Commons.CoordinateTransform
{
    public class TransformTree
    {
        private readonly Graph transformTree = new Graph();

        public TransformTree(string originName, Type originType)
        {
            var originVertex = new Vertex(transformTree.GetUnusedVertexId())
            {
                Object = new TransformTreeNode(originName, originType)
            };
            transformTree.AddVertex(originVertex);
        }

        public void AddCoordinateTransform<TIn, TOut>(
            string originCoordinateSystemName,
            string newCoordinateSystemName,
            ICoordinateTransform<TIn, TOut> transform)
        {
            Vertex originVertex = VertexFromCoordinateSystemName(originCoordinateSystemName);
            if (originVertex == null)
                throw new ArgumentException($"No coordinate system with name '{originCoordinateSystemName}' exists in {nameof(TransformTree)}");
            var originType = ((TransformTreeNode)originVertex.Object).PointType;
            if (typeof(TIn) != originType)
                throw new ArgumentException("Point type of origin coordinate system doesn't correspond input type of coordinate transform");

            var newVertex = VertexFromCoordinateSystemName(newCoordinateSystemName);
            if (newVertex != null)
            {
                if (((TransformTreeNode)newVertex.Object).PointType != typeof(TOut))
                    throw new ArgumentException("Point type of target coordinate system doesn't correspond output type of coordinate transform");
                ValidateConsistencyOfNewTransform(originVertex, newVertex, transform);
            }

            if (newVertex == null)
            {
                var newTransformNode = new TransformTreeNode(newCoordinateSystemName, typeof(TOut));
                newVertex = new Vertex(transformTree.GetUnusedVertexId())
                {
                    Object = newTransformNode
                };
                transformTree.AddVertex(newVertex);
            }
            var connectingEdge = new Edge(transformTree.GetUnusedEdgeId(),
                originVertex.Id,
                newVertex.Id)
            {
                Object = new TransformTreeEdge<TIn, TOut> { Transform = transform }
            };
            transformTree.AddEdge(connectingEdge);
        }

        private Vertex VertexFromCoordinateSystemName(string originCoordinateSystemName)
        {
            return transformTree.Vertices.Values
                .SingleOrDefault(v => ((TransformTreeNode)v.Object).CoordinateName == originCoordinateSystemName);
        }

        private void ValidateConsistencyOfNewTransform<TIn, TOut>(Vertex originVertex, 
            Vertex newVertex, 
            ICoordinateTransform<TIn, TOut> transform)
        {
            var pathsFromOrigin = GraphAlgorithms.ShortestPaths(transformTree, originVertex);
            if(double.IsPositiveInfinity(pathsFromOrigin.PathLengthTo(newVertex)))
                return; // No path between nodes, so they cannot have conflicting transforms

            // TODO: Generate random points and check that new transform is consistent with existing transform path
        }

        public TOut Transform<TIn, TOut>(TIn point,
            string inputCoordinateSystemName,
            string outputCoordinateSystemName)
        {
            var transform = GetTransform<TIn, TOut>(inputCoordinateSystemName, outputCoordinateSystemName);
            return transform.Transform(point);
        }

        public ICoordinateTransform<TIn, TOut> GetTransform<TIn, TOut>(
            string inputCoordinateSystemName,
            string outputCoordinateSystemName)
        {
            var inputVertex = VertexFromCoordinateSystemName(inputCoordinateSystemName);
            if (inputVertex == null)
                throw new ArgumentException($"Unknown input coordinate system '{inputCoordinateSystemName}'");
            var inputNode = (TransformTreeNode)inputVertex.Object;
            if (typeof(TIn) != inputNode.PointType)
            {
                throw new ArgumentException("Expected input type of transform doesn't match input coordinate system. " +
                                            $"Expected '{typeof(TIn)}' but input coordinate system type is '{inputNode.PointType}'");
            }
            var outputVertex = VertexFromCoordinateSystemName(outputCoordinateSystemName);
            if (outputVertex == null)
                throw new ArgumentException($"Unknown output coordinate system '{outputCoordinateSystemName}'");
            var outputNode = (TransformTreeNode)outputVertex.Object;
            if (typeof(TOut) != outputNode.PointType)
            {
                throw new ArgumentException("Expected output type of transform doesn't match output coordinate system. " +
                                            $"Expected '{typeof(TIn)}' but output coordinate system type is '{outputNode.PointType}'");
            }
            var pathsFromInput = GraphAlgorithms.ShortestPaths(transformTree, inputVertex);
            if (pathsFromInput.PathLengthTo(outputVertex).IsPositiveInfinity())
                throw new InvalidOperationException("No transform path between input and output coordinate system");
            var pathToOutputCoordinateSystem = pathsFromInput.PathTo(outputVertex);
            var transforms = pathToOutputCoordinateSystem.Path
                .Select(edge => ((TransformTreeEdge<object, object>)edge.Object).Transform)
                .ToArray();
            var transformChain = new TransformChain<TIn, TOut>(transforms);
            return transformChain;
        }
    }
}