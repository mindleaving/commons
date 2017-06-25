using System.Globalization;
using System.IO;

namespace Commons.IO
{
    public static class CsvReader
    {
        private const char Delimiter = ';';

        public static double[,] ReadDoubleArray(string filename)
        {
            // Count rows and columns
            var rowCount = 0;
            var columnCount = 0;
            double[,] array;
            using (var fileStream = File.OpenRead(filename))
            using (var streamReader = new StreamReader(fileStream))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if(string.IsNullOrWhiteSpace(line))
                        break;
                    var splittedLine = line.Split(Delimiter);
                    if (splittedLine.Length > columnCount)
                        columnCount = splittedLine.Length;
                    rowCount++;
                }
                fileStream.Seek(0, SeekOrigin.Begin);

                array = new double[rowCount,columnCount];
                var rowIdx = 0;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        break;
                    var splittedLine = line.Split(Delimiter);
                    for (int columnIdx = 0; columnIdx < splittedLine.Length; columnIdx++)
                    {
                        array[rowIdx, columnIdx] = double.Parse(splittedLine[columnIdx], NumberStyles.Any, CultureInfo.InvariantCulture);
                    }
                    for (int columnIdx = splittedLine.Length; columnIdx < columnCount; columnIdx++)
                    {
                        array[rowIdx, columnIdx] = double.NaN;
                    }
                    rowIdx++;
                }
            }
            return array;
        }
    }
}
