using System.Text.RegularExpressions;

namespace Intl.Realty.Firm.Utilities
{
    public class StringManipulation
    {
        private static readonly Regex sWhitespace = new Regex(@"\s+");
        public static string ReplaceWhitespace(string input, string replacement)
        {
            return sWhitespace.Replace(input, replacement);
        }

    }
}
