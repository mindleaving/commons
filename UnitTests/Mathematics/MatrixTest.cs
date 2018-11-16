using Commons.Mathematics;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CommonsTest.Mathematics
{
    [TestFixture]
    public class MatrixTest
    {
        [Test]
        public void MatrixSerializationRoundtrip()
        {
            var matrix = new Matrix(new double[,]
            {
                {1, 2, 3},
                {4, 5, 6}
            });
            var serializedMatrix = JsonConvert.SerializeObject(matrix);
            var reconstructedMatrix = JsonConvert.DeserializeObject<Matrix>(serializedMatrix);
            Assert.That(reconstructedMatrix.Rows, Is.EqualTo(matrix.Rows));
            Assert.That(reconstructedMatrix.Columns, Is.EqualTo(matrix.Columns));
            for (int row = 0; row < matrix.Rows; row++)
            {
                for (int column = 0; column < matrix.Columns; column++)
                {
                    Assert.That(reconstructedMatrix[row, column], Is.EqualTo(matrix[row, column]));
                }
            }
        }
    }
}
