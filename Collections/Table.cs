using System.Collections.Generic;

namespace Commons.Collections
{
    public class Table<T>
    {
        public Table(params string[] columnNames)
        {
            foreach (var columnName in columnNames)
            {
                AddColumn(columnName);
            }
        }

        public ColumnCollection<T> Columns { get; } = new ColumnCollection<T>();
        public List<Row<T>> Rows { get; } = new List<Row<T>>();

        public void AddColumn(string columnName)
        {
            var column = new Column<T>(this, columnName);
            Columns.Add(column);
            Rows.ForEach(row => row.Extend());
        }

        public void AddRow(Dictionary<string, T> rowData)
        {
            var row = new Row<T>(this, rowData);
            Rows.Add(row);
        }

        public void AddRow(IList<T> rowData)
        {
            var row = new Row<T>(this, rowData);
            Rows.Add(row);
        }
    }
}