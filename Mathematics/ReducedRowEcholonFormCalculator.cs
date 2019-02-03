using System;
using Commons.Extensions;

namespace Commons.Mathematics
{
    internal static class ReducedRowEcholonFormCalculator
    {
        public static double[,] Calculate(double[,] array)
        {
            if (array == null)
            {
                return null;
            }
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);
            var rrefArray = new double[rows, cols];
            Array.Copy(array, rrefArray, rows*cols);

            var operationColumn = 0;
            var operationRow = 0;
            while (operationColumn < cols && operationRow < rows)
            {
                if (!TryFindLargestNonZeroValueInColumnStartingFromRow(rrefArray, operationColumn, operationRow, out var nonZeroRowIndex))
                {
                    operationColumn++;
                    continue;
                }
                if(nonZeroRowIndex != operationRow)
                    AddRow1ToRow2(rrefArray, nonZeroRowIndex, operationRow);
                DivideRowWith(rrefArray, operationRow, rrefArray[operationRow, operationColumn], operationColumn);
                SubtractOperationRowFromOtherRows(rrefArray, operationRow, operationColumn);
                operationRow++;
                operationColumn++;
            }

            return rrefArray;
        }

        private static void SubtractOperationRowFromOtherRows(double[,] array, int operationRow, int operationColumn)
        {
            var rows = array.GetLength(0);
            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                if (rowIndex == operationRow)
                    continue;
                if (array[rowIndex, operationColumn] == 0)
                    continue;
                var valueInOperationColumn = array[rowIndex, operationColumn];
                SubtractMultipleOfRow1FromRow2(array, operationRow, rowIndex, valueInOperationColumn, operationColumn);
            }
        }

        private static bool TryFindLargestNonZeroValueInColumnStartingFromRow(double[,] array, int columnIndex, int startRowIndex, out int nonZeroRowIndex)
        {
            var maxAbsValue = 0d;
            nonZeroRowIndex = startRowIndex;
            var rows = array.GetLength(0);
            for (int rowIndex = startRowIndex; rowIndex < rows; rowIndex++)
            {
                var abs = array[rowIndex, columnIndex].Abs();
                if (abs > maxAbsValue)
                {
                    nonZeroRowIndex = rowIndex;
                    maxAbsValue = abs;
                }
            }
            return maxAbsValue > 0;
        }

        private static void AddRow1ToRow2(double[,] array, int row1Index, int row2Index)
        {
            var columns = array.GetLength(1);
            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                array[row2Index, columnIndex] += array[row1Index, columnIndex];
            }
        }

        private static void DivideRowWith(double[,] array, int rowIndex, double value, int startColumnIndex)
        {
            var columns = array.GetLength(1);
            for (int columnIndex = startColumnIndex; columnIndex < columns; columnIndex++)
            {
                array[rowIndex, columnIndex] /= value;
            }
        }

        private static void SubtractMultipleOfRow1FromRow2(double[,] array, int row1Index, int row2Index, double multiple, int startColumnIndex)
        {
            var columns = array.GetLength(1);
            for (int columnIndex = startColumnIndex; columnIndex < columns; columnIndex++)
            {
                array[row2Index, columnIndex] -= multiple * array[row1Index, columnIndex];
            }
        }
    }
}
