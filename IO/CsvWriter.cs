using System.Globalization;
using System.IO;

namespace Commons.Debug
{
    public static class CsvWriter
    {
        private const string Delimiter = ";";

        public static void Write(double[,] array, string filename)
        {
            var rowCount = array.GetLength(0);
            var columnCount = array.GetLength(1);

            using (var fileStream = File.Open(filename, FileMode.Create))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
                {
                    var line = string.Empty;
                    for (int columnIdx = 0; columnIdx < columnCount; columnIdx++)
                    {
                        if (columnIdx > 0)
                            line += Delimiter;
                        line += array[rowIdx, columnIdx].ToString("G4", CultureInfo.InvariantCulture);
                    }
                    streamWriter.WriteLine(line);
                }
            }
        }
    }
}
