using System;

namespace Commons.Optimization
{
    public class ParameterSetting
    {
        public ParameterSetting(double lower, double upper, double stepSize, double start)
        {
            if (lower > upper) { throw new ArgumentException("lower > upper"); }
            if (stepSize < 0) { throw new ArgumentException("stepSize < 0"); }
            StepSize = stepSize;
            Lower = double.IsNaN(lower) ? Double.NegativeInfinity : lower;
            Upper = double.IsNaN(upper) ? Double.PositiveInfinity : upper;
            Start = double.IsNaN(start) ? Lower : start;
        }

        public ParameterSetting(string name, double lower, double upper, double stepSize, double start)
        {
            if (lower > upper) { throw new ArgumentException("lower > upper"); }
            if (stepSize < 0) { throw new ArgumentException("stepSize < 0"); }
            Name = name;
            StepSize = stepSize;
            Lower = double.IsNaN(lower) ? Double.NegativeInfinity : lower;
            Upper = double.IsNaN(upper) ? Double.PositiveInfinity : upper;
            Start = double.IsNaN(start) ? Lower : start;
        }

        public string Name { get; set; }

        public double Lower { get; private set; }
        public double Upper { get; private set; }

        public double Start { get; set; }
        public double StepSize { get; private set; }
    }
}