namespace Commons.CoordinateTransform
{
    public class TransformTreeEdge<TIn, TOut>
    {
        public ICoordinateTransform<TIn, TOut> Transform { get; set; }
    }
}
