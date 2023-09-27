using System;
using System.Linq;

namespace Commons.Extensions
{
    public static class StringExtensions
    {
        public static string FirstLetterToUpperInvariant(this string input)
        {
            var words = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            return words
                .Select(str => char.ToUpperInvariant(str[0]) + str[1..].ToLowerInvariant())
                .Aggregate((a, b) => a + " " + b);
        }

        public static string FirstLetterToLowerInvariant(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return char.ToLowerInvariant(str[0]) + str[1..];
        }
    }
}
