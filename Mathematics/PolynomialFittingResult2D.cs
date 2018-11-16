using Commons.Extensions;

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

        public double Apply(double x)
        {
            var sum = Beta[0];
            for (int exp = 1; exp < Beta.Length; exp++)
            {
                sum += Beta[exp] * x.IntegerPower(exp);
            }

            return sum;
        }
    }
}
