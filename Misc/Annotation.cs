using System.Collections.Generic;
using Commons.Mathematics;

namespace Commons.Misc
{
    public class Annotation
    {
        public Annotation(List<Point2D> points, AnnotationShapeType shapeType)
        {
            Points = points;
            ShapeType = shapeType;
        }

        public AnnotationShapeType ShapeType { get; }
        public List<Point2D> Points { get; }
    }
}