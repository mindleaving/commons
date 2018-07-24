using Commons.Mathematics;

namespace Commons.CoordinateTransform
{
    public class Calibration
    {
        public Point2D FocalLength { get; set; }
        public Point2D PrincipalPoint { get; set; }
        public DistortionParameters Distortion { get; set; }
        public Vector3D TranslationVector { get; set; }
        public Matrix3X3 RotationMatrix { get; set; }
    }
}