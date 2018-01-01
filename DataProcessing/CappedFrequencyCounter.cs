using System.Collections.Generic;
using System.Linq;

namespace Commons.DataProcessing
{
    /// <summary>
    /// Counts frequency of items, but is limited to a maximum number of unique items.
    /// If the maxmimum number of items is reached, all items with a frequency of 1
    /// are discarded. If none items have a frequency of 1, new items are discarded.
    /// 
    /// This class is useful when counting sequence frequencies with huge amount of different sequences occurring,
    /// e.g. in amino acid sequences.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CappedFrequencyCounter<T>
    {
        private readonly int maxCapacity;
        private bool capActive;
        public ulong TotalItemCount { get; set; }
        public Dictionary<T, ulong> Counter { get; } = new Dictionary<T, ulong>();
        public ulong OtherCount { get; private set; }

        public CappedFrequencyCounter(int maxCapacity)
        {
            this.maxCapacity = maxCapacity;
        }

        public void Add(T item)
        {
            TotalItemCount++;
            if (Counter.ContainsKey(item))
                Counter[item]++;
            else
            {
                if (!capActive && Counter.Count == maxCapacity)
                {
                    var oneItemKeys = Counter.Where(kvp => kvp.Value == 1).Select(kvp => kvp.Key).ToList();
                    if(oneItemKeys.Any())
                        oneItemKeys.ForEach(x => Counter.Remove(x));
                    else
                        capActive = true;
                }
                if (capActive)
                    OtherCount++;
                else
                    Counter.Add(item, 1);
            }
        }
    }
}
