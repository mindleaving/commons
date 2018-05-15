using System.Runtime.Serialization;
using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
    [DataContract]
    public class Calibration
    {
        [DataMember]
        public Point2D FocalLength { get; set; }
        [DataMember]
        public Point2D PrincipalPoint { get; set; }
        [DataMember]
        public DistortionParameters Distortion { get; set; }
        [DataMember]
        public Vector3D TranslationVector { get; set; }
        [DataMember]
        public Matrix3X3 RotationMatrix { get; set; }
    }
}