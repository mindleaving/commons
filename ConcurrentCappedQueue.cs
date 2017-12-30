using System.Collections.Concurrent;

namespace Commons
{
    public class ConcurrentCappedQueue<T> : ConcurrentQueue<T>
    {
        public int Cap { get; }

        public ConcurrentCappedQueue(int cap)
        {
            Cap = cap;
        }

        public new void Enqueue(T item)
        {
            base.Enqueue(item);
            while (Count > Cap)
            {
                TryDequeue(out _);
            }
        }
    }
}
