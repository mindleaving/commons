using Commons.Mathematics;
using NUnit.Framework;

namespace CommonsTest.Mathematics
{
    [TestFixture]
    public class MatrixOperationsTest
    {
        [Test]
        public void VectorizeReshapeReconstructsMatrix()
        {
            var matrix = new double[,]
            {
                {1, 2, 3},
                {4, 5, 6}
            };
            var vectorized = matrix.Vectorize();
            var reshaped = vectorized.Reshape(matrix.GetLength(0), matrix.GetLength(1));
            Assert.That(reshaped.GetLength(0), Is.EqualTo(matrix.GetLength(0)));
            Assert.That(reshaped.GetLength(1), Is.EqualTo(matrix.GetLength(1)));
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int column = 0; column < matrix.GetLength(1); column++)
                {
                    Assert.That(reshaped[row, column], Is.EqualTo(matrix[row, column]));
                }
            }
        }
    }
}
