using System;

namespace Commons
{
    public static class StaticRandom
    {
        public static Random Rng { get; } = new Random();

        public static double NextGaussian(double mean, double std)
        {
            // Implementation from here:
            // https://stackoverflow.com/questions/218060/random-gaussian-variables/18460552
            var u1 = 1.0-Rng.NextDouble();
            var u2 = 1.0-Rng.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + std * randStdNormal;
        }
    }
}
