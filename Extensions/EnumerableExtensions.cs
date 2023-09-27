﻿using System;
using System.Collections.Generic;
using System.Linq;
using Commons.Mathematics;

namespace Commons.Extensions
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> ts, Action<T> @do)
        {
            foreach (var t in ts)
            {
                @do(t);
            }
        }

        public static IEnumerable<TOut> PairwiseOperation<T1, T2, TOut>(this IList<T1> values1, IList<T2> values2, Func<T1, T2, TOut> operation)
        {
            if(values1.Count != values2.Count)
                throw new InvalidOperationException("Cannot apply pairwise operation to lists of different length");
            var result = new List<TOut>(values1.Count);
            for (int idx = 0; idx < values1.Count; idx++)
            {
                result.Add(operation(values1[idx],values2[idx]));
            }
            return result;
        }

        public static T MaximumItem<T>(this IEnumerable<T> collection, Func<T, double> valueSelector)
        {
            double InverseValueSelector(T item) => -valueSelector(item);

            return collection.MinimumItem(InverseValueSelector);
        }
        public static T MinimumItem<T>(this IEnumerable<T> collection, Func<T, double> valueSelector)
        {
            var collectionList = collection.ToList();
            if (!collectionList.Any())
                throw new InvalidOperationException("Sequence does not contain any items");

            var minValue = double.PositiveInfinity;
            var minValueItem = default(T);
            foreach (var item in collectionList)
            {
                var itemValue = valueSelector(item);
                if (itemValue < minValue)
                {
                    minValue = itemValue;
                    minValueItem = item;
                }
            }
            if (double.IsPositiveInfinity(minValue))
                throw new InvalidOperationException("No minimum found");
            return minValueItem;
        }

        public static IList<T> SubArray<T>(this IList<T> array, int startIdx, int count)
        {
            var subArray = new T[count];
            for (int targetIdx = 0; targetIdx < count; targetIdx++)
            {
                var sourceIdx = targetIdx + startIdx;
                subArray[targetIdx] = array[sourceIdx];
            }
            return subArray;
        }

        public static IList<T> SubArray<T>(this IList<T> array, Range<int> range)
        {
            return SubArray(array, range.From, range.To - range.From + 1);
        }

        public static bool IsEquivalent<T>(this IList<T> list1, IList<T> list2)
        {
            if (list1.Count != list2.Count)
                return false;
            var intersect = list1.Intersect(list2);

            return intersect.Count() == list1.Distinct().Count();
        }

        public static T ModalValue<T>(
            this IEnumerable<T> items)
        {
            return items.GroupBy(x => x)
                .Select(x => new { Value = x.Key, Count = x.Count()})
                .MaximumItem(x => x.Count)
                .Value;
        }

        public static bool ContainsAny<T>(
            this IEnumerable<T> items,
            params T[] queryItems)
        {
            return items.Intersect(queryItems).Any();
        }
    }
}