using System;
using System.Text.RegularExpressions;

namespace Commons.Extensions
{
    public static class SearchTermSplitter
    {
        public static string[] SplitAndToLower(string searchText)
        {
            return searchText.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public static string ToSecureNormalizedSearchText(
            string searchText)
        {
            var lowerSearchText = searchText.ToLower();
            var trimmedSearchText = Regex.Replace(lowerSearchText, @"\s+", " ");
            var secureSearchText = Regex.Replace(trimmedSearchText, @"[^a-z0-9-\s]", "");
            return secureSearchText;
        }
    }
}
