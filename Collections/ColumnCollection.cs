using System;
using System.Collections;
using System.Collections.Generic;

namespace Commons.Collections
{
    public class ColumnCollection<T> : IEnumerable<Column<T>>
    {
        private List<Column<T>> Columns { get; } = new List<Column<T>>();
        private readonly Dictionary<string, int> nameToIndexMap = new Dictionary<string, int>();

        public int Count => Columns.Count;

        public void Add(Column<T> column)
        {
            if(Contains(column.Name))
                throw new InvalidOperationException($"Column with name '{column.Name}' already exists");
            nameToIndexMap.Add(column.Name, Columns.Count);
            Columns.Add(column);
        }

        public bool Contains(string columnName)
        {
            return nameToIndexMap.ContainsKey(columnName);
        }

        public int IndexOf(string columName)
        {
            return nameToIndexMap[columName];
        }

        public Column<T> this[string columnName]
        {
            get => Columns[nameToIndexMap[columnName]];
        }

        public Column<T> this[int index]
        {
            get => Columns[index];
        }
        public IEnumerator<Column<T>> GetEnumerator()
        {
            return Columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}