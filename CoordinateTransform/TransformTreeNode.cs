using System;

namespace Commons.CoordinateTransform
{
    public class TransformTreeNode
    {
        public TransformTreeNode(string coordinateName, Type pointType)
        {
            CoordinateName = coordinateName;
            PointType = pointType;
        }

        public string CoordinateName { get; }
        public Type PointType { get; }
    }
}