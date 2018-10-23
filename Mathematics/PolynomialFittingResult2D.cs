namespace Commons.Mathematics
{
    public class PolynomialFittingResult2D
    {
        public PolynomialFittingResult2D(double[] beta, double meanSquareError)
        {
            Beta = beta;
            MeanSquareError = meanSquareError;
        }

        public double[] Beta { get; }
        public double MeanSquareError { get; }
    }
}
