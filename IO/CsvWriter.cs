using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Commons.Collections;

namespace Commons.IO
{
    public static class CsvWriter
    {
        private static string DefaultToString<T>(T x) => x.ToString();

        public static void Write<T>(
            Table<T> table,
            string filename,
            char delimiter = ';',
            Func<T, string> toStringFunc = null)
        {
            Write(table, () => new StreamWriter(filename, false), delimiter, toStringFunc);
        }

        public static void Write<T>(
            Table<T> table, 
            Func<TextWriter> textWriterFunc,
            char delimiter = ';',
            Func<T, string> toStringFunc = null)
        {
            if (toStringFunc == null)
                toStringFunc = DefaultToString;
            using (var textWriter = textWriterFunc())
            {
                var header = table.Columns.Select(c => c.Name).Aggregate((a, b) => a + delimiter + b);
                textWriter.WriteLine(header);
                foreach (var row in table.Rows)
                {
                    var line = row.Select(toStringFunc).Aggregate((a, b) => a + delimiter + b);
                    textWriter.WriteLine(line);
                }
            }
        }

        public static void Write(double[,] array, string filename, char delimiter = ';')
        {
            Write(array, () => new StreamWriter(filename, false), delimiter);
        }

        public static void Write(double[,] array, Func<TextWriter> textWriterFactory, char delimiter = ';')
        {
            var rowCount = array.GetLength(0);
            var columnCount = array.GetLength(1);

            using (var textWriter = textWriterFactory())
            {
                for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
                {
                    var line = string.Empty;
                    for (int columnIdx = 0; columnIdx < columnCount; columnIdx++)
                    {
                        if (columnIdx > 0)
                            line += delimiter;
                        line += array[rowIdx, columnIdx].ToString("G4", CultureInfo.InvariantCulture);
                    }
                    textWriter.WriteLine(line);
                }
            }
        }
    }
}
