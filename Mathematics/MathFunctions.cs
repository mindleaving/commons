namespace Commons.Mathematics
{
    public static class MathFunctions
    {
        public static double Sigmoid(double x)
        {
            return 1.0/(1 + System.Math.Exp(-x));
        }
    }
}
