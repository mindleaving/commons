namespace Commons.Mathematics
{
    public class LinearFittingResult2D : PolynomialFittingResult2D
    {
        public LinearFittingResult2D(double offset, double slope, double meanSquareError)
            : base(new Vector(offset, slope), meanSquareError)
        {
        }

        public double Offset => Beta[0];
        public double Slope => Beta[1];
    }
}
