using System;
using System.Linq;

namespace Commons.Optimization
{
    /// <summary>
    /// Minimizes cost function by cycling through parameters while keeping the others constant
    /// </summary>
    public static class ParameterCyclingOptimizer
    {
        public static OptimizationResult Optimize(Func<double[], double> costFunc, ParameterSetting[] parameterSetting, int maxIterations = -1)
        {
            var iteration = 0;
            var parameterCount = parameterSetting.Length;
            var currentParameter = 0; // Parameter index currently being optimized
            var polarity = 1; // 1: Increase parameter values, -1: Decrease parameter values
            var maxIterationsWithoutImprovement = 2*parameterCount;
            var timeSinceImprovement = 0;
            var stepSizeMultiplier = 1.0;

            var optimalValues = parameterSetting.Select(p => p.Start).ToArray();
            var currentValues = (double[])optimalValues.Clone();
            var lowestCost = costFunc(optimalValues);
            while (iteration != maxIterations)
            {
                iteration++;

                // Alter parameter if it stays within bounds
                var newValue = currentValues[currentParameter] + polarity * stepSizeMultiplier * parameterSetting[currentParameter].StepSize;
                var canUpdate = parameterSetting[currentParameter].StepSize != 0 &&
                                newValue >= parameterSetting[currentParameter].Lower &&
                                newValue <= parameterSetting[currentParameter].Upper;

                var newOptimaFound = false;
                if (canUpdate)
                {
                    currentValues[currentParameter] = newValue;
                    var currentCost = costFunc(currentValues);
                    if (currentCost < lowestCost)
                    {
                        lowestCost = currentCost;
                        currentValues.CopyTo(optimalValues, 0);
                        timeSinceImprovement = 0;
                        newOptimaFound = true;
                    }
                }
                if (!newOptimaFound)
                {
                    optimalValues.CopyTo(currentValues,0);
                    currentParameter++;
                    if (currentParameter == parameterCount)
                    {
                        currentParameter = 0;

                        // Change polarity
                        polarity *= -1;
                    }
                    timeSinceImprovement++;
                }
                if (timeSinceImprovement > maxIterationsWithoutImprovement)
                {
                    if(stepSizeMultiplier <= 0.25)
                        break;

                    // Search for optimal values in a larger range to overcome local optima
                    stepSizeMultiplier /= 2;
                    timeSinceImprovement = 0;
                }
                    
            }
            return new OptimizationResult(optimalValues, lowestCost, iteration);
        }
    }
}
