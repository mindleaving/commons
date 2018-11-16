using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Mathematics;

namespace Commons.Extensions
{
    public static class FittingExtensions
    {
        public static LinearFittingResult2D FitLine(this IList<Point2D> points)
        {
            var polynomialFittingResult = FitPolynomial(points, 1); 
            return new LinearFittingResult2D(
                polynomialFittingResult.Beta[0], 
                polynomialFittingResult.Beta[1], 
                polynomialFittingResult.MeanSquareError);
        }

        public static PolynomialFittingResult2D FitPolynomial(this IList<Point2D> points, int order)
        {
            var effectiveOrder = Math.Min(order, points.Count-1);
            var X = new double[points.Count, effectiveOrder+1];
            var y = new double[points.Count];
            for (int pointIndex = 0; pointIndex < points.Count; pointIndex++)
            {
                var point = points[pointIndex];
                for (int exponent = 0; exponent <= effectiveOrder; exponent++)
                {
                    X[pointIndex, exponent] = point.X.IntegerPower(exponent);
                }
                y[pointIndex] = point.Y;
            }

            var beta = (X.Transpose().Multiply(X)).Inverse()
                .Multiply(X.Transpose().Multiply(y));
            var fullBeta = beta.Concat(Enumerable.Repeat(0.0, order - effectiveOrder)).ToArray();
            var mse = X.Multiply(beta).Subtract(y).Average(x => x * x);
            return new PolynomialFittingResult2D(fullBeta, mse);
        }
    }
}
