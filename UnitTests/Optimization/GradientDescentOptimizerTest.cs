using System;
using System.Linq;
using Commons.Optimization;
using NUnit.Framework;

namespace CommonsTest.Optimization
{
    [TestFixture]
    public class GradientDescentOptimizerTest
    {
        [Test]
        public void GradientDescentOptimizerReturnsValueCloseToOptimumOneParameter()
        {
            var expected = 42.0;
            var parameterSettings = new[]
            {
                new ParameterSetting(0, 50, 1, 30)
            };

            Func<double[], double> costFunc = parameters => Math.Abs(expected - parameters[0]);
            var actual = GradientDescentOptimizer.Optimize(costFunc, parameterSettings, parameterSettings[0].StepSize);

            Assert.That(actual.Parameters[0], Is.EqualTo(expected).Within(parameterSettings[0].StepSize));
        }

        [Test]
        public void GradientDescentOptimizerReturnsValueCloseToOptimumThreeParameters()
        {
            var expected = new double[] { 1, 13, -4 };
            var parameterSettings = new[]
            {
                new ParameterSetting(0, 10, 0.1, Double.NaN),
                new ParameterSetting(5, 20, 0.5, 19),
                new ParameterSetting(-10, 10, 0.2, 1.1)
            };

            Func<double[], double> costFunc = parameters => Math.Abs(expected[0] - parameters[0]) + Math.Abs(expected[1] - parameters[1]) + Math.Abs(expected[2] - parameters[2]);
            var actual = GradientDescentOptimizer.Optimize(costFunc, parameterSettings, parameterSettings.Sum(p => p.StepSize));

            for (int p = 0; p < expected.Length; p++)
            {
                Assert.That(actual.Parameters[p], Is.EqualTo(expected[p]).Within(parameterSettings[p].StepSize));
            }
        }

        [Test]
        public void GradientDescentOptimizerHandlesConstantCostFunction()
        {
            var parameterSettings = new[]
            {
                new ParameterSetting(0, 50, 1, 30)
            };

            Func<double[], double> costFunc = parameters => 0;
            OptimizationResult actual = null;
            Assert.That(() => actual = GradientDescentOptimizer.Optimize(costFunc, parameterSettings, parameterSettings[0].StepSize), Throws.Nothing);
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Parameters[0], Is.Not.NaN);
        }
    }
}
