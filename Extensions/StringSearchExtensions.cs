using System;
using System.Collections.Generic;
using Commons.Annotations;

namespace Commons.Extensions
{
    public static class StringSearchExtensions
    {
        public static List<int> AllIndicesOf([NotNull] this string input, string searchValue)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var currentPosition = input.IndexOf(searchValue, StringComparison.InvariantCulture);
            var indices = new List<int>();
            while (currentPosition >= 0)
            {
                indices.Add(currentPosition);
                currentPosition = input.IndexOf(searchValue, currentPosition+searchValue.Length, StringComparison.InvariantCulture);
            }
            return indices;
        }
    }
}