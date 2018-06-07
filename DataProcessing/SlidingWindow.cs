using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commons.Mathematics;

namespace Commons.DataProcessing
{
    public class SlidingWindow<T> : IEnumerable<T>
    {
        private readonly Queue<T> itemQueue;
        private readonly Queue<T> itemsInWindow = new Queue<T>();
        private readonly Func<T, double> valueSelector;
        private readonly double windowSize;
        private readonly WindowPositioningType windowPositioningType;

        public SlidingWindow(
            IEnumerable<T> items,
            Func<T, double> valueSelector,
            double windowSize,
            WindowPositioningType windowPositioningType)
        {
            itemQueue = new Queue<T>(items.OrderBy(valueSelector));
            this.valueSelector = valueSelector;
            this.windowSize = windowSize;
            this.windowPositioningType = windowPositioningType;
        }

        public Range<double> Window { get; private set; }

        public void SetPosition(double position)
        {
            UpdateWindowRange(position);
            UpdateItemQueue();
        }

        private void UpdateWindowRange(double position)
        {
            double windowStart, windowEnd;
            switch (windowPositioningType)
            {
                case WindowPositioningType.StartingAtPosition:
                    windowStart = position;
                    windowEnd = position + windowSize;
                    break;
                case WindowPositioningType.CenteredAtPosition:
                    windowStart = position - windowSize / 2;
                    windowEnd = position + windowSize / 2;
                    break;
                case WindowPositioningType.EndsAtPosition:
                    windowStart = position - windowSize;
                    windowEnd = position;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Window != null && Window.From > windowStart)
            {
                throw new InvalidOperationException("Window can only be moved forward. "
                                                    + $"Is currently at ({Window.From},{Window.To}) and was requested to move to ({windowStart},{windowEnd})");
            }

            Window = new Range<double>(windowStart, windowEnd);
        }

        private void UpdateItemQueue()
        {
            // Remove items no longer in window
            while (itemsInWindow.Count > 0 && valueSelector(itemsInWindow.Peek()) < Window.From)
            {
                itemsInWindow.Dequeue();
            }

            // Discard items which are before the window start position.
            // They will never get in window, because the window can only move forward
            while (itemQueue.Count > 0 && valueSelector(itemQueue.Peek()) < Window.From)
            {
                itemQueue.Dequeue();
            }

            // Add items in window to window queue
            while (itemQueue.Count > 0 && valueSelector(itemQueue.Peek()) < Window.To)
            {
                itemsInWindow.Enqueue(itemQueue.Dequeue());
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            if(Window == null)
                throw new InvalidOperationException("Window position not set");
            return itemsInWindow.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public enum WindowPositioningType
    {
        StartingAtPosition,
        CenteredAtPosition,
        EndsAtPosition
    }
}
