using System.Collections.Generic;
using System.Runtime.Serialization;
using Commons.Mathematics;

namespace Commons.Misc
{
    [DataContract]
    public class Annotation
    {
        private Annotation() {} // For deserialization
        public Annotation(List<Point2D> points, AnnotationShapeType shapeType)
        {
            Points = points;
            ShapeType = shapeType;
        }

        [DataMember]
        public AnnotationShapeType ShapeType { get; private set; }
        [DataMember]
        public List<Point2D> Points { get; private set; }
    }
}