using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Commons.Collections
{
    public class Column<T> : IEnumerable<T>
    {
        private readonly Table<T> table;

        public Column(Table<T> table, string name)
        {
            this.table = table;
            Name = name;
        }

        public string Name { get; }

        public T this[int index]
        {
            get => table.Rows[index][Name];
            set => table.Rows[index][Name] = value;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Enumerable.Range(0, table.Rows.Count)
                .Select(rowIdx => table.Rows[rowIdx][Name])
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}