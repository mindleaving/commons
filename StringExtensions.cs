using System;
using System.Linq;

namespace Commons
{
    public static class StringExtensions
    {
        public static string FirstLetterToUpperInvariant(this string input)
        {
            var words = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            return words
                .Select(x => x.Substring(0, 1).ToUpperInvariant() + x.Substring(1).ToLowerInvariant())
                .Aggregate((a, b) => a + " " + b);
        }
    }
}
