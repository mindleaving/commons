using System.Collections.Generic;
using Commons.Mathematics;
using Newtonsoft.Json;

namespace Commons.Misc
{
    public class Annotation
    {
        [JsonConstructor]
        public Annotation(
            List<Point2D> points, 
            AnnotationShapeType shapeType, 
            Dictionary<string, string> additionalInformation = null)
        {
            Points = points;
            ShapeType = shapeType;
            AdditionalInformation = additionalInformation ?? new Dictionary<string, string>();
        }

        public AnnotationShapeType ShapeType { get; }
        public List<Point2D> Points { get; }
        public Dictionary<string,string> AdditionalInformation { get; }
    }
}