using System.Text.RegularExpressions;

namespace FWTL.Common.Helpers
{
    public static class RegexExpressionsExtensions
    {
        public static string Replace(this string pattern, string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            return Regex.Replace(input, pattern, "");
        }
    }
}