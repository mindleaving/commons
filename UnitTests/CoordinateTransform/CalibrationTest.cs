using System.IO;
using System.Runtime.Serialization;
using Commons.CoordinateTransform;
using Commons.IO;
using Commons.Mathematics;
using NUnit.Framework;

namespace Commons.UnitTests.CoordinateTransform
{
    [TestFixture]
    public class CalibrationTest
    {
        private const double Tolerance = 1e-5;

        [Test]
        public void CalibrationCanBeSerialized()
        {
            var rotationMatrix = new Matrix3X3();
            rotationMatrix.Data[1, 1] = 3.5;
            var calibration = new Calibration
            {
                FocalLength = new Point2D(3005, 3008),
                PrincipalPoint = new Point2D(1024, 580),
                Distortion = new DistortionParameters
                {
                    Radial2 = 0.7,
                    Radial4 = 0.75
                },
                TranslationVector = new Vector3D(2, 15, 3050),
                RotationMatrix = rotationMatrix
            };
            var serializer = new Serializer<Calibration>();
            using (var stream = new MemoryStream())
            {
                Assert.That(() => serializer.Store(calibration, stream), Throws.Nothing);
                stream.Seek(0, SeekOrigin.Begin);
                var json = new StreamReader(stream).ReadToEnd();
                stream.Seek(0, SeekOrigin.Begin);
                Calibration deserializedCalibration = null;
                Assume.That(() => deserializedCalibration = serializer.Load(stream), Throws.Nothing);
                Assert.That(deserializedCalibration.FocalLength, Is.EqualTo(calibration.FocalLength));
                Assert.That(deserializedCalibration.PrincipalPoint, Is.EqualTo(calibration.PrincipalPoint));
                Assert.That(deserializedCalibration.Distortion.Radial2, Is.EqualTo(0.7).Within(Tolerance));
                Assert.That(deserializedCalibration.TranslationVector.Y, Is.EqualTo(15).Within(Tolerance));
                Assert.That(deserializedCalibration.RotationMatrix[1,1], Is.EqualTo(3.5).Within(Tolerance));
            }
        }
    }
}
