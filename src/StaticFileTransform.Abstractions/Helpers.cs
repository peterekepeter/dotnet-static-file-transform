using System;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticFileTransform.Abstractions
{
    internal class Helpers
    {
        internal static Regex PatternToRegex(string pattern) =>
            new Regex(
                // ReSharper disable once UseStringInterpolation
                string.Format("{0}$", pattern
                    .Replace("\\", "\\\\")
                    .Replace("^", "\\^")
                    .Replace("$", "\\$")
                    .Replace("(", "\\(")
                    .Replace(")", "\\)")
                    .Replace("[", "\\[")
                    .Replace("]", "\\]")
                    .Replace("{", "\\{")
                    .Replace("}", "\\}")
                    .Replace(".", "\\.")
                    .Replace("?", "(.?)")
                    .Replace("*", "(.*?)")),
            RegexOptions.Compiled | RegexOptions.CultureInvariant);

        internal static String PatternToStitch(string pattern) 
        {
            char[] separators = { '?', '*' };
            var builder = new StringBuilder();
            var split = pattern.Split(separators);
            builder.Append(split[0]);
            for (int i=1; i<split.Length; i++)
            {
                builder.Append($"${i}");
                builder.Append(split[i]);
            }
            return builder.ToString();
        }

        internal static Func<string, bool> PatternToFunction(string pattern) => 
            pattern.Contains("*") || pattern.Contains("?")
            ? (Func<string, bool>) PatternToRegex(pattern).IsMatch
            : (filename => filename.EndsWith(pattern));
    }
}
