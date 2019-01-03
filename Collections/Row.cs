using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Collections
{
    public class Row<T> : IEnumerable<T>
    {
        private readonly Table<T> table;
        private readonly IList<T> values;

        public Row(Table<T> table, IList<T> values)
        {
            if(values.Count != table.Columns.Count)
                throw new ArgumentException($"Length of row data ({values.Count}) doesn't match number of columns ({table.Columns.Count})");
            this.table = table;
            this.values = values;
        }
        public Row(Table<T> table, Dictionary<string, T> values)
        {
            this.table = table;
            this.values = Enumerable.Range(0, table.Columns.Count)
                .Select(idx =>
                {
                    var columnName = table.Columns[idx].Name;
                    if (values.ContainsKey(columnName))
                        return values[columnName];
                    return default(T);
                }).ToList();
        }

        public void Extend()
        {
            values.Add(default(T));
        }

        public T this[string columnName]
        {
            get => values[table.Columns.IndexOf(columnName)];
            set => values[table.Columns.IndexOf(columnName)] = value;
        }

        public T this[int idx]
        {
            get => values[idx];
            set => values[idx] = value;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}