using System;
using Commons.IO;
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

        [Test]
        public void ReducedRowEcholonFormAsExpected()
        {
            var matrix = new double[,]
            {
                {8, 1, 0, 6},
                {3, 5, 0, 7},
                {4, 9, 0, 2}
            };
            var actual = matrix.ReducedRowEchelonForm();
            var expected = new double[,]
            {
                {1, 0, 0, 0},
                {0, 1, 0, 0},
                {0, 0, 0, 1}
            };
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int column = 0; column < matrix.GetLength(1); column++)
                {
                    Assert.That(actual[row, column], Is.EqualTo(expected[row, column]));
                }
            }
        }

        [Test]
        [Ignore("Depends on files")]
        public void LargeMatrixReducedRowEcholonFormAsExpected()
        {
            var matrix = CsvReader.ReadDoubleArray(@"C:\temp\X.csv");
            var actual = matrix.ReducedRowEchelonForm();
            var expected = CsvReader.ReadDoubleArray(@"C:\temp\rref.csv");
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int column = 0; column < matrix.GetLength(1); column++)
                {
                    Assert.That(actual[row, column], Is.EqualTo(expected[row, column]));
                }
            }
        }
    }
}
