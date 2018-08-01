using System;
using Commons.Optimization;
using NUnit.Framework;

namespace CommonsTest.Optimization
{
    [TestFixture]
    public class ParameterCyclingOptimizerTest
    {
        [Test]
        public void ParameterCyclingOptimizerReturnsValueCloseToOptimumOneParameter()
        {
            var expected = 42.0;
            var parameterSettings = new ParameterSetting[]
            {
                new ParameterSetting(0, 50, 1, 30)
            };

            Func<double[],double> costFunc = parameters => Math.Abs(expected - parameters[0]);
            var actual = ParameterCyclingOptimizer.Optimize(costFunc, parameterSettings,100);
            
            Assert.That(actual.Parameters[0],Is.EqualTo(expected).Within(parameterSettings[0].StepSize));
        }

        [Test]
        public void ParameterCyclingOptimizerReturnsValueCloseToOptimumThreeParameters()
        {
            var expected = new double[] { 1, 13, -4 };
            var parameterSettings = new[]
            {
                new ParameterSetting(0, 10, 0.1, Double.NaN),
                new ParameterSetting(5, 20, 0.5, 19),
                new ParameterSetting(-10, 10, 0.2, 1.1)
            };

            Func<double[], double> costFunc = parameters => Math.Abs(expected[0] - parameters[0]) + Math.Abs(expected[1] - parameters[1]) + Math.Abs(expected[2] - parameters[2]);
            var actual = ParameterCyclingOptimizer.Optimize(costFunc, parameterSettings, 1000);

            for (int p = 0; p < expected.Length; p++)
            {
                Assert.That(actual.Parameters[p],Is.EqualTo(expected[p]).Within(parameterSettings[p].StepSize));
            }
        }
    }
}
