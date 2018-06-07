using System.Collections.Generic;

namespace Commons.Collections
{
    public class Cluster<T>
    {
        public Cluster(List<T> items)
        {
            Items = items;
        }

        public List<T> Items { get; }
    }
}