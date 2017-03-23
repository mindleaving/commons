namespace Commons.CoordinateTransform
{
    public interface ICoordinateTransform<TIn, TOut>
    {
        TOut Transform(TIn point);
        TIn InverseTransform(TOut point);
    }
}