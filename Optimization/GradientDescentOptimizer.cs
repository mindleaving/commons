using System;
using System.Linq;

namespace Commons.Optimization
{
    public static class GradientDescentOptimizer
    {
        /// <summary>
        /// This is a modified version of the gradient descent optimization algorithm, where
        /// the gradient, after being normalized the largest gradient-component to 1,
        /// is multiplied by the parameter step sizes to obtain the updates.
        /// Also the algorithm will use a simulated annealing like approach for getting out of
        /// local minima by assigning random values to randomly selected parameters if a local optima
        /// is encountered, if the cost is above the accepted cost.
        /// </summary>
        /// <param name="costFunc">Cost function to be minimized</param>
        /// <param name="parameterSetting">Parameter settings, containing start value, lower and upper limits and stepsize</param>
        /// <param name="acceptedCost">Return if cost is below this threshold</param>
        /// <param name="randomizingRounds">Maximum number of rounds where parameters are randomized when encountering a local optima</param>
        /// <param name="maxIterations">Maximum iterations. Negative values will disable limit</param>
        /// <returns>Optimized parameters</returns>
        public static OptimizationResult Optimize(Func<double[], double> costFunc, ParameterSetting[] parameterSetting,
            double acceptedCost, int randomizingRounds = 5, int maxIterations = -1)
        {
            var rng = new Random(0);

            var iteration = 0;
            var randomizingRound = 0;
            var parameterCount = parameterSetting.Length;
            var stepSizes = parameterSetting.Select(p => p.StepSize).ToArray();

            var overallOptimalValues = parameterSetting.Select(p => p.Start).ToArray();

            var currentValues = (double[])overallOptimalValues.Clone();
            var roundBestCost = costFunc(currentValues);
            var overallBestCost = roundBestCost;
            while (iteration != maxIterations)
            {
                iteration++;

                // Calculate local gradient
                var gradient = GradientCalculator.Gradient(costFunc, currentValues, stepSizes);
                var largestAbsoluteGradient = gradient.Max(x => Math.Abs(x));

                // Normalize gradient, such that the largest absolute gradient value is +/-1
                var normalizedGradient = gradient.Select(x => x/largestAbsoluteGradient).ToList();

                for (int parameterIdx = 0; parameterIdx < parameterCount; parameterIdx++)
                {
                    currentValues[parameterIdx] -= normalizedGradient[parameterIdx]*stepSizes[parameterIdx];
                    if (currentValues[parameterIdx] < parameterSetting[parameterIdx].Lower)
                        currentValues[parameterIdx] = parameterSetting[parameterIdx].Lower;
                    if (currentValues[parameterIdx] > parameterSetting[parameterIdx].Upper)
                        currentValues[parameterIdx] = parameterSetting[parameterIdx].Upper;
                }
                var cost = costFunc(currentValues);
                if (cost < roundBestCost)
                {
                    roundBestCost = cost;
                    if (cost < overallBestCost)
                    {
                        overallBestCost = cost;
                        currentValues.CopyTo(overallOptimalValues,0);
                    }
                }
                else
                {
                    overallOptimalValues.CopyTo(currentValues,0);
                    if(overallBestCost <= acceptedCost || randomizingRound >= randomizingRounds)
                        break;
                    
                    // Randomize a random parameter (inside lower-upper range)
                    randomizingRound++;
                    roundBestCost = double.PositiveInfinity;
                    var nonZeroStepSizeParameterIndices = Enumerable.Range(0, parameterCount)
                        .Where(idx => parameterSetting[idx].StepSize > 0).ToList();
                    var randomizingParameterIdx = nonZeroStepSizeParameterIndices[rng.Next(nonZeroStepSizeParameterIndices.Count)];
                    var randomRatio = rng.NextDouble();
                    currentValues[randomizingParameterIdx] = randomRatio*parameterSetting[randomizingParameterIdx].Lower
                                                             + (1 - randomRatio)*parameterSetting[randomizingParameterIdx].Upper;
                }
            }
            return new OptimizationResult(overallOptimalValues,overallBestCost,iteration);
        }
    }
}
