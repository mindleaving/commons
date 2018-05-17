namespace Commons.Mathematics
{
    public class Circle
    {
        public Circle(Point2D centerPoint, double radius)
        {
            Center = centerPoint;
            Radius = radius;
        }

        public Point2D Center { get; set; }
        public double Radius { get; set; }
    }
}
