using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StaticFileTransform.Abstractions
{
    public class StaticFileTransform : IStaticFileTransform
    {
        private readonly Func<String, IContentProvider, String> transform;
        private readonly Func<String, Boolean> matcher;
        private readonly int priority;

        public StaticFileTransform(Func<string, bool> matcher, Func<string, IContentProvider, string> transform, int priority)
        {
            this.matcher = matcher;
            this.transform = transform;
            this.priority = priority;
        }

        public string Apply(string filename, IContentProvider provider) => transform(filename, provider);

        public bool Matches(string filename) => matcher(filename);

        public int Priority => priority;


        class MatchBuilder
        {
            private readonly Func<String, IContentProvider, String> transform;

            public StaticFileTransformBuilder IfMatches(Func<String, Boolean> rule)
            {
                matcher = rule;
                return this;
            }

            public StaticFileTransformBuilder IfMatches(Regex regex)
            {
                matcher = regex.IsMatch;
                return this;
            }

            public StaticFileTransformBuilder IfMatches(string pattern)
            {
                matcher = GlobPatternToFunction(pattern);
                return this;
            }

            public StaticFileTransformBuilder IfFilenameEndsWith(string filter)
            {
                matcher = input => input.EndsWith(filter);
                return this;
            }

            public StaticFileTransformBuilder IfMatchesExactly(string filename)
            {
                matcher = input => input == filename;
                return this;
            }

            internal static Func<string, bool> GlobPatternToFunction(string pattern)
            {
                if (pattern.Contains("*"))
                {
                    pattern = pattern
                        .Replace(".", "\\.")
                        .Replace("?", ".?")
                        .Replace("*", ".*?")
                        .Replace("(", "\\(")
                        .Replace(")", "\\)")
                        .Replace("[", "\\[")
                        .Replace("]", "\\]")
                        .Replace("{", "\\{")
                        .Replace("}", "\\}");
                    var regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
                    return filename => regex.IsMatch(filename);
                }
                else
                {
                    return filename => filename.EndsWith(filename);
                }
            }
        }

        public static Create()
        {
            
        }
    }
}
