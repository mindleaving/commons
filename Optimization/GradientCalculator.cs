using System;
using System.Linq;

namespace Commons.Optimization
{
    public static class GradientCalculator
    {
        public static double[] Gradient(Func<double[], double> func, double[] x, double[] delta = null)
        {
            if (delta == null)
                delta = Enumerable.Repeat(1.0, x.Length).ToArray();

            var fx = func(x);

            var parameterCount = x.Length;
            var gradient = new double[parameterCount];
            for (int parameterIdx = 0; parameterIdx < parameterCount; parameterIdx++)
            {
                var parameterIdx0 = parameterIdx;
                var xiPlusDelta = delta.Select((d, idx) => idx == parameterIdx0 ? x[idx] + d : x[idx]).ToArray();
                var fxPlusDelta = func(xiPlusDelta);
                var xiMinusDelta = delta.Select((d, idx) => idx == parameterIdx0 ? x[idx] - d : x[idx]).ToArray();
                var fxMinusDelta = func(xiMinusDelta);

                if ((fxPlusDelta > fx && fxMinusDelta > fx)
                    || (fxPlusDelta < fx && fxMinusDelta < fx))
                {
                    gradient[parameterIdx] = 0;
                }
                else
                {
                    gradient[parameterIdx] = (fxPlusDelta - fxMinusDelta) / 2.0*delta[parameterIdx];
                }
            }
            return gradient;
        }
    }
}
