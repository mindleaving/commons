namespace Commons.Optimization
{
    public class OptimizationResult
    {
        public double Cost { get; private set; }
        public int Iterations { get; private set; }
        public double[] Parameters { get; private set; }

        public OptimizationResult(double[] optimalParameters, double cost, int iterations)
        {
            Parameters = optimalParameters;
            Cost = cost;
            Iterations = iterations;
        }

        public override string ToString()
        {
            return $"Cost: {Cost}";
        }
    }
}
