namespace Commons.Mathematics
{
    public class PolarPoint
    {
        public PolarPoint(double angle, double distanceFromCenter)
        {
            Angle = angle;
            DistanceFromCenter = distanceFromCenter;
        }

        public double Angle { get; }
        public double DistanceFromCenter { get; }
    }
}
