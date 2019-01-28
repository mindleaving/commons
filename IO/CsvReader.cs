using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Commons.Collections;

namespace Commons.IO
{
    public static class CsvReader
    {
        public static double[,] ReadDoubleArray(string filename, char delimiter = ';')
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
                    var splittedLine = line.Split(delimiter);
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
                    var splittedLine = line.Split(delimiter);
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

        public static Table<T> ReadTable<T>(
            string filename,
            Func<string, T> parseFunc,
            bool hasHeader = true,
            char delimiter = ';')
        {
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            if (parseFunc == null) throw new ArgumentNullException(nameof(parseFunc));

            return ReadTable(() => new StreamReader(filename), parseFunc, hasHeader, delimiter);
        }

        public static Table<T> ReadTable<T>(
            Func<TextReader> textReaderFactory,
            Func<string, T> parseFunc,
            bool hasHeader = true, 
            char delimiter = ';')
        {
            if (textReaderFactory == null) throw new ArgumentNullException(nameof(textReaderFactory));
            if (parseFunc == null) throw new ArgumentNullException(nameof(parseFunc));

            var table = new Table<T>();
            using (var textReader = textReaderFactory())
            {
                var isFirstLine = true;
                string line;
                while ((line = textReader.ReadLine()) != null)
                {
                    var splittedLine = line.Split(delimiter);
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        if (hasHeader)
                        {
                            foreach (var columnName in splittedLine)
                            {
                                table.AddColumn(columnName);
                            }
                        }
                        else
                        {
                            for (int columnIndex = 0; columnIndex < splittedLine.Length; columnIndex++)
                            {
                                table.AddColumn(columnIndex.ToString());
                            }
                        }
                        if(hasHeader)
                            continue;
                    }
                    table.AddRow(splittedLine.Select(parseFunc).ToList());
                }
            }
            return table;
        }
    }
}
