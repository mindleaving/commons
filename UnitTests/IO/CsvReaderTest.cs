using System.IO;
using System.Linq;
using System.Text;
using Commons.Collections;
using Commons.IO;
using NUnit.Framework;

namespace CommonsTest.IO
{
    [TestFixture]
    public class CsvReaderTest
    {
        [Test]
        public void TableWriteReadRoundtrip()
        {
            var table = new Table<double>("C1", "C2");
            table.AddRow(new[] { 1.3, -2.1 });
            Table<double> reconstructedTable;
            using (var memoryStream = new MemoryStream())
            {
                CsvWriter.Write(
                    table, 
                    () => new StreamWriter(memoryStream, Encoding.UTF8, 1024, true), 
                    toStringFunc: x => x.ToString("F2") + "_");
                memoryStream.Seek(0, SeekOrigin.Begin);
                reconstructedTable = CsvReader.ReadTable(
                    () => new StreamReader(memoryStream, Encoding.UTF8, false, 1024, true),
                    parseFunc: str => double.Parse(str.Substring(0, str.Length - 1)));
            }
            Assert.That(reconstructedTable.Columns.Count, Is.EqualTo(table.Columns.Count));
            Assert.That(reconstructedTable.Rows.Count, Is.EqualTo(table.Rows.Count));
            var firstRow = reconstructedTable.Rows.First();
            Assert.That(firstRow["C1"], Is.EqualTo(table.Rows[0]["C1"]).Within(1e-5));
        }
    }
}
